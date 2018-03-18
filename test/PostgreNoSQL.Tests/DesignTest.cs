namespace PostgreNoSQL.Tests
{
    using System.Linq;
    using Xunit;

    public class DesignTest
    {
        [Fact]
        public void Test1()
        {
            using (var db = new SupplierDbContext())
            {
                var suppliers = db.Suppliers.Where(x => x.Id == 1 && (x.Name == "Philippe" || x.Name == "Christophe"))
                                            .OrderBy(x => x.Id).ToList();
            }
        }
    }

    public class SupplierDbContext : DbContext
    {
        public DbDocument<Supplier> Suppliers { get; set; }
    }

    public class Supplier
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
