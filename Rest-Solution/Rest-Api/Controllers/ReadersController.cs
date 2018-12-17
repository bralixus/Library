using Rest_Api.Context;
using Rest_Api.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Rest_Api.Controllers
{
	/// <summary>
	///     Kontroler API dla Czytelników.
	/// </summary>
	[EnableCors(origins: "*", headers: "*", methods: "GET, POST, PUT, DELETE, PATCH")]
	public class ReadersController : ApiController
	{
		/// <summary>
		///     Kontekst.
		/// </summary>
		protected EFCContext Context { get; set; }

		/// <summary>
		///     Konstruktor.
		/// </summary>
		/// <param name="context">Obiekt Kontekstu.</param>
		public ReadersController(EFCContext context)
		{
			Context = context;
		}

		/// <summary>
		///     Akcja Read List, zwracająca wszystkich czytelników. Dostępna pod API: <strong>/api/readers</strong>.
		/// </summary>
		/// <returns>Tablica z obiektami.</returns>
		public IEnumerable<ReaderModel> Get() => Context.Readers.ToArray();

		/// <summary>
		///     Akcja Read, zwracająca obiekt o podanym ID. Dostępna pod API: <strong>/api/readers/1</strong>.
		/// </summary>
		/// <param name="id">Pole ID obiektu, który zostanie zwrócony.</param>
		/// <returns>Znaleziony obiekt lub kod 404, gdy nie odnaleziono szukanego obiektu.</returns>
		public IHttpActionResult Get(int id)
		{
			var record = Context.Readers.SingleOrDefault(x => x.ID == id);
			if (record == null)
			{
				return NotFound();
			}
			return Ok(record);
		}

		/// <summary>
		///     Akcja Create, tworząca nowy obiekt. Dostępna pod API: <strong>/api/readers</strong>.
		/// </summary>
		/// <param name="input">Obiekt czytelnika.</param>
		/// <returns>
		///     Kod 400 jeśli walidacja danych nie powiodła się, kod 409 jeśli zasób już istnieje.
		///     Kod 200 jeśli aktualizacja przebiegła pomyślnie.
		/// </returns>
		public IHttpActionResult Post(ReaderModel input)
		{
			if (string.IsNullOrWhiteSpace(input.Name) || input.Age < 10)
			{
				return StatusCode(HttpStatusCode.BadRequest);
			}

            var record = Context.Readers.SingleOrDefault(x => x.Name == input.Name && x.Age == input.Age);

			if (record != null)
			{
				return StatusCode(HttpStatusCode.Conflict);
			}
			var readerModel = new ReaderModel()
			{
				Name = input.Name,
				Age = input.Age
			};
			Context.Readers.Add(readerModel);
			Context.SaveChanges();
			return Created("Get", readerModel.ID);
		}

		/// <summary>
		///     Akcja Update, aktualizująca obiekt. Dostępna pod API: <strong>/api/readers/1</strong>.
		/// </summary>
		/// <param name="input">Obiekt czytelnika.</param>
		/// <returns>
		///     Kod 400 jeśli walidacja danych nie powiodła się, kod 404 jeśli nie można odnaleźć zasobu o podanym identyfikatorze.
		///     Kod 200 jeśli aktualizacja przebiegła pomyślnie.
		/// </returns>
		public IHttpActionResult Put(int id, ReaderModel input)
		{
			if (string.IsNullOrWhiteSpace(input.Name) || input.Age < 10)
			{
				return StatusCode(HttpStatusCode.BadRequest);
			}
			var record = Context.Readers.SingleOrDefault(x => x.ID == id);
			if (record == null)
			{
				return NotFound();
			}
			record.Name = input.Name;
			record.Age = input.Age;
			Context.SaveChanges();
			return Ok();
		}

		/// <summary>
		///     Akcja Delete, usuwająca zasób. Dostępna pod API: <strong>/api/readers/1</strong>.
		/// </summary>
		/// <param name="id">Identyfikator zasobu.</param>
		/// <returns>
		///     Kod 404 jeśli nie można odnaleźć zasobu o podanym identyfikatorze. Kod 200 jeśli zasób został usunięty
		///     pomyślnie.
		/// </returns>
		public IHttpActionResult Delete(int id)
		{
			var record = Context.Readers.SingleOrDefault(x => x.ID == id);
			if (record == null)
			{
				return NotFound();
			}
			Context.Readers.Remove(record);
			Context.SaveChanges();
			return Ok();
		}
	}
}