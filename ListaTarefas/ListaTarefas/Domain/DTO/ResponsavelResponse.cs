using ListaTarefas.Domain.Entity;

namespace ListaTarefas.Domain.DTO
{
    public class ResponsavelResponse
    {
        public ResponsavelResponse(Responsavel responsavel)
        { 
            IdResponsavel = responsavel.IdResponsavel;
            IdTarefa = responsavel.IdTarefa;
            Nome = responsavel.Nome;
        }
        public int IdResponsavel { get; set; }
        public int IdTarefa { get; set; }
        public string Nome { get; set; }
    }
}
