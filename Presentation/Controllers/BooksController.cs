using Entities.Expections;
using Entities.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace Presentation.Controllers;

[ApiController]
[Route("api/books")]
    public class BooksController : ControllerBase
    {
        private readonly IServiceManager _manager;

        //Otomatik olarak initialize ediliyor Dependency Injection

        public BooksController(IServiceManager manager)
        {
            _manager = manager;
        }

        [HttpGet]
        public IActionResult GetAllBooks()
        {
                var books = _manager.BookService.GetAllBooks(false);

                if (books is null)
                {
                    return NotFound("Listede Elaman Bulunamadı!");
                }

                return Ok(books);
        }
        
        [HttpGet("{id:int}")]
        public IActionResult GetOneBook([FromRoute(Name = "id")] int id)
        {
            var book = _manager.BookService.GetOneBookById(id, false);
            return Ok(book);
        }
        
        [HttpPost]
        public IActionResult CreateBook([FromBody] Book book)
        {
         
                if (book is null)
                {
                    return BadRequest("Gönderdiğiniz Veriler Boş");
                }

                _manager.BookService.CreateOneBook(book);
                return StatusCode(201, book);
         
        }

        //Put işlemi ile nesnenin tamamı güncelleniyorudu
        [HttpPut("{id:int}")]
        public IActionResult UpdateOneBook([FromRoute(Name = "id")] int id, [FromBody] Book book)
        {
                if (book is null)
                {
                    return BadRequest("Gönderdiğiniz Veriler Boş");
                }
                _manager.BookService.UpdateOneBook(id, book, true);

                return NoContent();
           
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteOneBooks([FromRoute(Name ="id")] int id)
        {
        
                _manager.BookService.DeleteOneBook(id, false);
                return NoContent();
            
        }

        [HttpPatch("{id:int}")]
        public IActionResult PartiallyUpdateOneBook([FromRoute(Name = "id")] int id,
            [FromBody] JsonPatchDocument<Book> bookPatch)
        {
            if (bookPatch == null)
            {
                return BadRequest("Patch document is null");
            }

            var entity = _manager.BookService.GetOneBookById(id, true);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _manager.BookService.UpdateOneBook(id, entity, true);
            return NoContent();
        }
        
    }
