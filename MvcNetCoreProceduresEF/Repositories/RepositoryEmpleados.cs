using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using MvcNetCoreProceduresEF.Data;
using MvcNetCoreProceduresEF.Models;

#region
//CREATE VIEW V_EMPLEADOS_DEPARTAMENTOS
//AS
//SELECT CAST(ISNULL(ROW_NUMBER() OVER (ORDER BY APELLIDO), 0) AS int) as ID,
//EMP.APELLIDO, EMP.OFICIO, EMP.SALARIO, DEPT.DNOMBRE AS DEPARTAMENTO,
//DEPT.LOC AS LOCALIDAD FROM EMP
//INNER JOIN DEPT ON EMP.DEPT_NO = DEPT.DEPT_NO
//GO
//CREATE VIEW V_WORKERS
//AS
//SELECT EMP_NO AS IDWORKER, APELLIDO, OFICIO, SALARIO FROM EMP UNION
//SELECT DOCTOR_NO, APELLIDO, ESPECIALIDAD, SALARIO FROM DOCTOR UNION
//SELECT EMPLEADO_NO, APELLIDO, FUNCION, SALARIO FROM PLANTILLA
//GO
//CREATE PROCEDURE SP_WORKERS_OFICIO
//(@oficio nvarchar(50), @personas int out, @media int out, @suma int out)
//AS
//SELECT * FROM V_WORKERS WHERE OFICIO=@oficio
//SELECT @personas = COUNT(IDWORKER), @media = AVG(SALARIO), @suma = SUM(SALARIO)
//FROM V_WORKERS WHERE OFICIO=@oficio
//GO
#endregion
namespace MvcNetCoreProceduresEF.Repositories
{
    public class RepositoryEmpleados
    {
        private HospitalContext context;

        public RepositoryEmpleados(HospitalContext context)
        {
            this.context = context;
        }

        public async Task<List<VistaEmpleado>> GetVistaEmpleadosAsync()
        {
            var consulta = from datos in this.context.VistasEmpleados select datos;
            return await consulta.ToListAsync();
        }
    }
}
