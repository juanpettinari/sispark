namespace SisPark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TicketLog")]
    public partial class TicketLog
    {
        public int TicketLogId { get; set; }

        public int TicketId { get; set; }

        public int UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string TransactionType { get; set; }

        public DateTime DateTimeLog { get; set; }

        public virtual Ticket Ticket { get; set; }

        public virtual User User { get; set; }
    }
}
