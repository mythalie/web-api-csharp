using System.ComponentModel.DataAnnotations;

namespace ListaTarefas.Domain.DTO
{
    public class ResponsavelCreateRequest
    {
        [Required]
        public int IdTarefa { get; set; }
        public string Nome { get; set; }
    }
}
