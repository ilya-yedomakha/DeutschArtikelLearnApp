using System.Text.Json;
using System.Text.RegularExpressions;
using DeutschArtikelLearnApp.DTO;
using DeutschArtikelLearnApp.DTO.Create;
using DeutschArtikelLearnApp.Help.Result;
using DeutschArtikelLearnApp.Help.Result.ModelErrors;
using DeutschArtikelLearnApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace DeutschArtikelLearnApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public partial class WordController : ControllerBase
    {
        private readonly RightFormService _rightFormService;

        public WordController(RightFormService rightFormService)
        {
            _rightFormService = rightFormService;
        }

        [HttpGet("GetTestWord")]
        public async Task<object> GetTestWord()
        {
            return await _rightFormService.SingleWordWiktapiDataRequest("Land");
        }

        [HttpPost("SingleWordCheck")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> SingleWordCheck([FromBody] SingleRequestWord singleRequestWord)
        {

            var result = await _rightFormService.SingleWordWiktapiDataRequest(singleRequestWord.Word);
            if (result.IsFailure)
            {
                return NotFound(result.Error);
            }

            return Ok(result.ModelDTO);
        }


        [HttpPost("SingleWordSave")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> SingleWordSave([FromBody] SingleRequestWord singleRequestWord)
        {

            var result = await _rightFormService.CreateRightForm(singleRequestWord);
            if (result.IsFailure)
            {
                return NotFound(result.Error);
            }

            return Ok(result.Model);
        }

        #region util methods
        
        #endregion util methods
    }
}
