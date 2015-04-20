using System.Data.Entity;
using ESS.Domain.JXC.Models.Mapping;

namespace ESS.Domain.JXC.Models
{
    public class ESS_ERPContext : DbContext
    {
        static ESS_ERPContext()
        {
            Database.SetInitializer<ESS_ERPContext>(null);
        }

        public ESS_ERPContext()
            : base("Name=connectionString")
        {
        }

        public DbSet<Bank> Banks { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductClass> ProductClasses { get; set; }
        public DbSet<SalesMan> SalesMen { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Column> Columns { get; set; }
        public DbSet<Config> Configs { get; set; }
        public DbSet<DdlControl> DdlControls { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Privilege> Privileges { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ChangeDetail> ChangeDetails { get; set; }
        public DbSet<ChangeMaster> ChangeMasters { get; set; }
        public DbSet<DeliveryDetail> DeliveryDetails { get; set; }
        public DbSet<DeliveryMaster> DeliveryMasters { get; set; }
        public DbSet<PaymentDetail> PaymentDetails { get; set; }
        public DbSet<PaymentMaster> PaymentMasters { get; set; }
        public DbSet<ProductStock> ProductStocks { get; set; }
        public DbSet<PurchaseDetail> PurchaseDetails { get; set; }
        public DbSet<PurchaseMaster> PurchaseMasters { get; set; }
        public DbSet<ReceiveDetail> ReceiveDetails { get; set; }
        public DbSet<ReceiveMaster> ReceiveMasters { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new BankMap());
            modelBuilder.Configurations.Add(new BrandMap());
            modelBuilder.Configurations.Add(new CustomerMap());
            modelBuilder.Configurations.Add(new ProductMap());
            modelBuilder.Configurations.Add(new ProductClassMap());
            modelBuilder.Configurations.Add(new SalesManMap());
            modelBuilder.Configurations.Add(new SupplierMap());
            modelBuilder.Configurations.Add(new ColumnMap());
            modelBuilder.Configurations.Add(new ConfigMap());
            modelBuilder.Configurations.Add(new DdlControlMap());
            modelBuilder.Configurations.Add(new FavoriteMap());
            modelBuilder.Configurations.Add(new MenuMap());
            modelBuilder.Configurations.Add(new PrivilegeMap());
            modelBuilder.Configurations.Add(new RoleMap());
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new ChangeDetailMap());
            modelBuilder.Configurations.Add(new ChangeMasterMap());
            modelBuilder.Configurations.Add(new DeliveryDetailMap());
            modelBuilder.Configurations.Add(new DeliveryMasterMap());
            modelBuilder.Configurations.Add(new PaymentDetailMap());
            modelBuilder.Configurations.Add(new PaymentMasterMap());
            modelBuilder.Configurations.Add(new ProductStockMap());
            modelBuilder.Configurations.Add(new PurchaseDetailMap());
            modelBuilder.Configurations.Add(new PurchaseMasterMap());
            modelBuilder.Configurations.Add(new ReceiveDetailMap());
            modelBuilder.Configurations.Add(new ReceiveMasterMap());
        }
    }
}