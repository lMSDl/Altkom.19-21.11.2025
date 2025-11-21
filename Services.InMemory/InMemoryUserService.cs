using Models;
using Services.Interfaces;

namespace Services.InMemory
{
    public class InMemoryUserService : InMemoryCrudService<User>, IAuth
    {
        public InMemoryUserService()
        {
        }

        protected InMemoryUserService(ICollection<User> entites) : base(entites)
        {
            CreateAsync(new User { Username = "admin", Password = "admin", Roles = Roles.Create | Roles.Edit | Roles.Delete }).Wait();
        }

        public Task<User?> GetAsync(string username, string password)
        {
            return Task.FromResult(_entities.Where(x => x.Username == username).SingleOrDefault(x => x.Password == password));
        }
    }
}
