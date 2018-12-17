using System.Web.Mvc;

namespace Rest_Api.Controllers
{
	/// <summary>
	///     Kontroler dla Widoków.
	/// </summary>
	public class HomeController : Controller
	{
		/// <summary>
		///     Zwraca główny widok.
		/// </summary>
		/// <returns>Widok.</returns>
		public ActionResult Index()
		{
			return View();
		}
	}
}