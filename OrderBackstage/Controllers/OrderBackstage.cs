using Microsoft.AspNetCore.Mvc;

namespace OrderBackstage.Controllers
{
	public class OrderBackstage : Controller
	{
		public IActionResult Index()
		{
			return View();
		}


	}
}
