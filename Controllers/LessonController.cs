using DeutschArtikelLearnApp.DTO.Create;
using DeutschArtikelLearnApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DeutschArtikelLearnApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LessonController : ControllerBase
    {
        private readonly LessonService _lessonService;

        public LessonController(LessonService lessonService)
        {
            _lessonService = lessonService;
        }

        [HttpPost("LessonCreate")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult LessonCreate([FromBody] LessonCreateDTO lessonCreateDTO)
        {

            var result = _lessonService.CreateLesson(lessonCreateDTO);
            if (result.IsFailure)
            {
                return NotFound(result.Error);
            }

            return Ok(result.Model);
        }
    }
}
