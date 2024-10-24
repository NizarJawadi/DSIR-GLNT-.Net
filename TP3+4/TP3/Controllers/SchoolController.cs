using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TP3.Models;
using TP3.Models.Repositories;

namespace TP3.Controllers
{
    [Authorize]
    public class SchoolController : Controller

    {
       readonly ISchoolRepository SchoolRepository;
        readonly IStudentRepository studentRepository;
        public SchoolController(ISchoolRepository schoolRepository)
        {
            SchoolRepository = schoolRepository;


        }

        [AllowAnonymous]
        // GET: SchoolController
        public ActionResult Index()
        {
            var school = SchoolRepository.GetAll();
            return View(school);
        }

        // GET: SchoolController/Details/5
        public ActionResult Details(int id)
        {
            
            var school = SchoolRepository.GetById(id);
            return View(school);
        }

        // GET: SchoolController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SchoolController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(School s)
        {
            try
            {
                SchoolRepository.Add(s);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SchoolController/Edit/5
        public ActionResult Edit(School s)
        {
            SchoolRepository.Edit(s);
            return View();
        }

        // POST: SchoolController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SchoolController/Delete/5
        public ActionResult Delete(School s)
        {
            SchoolRepository.Delete(s);
            return View();
        }

        // POST: SchoolController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
