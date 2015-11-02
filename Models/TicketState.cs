namespace SisPark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TicketState")]
    public partial class TicketState
    {
        public TicketState()
        {
            Ticket = new HashSet<Ticket>();
        }

        public int TicketStateId { get; set; }

        [Required]
        [StringLength(50)]
        public string description { get; set; }

        [StringLength(250)]
        public string observacion { get; set; }

        public virtual ICollection<Ticket> Ticket { get; set; }
    }
}
