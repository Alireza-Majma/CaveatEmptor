using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using log4net;
using NHibernate;
using CaveatEmptor.Persistence;
using CaveatEmptor.Exceptions;
using NHibernate.Hql.Ast.ANTLR;
using NHibernate.Criterion;

namespace CaveatEmptor.DataAccess
{
    /// <summary>
    /// A generic DAO class for data access for any mapped entity.
    /// </summary>
    /// <typeparam name="T">The type of entity</typeparam>
    public class Dao<T> where T : class
    {
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public Dao()
        {
            _log.Debug("Created new DAO");
            NHibernateHelper.BeginTransaction();
        }

        /// <summary>
        /// Get an entity by ID, with no upgrade lock.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual T GetById(long id)
        {
            return GetById(id, false);
        }
        
        /// <summary>
        /// Get an entity by ID.
        /// </summary>
        /// <param name="id">The id of the entity to load.</param>
        /// <param name="lockIt">Specify true if you want an upgrade lock. 
        /// This will db lock the item for update until the current transaction ends. 
        /// It will also do a version check (comparing columns or version no).</param>
        public virtual T GetById(long id, bool lockIt)
        {
            _log.Debug("Called GetById: " + id);
            ISession session = NHibernateHelper.Session;
            try
            {
                if (lockIt)
                    return session.Get<T>(id, LockMode.Upgrade);
                else
                    return session.Get<T>(id);
            }
            catch (HibernateException ex)
            {
                throw new InfrastructureException(ex);
            }
        }

        /// <summary>
        /// Method to find all entities.
        /// </summary>
        /// <returns></returns>
        public virtual IList<T> FindAll()
        {
            _log.Debug("Called FindAll");
            IList<T> entities;
            try
            {
                entities = NHibernateHelper.Session.CreateCriteria<T>().List<T>();
            }
            catch (HibernateException ex)
            {
                throw new InfrastructureException(ex);
            }
            return entities;
        }

        /// <summary>
        /// Find entities matching this the example one given
        /// </summary>
        /// <param name="exampleEntity">Entity with example properties we'd like to match</param>
        public virtual IList<T> FindByExample(T exampleEntity)
        {
            _log.Debug("FindByExample: " + exampleEntity);
            IList<T> entities;
            try
            {
                ICriteria crit = NHibernateHelper.Session.CreateCriteria<T>();
                entities = crit.Add(Example.Create(exampleEntity)).List<T>();
            }
            catch (HibernateException ex)
            {
                throw new InfrastructureException(ex);
            }
            return entities;
        }


        /// <summary>
        /// Make this transient or persistent entity persistent in the database
        /// </summary>
        /// <param name="entity"></param>
        public virtual void SaveOrUpdate(T entity)
        {
            _log.Debug("Called MakePersistent: " + entity);
            try
            {
                NHibernateHelper.Session.SaveOrUpdate(entity);
            }
            catch (HibernateException ex)
            {
                throw new InfrastructureException(ex);
            }
        }


        /// <summary>
        /// Make the entity transient
        /// </summary>
        /// <remarks>
        /// Making something transient means that it doesn't have a corresponding record in the database; 
        /// It is no longer persistent.
        /// </remarks>
        /// <param name="entity">The entity who's database state will be deleted.</param>
        public virtual void Delete(T entity)
        {
            _log.Debug("Called MakeTransient: " + entity);
            try
            {
                NHibernateHelper.Session.Delete(entity);
            }
            catch (HibernateException ex)
            {
                throw new InfrastructureException(ex);
            }
        }
    }
}
