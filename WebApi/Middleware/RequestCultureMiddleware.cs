using System.Globalization;

namespace WebApi.Middleware
{
    public class RequestCultureMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestCultureMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var cultureQuery = context.Request.RouteValues["culture"] as string;

            if (string.IsNullOrEmpty(cultureQuery))
            {
                // Tenta obter a cultura do cabeçalho Accept-Language
                cultureQuery = context.Request.Headers["Accept-Language"].ToString().Split(',').FirstOrDefault();
            }

            if (!string.IsNullOrEmpty(cultureQuery))
            {
                // Verifica se a cultura é válida
                var cultureInfo = GetValidCulture(cultureQuery);
                if (cultureInfo != null)
                {
                    CultureInfo.CurrentCulture = cultureInfo;
                    CultureInfo.CurrentUICulture = cultureInfo;
                }
            }

            await _next(context);
        }

        private CultureInfo GetValidCulture(string cultureQuery)
        {
            try
            {
                return CultureInfo.GetCultures(CultureTypes.SpecificCultures).FirstOrDefault(c => c.Name.Equals(cultureQuery, StringComparison.OrdinalIgnoreCase)) ?? new CultureInfo("pt-BR");
            }
            catch (CultureNotFoundException)
            {
                return new CultureInfo("pt-BR");
            }
        }
    }
}
