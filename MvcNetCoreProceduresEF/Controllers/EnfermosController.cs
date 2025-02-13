using Microsoft.AspNetCore.Mvc;
using MvcNetCoreProceduresEF.Models;
using MvcNetCoreProceduresEF.Repositories;

namespace MvcNetCoreProceduresEF.Controllers
{
    public class EnfermosController : Controller
    {

        RepositoryEnfermos repo;

        public EnfermosController(RepositoryEnfermos repo)
        {
            this.repo = repo;
        }
        public async Task<IActionResult> Index()
        {
            List<Enfermo> enfermos = await this.repo.GetEnfermosAsync();
            return View(enfermos);
        }

        public IActionResult Details(string inscripcion)
        {
            Enfermo e = this.repo.FindEnfermo(inscripcion);
            return View(e);
        }

        public IActionResult Delete(string inscripcion)
        {
            this.repo.DeleteEnfermoRawAsync(inscripcion);
            return RedirectToAction("Index");
        }
    }
}
