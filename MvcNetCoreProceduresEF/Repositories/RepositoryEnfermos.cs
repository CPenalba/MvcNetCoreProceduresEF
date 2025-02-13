using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MvcNetCoreProceduresEF.Data;
using MvcNetCoreProceduresEF.Models;
using System.Data.Common;

#region
//create procedure SP_TODOS_ENFERMOS
//AS 
//SELECT * FROM ENFERMO
//GO
//CREATE PROCEDURE SP_FIND_ENFERMO
//(@inscripcion nvarchar(50))
//AS
//SELECT * FROM ENFERMO WHERE INSCRIPCION=@inscripcion
//GO
//CREATE PROCEDURE SP_DELETE_ENFERMO
//(@inscripcion nvarchar(50))
//AS
//DELETE FROM ENFERMO WHERE INSCRIPCION=@inscripcion
//GO
#endregion

namespace MvcNetCoreProceduresEF.Repositories
{
    public class RepositoryEnfermos
    {
        private EnfermosContext context;

        public RepositoryEnfermos(EnfermosContext context)
        {
            this.context = context;
        }

        public async Task<List<Enfermo>> GetEnfermosAsync()
        {
            //PARA CONSULTAS DE SELECCION DEBEMOS MAPEAR 
            //MANUALMENTE LOS DATOS.
            using (DbCommand com =
                this.context.Database.GetDbConnection().CreateCommand())
            {
                string sql = "SP_TODOS_ENFERMOS";
                com.CommandType = System.Data.CommandType.StoredProcedure;
                com.CommandText = sql;
                //ABRIMOS LA CONEXION A TRAVES DEL COMMAND
                await com.Connection.OpenAsync();
                //EJECUTAMOS NUESTRO READER
                DbDataReader reader = await com.ExecuteReaderAsync();
                List<Enfermo> enfermos = new List<Enfermo>();
                while (await reader.ReadAsync())
                {
                    Enfermo enfermo = new Enfermo
                    {
                        Inscripcion = reader["INSCRIPCION"].ToString(),
                        Apellido = reader["APELLIDO"].ToString(),
                        Direccion = reader["DIRECCION"].ToString(),
                        FechaNacimiento =
                        DateTime.Parse(reader["FECHA_NAC"].ToString()),
                        Genero = reader["S"].ToString()
                    };
                    enfermos.Add(enfermo);
                }
                await reader.CloseAsync();
                await com.Connection.CloseAsync();
                return enfermos;
            }
        }
        
        public Enfermo FindEnfermo(string inscripcion)
        {
            //PARA LLAMAR A PROCEDIMIENTOS ALAMCENADOS CON PARAMETROS LA LLAMADA SE REALIZA MEDIANTE EL NOMBRE DEL PROCEDIMINETO 
            //Y CADA PARAMETRO A CONTINUACION SEPARADO MEDIANTE COMAS SP_PROCEDIMIENTO @PARAM1, @PARAM2
            string sql = "SP_FIND_ENFERMO @INSCRIPCION";
            //DEBEMOS CREAR LOS PARAMETROS
            SqlParameter pamIns = new SqlParameter("@INSCRIPCION", inscripcion);
            //SI LOS DATOS QUE DEVUELVE EL PROCEDIMIENTO ESTAN`MAPEADOS CON UN MODEL, PODEMOS UTILIZAR EL METODO FROMSQLRAW CON LINQ
            //CUANDO UTILIZAMOS LINQ CON PROCEDIMIENTOS ALMACENADOS LA CONSULTA Y LA EXTRACCION DE DATOS SE REALIZAN EN DOS PASOS
            //NO SE PUEDE HCAER LINQ Y EXTAER A LA VEZ
            var consulta = this.context.Enfermos.FromSqlRaw(sql, pamIns);
            //PARA EXTRAER LOS DATOS NECESITAMOS TAMBIEN EL METODO ASENUMERABLE()
            Enfermo enfermo = consulta.AsEnumerable().FirstOrDefault();
            return enfermo;
        }

        public void DeleteEnfermo(string inscripcion)
        {
            string sql = "SP_DELETE_ENFERMO";
            SqlParameter pamIns = new SqlParameter("@INSCRIPCION", inscripcion);
            using (DbCommand com = this.context.Database.GetDbConnection().CreateCommand())
            {
                com.CommandType = System.Data.CommandType.StoredProcedure;
                com.CommandText = sql;
                com.Parameters.Add(pamIns);
                com.Connection.Open();
                com.ExecuteNonQuery();
                com.Connection.Close();
                com.Parameters.Clear();
            }
        }

        public async void DeleteEnfermoRawAsync(string inscripcion)
        {
            string sql = "SP_DELETE_ENFERMO @INSCRIPCION";
            SqlParameter pamIns = new SqlParameter("@INSCRIPCION", inscripcion);
            //DENTRO DEL CONTEXT TENEMOS UN METODO PARA PODER LLAMAR A PROCEDIMIENTOS DE CONSULTAS DE ACCION
            this.context.Database.ExecuteSqlRawAsync(sql, pamIns);
        }
    }
}
