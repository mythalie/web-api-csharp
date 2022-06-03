using ListaTarefas.DAL;
using ListaTarefas.Domain.DTO;
using ListaTarefas.Domain.Entity;
using ListaTarefas.Services.Base;

namespace ListaTarefas.Services
{
    public class ResponsaveisService
    {
        private readonly AppDbContext _dbContext;
        public ResponsaveisService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public ServiceResponse<ResponsavelResponse> CadastrarNovo(ResponsavelCreateRequest model)
        {
            // validação de integridade
            var resultado = _dbContext.Tarefas.FirstOrDefault(x => x.IdTarefa == model.IdTarefa);
            if (resultado == null)
                return new ServiceResponse<ResponsavelResponse>("Tarefa não encontrada");
            //Regra
            if (model.Nome == null)
            {
                return new ServiceResponse<ResponsavelResponse>("Digite um nome");
            }
            //tudo certo, só cadastrar
            var nova = new Responsavel()
            {
                Nome = model.Nome,
                IdTarefa = model.IdTarefa
            };
            _dbContext.Add(nova);
            _dbContext.SaveChanges();
            var retorno = new ResponsavelResponse(nova);
            return new ServiceResponse<ResponsavelResponse>(retorno);
        }
    }
}
