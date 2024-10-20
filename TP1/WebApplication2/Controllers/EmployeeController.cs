using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;
using WebApplication2.Models.Repositories;









namespace WebApplication1.Controllers
{

    public class EmployeeController : Controller
    {
        readonly IRepository<Employee> EmployeRepository ;
        //injection de dépendance
        public EmployeeController(IRepository<Employee> empRepository)
        {
            EmployeRepository = empRepository;
        }


        // GET: EmployeeController
        public ActionResult Index()
        {
            var employees = EmployeRepository.GetAll();
            ViewData["EmployeeCount"] = employees.Count;
            ViewData["SalaryAverage"] = EmployeRepository.SalaryAverage();
            ViewData["MaxSalary"] = EmployeRepository.MaxSalary();
            ViewData["HREmployeesCount"] = EmployeRepository.HrEmployeesCount();
            return View(employees);
        }

        // GET: EmployeeController/Details/5
        public ActionResult Details(int id)
        {
            var employee = EmployeRepository.FindByID(id);
            return View(employee);
        }

        // GET: EmployeeController/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: EmployeeController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Employee e)
        {
            try


            {
                EmployeRepository.Add(e);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: EmployeeController/Edit/5
        public ActionResult Edit(int id)
        {
            var employee = EmployeRepository.FindByID(id);
            return View();
        }

        // POST: EmployeeController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Employee newemployee)
        {
            try
            {
                EmployeRepository.Update(id, newemployee);

                return RedirectToAction(nameof(Index));
            }
            catch
            {

                return View();
            }
        }

        // GET: EmployeeController/Delete/5
        public ActionResult Delete(int id)
        {
            EmployeRepository.Delete(id);
            return View();
        }

        // POST: EmployeeController/Delete/5
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

        public ActionResult Search(string term)
        {

            var result = EmployeRepository.Search(term);
            return View("Index", result);
        }
    }
}
