using RazorApp1.Domain;

namespace RazorApp1
{
    public class ContextData
    {
        public string? RecipientEmail
        {
            get
            {
                if (_httpCcontext != null && _httpCcontext.Request.Cookies.ContainsKey(DataKey.Email))
                {
                    return _httpCcontext.Request.Cookies[DataKey.Email] ?? "";
                }
                return null;
            }
            set
            {
                ArgumentNullException.ThrowIfNull(_httpCcontext);

                if (_httpCcontext.Request.Cookies.ContainsKey(DataKey.Email))
                {
                    _httpCcontext.Response.Cookies.Delete(DataKey.Email);
                }

                _httpCcontext.Response.Cookies.Append(DataKey.Email, value ?? "");
            }
        }

        private readonly HttpContext? _httpCcontext;

        public ContextData(IHttpContextAccessor accessor)
        {
            _httpCcontext = accessor?.HttpContext;
        }
    }
}
