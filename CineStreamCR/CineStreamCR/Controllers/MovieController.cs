using Microsoft.AspNetCore.Mvc;

namespace StreamingApp.Controllers
{
    public class MovieController : Controller
    {
        public IActionResult Detalles(int id)
        {
            return View();
        }
    }
}