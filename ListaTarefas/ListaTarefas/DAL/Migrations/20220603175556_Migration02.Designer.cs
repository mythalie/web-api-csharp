﻿// <auto-generated />
using ListaTarefas.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ListaTarefas.DAL.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20220603175556_Migration02")]
    partial class Migration02
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ListaTarefas.Domain.Entity.Responsavel", b =>
                {
                    b.Property<int>("IdResponsavel")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdResponsavel"), 1L, 1);

                    b.Property<int>("IdTarefa")
                        .HasColumnType("int");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.HasKey("IdResponsavel");

                    b.HasIndex("IdTarefa");

                    b.ToTable("Responsaveis");
                });

            modelBuilder.Entity("ListaTarefas.Domain.Entity.Tarefa", b =>
                {
                    b.Property<int>("IdTarefa")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdTarefa"), 1L, 1);

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<int>("Prioridade")
                        .HasColumnType("int");

                    b.Property<string>("Titulo")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.HasKey("IdTarefa");

                    b.ToTable("Tarefas");
                });

            modelBuilder.Entity("ListaTarefas.Domain.Entity.Responsavel", b =>
                {
                    b.HasOne("ListaTarefas.Domain.Entity.Tarefa", "Tarefa")
                        .WithMany("Responsavels")
                        .HasForeignKey("IdTarefa")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Tarefa");
                });

            modelBuilder.Entity("ListaTarefas.Domain.Entity.Tarefa", b =>
                {
                    b.Navigation("Responsavels");
                });
#pragma warning restore 612, 618
        }
    }
}
