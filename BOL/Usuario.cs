using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using System.Data;
using System.Data.SqlClient;
using ENTITIES;

namespace BOL
{
    public class Usuario
    {
        DBAccess conexion = new DBAccess();
        /// <summary>
        /// Inicia sesion utiluzando datos del servidor
        /// </summary>
        /// <param name="email">Identificador de usuario</param>
        /// <returns>
        /// Objeto conteniendo toDA LA FILA
        /// </returns>
        public DataTable iniciarSesion(string email)
        {
            //1. objeto que contendrá el resultado
            DataTable dt = new DataTable(); ;

            //2. Abrir conexión
            conexion.abrirConexion();

            //3. Objeto para enviar consulta
            SqlCommand comando = new SqlCommand("spu_usuarios_login", conexion.getConnection());


            //4. Tipo de comando (Procedimiento almacenado)
            comando.CommandType = CommandType.StoredProcedure;

            //5. Pasar la(s) variable(s)
            comando.Parameters.AddWithValue("@email", email);

            //6. Ejecutar y obtener/leer los datos
            dt.Load(comando.ExecuteReader());

            //7. Cerrar
            conexion.cerrarConexion();

            //8. Reornamos el objeto con la info

            return dt;
        }

        public DataTable login(string email)
        {
            return conexion.listardatosvariable("spu_usuarios_login", "@email", email);
        }



        /// <summary>
        /// Registra un Usuario ya sera Admin o INV 
        /// </summary>
        /// <param name="entidad">
        /// Persona registrada
        /// </param>
        public int Registrar(EUsuario entidad)
        {
            int totalRegistros = 0;
            SqlCommand comando = new SqlCommand("spu_usuarios_registrar", conexion.getConnection());
            comando.CommandType = CommandType.StoredProcedure;
            conexion.abrirConexion();
            try
            {
                comando.Parameters.AddWithValue("@apellidos", entidad.apellidos);
                comando.Parameters.AddWithValue("@nombres", entidad.nombres);
                comando.Parameters.AddWithValue("@email", entidad.email);
                comando.Parameters.AddWithValue("@claveacceso", entidad.claveacceso);
                comando.Parameters.AddWithValue("@nivelacceso", entidad.nivelAcceso);


                //solo ejecuta
                totalRegistros = comando.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                totalRegistros = -1;
            }
            finally
            {
                conexion.cerrarConexion();
            }

            return totalRegistros;
        }
        public DataTable Listar()
        {
            DataTable dt = new DataTable();
            SqlCommand comando = new SqlCommand("spu_usuarios_listar", conexion.getConnection());
            comando.CommandType = CommandType.StoredProcedure;

            conexion.abrirConexion();
            dt.Load(comando.ExecuteReader());
            conexion.cerrarConexion();

            return dt;
        }
    }
}
