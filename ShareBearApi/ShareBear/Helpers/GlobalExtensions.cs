namespace ShareBear.Helpers
{
    public static class GlobalExtensions
    {
        public static string GetVisitorId(this HttpContext context)
        {
            if (!context.Request.Headers.ContainsKey("VisitorIdHeader"))
            {
                throw new ArgumentException("No visitor id header provided.");
            }

            var visitorId = context.Request.Headers["VisitorIdHeader"];

            return visitorId;
        }
    }
}
