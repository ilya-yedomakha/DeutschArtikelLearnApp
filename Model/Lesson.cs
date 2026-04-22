using DeutschArtikelLearnApp.Controllers;
using DeutschArtikelLearnApp.Model.Base;

namespace DeutschArtikelLearnApp.Model
{
    public class Lesson : NamedEntity
    {
        public List<RightForm> RightForms { get; } = [];
    }
}
