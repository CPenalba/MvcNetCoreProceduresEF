using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using MvcNetCoreProceduresEF.Data;
using MvcNetCoreProceduresEF.Models;
using System.Data.Common;

#region
//CREATE PROCEDURE SP_TODOS_DOCTORES
//AS
//SELECT * FROM DOCTOR
//GO
//CREATE PROCEDURE SP_INCREMENTAR_SALARIO
//(
//    @especialidad nvarchar(50),
//    @salarioIncremento int
//)
//AS
//    UPDATE DOCTOR
//    SET SALARIO = SALARIO + @salarioIncremento
//    WHERE ESPECIALIDAD = @especialidad
//GO
//CREATE PROCEDURE SP_OBTENER_ESPECIALIDADES
//AS
//    SELECT DISTINCT ESPECIALIDAD FROM DOCTOR;
//GO

//CREATE PROCEDURE SP_DOCTORES_ESPECIALIDAD
//(@especialidad NVARCHAR(50))
//AS
//    SELECT * FROM DOCTOR WHERE ESPECIALIDAD = @especialidad;
//GO
#endregion

namespace MvcNetCoreProceduresEF.Repositories
{
    public class RepositoryDoctores
    {
        private HospitalContext context;

        public RepositoryDoctores(HospitalContext context)
        {
            this.context = context;
        }

        public async Task<List<Doctor>> GetDoctoresAsync()
        {
            using (DbCommand com =
                this.context.Database.GetDbConnection().CreateCommand())
            {
                string sql = "SP_TODOS_DOCTORES";
                com.CommandType = System.Data.CommandType.StoredProcedure;
                com.CommandText = sql;
                await com.Connection.OpenAsync();
                DbDataReader reader = await com.ExecuteReaderAsync();
                List<Doctor> doctores = new List<Doctor>();
                while (await reader.ReadAsync())
                {
                    Doctor d = new Doctor
                    {
                        IdHospital = int.Parse(reader["HOSPITAL_COD"].ToString()),
                        IdDoctor = int.Parse(reader["DOCTOR_NO"].ToString()),
                        Apellido = reader["APELLIDO"].ToString(),
                        Especialidad = reader["ESPECIALIDAD"].ToString(),
                        Salario = int.Parse(reader["SALARIO"].ToString())
                    };
                    doctores.Add(d);
                }
                await reader.CloseAsync();
                await com.Connection.CloseAsync();
                return doctores;
            }
        }

        public async Task<List<string>> GetEspecialidadesAsync()
        {
            using (DbCommand com = this.context.Database.GetDbConnection().CreateCommand())
            {
                string sql = "SP_OBTENER_ESPECIALIDADES";
                com.CommandType = System.Data.CommandType.StoredProcedure;
                com.CommandText = sql;
                await com.Connection.OpenAsync();
                DbDataReader reader = await com.ExecuteReaderAsync();
                List<string> especialidades = new List<string>();
                while (await reader.ReadAsync())
                {
                    especialidades.Add(reader["ESPECIALIDAD"].ToString());
                }
                await reader.CloseAsync();
                await com.Connection.CloseAsync();
                return especialidades;
            }
        }

        public async Task<List<Doctor>> GetDoctoresPorEspecialidadAsync(string especialidad)
        {
            using (DbCommand com =
                this.context.Database.GetDbConnection().CreateCommand())
            {
                string sql = "SP_DOCTORES_ESPECIALIDAD";
                com.CommandType = System.Data.CommandType.StoredProcedure;
                com.CommandText = sql;
                com.Parameters.Add(new SqlParameter("@especialidad", especialidad));
                await com.Connection.OpenAsync();
                DbDataReader reader = await com.ExecuteReaderAsync();
                List<Doctor> doctores = new List<Doctor>();
                while (await reader.ReadAsync())
                {
                    Doctor d = new Doctor
                    {
                        IdHospital = int.Parse(reader["HOSPITAL_COD"].ToString()),
                        IdDoctor = int.Parse(reader["DOCTOR_NO"].ToString()),
                        Apellido = reader["APELLIDO"].ToString(),
                        Especialidad = reader["ESPECIALIDAD"].ToString(),
                        Salario = int.Parse(reader["SALARIO"].ToString())
                    };
                    doctores.Add(d);
                }
                await reader.CloseAsync();
                await com.Connection.CloseAsync();
                return doctores;
            }
        }

        public async Task IncrementarSalario(string especialidad, int incremento)
        {   
            string sql = "SP_INCREMENTAR_SALARIO";
            using (DbCommand com = this.context.Database.GetDbConnection().CreateCommand())
            {
                com.CommandType = System.Data.CommandType.StoredProcedure;
                com.CommandText = sql;            
                com.Parameters.Add(new SqlParameter("@especialidad", especialidad));
                com.Parameters.Add(new SqlParameter("@salarioIncremento", incremento));
                await com.Connection.OpenAsync();
                await com.ExecuteNonQueryAsync(); 
                await com.Connection.CloseAsync();
            }
        }
    }
}
