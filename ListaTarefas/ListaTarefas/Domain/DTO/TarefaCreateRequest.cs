using System;
using System.ComponentModel.DataAnnotations;

namespace ListaTarefas.Domain.DTO
{
    public class TarefaCreateRequest
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "O Título é obrigatório!")]
        public string Titulo { get; set; }

        public string Descricao { get; set; }

        public int Prioridade { get; set; }
    }
}
