using Shared.UI;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net;
using System.Text.Json;

namespace UI.Helpers
{
    public static class HttpClientExtenstions
    {
        public static ReserverTokenAuthenticationStateProvider tokenStorage;
        public static async Task<APIReturn<T>> GetJsonAsync<T>(this HttpClient http, string url,
            PagingData paging = null, string orderBy = "", string overrideToken = "")
        {
            if (url[0] == '/')
            {
                url = url.Substring(1);
            }
            if (paging != null)
            {
                if (url.Contains("?"))
                {
                    url = url + $"&pageNumber={paging.PageNumber}&pageSize={paging.PageSize}";
                }
                else
                {
                    url = url + $"?pageNumber={paging.PageNumber}&pageSize={paging.PageSize}";
                }
            }

            if (orderBy != "")
            {
                if (url.Contains("?"))
                {
                    url = url + "&order=" + orderBy;
                }
                else
                {
                    url = url + "?order=" + orderBy;
                }
            }
            //Console.WriteLine(url);
            try
            {
                //var t = await tokenStorage.GetTokenAsync();
                //Console.WriteLine("ssvvvv");

                http.DefaultRequestHeaders.Authorization =
       new AuthenticationHeaderValue("Bearer", string.IsNullOrEmpty(overrideToken)
       ? await tokenStorage.GetTokenAsync() : overrideToken);

                var httpResponse = await http.GetAsync($"{url}");


                if (httpResponse.IsSuccessStatusCode)
                {
                    if (httpResponse.Headers.Contains("pagesQuantity"))
                    {
                        var responseString = await httpResponse.Content.ReadAsStringAsync();
                        //Console.WriteLine(responseString);
                        var content = JsonSerializer.Deserialize<APIReturn<T>>(responseString,
                            new JsonSerializerOptions()
                            {
                                PropertyNameCaseInsensitive = true
                            });

                        return content;
                    }
                    else
                    {
                        var responseString = await httpResponse.Content.ReadAsStringAsync();
                        //Console.WriteLine(responseString);

                        var content = JsonSerializer.Deserialize<APIReturn<T>>(responseString,
                            new JsonSerializerOptions()
                            {
                                PropertyNameCaseInsensitive = true
                            });
                        //Console.WriteLine(JsonSerializer.Serialize(content));
                        return content;
                        //return new APIReturn<T>(1, content.Data, content.Errors);
                    }
                }
                else if (httpResponse.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return new APIReturn<T>() { Errors = new List<Error>() { new Error("Un authorized") } };
                }
                else if (httpResponse.StatusCode == HttpStatusCode.NotFound)
                {
                    return new APIReturn<T>() { Errors = new List<Error>() { new Error("URL not found") } };
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception e)
            {
                return new APIReturn<T>()
                { Errors = new List<Error>() { new Error("A problem occured while calling api") } };
            }
        }


        public static async Task<APIReturn<T>> GetExternalJsonAsync<T>(this HttpClient http, string url,
            PagingData paging = null, string orderBy = "")
        {
            if (url[0] == '/')
            {
                url = url.Substring(1);
            }
            if (paging != null)
            {
                if (url.Contains("?"))
                {
                    url = url + $"&pageNumber={paging.PageNumber}&pageSize={paging.PageSize}";
                }
                else
                {
                    url = url + $"?pageNumber={paging.PageNumber}&pageSize={paging.PageSize}";
                }
            }

            if (orderBy != "")
            {
                if (url.Contains("?"))
                {
                    url = url + "&order=" + orderBy;
                }
                else
                {
                    url = url + "?order=" + orderBy;
                }
            }

            var httpResponse = await http.GetAsync($"{url}");
            if (httpResponse.IsSuccessStatusCode)
            {
                if (httpResponse.Headers.Contains("pagesQuantity"))
                {
                    // int totalPageQuantity = int.Parse(httpResponse.Headers.GetValues("pagesQuantity").FirstOrDefault());
                    var responseString = await httpResponse.Content.ReadAsStringAsync();
                    var content = JsonSerializer.Deserialize<T>(responseString,
                        new JsonSerializerOptions()
                        {
                            PropertyNameCaseInsensitive = true
                        });

                    return new APIReturn<T>(1, content, new List<Error>());
                }
                else
                {
                    var responseString = await httpResponse.Content.ReadAsStringAsync();
                    var content = JsonSerializer.Deserialize<T>(responseString,
                        new JsonSerializerOptions()
                        {
                            PropertyNameCaseInsensitive = true
                        });

                    return new APIReturn<T>(1, content, new List<Error>());
                }
            }
            else
            {
                throw new Exception();

                // handle error
            }


            //var response = await http.GetAsync(url);

            //if (response.IsSuccessStatusCode == true)
            //{
            //    string res = await response.Content.ReadAsStringAsync();
            //    var content = JsonConvert.DeserializeObject<T>(res);
            //    return content;
            //}
            //else
            //{
            //    throw new Exception();
            //}
        }

        public static async Task<APIReturn<TReturn>> PostJsonAsync<TReturn, TValue>(this HttpClient http, string url,
            TValue value)
        {
            if (url[0] == '/')
            {
                url = url.Substring(1);
            }
            var tt = await tokenStorage.GetTokenAsync();
            http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", await tokenStorage.GetTokenAsync());

            var httpResponse = await http.PostAsJsonAsync<TValue>($"{url}", value);
            var responseString = await httpResponse.Content.ReadAsStringAsync();
            if (httpResponse.IsSuccessStatusCode)
            {
                var content = JsonSerializer.Deserialize<APIReturn<TReturn>>(responseString,
                    new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    });

                return content;
                return new APIReturn<TReturn>(1, content.Data, content.Errors);
            }
            else if (httpResponse.StatusCode == HttpStatusCode.Unauthorized)
            {
                await tokenStorage.SetTokenAsync(null);
                return new APIReturn<TReturn>() { Errors = new List<Error>() { new Error("Un authorized") } };

            }
            else
            {
                throw new Exception();
              
            }


        }


        public static async Task<APIReturn<TReturn>> PostExternalJsonAsync<TReturn, TValue>(this HttpClient http, string url,
        TValue value)
        {
            if (url[0] == '/')
            {
                url = url.Substring(1);
            }
         

            var httpResponse = await http.PostAsJsonAsync<TValue>($"{url}", value);
            var responseString = await httpResponse.Content.ReadAsStringAsync();
            if (httpResponse.IsSuccessStatusCode)
            {
                var content = JsonSerializer.Deserialize<APIReturn<TReturn>>(responseString,
                    new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    });

                return content;
                return new APIReturn<TReturn>(1, content.Data, content.Errors);
            }
            else if (httpResponse.StatusCode == HttpStatusCode.Unauthorized)
            {
                await tokenStorage.SetTokenAsync(null);
                return new APIReturn<TReturn>() { Errors = new List<Error>() { new Error("Un authorized") } };

            }
            else
            {
                throw new Exception();
                
            }


        }
    }
}