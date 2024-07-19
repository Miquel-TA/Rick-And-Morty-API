using Domain.Models;

namespace BusinessLogic.Contracts
{
    public interface ICharacterService
    {
        Task<List<Character>> GetAllCharactersAsync(string name, string status, string species, string type, string gender);
        Task<Character> GetCharacterByIdAsync(int id);
    }
}
