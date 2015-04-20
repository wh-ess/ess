using System.Data.Entity;
using ESS.Domain.Car.Models;

namespace ESS.Domain.Car
{
    public class DB : DbContext
    {
        static DB()
        {
            Database.SetInitializer<DB>(new DropCreateDatabaseAlways<DB>());
        }

        public DB()
            : base("Name=connectionString")
        {
        }

        public DbSet<CarInfo> CarInfos  {get; set; }


    }
}