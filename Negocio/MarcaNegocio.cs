using System;
using System.Collections.Generic;
using Dominio;

namespace Negocio
{
    public class MarcaNegocio
    {
        // soloActivas = true: solo las marcas activas (para listados y desplegables).
        // soloActivas = false: todas, incluidas las dadas de baja.
        public List<Marca> listar(bool soloActivas = true)
        {
            List<Marca> lista = new List<Marca>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearProcedimiento("SP_ListarMarcas");
                datos.setearParametro("@SoloActivas", soloActivas);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Marca aux = new Marca();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    aux.PaisOrigen = datos.Lector["PaisOrigen"] is DBNull ? null : (string)datos.Lector["PaisOrigen"];
                    aux.Activo = (bool)datos.Lector["Activo"];
                    lista.Add(aux);
                }

                return lista;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        // Devuelve el Id de la marca nueva.
        public int agregar(Marca nueva)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearProcedimiento("SP_AgregarMarca");
                datos.setearParametro("@Descripcion", nueva.Descripcion);
                datos.setearParametro("@PaisOrigen", nueva.PaisOrigen);
                return Convert.ToInt32(datos.ejecutarAccionEscalar());
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        public void modificar(Marca marca)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearProcedimiento("SP_ModificarMarca");
                datos.setearParametro("@Id", marca.Id);
                datos.setearParametro("@Descripcion", marca.Descripcion);
                datos.setearParametro("@PaisOrigen", marca.PaisOrigen);
                datos.ejecutarAccion();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }

        // Baja lógica: la marca queda con Activo = 0 y deja de aparecer en los listados.
        public void eliminar(int id)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearProcedimiento("SP_BajaMarca");
                datos.setearParametro("@Id", id);
                datos.ejecutarAccion();
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
    }
}
