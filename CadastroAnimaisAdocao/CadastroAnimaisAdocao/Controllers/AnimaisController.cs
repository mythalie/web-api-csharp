using CadastroAnimaisAdocao.Domain.DTO;
using CadastroAnimaisAdocao.Domain.Entity;
using CadastroAnimaisAdocao.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CadastroAnimaisAdocao.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnimaisController : Controller
    {

        private readonly AnimaisService animaisService;

        public AnimaisController(AnimaisService animaisService)
        {
            this.animaisService = animaisService;
        }

        [HttpGet]
        public IEnumerable<Animal> Get()
        {
            return animaisService.ListarTodos();
        }

        [HttpGet("id")]
        public IActionResult GetById(int id)
        {
            var retorno = animaisService.PesquisarPorId(id);

            if (retorno.Sucesso)
            {
                return Ok(retorno.ObjetoRetorno);
            }
            else
            {
                return NotFound(retorno.Mensagem);
            }
        }

        [HttpGet("nome/nome")]
        public IActionResult GetByNome(string nome) 
        {
            var retorno = animaisService.PesquisarPorNome(nome);

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
        public IActionResult Post([FromBody] AnimalCreateRequest postModel)
        {
            if (ModelState.IsValid)
            {
                var retorno = animaisService.CadastrarNovo(postModel);
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
        public IActionResult Put(int id, [FromBody] AnimalUpdateRequest putModel)
        {
            if (ModelState.IsValid)
            {
                var retorno = animaisService.Editar(id, putModel);
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
            var retorno = animaisService.Deletar(id);
            if (!retorno.Sucesso)
                return BadRequest(retorno.Mensagem);
            return Ok();
        }
    }
}
