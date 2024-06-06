using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class GuestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
