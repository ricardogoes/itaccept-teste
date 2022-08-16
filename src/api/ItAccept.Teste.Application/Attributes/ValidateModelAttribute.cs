using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ItAccept.Teste.Domain.Models;

namespace ItAccept.Teste.Application.Attributes
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var dicErros = new Dictionary<string, List<string>>();
                foreach (var key in context.ModelState.Keys)
                {
                    var erros = new List<string>();
                    foreach (var erro in context.ModelState[key].Errors)
                    {
                        erros.Add(erro.ErrorMessage);
                    }
                    dicErros.Add(key, erros);
                }

                context.Result = new BadRequestObjectResult(new ApiResponse(ApiResponseState.Failed, "Entidade inválida", dicErros));
            }
        }
    }
}
