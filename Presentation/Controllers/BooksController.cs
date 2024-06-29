using Entities.DataTransferObjects;
using Entities.Expections;
using Entities.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Services.Contracts;

namespace Presentation.Controllers;

[ServiceFilter(typeof(LogFilterAttribute))]
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
        public async Task<IActionResult> GetAllBooks()
        {
                var books = await _manager.BookService.GetAllBooksAsync(false);

                if (books is null)
                {
                    return NotFound("Listede Elaman Bulunamadı!");
                }

                return Ok(books);
        }
        
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOneBook([FromRoute(Name = "id")] int id)
        {
            var book = await _manager.BookService.GetOneBookByIdAsync(id, false);
            return Ok(book);
        }
        
        [ServiceFilter(typeof(ValidationAttributeFilter))]
        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] BookDtoForInsertion bookDto)
        { 
            await _manager.BookService.CreateOneBookAsync(bookDto); 
            return StatusCode(201, bookDto);    
        }

        //Put işlemi ile nesnenin tamamı güncelleniyorudu
        //[ServiceFilter(typeof(LogFilterAttribute), Order = 2)] Önce validation daha sonra logging
        [ServiceFilter(typeof(ValidationAttributeFilter), Order = 1)] //Once validation'a bak daha sonra 2 yani logla üst satırda
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateOneBook([FromRoute(Name = "id")] int id, [FromBody] BookDtoForUpdate book)
        {
                await _manager.BookService.UpdateOneBookAsync(id, book, true);
                return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteOneBooks([FromRoute(Name ="id")] int id)
        {
            await _manager.BookService.DeleteOneBookAsync(id, false);
            return NoContent();
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> PartiallyUpdateOneBook([FromRoute(Name = "id")] int id,
            [FromBody] JsonPatchDocument<BookDtoForUpdate> bookPatch)
        {
            if (bookPatch == null)
            {
                return BadRequest("Patch document is null");
            }

            var result = await _manager.BookService.GetOneBookForPatchAsync(id, false);

            bookPatch.ApplyTo(result.bookDtoForUpdate, ModelState);

            TryValidateModel(result.bookDtoForUpdate);
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState);
            }

           await _manager.BookService.SaveChangesForPatchAsync(result.bookDtoForUpdate, result.book);
            return NoContent();
        }
        
    }
