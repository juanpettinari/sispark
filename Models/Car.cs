namespace SisPark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Car")]
    public partial class Car
    {
        public Car()
        {
            Ticket = new HashSet<Ticket>();
        }

        public int CarId { get; set; }

        public int CarTypeId { get; set; }

        [Required]
        [StringLength(6,MinimumLength=6,ErrorMessage="La {0} debe tener {1} caracteres.")]
        public string Patente { get; set; }

        [StringLength(50)]
        public string Description { get; set; }

        [StringLength(50)]
        public string NombreCliente { get; set; }

        [StringLength(10)]
        public string TelCliente { get; set; }

        public virtual CarType CarType { get; set; }

        public virtual ICollection<Ticket> Ticket { get; set; }
    }
}
