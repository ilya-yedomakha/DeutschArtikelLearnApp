using DeutschArtikelLearnApp.Model.Lessons.Enums;

namespace DeutschArtikelLearnApp.DTO.Create
{
    public class LessonCreateDTO : BaseCreateDTO
    {
        public required string Name { get; set; }
        public required LangLevel Level { get; set; }
    }
}
