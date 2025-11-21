using Bogus;
using Models;

namespace Services.Bogus.Fakers
{
    public class EntityFaker<T> : Faker<T> where T : Entity
    {
        public EntityFaker(BogusConfig config) : base(config.Locale)
        {
            RuleFor(e => e.Id, f => f.IndexFaker + 1);
        }
    }
}
