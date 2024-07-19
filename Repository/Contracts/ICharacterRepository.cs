using Domain.Models;

namespace Repository.Contracts
{
    public interface ICharacterRepository
    {
        Task<string> HttpQuery(string url);
    }
}
