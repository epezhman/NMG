using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using NHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Cfg;
using System.Configuration;

namespace NMG.DAL
{
    public class SessionManager
    {
        private readonly ISessionFactory sessionFactory;
        public static ISessionFactory SessionFactory
        {
            get { return Instance.sessionFactory; }
        }

        private ISessionFactory GetSessionFactory()
        {
            return sessionFactory;
        }

        public static SessionManager Instance
        {
            get
            {
                return NestedSessionManager.sessionManager;
            }
        }

        public static ISession OpenSession()
        {
            return Instance.GetSessionFactory().OpenSession();
        }

        public static ISession CurrentSession
        {
            get
            {
                //return Instance.GetSessionFactory().GetCurrentSession();
                return Instance.GetSessionFactory().OpenSession();
            }
        }

        private SessionManager()
        {
            //Configuration configuration = new Configuration().Configure();
            //sessionFactory = configuration.BuildSessionFactory();

            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
            sessionFactory = Fluently.Configure().Database(MsSqlConfiguration.MsSql2008.ConnectionString(connectionString)).CurrentSessionContext("managed_web")
                              .Mappings(mapping => mapping.FluentMappings.AddFromAssemblyOf<NoMapAttribute>()).BuildSessionFactory();
        }

        class NestedSessionManager
        {
            internal static readonly SessionManager sessionManager =new SessionManager();
        }
    }
}
