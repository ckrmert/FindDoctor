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
    using System.Collections.Generic;
    
    public partial class Doctors
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Doctors()
        {
            this.Blog = new HashSet<Blog>();
            this.Favorites = new HashSet<Favorites>();
            this.JDoctorsSchools = new HashSet<JDoctorsSchools>();
            this.Prices = new HashSet<Prices>();
            this.Comments = new HashSet<Comments>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public string Biography { get; set; }
        public string Adress { get; set; }
        public int Phone { get; set; }
        public Nullable<int> OfficePhone { get; set; }
        public Nullable<int> Viewed { get; set; }
        public Nullable<int> Patiens { get; set; }
        public int SpecializationId { get; set; }
        public int CityId { get; set; }
        public string ProfileImage { get; set; }
        public Nullable<int> AverageVote { get; set; }
        public Nullable<int> TotalVote { get; set; }
        public Nullable<bool> CanBlog { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Blog> Blog { get; set; }
        public virtual Cities Cities { get; set; }
        public virtual Specializations Specializations { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Favorites> Favorites { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<JDoctorsSchools> JDoctorsSchools { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Prices> Prices { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comments> Comments { get; set; }
    }
}
