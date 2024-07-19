using Repository.Contracts;

namespace Repository.Implementations
{
    public class CharacterRepository : ICharacterRepository
    {
        private readonly HttpClient _httpClient;

        public CharacterRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://rickandmortyapi.com/api/");
        }

        public async Task<string> HttpQuery(string url)
        {
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            throw new HttpRequestException($"Failed to retrieve data. Status code: {response.StatusCode}");
        }
    }
}