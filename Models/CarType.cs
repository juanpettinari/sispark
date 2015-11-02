namespace SisPark.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CarType")]
    public partial class CarType
    {
        private int carTypeId;
        private ICalculadorPrecio calculadorPrecio;

        public CarType()
        {
            Car = new HashSet<Car>();
        }

        public int CarTypeId {
            get
            {
                return carTypeId;
            }
            set
            {
                this.carTypeId = value;
                var factory = FactoryCalculadoresPrecio.GetInstance();
                this.calculadorPrecio = factory.CreateCalculador(this.carTypeId);
            }
        }

        public decimal CalcularPrecio(double time)
        {
            return this.calculadorPrecio.CalcularPrecio(time);
        }


        [Required]
        [StringLength(50)]
        public string description { get; set; }

        public float tarifa { get; set; }

        public virtual ICollection<Car> Car { get; set; }
    }
}
