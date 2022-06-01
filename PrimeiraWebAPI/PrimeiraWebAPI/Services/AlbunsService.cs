using PrimeiraWebAPI.Domain.DTO;
using PrimeiraWebAPI.Domain.Entity;
using PrimeiraWebAPI.Services.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PrimeiraWebAPI.Services
{
    public class AlbunsService
    {
        private static List<Album> listaDeAlbuns; // fake, só para aprendizado
        private static int proximoId = 1;
        //iniciar a lista de albuns no construtor da classe
        public AlbunsService()
        {

            if (listaDeAlbuns == null)
            {
                listaDeAlbuns = new List<Album>();
                listaDeAlbuns.Add(new Album()
                {
                    IdAlbum = proximoId++,
                    Nome = "Da Lama ao Caos",
                    Artista = "Chico Science & Nação Zumbi",
                    AnoLancamento = 1994
                });
                listaDeAlbuns.Add(new Album()
                {
                    IdAlbum = proximoId++,
                    Nome = "Fragile",
                    Artista = "Yes",
                    AnoLancamento = 1971
                });
                listaDeAlbuns.Add(new Album()
                {
                    IdAlbum = proximoId++,
                    Nome = "This Is Acting",
                    Artista = "Dia",
                    AnoLancamento = 2016
                });
                listaDeAlbuns.Add(new Album()
                {
                    IdAlbum = proximoId++,
                    Nome = "Clube da Esquina",
                    Artista = "Milton Nascimento e Lô Borges",
                    AnoLancamento = 1972
                });
            }
        }

        public ServiceResponse<Album> CadastrarNovo(AlbumCreateRequest model)
        {
            //Regra
            // Somente albums lançados entre 1950 e o ano atual.
            if (!model.AnoLancamento.HasValue || model.AnoLancamento < 1950 || model.AnoLancamento > DateTime.Today.Year)
            {
                return new ServiceResponse<Album>("Somente é possível cadastrar albuns lançados entre 1950 e o ano atual");
            }

            //tudo certo, só cadastrar
            var novoAlbum = new Album()
            {
                IdAlbum = proximoId++,
                Nome = model.Nome,
                Artista = model.Artista,
                AnoLancamento = model.AnoLancamento.Value
            };

            listaDeAlbuns.Add(novoAlbum);

            return new ServiceResponse<Album>(novoAlbum);
        }

        public List<Album> ListarTodos()
        {
            return listaDeAlbuns;
        }

        public ServiceResponse<Album> PesquisarPorId(int id)
        {
            // Lambda Expression / Expressões lambda
            // Operação em conjunto de dados
            // select top 1 * from albuns x where x.IdAlbum == id 
            var resultado = listaDeAlbuns.Where(x => x.IdAlbum == id).FirstOrDefault();
            if (resultado == null)
                return new ServiceResponse<Album>("Não encontrado!");
            else
                return new ServiceResponse<Album>(resultado);

        }

        public ServiceResponse<Album> PesquisarPorNome(string nome)
        {
            // Lambda Expression / Expressões lambda
            // Operação em conjunto de dados
            // select top 1 * from albuns x where x.IdAlbum == id 
            var resultado = listaDeAlbuns.Where(x => x.Nome == nome).FirstOrDefault();
            if (resultado == null)
                return new ServiceResponse<Album>("Não encontrado!");
            else
                return new ServiceResponse<Album>(resultado);
        }

        public ServiceResponse<Album> Editar(int id, AlbumUpdateRequest model)
        {
            // select top 1 * from albuns x where x.IdAlbum == id 
            var resultado = listaDeAlbuns.Where(x => x.IdAlbum == id).FirstOrDefault();

            if (resultado == null)
                return new ServiceResponse<Album>("Album não encontrado!");

            //tudo certo, só atualizar
            resultado.Artista = model.Artista;

            return new ServiceResponse<Album>(resultado);
        }

        public ServiceResponse<bool> Deletar(int id)
        {
            // select top 1 * from albuns x where x.IdAlbum == id 
            var resultado = listaDeAlbuns.Where(x => x.IdAlbum == id).FirstOrDefault();

            if (resultado == null)
                return new ServiceResponse<bool>("Album não encontrado!");

            //tudo certo, só atualizar
            listaDeAlbuns.Remove(resultado);

            return new ServiceResponse<bool>(true);
        }

    }
}