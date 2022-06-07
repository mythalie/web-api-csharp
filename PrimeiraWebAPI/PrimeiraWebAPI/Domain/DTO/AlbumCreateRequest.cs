using System;
using System.ComponentModel.DataAnnotations;

namespace PrimeiraWebAPI.Domain.DTO
{
    public class AlbumCreateRequest
    {
        [Required(AllowEmptyStrings = false)]
        public string Nome { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "O Artista é obrigatório!")]
        public string Artista { get; set; }
        [Required]
        public int? AnoLancamento { get; set; }
        public string AvaliacaoMedia { get; set; }
        //int? e Required????
        //Sem isso este erro nunca vai acontecer.
        //Com um int normal, o valor padrão vai sempre ser 0
        //e nunca vamos saber se é o valor passado ou o padrão
        //Required é um atributo que indica que este campo é obrigatório
    }
}