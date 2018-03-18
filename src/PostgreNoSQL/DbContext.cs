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
    ///         Typically you create a class that derives from DbContext and contains <see cref="DbDocument{TEntity}" />
    ///         properties for each entity in the model. If the <see cref="DbDocument{TEntity}" /> properties have a public setter,
    ///         they are automatically initialized when the instance of the derived context is created.
    ///     </para>
    /// </remarks>
    public class DbContext : IDisposable
    {
        private readonly ConcurrentDictionary<Type, IReadOnlyList<DbDocumentProperty>> _cache = new ConcurrentDictionary<Type, IReadOnlyList<DbDocumentProperty>>();
        private readonly DbContextOptions _options;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DbContext" /> class. The
        ///     <see cref="OnConfiguring(DbContextOptions)" />
        ///     method will be called to configure the database (and other options) to be used for this context.
        /// </summary>
        protected DbContext() 
            : this(new DbContextOptions())
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="DbContext" /> class using the specified options.
        ///     The <see cref="OnConfiguring(DbContextOptions)" /> method will still be called to allow further
        ///     configuration of the options.
        /// </summary>
        /// <param name="options"> The options for this context. </param>
        public DbContext(DbContextOptions options)
        {
            _options = Check.NotNull(options, nameof(options));

            OnConfiguring(_options);
            InitializeDocuments(this);
        }

        public virtual IReadOnlyList<DbDocumentProperty> FindDocuments(DbContext context) => _cache.GetOrAdd(context.GetType(), FindDocuments);

        public void Dispose()
        {
        }

        /// <summary>
        ///     Override this method to configure the database (and other options) to be used for this context.
        ///     This method is called for each instance of the context that is created.
        ///     The base implementation does nothing.
        /// </summary>
        /// <param name="options"> The options for this context. </param>
        protected internal virtual void OnConfiguring(DbContextOptions options)
        {
        }

        protected virtual void InitializeDocuments(DbContext context)
        {
            foreach (var documentInfo in FindDocuments(context))
            {
                documentInfo.SetClrValue(this);
            }
        }

        private static DbDocumentProperty[] FindDocuments(Type contextType)
        {
            return contextType.GetRuntimeProperties()
                .Where(
                    p => !p.IsStatic()
                         && !p.GetIndexParameters().Any()
                         && p.DeclaringType != typeof(DbContext)
                         && p.PropertyType.GetTypeInfo().IsGenericType
                         && (p.PropertyType.GetGenericTypeDefinition() == typeof(DbDocument<>)))
                .OrderBy(p => p.Name)
                .Select(
                    p => new DbDocumentProperty(p))
                .ToArray();
        }
    }
}
