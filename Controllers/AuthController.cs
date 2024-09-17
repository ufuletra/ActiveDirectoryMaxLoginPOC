using Microsoft.AspNetCore.Mvc;
using System.DirectoryServices.AccountManagement;

namespace ActiveDirectoryMaxLoginPOC.Controllers
{
    public class AuthController : Controller
    {
        [HttpPost]
        [Route("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            PrincipalContext principalContext = new(ContextType.Domain, "cpa.local");
            if (principalContext.ValidateCredentials(request.UserName, request.Password))
            {
                UserPrincipal userPrincipal = UserPrincipal.FindByIdentity(principalContext, request.UserName);
                if (userPrincipal != null)
                {
                    int badLogonCount = userPrincipal.BadLogonCount;
                    Console.WriteLine($"Bad Login Count: {badLogonCount}");
                    if (badLogonCount >= 3)
                    {
                        return Ok("Account Locked");
                    }
                }
                return Ok("Valid Credentials");
            }
            else
            {
                return Ok("Invalid Credentials");
            }

        }
    }
   
    public class LoginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
