using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SisPark.Models
{
    public class FactoryCalculadoresPrecio
    {
        private static FactoryCalculadoresPrecio instance;
    
        private FactoryCalculadoresPrecio()
        {

        }

        public static FactoryCalculadoresPrecio GetInstance()
        {
            if (instance == null)
                instance = new FactoryCalculadoresPrecio();
            return instance;
        }

        public ICalculadorPrecio CreateCalculador(int carTypeId)
        {
            switch(carTypeId)
            {
                case 1:
                    return new CalculadorPrecioAuto();
                case 2:
                    return new CalculadorPrecioCamioneta();
            }
        }
    }
}