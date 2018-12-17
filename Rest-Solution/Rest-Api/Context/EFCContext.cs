using Bogus;
using Rest_Api.Extensions;
using Rest_Api.Models;
using System;
using System.Data.Common;
using System.Data.Entity;

namespace Rest_Api.Context
{
	/// <summary>
	///     Klasa kontekstu dostępu do bazy danych.
	/// </summary>
	public class EFCContext : DbContext
	{
		/// <summary>
		///     Książki.
		/// </summary>
		public DbSet<BookModel> Books { get; set; }

		/// <summary>
		///     Czytelnicy.
		/// </summary>
		public DbSet<ReaderModel> Readers { get; set; }

		/// <summary>
		///     Relacja między Książkami, a Czytelnikami.
		/// </summary>
		public DbSet<BookReaderModel> BookReader { get; set; }

		public EFCContext()
		{

		}

		public EFCContext(DbConnection connection)
			: base(connection, true)
		{
			CreateData();
		}

		private void CreateData()
		{
			var bookFaker = new Faker<BookModel>();
			bookFaker.RuleFor(x => x.Title, faker => faker.Lorem.Word().ToUpperFirstLetter());
			bookFaker.RuleFor(x => x.Author, faker => $"{faker.Person.FirstName} {faker.Person.LastName}");
			bookFaker.RuleFor(x => x.ID, f => f.IndexFaker);
			var bookModels = bookFaker.Generate(6);

			foreach (var b in bookModels)
			{
				Console.WriteLine($"Book: {b.ID}, {b.Title}, {b.Author}");
			}

			var readerFaker = new Faker<ReaderModel>();
			readerFaker.RuleFor(x => x.Name, faker => $"{faker.Person.FirstName} {faker.Person.LastName}");
			readerFaker.RuleFor(x => x.Age, faker => faker.Random.Number(10, 100));
			readerFaker.RuleFor(x => x.ID, f => f.IndexFaker);
			var readerModels = readerFaker.Generate(8);

			foreach (var r in readerModels)
			{
				Console.WriteLine($"Reader: {r.ID}, {r.Name}, {r.Age}");
			}

			var bookReaderFaker = new Faker<BookReaderModel>();
			bookReaderFaker.RuleFor(x => x.BookID, faker => faker.IndexFaker);
			bookReaderFaker.RuleFor(x => x.ReaderID, faker => faker.IndexFaker);
			bookReaderFaker.RuleFor(x => x.LendDate, faker => faker.Date.Past());
			bookReaderFaker.RuleFor(x => x.ID, f => f.IndexFaker);
			var bookReaderModels = bookReaderFaker.Generate(2);

			foreach (var br in bookReaderModels)
			{
				Console.WriteLine($"Book-Reader: {br.ID}, {br.BookID}, {br.ReaderID}, {br.LendDate}");
			}

			this.Books.AddRange(bookModels);
			this.Readers.AddRange(readerModels);
			this.BookReader.AddRange(bookReaderModels);
			this.SaveChanges();
		}
	}
}