namespace PostgreNoSQL.Internal
{
    using System;
    using System.Linq;
    using System.Reflection;

    public class DbSetProperty
    {
        public DbSetProperty(PropertyInfo propertyInfo)
        {
            Info = propertyInfo;
        }

        public PropertyInfo Info { get; set; }
        public string Name => Info.Name;
        public Type ClrType => Info.PropertyType.GetTypeInfo().GenericTypeArguments.Single();

        public void SetClrValue(DbContext context) 
            => Info.SetValue(context, Activator.CreateInstance(Info.PropertyType));

    }
}
