using ListaTarefas.Domain.DTO;
using ListaTarefas.Domain.Entity;
using ListaTarefas.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ListaTarefas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarefasController : ControllerBase
    {
        private readonly TarefasService tarefasService;

        public TarefasController(TarefasService tarefasService)
        {
            this.tarefasService = tarefasService;
        }

        [HttpGet]
        public IEnumerable<TarefaResponse> Get()
        {
            return tarefasService.ListarTodos();
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var retorno = tarefasService.PesquisarPorId(id);

            if (retorno.Sucesso)
            {
                return Ok(retorno.ObjetoRetorno);
            }
            else
            {
                return NotFound(retorno.Mensagem);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] TarefaCreateRequest postModel)
        {
            if (ModelState.IsValid)
            {
                var retorno = tarefasService.CadastrarNovo(postModel);
                if (!retorno.Sucesso)
                    return BadRequest(retorno.Mensagem);
                else
                    return Ok(retorno);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] TarefaUpdateRequest putModel)
        {
            if (ModelState.IsValid)
            {
                var retorno = tarefasService.Editar(id, putModel);
                if (!retorno.Sucesso)
                    return BadRequest(retorno.Mensagem);
                return Ok(retorno.ObjetoRetorno);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            //Validação modelo de entrada
            var retorno = tarefasService.Deletar(id);
            if (!retorno.Sucesso)
                return BadRequest(retorno.Mensagem);
            return Ok();
        }
    }
}