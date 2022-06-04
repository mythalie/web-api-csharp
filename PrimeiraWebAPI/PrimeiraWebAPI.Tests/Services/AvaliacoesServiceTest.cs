using Microsoft.EntityFrameworkCore;
using PrimeiraWebAPI.DAL;
using PrimeiraWebAPI.Domain.DTO;
using PrimeiraWebAPI.Domain.Entity;
using PrimeiraWebAPI.Services;
using System;
using Xunit;
namespace PrimeiraWebAPI.Tests.Services
{
    public class AvaliacoesServiceTest : IDisposable
    {
        /// <summary>
        /// DbContext é a camada de acesso ao banco
        /// </summary>
        private readonly AppDbContext _dbContext;
        /// <summary>
        /// Service que iremos testar
        /// </summary>
        private readonly AvaliacoesService _service;
        /// <summary>
        /// Aqui preparamos os testes
        /// </summary>
        public AvaliacoesServiceTest()
        {
            // Criando banco em memória
            var options = new DbContextOptionsBuilder<AppDbContext>()
                // Guid.NewGuid().ToString(): Garantindo a criação de um banco novo
                //  a cada execução de teste, evitando a existência de dados não inseridos durante os testes
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            // Criamos as instâncias que vamos usar nos testes
            _dbContext = new AppDbContext(options);
            _service = new AvaliacoesService(_dbContext);
        }
        [Fact]
        public void Quando_PassadoAvaliacaoValida_DeveCadastrar_E_Retornar()
        {
            var album = AlbumStub();
            // Preparando entrada
            var request = new AvaliacaoCreateRequest()
            {
                Comentario = "Muito bom!",
                Nota = 1,
                IdAlbum = album.IdAlbum
            };
            // Executando
            var retorno = _service.CadastrarNovo(request);
            // Validando resultados
            Assert.Equal(retorno.ObjetoRetorno.Comentario, request.Comentario);
            Assert.Equal(retorno.ObjetoRetorno.IdAlbum, request.IdAlbum);
            Assert.Equal(retorno.ObjetoRetorno.Nota, request.Nota);
        }
        [Fact]
        public void Quando_PassadoAvaliacaoInvalida_Deve_RetornarErro()
        {
            // Preparando dados no banco
            //  Stub: Conjunto de dados que mockamos para usar em testes
            var album = AlbumStub();
            var mensagemEsperada = "A nota da avaliação deve ser um número entre 1 e 5";
            // Preparando entrada
            var request = new AvaliacaoCreateRequest()
            {
                Comentario = "Muito bom!",
                // Nota inválida para provocar o erro de validação
                Nota = 0,
                IdAlbum = album.IdAlbum
            };
            // Executando
            var retorno = _service.CadastrarNovo(request);
            // Validando resultados
            Assert.False(retorno.Sucesso);
            Assert.Equal(retorno.Mensagem, mensagemEsperada);
        }
        [Fact]
        public void Quando_PassadoAvaliacao_ComAlbumInexistente_Deve_RetornarErro()
        {
            // Preparando dados no banco
            //  Stub: Conjunto de dados que mockamos para usar em testes
            var album = AlbumStub();
            var mensagemEsperada = "Album não encontrado";

            // Preparando entrada
            var request = new AvaliacaoCreateRequest()
            {
                Comentario = "Muito bom!",
                Nota = 1,
                // usa id que foi cadastrado no banco + 1,
                // assim temos certeza que vamos consultar um id que não existe
                IdAlbum = album.IdAlbum + 1
            };
            // Executando
            var retorno = _service.CadastrarNovo(request);
            // Validando resultados
            Assert.False(retorno.Sucesso);
            Assert.Equal(retorno.Mensagem, mensagemEsperada);
        }
        /// <summary>
        /// Stub: Para cadastrar uma avaliação preciso que um album exista
        /// </summary>
        /// <returns>Dados que mockamos para usar em testes</returns>
        private Album AlbumStub()
        {
            // Dados para mock
            var album = new Album()
            {
                Nome = "Album Test",
                AnoLancamento = 1950,
                Artista = "Artista Test"
            };
            // Salvamos os dados no banco
            _dbContext.Add(album);
            _dbContext.SaveChanges();
            // Retornamos para usar nas validações
            return album;
        }
        public void Dispose()
        {
            _dbContext.Database.EnsureDeleted();
            GC.SuppressFinalize(this);
        }
    }
}