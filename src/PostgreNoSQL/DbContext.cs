namespace PostgreNoSQL
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using PostgreNoSQL.Internal;

    /// <summary>
    ///     A DbContext instance represents a session with the database and can be used to query and save
    ///     instances of your entities. DbContext is a combination of the Unit Of Work and Repository patterns.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Typically you create a class that derives from DbContext and contains <see cref="DbSet{TEntity}" />
    ///         properties for each entity in the model. If the <see cref="DbSet{TEntity}" /> properties have a public setter,
    ///         they are automatically initialized when the instance of the derived context is created.
    ///     </para>
    /// </remarks>
    public class DbContext : IDisposable
    {
        private readonly ConcurrentDictionary<Type, IReadOnlyList<DbSetProperty>> _cache = new ConcurrentDictionary<Type, IReadOnlyList<DbSetProperty>>();

        public DbContext()
        {
            InitializeSets(this);
        }

        public virtual void InitializeSets(DbContext context)
        {
            foreach (var setInfo in FindSets(context))
            {
                setInfo.SetClrValue(this);
            }
        }

        public void Dispose()
        {
        }   

        public virtual IReadOnlyList<DbSetProperty> FindSets(DbContext context) => _cache.GetOrAdd(context.GetType(), FindSets);

        private static DbSetProperty[] FindSets(Type contextType)
        {
            return contextType.GetRuntimeProperties()
                .Where(
                    p => !p.IsStatic()
                         && !p.GetIndexParameters().Any()
                         && p.DeclaringType != typeof(DbContext)
                         && p.PropertyType.GetTypeInfo().IsGenericType
                         && (p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>)))
                .OrderBy(p => p.Name)
                .Select(
                    p => new DbSetProperty(p))
                .ToArray();
        }
    }
}
