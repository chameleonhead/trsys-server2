using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Trsys.Frontend.Web.Controllers
{
    [Route("admin")]
    public class AdminController : Controller
    {
        [HttpGet("")]
        public ActionResult Index()
        {
            return RedirectToAction(nameof(Dashboard));
        }

        [HttpGet("dashboard")]
        public ActionResult Dashboard()
        {
            return View();
        }

        [HttpGet("clients")]
        public ActionResult Clients()
        {
            return View();
        }

        [HttpGet("clients/{id}")]
        public ActionResult ClientDetails(string id)
        {
            return View();
        }

        [HttpGet("order")]
        public ActionResult Order()
        {
            return View();
        }

        [HttpGet("history")]
        public ActionResult History()
        {
            return View();
        }

        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
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

        public ActionResult Edit(int id)
        {
            return View();
        }

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

        public ActionResult Delete(int id)
        {
            return View();
        }

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
