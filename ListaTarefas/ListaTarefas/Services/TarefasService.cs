using ListaTarefas.DAL;
using ListaTarefas.Domain.DTO;
using ListaTarefas.Domain.Entity;
using ListaTarefas.Services.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ListaTarefas.Services
{
    public class TarefasService
    {
        private readonly AppDbContext _dbContext;
        public TarefasService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ServiceResponse<Tarefa> CadastrarNovo(TarefaCreateRequest model)
        {
            if (model.Prioridade < 1 || model.Prioridade > 5)
            {
                return new ServiceResponse<Tarefa>("Digite um número entre 1 e 5");
            }

            if (model.Prioridade == 0)
            {
                model.Prioridade = 5;
            }

            var novaTarefa = new Tarefa()
            {
                Titulo = model.Titulo,
                Descricao = model.Descricao,
                Prioridade = model.Prioridade
            };

            _dbContext.Add(novaTarefa);
            _dbContext.SaveChanges();

            return new ServiceResponse<Tarefa>(novaTarefa);
        }

        public IEnumerable<TarefaResponse> ListarTodos()
        {
            // select  * from tarefas x
            // left join responsaveis a on a.idTarefa = x.idTarefa

            var retornoDoBanco = _dbContext.Tarefas.Include(x => x.Responsaveis).ToList();

            // Conveter para TarefaResponse
            IEnumerable<TarefaResponse> lista = retornoDoBanco.Select(x => new TarefaResponse(x));

            return lista;
        }

        public ServiceResponse<Tarefa> PesquisarPorId(int id)
        {
            // Lambda Expression / Expressões lambda
            // Operação em conjunto de dados
            // select top 1 * from tarefas x where x.IdTarefa == id 
            var resultado = _dbContext.Tarefas.Include(x => x.Responsaveis).FirstOrDefault(x => x.IdTarefa == id);
            if (resultado == null)
                return new ServiceResponse<Tarefa>("Não encontrado!");
            else
                return new ServiceResponse<Tarefa>(resultado);
        }

        public ServiceResponse<Tarefa> Editar(int id, TarefaUpdateRequest model)
        {
            // select top 1 * from tarefas x where x.IdTarefa == id 
            var resultado = _dbContext.Tarefas.FirstOrDefault(x => x.IdTarefa == id);

            if (resultado == null)
                return new ServiceResponse<Tarefa>("Tarefa não encontrada!");

            //tudo certo, só atualizar
            resultado.Concluido = model.Concluido;
            resultado.Prioridade = model.Prioridade;

            _dbContext.Tarefas.Add(resultado).State = EntityState.Modified;
            _dbContext.SaveChanges();

            return new ServiceResponse<Tarefa>(resultado);
        }

        public ServiceResponse<bool> Deletar(int id)
        {
            // select top 1 * from tarefas x where x.IdTarefa == id 
            var resultado = _dbContext.Tarefas.FirstOrDefault(x => x.IdTarefa == id);

            if (resultado == null)
                return new ServiceResponse<bool>("Tarefa não encontrada!");

            //tudo certo, só atualizar
            _dbContext.Tarefas.Remove(resultado);
            _dbContext.SaveChanges();


            return new ServiceResponse<bool>(true);
        }
    }
}
