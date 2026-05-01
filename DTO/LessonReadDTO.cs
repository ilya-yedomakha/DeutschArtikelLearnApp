using DeutschArtikelLearnApp.Model.Lessons.Enums;

namespace DeutschArtikelLearnApp.DTO
{
    public class LessonReadDTO : BaseReadDTO
    {
        public required string name;
        public required LangLevel level;
    }
}
