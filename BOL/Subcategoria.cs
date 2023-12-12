using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using DAL;

namespace BOL
{
    public class Subcategoria
    {
        DBAccess conexion = new DBAccess();

        public DataTable listar(int idcategoria)
        {
            return conexion.listardatosvariable("spu_subcategoria_listar", "@idcategoria", idcategoria);
        }
    }
}
