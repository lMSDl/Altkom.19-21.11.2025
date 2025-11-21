using Models;

namespace Services.Interfaces
{
    public interface IAuth
    {
        Task<User?> GetAsync(string username, string password);
    }
}
