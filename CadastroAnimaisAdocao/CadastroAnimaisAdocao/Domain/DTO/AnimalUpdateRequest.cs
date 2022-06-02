using System.ComponentModel.DataAnnotations;

namespace CadastroAnimaisAdocao.Domain.DTO
{
    public class AnimalUpdateRequest
    {
        [Required(ErrorMessage = "Nome obrigatório!")]
        public string Nome { get; set; }
    }
}
