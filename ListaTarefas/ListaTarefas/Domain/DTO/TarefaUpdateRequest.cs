using System.ComponentModel.DataAnnotations;

namespace ListaTarefas.Domain.DTO
{
    public class TarefaUpdateRequest
    {
        public bool Concluido { get; set; } = false;
        public int Prioridade { get; set; }
    }
}
