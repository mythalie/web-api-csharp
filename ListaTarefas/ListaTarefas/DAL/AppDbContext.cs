using Microsoft.EntityFrameworkCore;
using ListaTarefas.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ListaTarefas.DAL
{
        public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
       : base(options)
        {
        }
        public virtual DbSet<Tarefa> Tarefas { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tarefa>(entity =>
            {
                entity.Property(x => x.Titulo).IsUnicode(false);
                entity.Property(x => x.Descricao).IsUnicode(false);
            });

            modelBuilder.Entity<Responsavel>(entity =>
            {
                entity.Property(x => x.Nome).IsUnicode(false);
                entity.HasOne(x => x.Tarefa) // entidade virtual que indica relação
                .WithMany(y => y.Responsaveis) // coleção na entidade Tarefa
                .HasForeignKey(x => x.IdTarefa) //IdTarefa da entidade responsaveis
                .OnDelete(DeleteBehavior.Cascade); // cascade delete - quando um album for removido suas avaliações relacionadas também serão
            });
        }
    }
}

