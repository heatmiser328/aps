using System.Web.Mvc;

namespace aps.Web.Controllers
{
    public class HomeController : apsControllerBase
    {
        public ActionResult Index()
        { 
            return View("~/App/Main/views/layout/layout.cshtml"); //Layout of the angular application.
        }
	}
}