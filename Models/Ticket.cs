namespace SisPark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Ticket")]
    public partial class Ticket
    {
        public Ticket()
        {
            TicketLog = new HashSet<TicketLog>();
        }

        public int TicketId { get; set; }

        public int CarId { get; set; }

        public int TicketStateId { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime? FinishDateTime { get; set; }

        public float? TotalTime { get; set; }

        public float? TotalPrice { get; set; }

        public virtual Car Car { get; set; }

        public virtual TicketState TicketState { get; set; }

        public virtual ICollection<TicketLog> TicketLog { get; set; }
    }
}
