using BusinessLogic.Contracts;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CharactersController : ControllerBase
    {
        private readonly ICharacterService _characterService;

        public CharactersController(ICharacterService characterService)
        {
            _characterService = characterService;
        }

        /// <summary>
        /// Retrieves the top 100 characters that match the specified filters.
        /// </summary>
        /// <param name="name">Filter by character name.</param>
        /// <param name="status">Filter by character status.</param>
        /// <param name="species">Filter by character species.</param>
        /// <param name="type">Filter by character type.</param>
        /// <param name="gender">Filter by character gender.</param>
        /// <returns>A list of characters.</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Character>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllCharacters(string name = "", string status = "", string species = "", string type = "", string gender = "")
        {
            var result = await _characterService.GetAllCharactersAsync(name, status, species, type, gender);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a character by ID.
        /// </summary>
        /// <param name="id">The ID of the character.</param>
        /// <returns>A character.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Character))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetCharacter(int id)
        {
            var result = await _characterService.GetCharacterByIdAsync(id);
            return Ok(result);
        }
    }
}