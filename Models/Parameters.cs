namespace SisPark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Parameters
    {
        [Key]
        public int ParametrosId { get; set; }

        [Required]
        [StringLength(100)]
        public string Description { get; set; }

        public float ValorMinimo { get; set; }

        public float ValorMáximo { get; set; }
    }
}
