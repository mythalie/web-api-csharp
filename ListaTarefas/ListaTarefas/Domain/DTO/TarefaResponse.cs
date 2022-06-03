using ListaTarefas.Domain.Entity;

namespace ListaTarefas.Domain.DTO
{
    public class TarefaResponse
    {
        public TarefaResponse(Tarefa tarefa)
        {
            IdTarefa = tarefa.IdTarefa;
            Titulo = tarefa.Titulo;
            Descricao = tarefa.Descricao;
            Prioridade = tarefa.Prioridade;

            if (tarefa.Responsaveis != null)
            {
                Responsaveis = new List<ResponsavelResponse>();
                Responsaveis.AddRange(tarefa.Responsaveis.Select(x => new ResponsavelResponse(x)));
            }
        }

        public int IdTarefa { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public bool Concluido { get; set; } = false;
        public int Prioridade { get; set; }
        public List<ResponsavelResponse> Responsaveis { get; set; }
    }
}
