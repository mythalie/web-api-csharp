using CadastroAnimaisAdocao.Domain.DTO;
using CadastroAnimaisAdocao.Domain.Entity;
using CadastroAnimaisAdocao.Services.Base;

namespace CadastroAnimaisAdocao.Services
{
    public class AnimaisService
    {
        private static List<Animal> listaDeAnimais;
        private static int proximoId = 1;

        public AnimaisService()
        {

            if (listaDeAnimais == null)
            {
                listaDeAnimais = new List<Animal>();
                listaDeAnimais.Add(new Animal()
                {
                    Id = proximoId++,
                    Nome = "Leona",
                    Idade = 5,
                    Especie = "Gato",
                    DataNascimento = "01/01/2015",
                    NivelFofura = 5,
                    NivelCarinho = 5,
                    EmailContato = "mythalie@iteris.com.br"
                });
            }
        }

        public ServiceResponse<Animal> CadastrarNovo(AnimalCreateRequest model)
        {

            if (model.Idade == 0)
                return new ServiceResponse<Animal>("Digite a idade");

            if (model.Especie != "Cachorro" && model.Especie != "Gato" && model.Especie != "Coelho" && model.Especie != "Capivara")
                return new ServiceResponse<Animal>("Espécie inválida");

            if (model.NivelFofura < 1 || model.NivelFofura > 5)
                return new ServiceResponse<Animal>("Nível de fofura inválido");

            if (model.NivelCarinho < 1 || model.NivelFofura > 5)
                return new ServiceResponse<Animal>("Nível de carinho inválido");

            var novoAnimal = new Animal()
            {
                Nome = model.Nome,
                Idade = model.Idade,
                Especie = model.Especie,
                DataNascimento = model.DataNascimento,
                NivelFofura = model.NivelFofura,
                NivelCarinho = model.NivelCarinho,
                EmailContato = model.EmailContato,
            };

            listaDeAnimais.Add(novoAnimal);

            return new ServiceResponse<Animal>(novoAnimal);
        }

        internal object PesquisarPorIdade(int idade)
        {
            throw new NotImplementedException();
        }

        public List<Animal> ListarTodos()
        {
            return listaDeAnimais;
        }

        public ServiceResponse<Animal> PesquisarPorId(int id)
        {
            var resultado = listaDeAnimais.Where(x => x.Id == id).FirstOrDefault();
            if (resultado == null)
                return new ServiceResponse<Animal>("Não encontrado!");
            else
                return new ServiceResponse<Animal>(resultado);

        }

        public ServiceResponse<Animal> PesquisarPorNome(string nome)
        {
            var resultado = listaDeAnimais.Where(x => x.Nome == nome).FirstOrDefault();
            if (resultado == null)
                return new ServiceResponse<Animal>("Não encontrado!");
            else
                return new ServiceResponse<Animal>(resultado);
        }

        public ServiceResponse<Animal> Editar(int id, AnimalUpdateRequest model)
        {
            var resultado = listaDeAnimais.Where(x => x.Id == id).FirstOrDefault();

            if (resultado == null)
                return new ServiceResponse<Animal>("Animal não encontrado!");

            resultado.Nome = model.Nome;

            return new ServiceResponse<Animal>(resultado);
        }

        public ServiceResponse<bool> Deletar(int id)
        {
            var resultado = listaDeAnimais.Where(x => x.Id == id).FirstOrDefault();

            if (resultado == null)
                return new ServiceResponse<bool>("Animal não encontrado!");

            listaDeAnimais.Remove(resultado);

            return new ServiceResponse<bool>(true);
        }
    }
}
