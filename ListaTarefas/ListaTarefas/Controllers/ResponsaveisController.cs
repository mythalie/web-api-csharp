using ListaTarefas.Domain.DTO;
using ListaTarefas.Services;
using Microsoft.AspNetCore.Mvc;

namespace ListaTarefas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResponsaveisController : ControllerBase
    {
        private readonly ResponsaveisService responsavelService;
        public ResponsaveisController(ResponsaveisService responsavelService)
        {
            this.responsavelService = responsavelService;
        }
        [HttpPost]
        // FromBody para indicar de o corpo da requisição deve ser mapeado para o modelo
        public IActionResult Post([FromBody] ResponsavelCreateRequest postModel)
        {
            //Validação modelo de entrada
            if (ModelState.IsValid)
            {
                var retorno = responsavelService.CadastrarNovo(postModel);
                if (!retorno.Sucesso)
                    return BadRequest(retorno);
                else
                    return Ok(retorno.ObjetoRetorno);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
