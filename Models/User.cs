namespace SisPark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("User")]
    public partial class User
    {
        public User()
        {
            TicketLog = new HashSet<TicketLog>();
            UserLog = new HashSet<UserLog>();
        }

        public int UserId { get; set; }

        public int UserTypeId { get; set; }

        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(50)]
        public string Apellido { get; set; }

        [Required]
        [StringLength(50)]
        public string Telefono { get; set; }

        [Required]
        [StringLength(50)]
        public string Usuario { get; set; }

        [Required]
        [StringLength(200,MinimumLength=5)]
        public string Clave { get; set; }

        [StringLength(200)]
        public string ClaveSalt { get; set; }

        public virtual ICollection<TicketLog> TicketLog { get; set; }

        public virtual Usertype Usertype { get; set; }

        public virtual ICollection<UserLog> UserLog { get; set; }
    }
}
