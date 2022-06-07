using Microsoft.EntityFrameworkCore;
using PrimeiraWebAPI.DAL;
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
        private readonly AppDbContext _dbContext;
        public AlbunsService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
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
                Nome = model.Nome,
                Artista = model.Artista,
                AnoLancamento = model.AnoLancamento.Value,
                AvaliacaoMedia = model.AvaliacaoMedia,
            };

            _dbContext.Add(novoAlbum);
            _dbContext.SaveChanges();

            return new ServiceResponse<Album>(novoAlbum);
        }

        public IEnumerable<AlbumResponse> ListarTodos()
        {
            // select  * from albuns x
            // left join avaliacoes a on a.idAlbum = x.idAlbum

            var retornoDoBanco = _dbContext.Albuns.Include(x => x.Avaliacoes).ToList();

            // Conveter para AlbumResponse
            IEnumerable<AlbumResponse> lista = retornoDoBanco.Select(x => new AlbumResponse(x));

            return lista;
        }


        public ServiceResponse<AlbumResponse> PesquisarPorId(int id)
        {
            // Lambda Expression / Expressões lambda
            // Operação em conjunto de dados
            // select top 1 * from albuns x
            // left join avaliacoes a on a.idAlbum = x.idAlbum
            // where x.IdAlbum == id 
            var resultado = _dbContext.Albuns.Include(x => x.Avaliacoes).FirstOrDefault(x => x.IdAlbum == id);
            if (resultado == null)
                return new ServiceResponse<AlbumResponse>("Não encontrado!");
            else
                return new ServiceResponse<AlbumResponse>(new AlbumResponse(resultado));

        }

        public ServiceResponse<AlbumResponse> PesquisarPorNome(string nome)
        {
            // Lambda Expression / Expressões lambda
            // Operação em conjunto de dados
            // select top 1 * from albuns x
            // left join avaliacoes a on a.idAlbum = x.idAlbum
            // where x.nome == no,e 
            var resultado = _dbContext.Albuns.Include(x => x.Avaliacoes).FirstOrDefault(x => x.Nome == nome);
            if (resultado == null)
                return new ServiceResponse<AlbumResponse>("Não encontrado!");
            else
                return new ServiceResponse<AlbumResponse>(new AlbumResponse(resultado));
        }

        public ServiceResponse<Album> Editar(int id, AlbumUpdateRequest model)
        {
            // select top 1 * from albuns x where x.IdAlbum == id 
            var resultado = _dbContext.Albuns.FirstOrDefault(x => x.IdAlbum == id);

            if (resultado == null)
                return new ServiceResponse<Album>("Album não encontrado!");

            //tudo certo, só atualizar
            resultado.Artista = model.Artista;
            _dbContext.Albuns.Add(resultado).State = EntityState.Modified;
            _dbContext.SaveChanges();

            return new ServiceResponse<Album>(resultado);
        }

        public ServiceResponse<bool> Deletar(int id)
        {
            // select top 1 * from albuns x where x.IdAlbum == id 
            var resultado = _dbContext.Albuns.FirstOrDefault(x => x.IdAlbum == id);

            if (resultado == null)
                return new ServiceResponse<bool>("Album não encontrado!");

            //tudo certo, só atualizar
            _dbContext.Albuns.Remove(resultado);
            _dbContext.SaveChanges();

            return new ServiceResponse<bool>(true);
        }

    }
}