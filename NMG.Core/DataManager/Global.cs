using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
using NHibernate;
using Castle.Core;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using System.Reflection;
using FluentNHibernate.Automapping;
using Microsoft.Practices.ServiceLocation;
using CommonServiceLocator.WindsorAdapter;
using System.Linq.Expressions;
using System.Configuration;

namespace NMG.DAL
{
    public class Global
    {

        private ISessionFactory factory;

        public void Initialize(string connectionString = "")
        {
            if (string.IsNullOrEmpty(connectionString))
                connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Default"].ConnectionString;

            factory = Fluently.Configure().Database(MsSqlConfiguration.MsSql2008.ConnectionString(connectionString))
                              .Mappings(mapping => mapping.FluentMappings.AddFromAssemblyOf<NoMapAttribute>()).BuildSessionFactory();
            
            //            var container = new WindsorContainer();
            //            container.Register(
            //                Component.For<IDbContext>()
            //                .ImplementedBy<NHDbContext>()
            //                .LifeStyle.Is(LifestyleType.Transient),
            //                Component.For<ISession>()
            //                .UsingFactoryMethod(() => factory.OpenSession())
            //                .LifeStyle.Is(LifestyleType.PerWebRequest)
            //                );
          

            //ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(container));
        }
    }
}
