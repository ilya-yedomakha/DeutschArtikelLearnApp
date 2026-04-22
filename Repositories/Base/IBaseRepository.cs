using DeutschArtikelLearnApp.Model.Base;
using Microsoft.EntityFrameworkCore;

namespace DeutschArtikelLearnApp.Repositories.Base
{
    public interface IBaseRepository<TModel> where TModel : Entity
    {
        #region Common actions
        public bool AddModel(TModel model);
        public bool DeleteModel(TModel model);
        public bool Update(TModel model);
        public bool Save(DbContext _context);
        #endregion

    }
}
