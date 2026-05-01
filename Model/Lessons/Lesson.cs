using DeutschArtikelLearnApp.Controllers;
using DeutschArtikelLearnApp.Model.Base;
using DeutschArtikelLearnApp.Model.Lessons.Enums;

namespace DeutschArtikelLearnApp.Model.Lessons
{
    public class Lesson : NamedEntity
    {
        public required LangLevel level { get; set; }
        public HashSet<RightForm> RightForms { get; set; } = [];
    }
}
