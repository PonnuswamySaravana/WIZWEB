using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;




namespace WIZWEB.Filters
{
    public class ApiKeyAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private const string API_KEY_HEADER_NAME = "x-api-key";
        private const string VALID_API_KEY = "7413aea-5e6f-4e2f-b198-f6407413fb3b";

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(API_KEY_HEADER_NAME, out var extractedApiKey))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = StatusCodes.Status408RequestTimeout,
                    Content = "Need APIKEY Authentication for API"
                };
                return;
            }

            if (!string.Equals(extractedApiKey, VALID_API_KEY, StringComparison.Ordinal))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Content = "ApiKey is invalid."
                };
                return;
            }

            // Optionally set the user principal
            var claims = new[] { new Claim(ClaimTypes.Name, API_KEY_HEADER_NAME) };
            var identity = new ClaimsIdentity(claims, "ApiKey");
            context.HttpContext.User = new ClaimsPrincipal(identity);
        }
    }
}
