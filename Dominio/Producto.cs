using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Producto
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string UrlImagen { get; set; }

        public Marca Marca { get; set; }
        public Categoria Categoria { get; set; }

        public List<Proveedor> Proveedores { get; set; } = new List<Proveedor>();

        public int StockActual { get; set; }
        public int StockMinimo { get; set; }

        public decimal PorcentajeGanancia { get; set; }

        public decimal UltimoPrecioCompra { get; set; }

        public bool TieneCompras { get; set; }

        public bool Activo { get; set; } = true;

        public decimal PrecioVenta
        {
            get { return Math.Round(UltimoPrecioCompra * (1 + PorcentajeGanancia / 100), 2); }
        }

        public bool NecesitaReposicion
        {
            get { return StockActual <= StockMinimo; }
        }

        public override string ToString()
        {
            return Nombre;
        }
    }
}
