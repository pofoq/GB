
namespace RazorApp1.Middlewares
{
    public class BrowserCheckMiddleware
    {
        private readonly Func<HttpContext, Task> _next;

        public BrowserCheckMiddleware(Func<HttpContext, Task> next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, Func<Task> next)
        {
            var userAgent = context.Request.Headers.UserAgent.ToString();
            if (!userAgent.Contains("edg", StringComparison.OrdinalIgnoreCase))
            {
                context.Response.ContentType = "text/plain;charset=utf-8";
                await context.Response.WriteAsync("Ваш браузер не поддерживается");
                return;
            }

            await next();
        }
    }
}
