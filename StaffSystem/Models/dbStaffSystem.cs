namespace StaffSystem.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Linq;
    using StaffSystem.ViewModels;

    public class dbStaffSystem : DbContext
    {


        public DbSet<Staff> Staff { get; set; }
        public DbSet<ComplianceItems> ComplianceItems { get; set; }
        public DbSet<ComplianceTypes> ComplianceTypes { get; set; }
        public DbSet<StaffType> StaffType { get; set; }
        public DbSet<Products> Products { get; set; }
        
        // Your context has been configured to use a 'dbStaffSystem' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'StaffSystem.Models.dbStaffSystem' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'dbStaffSystem' 
        // connection string in the application configuration file.
        public dbStaffSystem()
            : base("name=dbStaffSystem")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Entity<Staff>()
                .HasMany(c => c.ComplianceItems).WithMany(i => i.Staff)
                .Map(t => t.MapLeftKey("Id")
                    .MapRightKey("ComplianceID")
                    .ToTable("StaffComplianceItems"));

            
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}