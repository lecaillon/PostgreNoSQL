namespace PostgreNoSQL
{
    using System.Reflection;

    internal  static class Extensions
    {
        public static bool IsStatic(this PropertyInfo property) => (property.GetMethod ?? property.SetMethod).IsStatic;
    }
}
