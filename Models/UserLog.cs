namespace SisPark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserLog")]
    public partial class UserLog
    {
        public int UserLogId { get; set; }

        public int UserId { get; set; }

        [Required]
        [StringLength(20)]
        public string Accion { get; set; }

        public DateTime FechaYHora { get; set; }

        public virtual User User { get; set; }
    }
}
