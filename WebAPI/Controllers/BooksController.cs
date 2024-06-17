using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Entities.Models;
using Repositories.Contracts;
using Repositories.EFCore;

namespace WebAPI.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IRepositoryManager _manager;

        //Otomatik olarak initialize ediliyor Dependency Injection

        public BooksController(IRepositoryManager manager)
        {
            _manager = manager;
        }

        [HttpGet]
        public IActionResult GetAllBooks()
        {
            try
            {
                var books = _manager.Book.GetAllBooks(false);

                if (books is null)
                {
                    return NotFound("Listede Elaman Bulunamadı!");
                }

                return Ok(books);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        
        [HttpGet("{id:int}")]
        public IActionResult GetOneBook([FromRoute(Name = "id")] int id)
        {
            try
            {
                var book = _manager.Book.GetOneBookById(id, false);

                if (book is null)
                {
                    return NotFound("Listede Elaman Bulunamadı!");
                }

                return Ok(book);
            }
            catch(Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }
        
        [HttpPost]
        public IActionResult CreateBook([FromBody] Book book)
        {
            try
            {
                if (book is null)
                {
                    return BadRequest("Gönderdiğiniz Veriler Boş");
                }

                _manager.Book.CreateOneBook(book);
                _manager.Save();
                return StatusCode(201, book);
            }
            catch(Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        //Put işlemi ile nesnenin tamamı güncelleniyorudu
        [HttpPut("{id:int}")]
        public IActionResult UpdateOneBook([FromRoute(Name = "id")] int id, [FromBody] Book book)
        {
            try
            {
                var entity = _manager.Book.GetOneBookById(id, true);
                if (entity is null)
                {
                    return NotFound("Güncellenecek Id Değerine Sahip Obje Bulunamadı"); //404 hata kodudur
                }

                if (id != book.Id)
                {
                    return BadRequest(); //400 hata kodudur
                }

                entity.Title = book.Title;
                entity.Price = book.Price;

                _manager.Save(); //Yapılan Değişiklerin artık database üzerinde gözükmesini sağlamış oluruz

                return Ok("Güncelleme İşlemi Tamamlandı");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteAllBooks([FromRoute(Name ="id")] int id)
        {
            try
            {
                var entity = _manager.Book.GetOneBookById(id, false);
                if (entity is null)
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = $"Book width id:{id} Not Found"
                    });
                }
                _manager.Book.DeleteOneBook(entity);
                _manager.Save();
                return NoContent();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpPatch("{id:int}")]
        public IActionResult PartiallyUpdateOneBook([FromRoute(Name = "id")] int id,
            [FromBody] JsonPatchDocument<Book> bookPatch)
        {
            if (bookPatch == null)
            {
                return BadRequest("Patch document is null");
            }

            var entity = _manager.Book.GetOneBookById(id, true);
            if (entity == null)
            {
                return NotFound(new { StatusCode = 404, Message = $"Book with id:{id} Not Found" });
            }

            bookPatch.ApplyTo(entity, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _manager.Book.Update(entity);
            _manager.Save();
            return NoContent();
        }

        
        
        
    }
}
