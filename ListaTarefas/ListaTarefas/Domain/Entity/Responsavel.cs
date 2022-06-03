using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ListaTarefas.Domain.Entity
{
    [Table("Responsaveis")]
    public class Responsavel
    {
        [Key]
        public int IdResponsavel { get; set; }

        [Required]
        public int IdTarefa { get; set; }

        [Required]
        [StringLength(255)]
        public string Nome { get; set; }

        // virtual para indicar o relacionamento
        public virtual Tarefa Tarefa { get; set; }
    }
}
