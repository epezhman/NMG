using System.Linq;
using System.Collections.Generic;

namespace NMG.DAL
{
    public interface IDbContext
    {
        IQueryable<T> Db<T>();
       
        T Fetch<T>(object id);
        void Edit<T>(T model);
        void Delete<T>(T model);
        void DeleteAll<T>(IEnumerable<T> model);
        object Add<T>(T model);
        void Commit();
        void RoleBack();
        void Flush();

    }
}
