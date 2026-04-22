using DeutschArtikelLearnApp.Data;
using DeutschArtikelLearnApp.Model;
using DeutschArtikelLearnApp.Repositories.Base;

namespace DeutschArtikelLearnApp.Repositories
{
    public class LessonRepository : BaseRepository<Lesson>
    {
        public LessonRepository(DataContext context) : base(context)
        {
        }

        public Lesson? GetModel(string name, bool useIncludes)
        {
            if (useIncludes)
            {
                var query = PrepareQueryWithIncludes();
                return query.SingleOrDefault(a => a.Name == name);
            }
            else
            {
                var query = _context.Set<Lesson>();
                return query.SingleOrDefault(a => a.Name == name);
            }
        }
    }
}
