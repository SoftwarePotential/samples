using System.Web.Mvc;

namespace Sp.Samples.Agent.Mvc.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

		public ActionResult Licensing()
		{
			return View();
		}
    }
}
