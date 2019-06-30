using System.Data.Entity;

namespace Task1
{
    public  class ApplicationContext:DbContext
    {
        
        public ApplicationContext() : base ("DbConnectionString")
        {
        }
        public DbSet<Person> Persons { get; set; }
    }
}
