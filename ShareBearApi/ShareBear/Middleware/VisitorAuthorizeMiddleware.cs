using Microsoft.Extensions.Options;
using ShareBear.Data.Models.FingerprintJs;
using ShareBear.Helpers;
using System.Text.Json;

namespace ShareBear.Middleware
{
    public class VisitorAuthorizeMiddleware
    {
        private readonly RequestDelegate _next;

        public VisitorAuthorizeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IHttpClientFactory httpContextFactory, IOptions<AppSettings> appSettings)
        {
            if(!context.Request.Headers.ContainsKey("VisitorIdHeader"))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            var visitorIdHeader = context.Request.Headers["VisitorIdHeader"];
            var httpClient = httpContextFactory.CreateClient();

            var fingerprintJsUrl = $"https://eu.api.fpjs.io/visitors/{visitorIdHeader}?api_key={appSettings.Value.FingerprintJSApiKey}";
            
            var visitorResponse = 
                await httpClient.GetAsync(fingerprintJsUrl);

            var responseString = await visitorResponse.Content.ReadAsStringAsync();
            var myDeserializedClass = JsonSerializer.Deserialize<FingerprintJsResponse>(responseString);


            await _next(context);
        }
    }
}
