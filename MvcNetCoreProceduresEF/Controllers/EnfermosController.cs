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

        public async Task<IActionResult> Details(string inscripcion)
        {
            Enfermo e = await this.repo.FindEnfermoAsync(inscripcion);
            return View(e);
        }

        public IActionResult Delete(string inscripcion)
        {
            this.repo.DeleteEnfermoRawAsync(inscripcion);
            return RedirectToAction("Index");
        }

        public IActionResult Insert()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Insert(Enfermo e)
        {
            await this.repo.InsertEnfermoAsync(e.Apellido, e.Direccion, e.FechaNacimiento, e.Genero);
            return RedirectToAction("Index");
        }
    }
}
