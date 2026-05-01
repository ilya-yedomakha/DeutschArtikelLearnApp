using DeutschArtikelLearnApp.Controllers;
using DeutschArtikelLearnApp.Model.Base;

namespace DeutschArtikelLearnApp.Model
{
    public class Lesson : NamedEntity
    {
        public HashSet<RightForm> RightForms { get; } = [];
    }
}
