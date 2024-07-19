using BusinessLogic.Contracts;
using Domain.Models;
using Newtonsoft.Json;
using Repository.Contracts;

namespace BusinessLogic.Implementations
{
    public class CharacterService : ICharacterService
    {
        private readonly ICharacterRepository _characterRepository;

        public CharacterService(ICharacterRepository characterRepository)
        {
            _characterRepository = characterRepository;
        }

        public async Task<List<Character>> GetAllCharactersAsync(string name, string status, string species, string type, string gender)
        {
            List<Character> characters = new List<Character>();
            string? nextUrl = ConstructInitialUrl(name: name, status: status, species: species, type: type, gender: gender);

            while (!string.IsNullOrEmpty(nextUrl) && characters.Count < 100)
            {
                var content = await _characterRepository.HttpQuery(nextUrl);
                var deserializedContent = JsonConvert.DeserializeObject<ExternalApiResponse>(content);
                if (deserializedContent?.Results != null)
                {
                    characters.AddRange(deserializedContent.Results.Take(100 - characters.Count));
                    nextUrl = deserializedContent.Info?.Next;
                }
                else nextUrl = null;
            }
            return characters;
        }

        public async Task<Character> GetCharacterByIdAsync(int id)
        {
            string url = ConstructInitialUrl(id: id);
            var content = await _characterRepository.HttpQuery(url);
            var deserializedContent = JsonConvert.DeserializeObject<Character>(content);
            if (deserializedContent != null) return deserializedContent;
            else throw new InvalidOperationException("Received null response data for character");
        }

        private string ConstructInitialUrl(string name = "", string status = "", string species = "", string type = "", string gender = "", int? id = null)
        {
            string baseUrl = "character/";
            if (id.HasValue) // If an ID has been specified, don't apply filters.
            {
                return $"{baseUrl}{id.Value}";
            }
            else
            {
                var queryParams = new Dictionary<string, string>
                {
                    { "name", name },
                    { "status", status },
                    { "species", species },
                    { "type", type },
                    { "gender", gender }
                };

                string queryString = string.Join("&", queryParams.Where(kvp => !string.IsNullOrEmpty(kvp.Value))
                                                                .Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}"));
                return $"{baseUrl}?{queryString}";
            }
        }
    }
}
