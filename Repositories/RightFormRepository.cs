using DeutschArtikelLearnApp.Data;
using DeutschArtikelLearnApp.Model;
using DeutschArtikelLearnApp.Repositories.Base;

namespace DeutschArtikelLearnApp.Repositories
{
    public class RightFormRepository : BaseRepository<RightForm>
    {
        public RightFormRepository(DataContext context) : base(context)
        {
        }

        public RightForm? GetModel(string name, bool useIncludes)
        {
            if (useIncludes)
            {
                var query = PrepareQueryWithIncludes();
                return query.SingleOrDefault(a => a.Name == name);
            }
            else
            {
                var query = _context.Set<RightForm>();
                return query.SingleOrDefault(a => a.Name == name);
            }
        }
    }
}
