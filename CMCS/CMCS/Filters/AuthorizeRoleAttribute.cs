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
            // Get role from session (FIXED)
            var currentRole = context.HttpContext.Session.GetString("UserRole");

            if (string.IsNullOrEmpty(currentRole))
            {
                // User not logged in
                context.Result = new RedirectToActionResult("Login", "Account",
                    new { returnUrl = context.HttpContext.Request.Path });
                return;
            }

            // Compare roles (case insensitive)
            if (!currentRole.Equals(_role, StringComparison.OrdinalIgnoreCase))
            {
                // User logged in but lacks permission
                context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
            }
        }
    }
}
