using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication3.Models.Repositories;
using WebApplication3.Models;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication3.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class StudentController : Controller
    {
        readonly IStudentRepository StudentRepository;
        readonly ISchoolRepository SchoolRepository;

        // Injection des dépendances
        public StudentController(IStudentRepository studentRepository, ISchoolRepository schoolRepository)
        {
            StudentRepository = studentRepository;
            SchoolRepository = schoolRepository;
        }

       
        // GET: StudentController
        public ActionResult Index()
        {
            // Charger la liste des écoles dans le ViewBag pour l'affichage dans la liste déroulante
            ViewBag.SchoolID = new SelectList(SchoolRepository.GetAll(), "SchoolID", "SchoolName");

            // Récupérer tous les étudiants (en initial)
            var students = StudentRepository.GetAll();

            return View(students);
        }



        // GET: StudentController/Details/5
        public ActionResult Details(int id)
        {
            // Récupère l'étudiant par son ID
            var student = StudentRepository.GetById(id);
            if (student == null)
            {
                return NotFound(); // Si l'étudiant n'est pas trouvé, retourner une erreur 404
            }

            return View(student); // Passer l'étudiant trouvé à la vue
        }

        // GET: StudentController/Create
        public ActionResult Create()
        {
            // Créer la liste déroulante des écoles
            ViewBag.SchoolID = new SelectList(SchoolRepository.GetAll(), "SchoolID", "SchoolName");
            return View();
        }

        // POST: StudentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Student student)
        {
            // Créer la liste déroulante des écoles
            ViewBag.SchoolID = new SelectList(SchoolRepository.GetAll(), "SchoolID", "SchoolName");

            try
            {
                // Vérifier si l'étudiant existe déjà
                if (StudentRepository.GetAll().Contains(student))
                {
                    ModelState.AddModelError("id_exist", "ID déjà existant");
                    return View(); // Si l'étudiant existe, revenir à la vue avec l'erreur
                }
                else
                {
                    // Ajouter l'étudiant au dépôt
                    StudentRepository.Add(student);
                    return RedirectToAction(nameof(Index)); // Rediriger vers la liste des étudiants
                }
            }
            catch
            {
                // En cas d'erreur, revenir à la vue
                return View();
            }
        }

        // GET: StudentController/Edit/5
        public ActionResult Edit(int id)
        {
            // Récupérer l'étudiant par son ID
            var student = StudentRepository.GetById(id);
            if (student == null)
            {
                return NotFound(); // Si l'étudiant n'existe pas, retourner une erreur 404
            }

            // Créer la liste déroulante des écoles et présélectionner l'école de l'étudiant
            ViewBag.SchoolID = new SelectList(SchoolRepository.GetAll(), "SchoolID", "SchoolName", student.SchoolID);

            // Passer l'étudiant à la vue
            return View(student);
        }

        // POST: StudentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Student student)
        {
            if (id != student.StudentId)
            {
                return NotFound(); // Si l'ID dans l'URL ne correspond pas à celui de l'étudiant, erreur 404
            }

            try
            {
                // Mise à jour de l'étudiant dans le repository
                StudentRepository.Edit(student); // Pas besoin de passer l'ID, le student contient déjà l'ID

                return RedirectToAction(nameof(Index)); // Rediriger vers la liste des étudiants
            }
            catch
            {
                // En cas d'erreur, retourner la vue avec l'étudiant et les écoles
                ViewBag.SchoolID = new SelectList(SchoolRepository.GetAll(), "SchoolID", "SchoolName", student.SchoolID);
                return View(student);
            }
        }




        // GET: StudentController/Delete/5
        public ActionResult Delete(int id)
        {
            var student = StudentRepository.GetById(id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: StudentController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                var student = StudentRepository.GetById(id);
                if (student != null)
                {
                    StudentRepository.Delete(student); // Supprimer l'étudiant
                }
                return RedirectToAction(nameof(Index)); // Rediriger vers la liste des étudiants
            }
            catch
            {
                return View();
            }
        }

        // GET: StudentController/Search

        // GET: StudentController/Search
        public ActionResult Search(string name, int? schoolid)
        {
            // Récupérer tous les étudiants (initialement, sans filtre)
            var result = StudentRepository.GetAll();

            // Si un nom est spécifié, filtrer les étudiants par nom
            if (!string.IsNullOrEmpty(name))
            {
                result = result.Where(s => s.StudentName.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Si un schoolid est spécifié, filtrer les étudiants par école
            if (schoolid.HasValue)
            {
                result = result.Where(s => s.SchoolID == schoolid.Value).ToList();
            }

            // Recharger la liste des écoles pour l'affichage dans la vue
            ViewBag.SchoolID = new SelectList(SchoolRepository.GetAll(), "SchoolID", "SchoolName");

            // Retourner la vue avec les résultats filtrés
            return View("Index", result);
        }


    }
}
