using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.ServiceLocation;
using NHibernate;
using NHibernate.Linq;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;

namespace NMG.DAL
{
    public class NHDbContext : IDbContext
    {
        private readonly ISessionFactory sessionFactory;
        public NHDbContext(ISession session)
        {
            _session = session;
        }
        public NHDbContext()
        {
            _session = SessionManager.CurrentSession;// SiceLocator.Current.GetInstance<ISession>();
        }

        public NHDbContext(bool type)
        {
            var connectionString = "Data Source=192.168.15.164;Initial Catalog=NMGTableMetaData;User ID=sa;Password=13581365";
            //System.Configuration.ConfigurationManager.AppSettings["Default"];
            sessionFactory = Fluently.Configure().Database(MsSqlConfiguration.MsSql2008.ConnectionString(connectionString)).CurrentSessionContext("managed_web")
                             .Mappings(mapping => mapping.FluentMappings.AddFromAssemblyOf<NoMapAttribute>()).BuildSessionFactory();
            _session = sessionFactory.OpenSession();
        }



        ISession _session = null;
        private ISession session
        {
            get
            {
                return _session;
            }
        }

        public IQueryable<T> Db<T>()
        {
            return session.Query<T>();
        }

        public T Fetch<T>(object id)
        {

            return session.Get<T>(id);
        }

        public void Edit<T>(T model)
        {
            if (model != null)
                session.Update(model);
        }

        public void Delete<T>(T model)
        {
            if (model != null)
                session.Delete(model);
        }

        public object Add<T>(T model)
        {
            if (model != null)
                return session.Save(model);
            return null;
        }

        public void Commit()
        {
            session.Flush();
        }

        public ITransaction BeginTransaction()
        {
            return session.BeginTransaction();
        }

        public void RoleBack()
        {
            session.Transaction.Rollback();
            //session.Transaction.
        }

        public void Flush()
        {
            session.Flush();
            //session.Transaction.
        }

        public void DeleteAll<T>(IEnumerable<T> model)
        {
            //var session = ServiceLocator.Current.GetInstance<ISession>();
            foreach (var i in model)
            {
                session.Delete(model);
            }
        }

    }
}
