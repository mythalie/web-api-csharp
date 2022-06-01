using System;
using System.ComponentModel.DataAnnotations;

namespace PrimeiraWebAPI.Domain.DTO
{
    public class AlbumUpdateRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "O Artista é obrigatório!")]
        public string Artista { get; set; }
    }
}