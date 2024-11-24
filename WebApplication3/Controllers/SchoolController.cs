using Microsoft.AspNetCore.Mvc;

using WebApplication3.Models.Repositories;
using WebApplication3.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication3.Controllers
{
    [Authorize(Roles = "Admin,Manager")]

    public class SchoolController : Controller
    {
        //injection de dépendane
        readonly ISchoolRepository SchoolRepository;
        public SchoolController(ISchoolRepository SchoolRepository)
        {
            this.SchoolRepository = SchoolRepository;
        }
        // GET: ProductController
        public ActionResult Index()
        {
            var Schools = SchoolRepository.GetAll();
            return View(Schools);
        }

        // GET: SchoolController/Details/5
        public ActionResult Details(int id)
        {
            var School = SchoolRepository.GetById(id);

            return View(School);

        }

        // GET: SController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SchoolController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(School S)
        {
            try
            {

                SchoolRepository.Add(S);
                return RedirectToAction(nameof(Index));


            }
            catch
            {
                return View();
            }
        }


        // GET: StudentController/Edit/5
        public ActionResult Edit(int id)
        {
            // Récupérer l'étudiant par son ID
            var school = SchoolRepository.GetById(id);
            if (school == null)
            {
                return NotFound(); // Si l'étudiant n'existe pas, retourner une erreur 404
            }

            
            return View(school);
        }

        // POST: StudentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, School school)
        {
            if (id != school.SchoolID)
            {
                return NotFound(); // Si l'ID dans l'URL ne correspond pas à celui de l'étudiant, erreur 404
            }

            try
            {
                // Mise à jour de l'étudiant dans le repository
                SchoolRepository.Edit(school); // Pas besoin de passer l'ID, le student contient déjà l'ID

                return RedirectToAction(nameof(Index)); // Rediriger vers la liste des étudiants
            }
            catch
            {
                // En cas d'erreur, retourner la vue avec l'étudiant et les écoles
                return View(school);
            }

        }




        // GET: SchoolController/Delete/5
        public ActionResult Delete(int id)
        {
            var school = SchoolRepository.GetById(id);

            return View(school);
        }

        // POST: SchoolController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(School s)
        {
            try
            {
                SchoolRepository.Delete(s);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}