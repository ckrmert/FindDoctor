//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FindDoctor.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ExDatabaseEntities : DbContext
    {
        public ExDatabaseEntities()
            : base("name=ExDatabaseEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Favorites> Favorites { get; set; }
        public virtual DbSet<Prices> Prices { get; set; }
        public virtual DbSet<Members> Members { get; set; }
        public virtual DbSet<Cities> Cities { get; set; }
        public virtual DbSet<Specializations> Specializations { get; set; }
        public virtual DbSet<JDoctorsSchools> JDoctorsSchools { get; set; }
        public virtual DbSet<Schools> Schools { get; set; }
        public virtual DbSet<Blog> Blog { get; set; }
        public virtual DbSet<BlogCategories> BlogCategories { get; set; }
        public virtual DbSet<BlogComments> BlogComments { get; set; }
        public virtual DbSet<Doctors> Doctors { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<Comments> Comments { get; set; }
    }
}
