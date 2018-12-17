using Rest_Api.Context;
using Rest_Api.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Rest_Api.Controllers
{
	/// <summary>
	///     Kontroler API dla Książek.
	/// </summary>
	[EnableCors(origins: "*", headers: "*", methods: "GET, POST, PUT, DELETE, PATCH")]
	public class BooksController : ApiController
	{
        //TODO: Dodaj obiekt kontekstu.
	    /// <summary>
	    ///     Kontekst.
	    /// </summary>
	    protected EFCContext Context { get; set; }

	    /// <summary>
	    ///     Konstruktor.
	    /// </summary>
	    /// <param name="context">Obiekt Kontekstu.</param>
	    public BooksController(EFCContext context)
	    {
	        Context = context;
	    }

	    /// <summary>
	    ///     Akcja Read List, zwracająca wszystkich czytelników. Dostępna pod API: <strong>/api/readers</strong>.
	    /// </summary>
	    /// <returns>Tablica z obiektami.</returns>
	    public IEnumerable<BookModel> Get() => Context.Books.ToArray();

	    /// <summary>
	    ///     Akcja Read, zwracająca obiekt o podanym ID. Dostępna pod API: <strong>/api/readers/1</strong>.
	    /// </summary>
	    /// <param name="id">Pole ID obiektu, który zostanie zwrócony.</param>
	    /// <returns>Znaleziony obiekt lub kod 404, gdy nie odnaleziono szukanego obiektu.</returns>
	    public IHttpActionResult Get(int id)
	    {
	        var record = Context.Books.SingleOrDefault(x => x.ID == id);
	        if (record == null)
	        {
	            return NotFound();
	        }
	        return Ok(record);
	    }
        
        //TODO: Dodaj metody umożliwiające pobieranie, tworzenie, aktualizację, usuwanie.
        public IHttpActionResult Post(BookModel input)
	    {
	        if (string.IsNullOrWhiteSpace(input.Title) || string.IsNullOrWhiteSpace(input.Author))
	        {
	            return StatusCode(HttpStatusCode.BadRequest);
	        }
	        var record = Context.Books.SingleOrDefault(x => x.Title == input.Title && x.Author == input.Author);
	        if (record != null)
	        {
	            return StatusCode(HttpStatusCode.Conflict);
	        }
	        var bookModel = new BookModel()
	        {
	            Title = input.Title,
	            Author = input.Author
	        };
	        Context.Books.Add(bookModel);
	        Context.SaveChanges();
	        return Created("Get", bookModel.ID);
	    }

	    public IHttpActionResult Put(int id, BookModel input)
	    {
	        if (string.IsNullOrWhiteSpace(input.Title) || string.IsNullOrWhiteSpace(input.Author))
	        {
	            return StatusCode(HttpStatusCode.BadRequest);
	        }
            var record = Context.Books.SingleOrDefault(x => x.ID == id);
	        if (record == null)
	        {
	            return NotFound();
	        }
	        record.Title = input.Title;
	        record.Author = input.Author;
	        Context.SaveChanges();
	        return Ok();
	    }

    }
}