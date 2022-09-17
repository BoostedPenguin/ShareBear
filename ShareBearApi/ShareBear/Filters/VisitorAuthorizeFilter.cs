using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using ShareBear.Data.Models.FingerprintJs;
using ShareBear.Helpers;
using System.Text.Json;

namespace ShareBear.Filters
{
    public class VisitorAuthorizeFilter : Attribute, IAsyncAuthorizationFilter
    {
        private readonly IHttpClientFactory httpContextFactory;
        private readonly IOptions<AppSettings> appSettings;

        public VisitorAuthorizeFilter(IHttpClientFactory httpContextFactory, IOptions<AppSettings> appSettings)
        {
            this.httpContextFactory = httpContextFactory;
            this.appSettings = appSettings;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.Headers.ContainsKey("VisitorIdHeader"))
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Result = new UnauthorizedObjectResult(new
                {
                    Message = "You aren't authorized to access this method"
                });
                return;
            }

            var visitorIdHeader = context.HttpContext.Request.Headers["VisitorIdHeader"];
            var httpClient = httpContextFactory.CreateClient();

            var fingerprintJsUrl = $"https://eu.api.fpjs.io/visitors/{visitorIdHeader}?api_key={appSettings.Value.FingerprintJSApiKey}";

            var visitorResponse =
                await httpClient.GetAsync(fingerprintJsUrl);

            var responseString = await visitorResponse.Content.ReadAsStringAsync();
            var mappedResponse = JsonSerializer.Deserialize<FingerprintJsResponse>(responseString);

            if (mappedResponse == null || mappedResponse.visits.Count == 0)
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Result = new UnauthorizedObjectResult(new
                {
                    Message = "You aren't authorized to access this method"
                });
                return;
            }
        }
    }
}
