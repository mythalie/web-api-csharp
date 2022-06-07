using Microsoft.EntityFrameworkCore;
using ListaTarefas.DAL;
using ListaTarefas.Domain.DTO;
using ListaTarefas.Domain.Entity;
using ListaTarefas.Services;

namespace ListaTarefas.Tests.Services
{
    public class TarefasServiceTest : IDisposable
    {
        /// <summary>
        /// DbContext é a camada de acesso ao banco
        /// </summary>
        private readonly AppDbContext _dbContext;

        /// <summary>
        /// Service que iremos testar
        /// </summary>
        private readonly TarefasService _service;

        /// <summary>
        /// Aqui preparamos os testes
        /// </summary>
        public TarefasServiceTest()
        {
            // Criando banco em memória
            var options = new DbContextOptionsBuilder<AppDbContext>()
                // Guid.NewGuid().ToString(): Garantindo a criação de um banco novo
                //  a cada execução de teste, evitando a existência de dados não inseridos durante os testes
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            // Criamos as instâncias que vamos usar nos testes
            _dbContext = new AppDbContext(options);
            _service = new TarefasService(_dbContext);
        }

        [Fact]
        public void Quando_PassadoTarefaValida_DeveCadastrar_E_Retornar()
        {
            // Preparando entrada
            var request = new TarefaCreateRequest()
            {
                Titulo = "Tarefa Test",
                Descricao = "Test",
                Prioridade = 5
            };

            // Executando
            var retorno = _service.CadastrarNovo(request);

            // Validando resultados
            Assert.Equal(retorno.ObjetoRetorno.Prioridade, request.Prioridade);
            Assert.Equal(retorno.ObjetoRetorno.Descricao, request.Descricao);
            Assert.Equal(retorno.ObjetoRetorno.Titulo, request.Titulo);
        }

        [Fact]
        public void Quando_PassadoTarefaInvalida_Deve_RetornarErro()
        {
            var mensagemEsperada = "Digite um número entre 1 e 5";

            // Preparando entrada
            var request = new TarefaCreateRequest()
            {
                // Titulo inválido para provocar erro de validação
                Titulo = "Tarefa Test",
                Descricao = "Test",
                Prioridade = 6
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
            var lista = ListaTarefasStub();

            // Executa e cria objeto do tipo lista a partir da execução
            var retorno = new List<TarefaResponse>(_service.ListarTodos());

            // Validando resultados
            Assert.Equal(retorno.Count, lista.Count);
        }

        [Fact]
        public void Quando_ChamadoPesquisaPorId_Com_IdExistente_Deve_RetornarTarefa()
        {
            // Preparando dados no banco
            //  Stub: Conjunto de dados que mockamos para usar em testes
            var lista = ListaTarefasStub();

            // Executa com id que foi cadastrado no banco
            var retorno = _service.PesquisarPorId(lista[0].IdTarefa);

            // Validando resultados
            Assert.Equal(retorno.ObjetoRetorno.IdTarefa, lista[0].IdTarefa);
            Assert.Equal(retorno.ObjetoRetorno.Titulo, lista[0].Titulo);
            Assert.Equal(retorno.ObjetoRetorno.Descricao, lista[0].Descricao);
            Assert.Equal(retorno.ObjetoRetorno.Prioridade, lista[0].Prioridade);
        }

        [Fact]
        public void Quando_ChamadoPesquisaPorId_Com_IdNaoExistente_Deve_RetornarErro()
        {
            // Preparando dados no banco
            //  Stub: Conjunto de dados que mockamos para usar em testes
            var lista = ListaTarefasStub();
            var mensagemEsperada = "Não encontrado!";

            // Executa com id que foi cadastrado no banco + 1,
            // assim temos certeza que vamos consultar um id que não existe
            var retorno = _service.PesquisarPorId(lista.Count + 1);

            // Validando resultados
            Assert.False(retorno.Sucesso);
            Assert.Equal(retorno.Mensagem, mensagemEsperada);
        }

        [Fact]
        public void Quando_ChamadoDeletar_Com_IdExistente_Deve_RetornarTarefaAtualizada()
        {
            // Preparando dados no banco
            //  Stub: Conjunto de dados que mockamos para usar em testes
            var lista = ListaTarefasStub();

            // Executa com id que foi cadastrado no banco
            var retorno = _service.Deletar(lista[0].IdTarefa);

            // Validando resultados
            Assert.True(retorno.ObjetoRetorno);
            // Verifica se existe um álbum a menos na base
            Assert.Equal(_dbContext.Tarefas.Count(), lista.Count - 1);
        }

        [Fact]
        public void Quando_ChamadoDeletar_Com_IdNaoExistente_Deve_RetornarErro()
        {
            // Preparando dados no banco
            //  Stub: Conjunto de dados que mockamos para usar em testes
            var lista = ListaTarefasStub();
            var mensagemEsperada = "Tarefa não encontrada!";

            // Executa com id que foi cadastrado no banco + 1,
            // assim temos certeza que vamos consultar um id que não existe
            var retorno = _service.Deletar(lista.Count + 1);

            // Validando resultados
            Assert.False(retorno.Sucesso);
            Assert.Equal(retorno.Mensagem, mensagemEsperada);
            // Verifica se existe o mesmo número de álbuns na base
            Assert.Equal(_dbContext.Tarefas.Count(), lista.Count);
        }

        /// <summary>
        /// Stub
        /// </summary>
        /// <returns>Conjunto de dados que mockamos para usar em testes</returns>
        private List<Tarefa> ListaTarefasStub()
        {
            // Dados para mock
            var lista = new List<Tarefa>()
            {
                new Tarefa()
                {
                    Titulo = "Tarefa Test 1",
                    Descricao = "Test 1",
                    Prioridade = 1,
                    Responsaveis = ListaResponsaveisStub()
                },
                new Tarefa()
                {
                    Titulo = "Tarefa Test 2",
                    Descricao = "Test 2",
                    Prioridade = 2,
                    Responsaveis = ListaResponsaveisStub()
                }
            };

            // Salvamos os dados no banco
            _dbContext.AddRange(lista);
            _dbContext.SaveChanges();

            // Retornamos para usar nas validações
            return lista;
        }

        /// <summary>
        /// Stub Responsaveis
        /// </summary>
        /// <returns>Conjunto de dados que mockamos para usar em testes</returns>
        private List<Responsavel> ListaResponsaveisStub()
        {
            return new List<Responsavel>()
            {
                new Responsavel()
                {
                    Nome = "Maria",
                },
                new Responsavel()
                {
                    Nome = "João",
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