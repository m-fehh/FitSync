using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net.NetworkInformation;

namespace WebApi.Core.Controllers
{
    public class ControllerBase : Controller
    {
        public IActionResult ChangeLanguage(string culture)
        {
            // Verifica se a cultura é válida
            var cultureInfo = GetValidCulture(culture);
            if (cultureInfo == null)
            {
                return BadRequest("Cultura inválida.");
            }

            // Define a cultura no cookie
            Response.Cookies.Append(
                "Culture",
                cultureInfo.Name,
                new CookieOptions { HttpOnly = true, Secure = true, SameSite = SameSiteMode.None }
            );

            // Retorna uma resposta bem-sucedida
            return Ok();
        }

        private CultureInfo GetValidCulture(string cultureQuery)
        {
            try
            {
                return CultureInfo.GetCultures(CultureTypes.SpecificCultures).FirstOrDefault(c => c.Name.Equals(cultureQuery, StringComparison.OrdinalIgnoreCase));
            }
            catch (CultureNotFoundException)
            {
                return null;
            }
        }

        //    $.ajax({
        //    url: '@Url.Action("ChangeLanguage", "ControllerBase")', // Nome correto do controlador
        //type: 'POST',
        //data: { culture: selectedLanguage },
        //success: function(response) {
        //            location.reload(); // Recarrega a página ou atualiza os elementos conforme necessário
        //        },
        //error: function(xhr, status, error) {
        //            console.error('Error changing language:', error);
        //            showToast('toast-error', 'Erro ao alterar a linguagem.'); // Exiba um toast de erro
        //        }
        //    });

    }
}
