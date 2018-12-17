using Rest_Api.Context;
using Rest_Api.Models;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Rest_Api.Controllers
{
	/// <summary>
	///     Kontroler API dla Wypożyczenia Książki przez Czytelnika.
	/// </summary>
	[EnableCors(origins: "*", headers: "*", methods: "GET, POST, PUT, DELETE, PATCH")]
	public class LendController : ApiController
	{
		/// <summary>
		///     Kontekst.
		/// </summary>
		protected EFCContext Context { get; set; }

		/// <summary>
		///     Konstruktor.
		/// </summary>
		/// <param name="context">Obiekt Kontekstu.</param>
		public LendController(EFCContext context)
		{
			Context = context;
		}

		/// <summary>
		///     Akcja Read List, zwracająca wszystkie wypożyczone książki. Dostępna pod API: <strong>/api/lend</strong>.
		/// </summary>
		/// <returns>Tablica z obiektami.</returns>
		public IHttpActionResult Get()
		{
			var list = Context.BookReader.Include(x => x.Book).Include(x => x.Reader).Select(x => new
			{
				ID = x.ID,
				BookID = x.BookID,
				ReaderID = x.ReaderID,
				LendDate = x.LendDate,
				Name = x.Reader.Name,
				Title = x.Book.Title
			}).ToArray();

			return Ok<object>(list);
		}

		/// <summary>
		///     Akcja Create, tworząca nowy obiekt. Dostępna pod API: <strong>/api/lend</strong>.
		/// </summary>
		/// <param name="input">Model wypożyczenia książki</param>
		///     Kod 400 jeśli walidacja danych nie powiodła się, kod 409 jeśli zasób już istnieje.
		///     Kod 200 jeśli aktualizacja przebiegła pomyślnie.
		public IHttpActionResult Post(BookReaderModel input)
		{
			if (input.BookID == 0 || input.ReaderID == 0)
			{
				return StatusCode(HttpStatusCode.BadRequest);
			}
			var record = Context.BookReader.SingleOrDefault(x => x.BookID == input.ID);
			if (record != null)
			{
				return StatusCode(HttpStatusCode.Conflict);
			}
			var bookReaderModel = new BookReaderModel()
			{
				BookID = input.ID,
				ReaderID = input.ReaderID,
				LendDate = input.LendDate
			};
			Context.BookReader.Add(bookReaderModel);
			Context.SaveChanges();
			return StatusCode(HttpStatusCode.Created);
		}

		/// <summary>
		///     Akcja Delete, usuwająca zasób. Dostępna pod API: <strong>/api/lend/1</strong>.
		/// </summary>
		/// <param name="id">Identyfikator zasobu.</param>
		/// <returns>
		///     Kod 404 jeśli nie można odnaleźć zasobu o podanym identyfikatorze. Kod 200 jeśli zasób został usunięty
		///     pomyślnie.
		/// </returns>
		public IHttpActionResult Delete(int id)
		{
			var record = Context.BookReader.SingleOrDefault(x => x.ID == id);
			if (record == null)
			{
				return NotFound();
			}
			Context.BookReader.Remove(record);
			Context.SaveChanges();
			return Ok();
		}
	}
}