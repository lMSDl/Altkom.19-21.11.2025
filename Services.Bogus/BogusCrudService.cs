using Bogus;
using Models;
using Services.InMemory;

namespace Services.Bogus
{
    public class BogusCrudService<T> : InMemoryCrudService<T> where T : Entity
    {
        public BogusCrudService(Faker<T> faker, BogusConfig config) : base(faker.Generate(config.Count))
        {
        }
    }
}
