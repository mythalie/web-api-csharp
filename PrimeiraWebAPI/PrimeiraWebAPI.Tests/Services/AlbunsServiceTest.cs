using Microsoft.EntityFrameworkCore;
using PrimeiraWebAPI.DAL;
using PrimeiraWebAPI.Domain.DTO;
using PrimeiraWebAPI.Domain.Entity;
using PrimeiraWebAPI.Services;
using System.Collections.Generic;
using System;
using Xunit;
using System.Linq;

namespace PrimeiraWebAPI.Tests.Services
{
    public class AlbunsServiceTest : IDisposable
    {
        /// <summary>
        /// DbContext é a camada de acesso ao banco
        /// </summary>
        private readonly AppDbContext _dbContext;

        /// <summary>
        /// Service que iremos testar
        /// </summary>
        private readonly AlbunsService _service;

        /// <summary>
        /// Nota padrão.
        /// Se todas as notas forem 1, então a média deve ser 1.
        /// </summary>
        private readonly int _notaPadrao = 1;

        /// <summary>
        /// Aqui preparamos os testes
        /// </summary>
        public AlbunsServiceTest()
        {
            // Criando banco em memória
            var options = new DbContextOptionsBuilder<AppDbContext>()
                // Guid.NewGuid().ToString(): Garantindo a criação de um banco novo
                //  a cada execução de teste, evitando a existência de dados não inseridos durante os testes
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            // Criamos as instâncias que vamos usar nos testes
            _dbContext = new AppDbContext(options);
            _service = new AlbunsService(_dbContext);
        }

        [Fact]
        public void Quando_PassadoAlbumValido_DeveCadastrar_E_Retornar()
        {
            // Preparando entrada
            var request = new AlbumCreateRequest()
            {
                Nome = "Album Test",
                AnoLancamento = 1990,
                Artista = "Artista Test",
                AvaliacaoMedia = "5"
            };

            // Executando
            var retorno = _service.CadastrarNovo(request);

            // Validando resultados
            Assert.Equal(retorno.ObjetoRetorno.AnoLancamento, request.AnoLancamento);
            Assert.Equal(retorno.ObjetoRetorno.Artista, request.Artista);
            Assert.Equal(retorno.ObjetoRetorno.Nome, request.Nome);
            Assert.Equal(retorno.ObjetoRetorno.AvaliacaoMedia, request.AvaliacaoMedia);
        }

        [Fact]
        public void Quando_PassadoAlbumInvalido_Deve_RetornarErro()
        {
            var mensagemEsperada = "Somente é possível cadastrar albuns lançados entre 1950 e o ano atual";

            // Preparando entrada
            var request = new AlbumCreateRequest()
            {
                Nome = "Album Test",
                // Ano inválido para provocar erro de validação
                AnoLancamento = 1949,
                Artista = "Artista Test"
            };

            // Executando
            var retorno = _service.CadastrarNovo(request);

            // Validando resultados
            Assert.False(retorno.Sucesso);
            Assert.Equal(retorno.Mensagem, mensagemEsperada);
        }

        [Fact]
        public void Quando_ChamadoListarTodos_Deve_RetornarTodos()
        {
            // Preparando dados no banco
            //  Stub: Conjunto de dados que mockamos para usar em testes
            var lista = ListaAlbunsStub();

            // Executa e cria objeto do tipo lista a partir da execução
            var retorno = new List<AlbumResponse>(_service.ListarTodos());

            // Validando resultados
            Assert.Equal(retorno.Count, lista.Count);
        }

        [Fact]
        public void Quando_ChamadoPesquisaPorId_Com_IdExistente_Deve_RetornarAlbum()
        {
            // Preparando dados no banco
            //  Stub: Conjunto de dados que mockamos para usar em testes
            var lista = ListaAlbunsStub();

            // Executa com id que foi cadastrado no banco
            var retorno = _service.PesquisarPorId(lista[0].IdAlbum);

            // Validando resultados
            Assert.Equal(retorno.ObjetoRetorno.IdAlbum, lista[0].IdAlbum);
            Assert.Equal(retorno.ObjetoRetorno.Nome, lista[0].Nome);
            Assert.Equal(retorno.ObjetoRetorno.Artista, lista[0].Artista);
            Assert.Equal(retorno.ObjetoRetorno.AnoLancamento, lista[0].AnoLancamento);
            // Se todas as notas forem 1, então a média deve ser 1 Mais a formatação: 1,00.
            Assert.Equal(retorno.ObjetoRetorno.AvaliacaoMedia, _notaPadrao.ToString("F2"));
        }

        [Fact]
        public void Quando_ChamadoPesquisaPorId_Com_IdNaoExistente_Deve_RetornarErro()
        {
            // Preparando dados no banco
            //  Stub: Conjunto de dados que mockamos para usar em testes
            var lista = ListaAlbunsStub();
            var mensagemEsperada = "Não encontrado!";

            // Executa com id que foi cadastrado no banco + 1,
            // assim temos certeza que vamos consultar um id que não existe
            var retorno = _service.PesquisarPorId(lista.Count + 1);

            // Validando resultados
            Assert.False(retorno.Sucesso);
            Assert.Equal(retorno.Mensagem, mensagemEsperada);
        }

        [Fact]
        public void Quando_ChamadoPesquisaPorNome_Com_NomeExistente_Deve_RetornarAlbum()
        {
            // Preparando dados no banco
            //  Stub: Conjunto de dados que mockamos para usar em testes
            var lista = ListaAlbunsStub();

            // Executa com nome que foi cadastrado no banco
            var retorno = _service.PesquisarPorNome(lista[0].Nome);

            // Validando resultados
            Assert.Equal(retorno.ObjetoRetorno.IdAlbum, lista[0].IdAlbum);
            Assert.Equal(retorno.ObjetoRetorno.Nome, lista[0].Nome);
            Assert.Equal(retorno.ObjetoRetorno.Artista, lista[0].Artista);
            Assert.Equal(retorno.ObjetoRetorno.AnoLancamento, lista[0].AnoLancamento);
            Assert.Equal(retorno.ObjetoRetorno.AvaliacaoMedia, _notaPadrao.ToString("F2"));
        }

        [Fact]
        public void Quando_ChamadoPesquisaPorNome_Com_NomeNaoExistente_Deve_RetornarErro()
        {
            // Preparando dados no banco
            //  Stub: Conjunto de dados que mockamos para usar em testes
            ListaAlbunsStub();
            var mensagemEsperada = "Não encontrado!";

            // Executa com nome que não foi cadastrado no banco
            var retorno = _service.PesquisarPorNome("Nome inexistente");

            // Validando resultados
            Assert.False(retorno.Sucesso);
            Assert.Equal(retorno.Mensagem, mensagemEsperada);
        }

        [Fact]
        public void Quando_ChamadoEditar_Com_IdExistente_Deve_RetornarAlbumAtualizado()
        {
            // Preparando dados no banco
            //  Stub: Conjunto de dados que mockamos para usar em testes
            var lista = ListaAlbunsStub();

            // Preparando entrada
            var request = new AlbumUpdateRequest()
            {
                Artista = "Novo nome de artista"
            };

            // Executa com id que foi cadastrado no banco
            var retorno = _service.Editar(lista[0].IdAlbum, request);

            // Validando resultados
            Assert.Equal(retorno.ObjetoRetorno.Artista, lista[0].Artista);
        }

        [Fact]
        public void Quando_ChamadoEditar_Com_IdNaoExistente_Deve_RetornarErro()
        {
            // Preparando dados no banco
            //  Stub: Conjunto de dados que mockamos para usar em testes
            var lista = ListaAlbunsStub();
            var mensagemEsperada = "Album não encontrado!";

            // Preparando entrada
            var request = new AlbumUpdateRequest();

            // Executa com id que foi cadastrado no banco + 1,
            // assim temos certeza que vamos consultar um id que não existe
            var retorno = _service.Editar(lista.Count + 1, request);

            // Validando resultados
            Assert.False(retorno.Sucesso);
            Assert.Equal(retorno.Mensagem, mensagemEsperada);
        }

        [Fact]
        public void Quando_ChamadoDeletar_Com_IdExistente_Deve_RetornarAlbumAtualizado()
        {
            // Preparando dados no banco
            //  Stub: Conjunto de dados que mockamos para usar em testes
            var lista = ListaAlbunsStub();

            // Executa com id que foi cadastrado no banco
            var retorno = _service.Deletar(lista[0].IdAlbum);

            // Validando resultados
            Assert.True(retorno.ObjetoRetorno);
            // Verifica se existe um álbum a menos na base
            Assert.Equal(_dbContext.Albuns.Count(), lista.Count - 1);
        }

        [Fact]
        public void Quando_ChamadoDeletar_Com_IdNaoExistente_Deve_RetornarErro()
        {
            // Preparando dados no banco
            //  Stub: Conjunto de dados que mockamos para usar em testes
            var lista = ListaAlbunsStub();
            var mensagemEsperada = "Album não encontrado!";

            // Executa com id que foi cadastrado no banco + 1,
            // assim temos certeza que vamos consultar um id que não existe
            var retorno = _service.Deletar(lista.Count + 1);

            // Validando resultados
            Assert.False(retorno.Sucesso);
            Assert.Equal(retorno.Mensagem, mensagemEsperada);
            // Verifica se existe o mesmo número de álbuns na base
            Assert.Equal(_dbContext.Albuns.Count(), lista.Count);
        }

        /// <summary>
        /// Stub
        /// </summary>
        /// <returns>Conjunto de dados que mockamos para usar em testes</returns>
        private List<Album> ListaAlbunsStub()
        {
            // Dados para mock
            var lista = new List<Album>()
            {
                new Album()
                {
                    Nome = "Album Test 1",
                    AnoLancamento = 1990,
                    Artista = "Artista Test 1",
                    Avaliacoes = ListaAvaliacoesStub(),
                    AvaliacaoMedia = "5"
                },
                new Album()
                {
                    Nome = "Album Test 2",
                    AnoLancamento = 1991,
                    Artista = "Artista Test 2",
                    Avaliacoes = ListaAvaliacoesStub(),
                    AvaliacaoMedia = "5"
                }
            };

            // Salvamos os dados no banco
            _dbContext.AddRange(lista);
            _dbContext.SaveChanges();

            // Retornamos para usar nas validações
            return lista;
        }

        /// <summary>
        /// Stub Avaliacoes
        /// </summary>
        /// <returns>Conjunto de dados que mockamos para usar em testes</returns>
        private List<Avaliacao> ListaAvaliacoesStub()
        {
            return new List<Avaliacao>()
            {
                new Avaliacao()
                {
                    Comentario = "Muito bom!",
                    // Se todas as notas forem 1, então a média deve ser 1.
                    Nota = _notaPadrao,
                },
                new Avaliacao()
                {
                    Comentario = "Muito ruim!",
                    // Se todas as notas forem 1, então a média deve ser 1.
                    Nota = _notaPadrao,
                }
            };
        }

        /// <summary>
        /// Método que é executado quando os testes são encerrados.
        /// O XUnit chama o método Dispose definido na interface IDisposable.
        /// </summary>
        public void Dispose()
        {
            // Garante que o banco usado nos testes foi deletado
            _dbContext.Database.EnsureDeleted();
            // Informa pro Garbage Collector que o objeto já foi limpo. Leia mais:
            // - https://docs.microsoft.com/dotnet/fundamentals/code-analysis/quality-rules/ca1816
            // - https://stackoverflow.com/a/151244/7467989
            GC.SuppressFinalize(this);
        }
    }
}