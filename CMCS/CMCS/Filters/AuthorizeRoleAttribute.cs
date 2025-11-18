using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CMCS.Filters
{
    public class AuthorizeRoleAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string _role;

        public AuthorizeRoleAttribute(string role)
        {
            _role = role;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var currentRole = context.HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(currentRole))
            {
                // Not logged in → redirect to Login
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }

            if (!currentRole.Equals(_role, StringComparison.OrdinalIgnoreCase))
            {
                // Logged in but wrong role
                context.Result = new ContentResult()
                {
                    Content = "Access Denied: You do not have permission to access this page."
                };
            }
        }
    }
}
