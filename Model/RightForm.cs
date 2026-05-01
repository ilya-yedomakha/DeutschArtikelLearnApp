using DeutschArtikelLearnApp.Model.Base;
using DeutschArtikelLearnApp.Model.Lessons;

namespace DeutschArtikelLearnApp.Model
{
        public class RightForm : NamedEntity
        {
            public required string Article { get; set; }
            public required string Plural { get; set; }
            public HashSet<Lesson> Lessons { get; set; } = [];
    }
}
