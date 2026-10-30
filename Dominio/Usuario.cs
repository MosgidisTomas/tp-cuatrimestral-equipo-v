using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Usuario
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }

        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public TipoPerfil Perfil { get; set; }
        public bool Activo { get; set; } = true;

        public bool EsAdministrador
        {
            get { return Perfil == TipoPerfil.Administrador; }
        }

        public string NombreCompleto
        {
            get { return Nombre + " " + Apellido; }
        }

        public override string ToString()
        {
            return NombreUsuario;
        }
    }
}
