using Bogus;
using Models;
using Services.InMemory;

namespace Services.Bogus
{
    public class BogusUserService : InMemoryUserService
    {

        public BogusUserService(Faker<User> faker, BogusConfig config) : base(faker.Generate(config.Count))
        {
        }
    }
}
