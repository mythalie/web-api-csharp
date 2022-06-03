
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ListaTarefas.Domain.Entity
{
    [Table("Tarefas")]
    public class Tarefa
    {
        [Key]
        public int IdTarefa { get; set; }

        [Required]
        [StringLength(255)]
        public string Titulo { get; set; }

        [Required]
        [StringLength(255)]
        public string Descricao { get; set; }

        [Required]
        public bool Concluido { get; set; } = false;

        [Required]
        public int Prioridade { get; set; }

        public virtual ICollection<Responsavel> Responsaveis { get; set; }
    }
}
