using Microsoft.AspNetCore.Authorization;

namespace Shared.Global
{
  
    public class Policies
    {
        public const string RequireSysAdmin = "RequireSysAdmin";
        //public const string RequireIDCAdmin = "RequireIDCAdmin";
        public const string RequireReserver = "RequireReserver";

        public Policies(AuthorizationOptions options)
        {
            //options.AddPolicy(RequireSysAdmin, x => x.RequireAssertion(SysAdminPolicyHandler));
            //options.AddPolicy(RequireReserver, x => x.RequireAssertion(ReserverHandler));
            //options.AddPolicy(RequireIDCAdmin, x => x.RequireAssertion(IDCAdminHandler));
        }


   

     
    }

}