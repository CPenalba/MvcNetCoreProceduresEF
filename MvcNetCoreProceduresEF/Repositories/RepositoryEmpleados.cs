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
