namespace SisPark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Usertype")]
    public partial class Usertype
    {
        public Usertype()
        {
            User = new HashSet<User>();
        }

        public int usertypeid { get; set; }

        [Required]
        [StringLength(30)]
        public string description { get; set; }

        public virtual ICollection<User> User { get; set; }
    }
}
