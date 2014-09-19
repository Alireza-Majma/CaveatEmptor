using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml.Schema;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.Loquacious;
using NHibernate.Criterion;
using NHibernate.Dialect;
using NHibernate.Driver;
using System.Reflection;
using FluentNHibernate.Cfg;
using NHibernate.Hql.Ast.ANTLR;
using NHibernate.Linq;
using FluentNHibernate.Cfg.Db;
using System.IO;
using NHibernate.Tool.hbm2ddl;
using NHibernate.Connection;
using System.Data.SQLite;
using CaveatEmptor.Model;

namespace CaveatEmptor
{
    public class Program
    {
        const String DbFile = @"c:\Temp\TryHibernate.db.sqlite";
        public static Boolean InitiateDb = true;
        private static void Main()
        {
            if (InitiateDb)
            {
                InitiateData();
            }

            ReportDate1();

            Console.ReadKey();
        }


        public static void InitiateData()
        {
            var sessionFactory = CreateSessionFactory();
            using (var session = sessionFactory.OpenSession())
            {
                // populate the database
                using (var transaction = session.BeginTransaction())
                {
                    // create a couple of Stores each with some Products and Employees
                    var daisy = new Employee { FirstName = "Daisy", LastName = "Harrison" };
                    var jack = new Employee { FirstName = "Jack", LastName = "Torrance" };
                    var sue = new Employee { FirstName = "Sue", LastName = "Walkters" };
                    var bill = new Employee { FirstName = "Bill", LastName = "Taft" };
                    var joan = new Employee { FirstName = "Joan", LastName = "Pope" };

                    
                    // save both stores, this saves everything else via cascading
                    session.SaveOrUpdate(daisy);
                    session.SaveOrUpdate(jack);
                    session.SaveOrUpdate(sue);
                    session.SaveOrUpdate(bill);
                    session.SaveOrUpdate(joan );
                }
            }
        }

        private static void ReportDate1()
        {
            var sessionFactory = CreateSessionFactory();
            using (var session = sessionFactory.OpenSession())
            {
                // retreive all stores and display them
                using (session.BeginTransaction())
                {
                    var employees = from e in session.Query<Employee>()
                                 select e;
                    foreach (var e in employees)
                    {
                        Console.WriteLine("Employee Name");
                        Console.WriteLine("Id:{0} {1}.{2}",e.Id,e.FirstName,e.LastName);
                    }
                }
            }

        }

        private static ISessionFactory CreateSessionFactory()
        {
            //var dCnf = OracleClientConfiguration.Oracle10
            //          .Driver<OracleClientDriver>()
            //          .Provider<DriverConnectionProvider>()
            //          .ConnectionString("Data Source=rd01.WORLD; User Id=sms; Password=royal1;")
            //          .ShowSql()
            //          .UseReflectionOptimizer()
            //          .AdoNetBatchSize(16);


            //SQLiteConnection.CreateFile(DbFile);
            var dCnf = SQLiteConfiguration.Standard.ConnectionString(String.Format("Data Source={0};Version=3;", DbFile)).
                UsingFile(DbFile).
                ShowSql();
            //.Standard.UsingFile(@"c:\Temp\firstProject.db");

            var builder = Fluently.Configure()
                .Database(dCnf)
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Employee>())
                .Diagnostics(x => x.Enable())
                .ExposeConfiguration(BuildSchema);
            return builder.BuildSessionFactory();
            //return builder.BuildConfiguration().BuildSessionFactory();
        }

        public static void BuildSchema(Configuration config)
        {
            // this NHibernate tool takes a configuration (with mapping info in)
            // and exports a database schema from it
            if (InitiateDb)
            {
                // delete the existing db on each run
                File.Delete(DbFile);
                new SchemaExport(config).Create(true, true);
                InitiateDb = false;
            }


        }

    }
}
