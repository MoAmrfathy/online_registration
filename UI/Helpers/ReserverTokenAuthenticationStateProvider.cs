using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.Text.Json;

namespace UI.Helpers
{
    public class ReserverTokenAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService storage;
        private readonly IJSRuntime _jsRuntime;
        //private readonly AuthConsumer _consumer;
        public ReserverTokenAuthenticationStateProvider(ILocalStorageService storage, IJSRuntime jsRuntime/*, AuthConsumer consumer*/)
        {
            this.storage = storage;
            this._jsRuntime = jsRuntime;
            //_consumer = consumer;
        }

        public async Task SetTokenAsync(string token, DateTime expiry = default)
        {
            if (token == null)
            {
                await storage.RemoveItemAsync("authTokenSessionManRev");
                await storage.RemoveItemAsync("authTokenSessionManRevExpiry");
            }
            else
            {
                await storage.SetItemAsync("authTokenSessionManRev", token);
                await storage.SetItemAsync("authTokenSessionManRevExpiry", DateTime.Now.AddDays(5).ToString());
            }
        }

        public async Task<string> GetTokenAsync(bool validate = false)
        {
            var expiry = await storage.GetItemAsync<string>("authTokenSessionManRevExpiry");
            if (expiry != null)
            {
                if (DateTime.Parse(expiry.ToString()) > DateTime.Now || true)
                {
                    var storage_token = await storage.GetItemAsync<string>("authTokenSessionManRev");
                    return storage_token;
                    //if (validate)
                    //{
                    //    var user = await _consumer.ValidateToken(storage_token);
                    //    if (user.HasErrors || user.Data == null || string.IsNullOrEmpty(user.Data.EmpNum))
                    //    {
                    //        await SetTokenAsync(null);
                    //        return "";
                    //    }
                    //    else
                    //    {
                    //        return storage_token;
                    //    }
                    //}
                    //else
                    //{
                    //    return storage_token;

                    //}
                }
                else
                {
                    await SetTokenAsync(null);
                }
            }
            return "";
        }
        private async Task<IEnumerable<Claim>> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = await ParseBase64WithoutPaddingAsync(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
        }

        private async Task<byte[]> ParseBase64WithoutPaddingAsync(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            // New Validation
            base64 = base64.Replace('_', '/').Replace("-", "+");
            try
            {
                return Convert.FromBase64String(base64);
            }
            catch
            {
                await storage.RemoveItemAsync("authTokenSessionManRev");
                await storage.RemoveItemAsync("authTokenSessionManRevExpiry");
                return null;
            }
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await GetTokenAsync(true);
            var identity = string.IsNullOrEmpty(token)
                ? new ClaimsIdentity()
                : new ClaimsIdentity(await ParseClaimsFromJwt(token), "jwt");
            return new AuthenticationState(new ClaimsPrincipal(identity));
        }
    }
}