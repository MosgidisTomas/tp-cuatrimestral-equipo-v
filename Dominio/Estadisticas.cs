using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class ProductoVendido
    {
        public string Nombre { get; set; }
        public int Unidades { get; set; }
        public decimal Total { get; set; }
    }

    public class VendedorDelMes
    {
        public string Nombre { get; set; }
        public int Ventas { get; set; }
        public decimal Total { get; set; }
    }

    public class VentaDiaria
    {
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public int Porcentaje { get; set; }
    }
}
