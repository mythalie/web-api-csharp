using System;
using System.ComponentModel.DataAnnotations;

namespace CadastroAnimaisAdocao.Domain.DTO
{
    public class AnimalCreateRequest
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome obrigatório!")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Idade obrigatória!")]
        public int? Idade { get; set; }

        [Required(ErrorMessage = "Espécie obrigatória!")]
        public string Especie { get; set; }

        [DataType(DataType.Date)]
        public string DataNascimento { get; set; }

        [Range(1, 5, ErrorMessage = "Número inválido. Insira de 1 a 5")]
        public int NivelFofura { get; set; }

        [Range(1, 5, ErrorMessage = "Número inválido. Insira de 1 a 5")]
        public int NivelCarinho { get; set; }

        [EmailAddress(ErrorMessage = "E-mail em formato inválido")]
        public string EmailContato { get; set; }
    }
}
