using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Expections;
using Entities.Models;
using Repositories.Contracts;
using Services.Contracts;
using Exception = System.Exception;

namespace Services;

public class BookManager : IBookService
{

    private readonly IRepositoryManager _manager;
    private readonly ILoggerService _logger;
    private readonly IMapper _mapper;
    

    public BookManager(IRepositoryManager manager, ILoggerService logger, IMapper mapper)
    {
        _manager = manager;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<IEnumerable<BookDto>> GetAllBooksAsync(bool trackChanges)
    {
        var books = await _manager.Book.GetAllBooksAsync(trackChanges);
        return _mapper.Map<IEnumerable<BookDto>>(books);
    }

    public async Task<BookDto>  GetOneBookByIdAsync(int id, bool trackChanges)
    {
        var book = await GetOneBookByIdAndCheckExist(id, trackChanges);

        return _mapper.Map<BookDto>(book);  //Kitapdan bookDto'ya geçiş yapmış olduk
    }

    public async Task<BookDto> CreateOneBookAsync(BookDtoForInsertion bookDto)
    {
        var entity = _mapper.Map<Book>(bookDto);
        _manager.Book.CreateOneBook(entity);
        await _manager.SaveAsync();
        return _mapper.Map<BookDto>(entity);
    }

    public async Task UpdateOneBookAsync(int id, BookDtoForUpdate bookDto, bool trackChanges)
    {
        var book = await GetOneBookByIdAndCheckExist(id, trackChanges);

        //Mapping var aslında burda bunu otomatikleştirecez. Yorum satırına alma kısayolu cmd k c
        
        // entity.Title = book.Title;
        // entity.Price = book.Price;

        book = _mapper.Map<Book>(bookDto);
        
        _manager.Book.Update(book);
        await _manager.SaveAsync();
    }

    public async Task DeleteOneBookAsync(int id, bool trackChanges)
    {
        var book = await GetOneBookByIdAndCheckExist(id, trackChanges);

        _manager.Book.DeleteOneBook(book);
        await _manager.SaveAsync();
    }

    public async Task<(BookDtoForUpdate bookDtoForUpdate, Book book)> GetOneBookForPatchAsync(int id, bool trackChanges)
    {
        var book = await GetOneBookByIdAndCheckExist(id, trackChanges);
        
        var bookDtoForUpdate = _mapper.Map<BookDtoForUpdate>(book);
        return (bookDtoForUpdate, book);
    }

    public async Task SaveChangesForPatchAsync(BookDtoForUpdate bookDtoForUpdate, Book book)
    {
        _mapper.Map(bookDtoForUpdate, book);
        await _manager.SaveAsync();
    }

    public async Task<Book> GetOneBookByIdAndCheckExist(int id, bool trackChanges)
    {
        var book = await _manager.Book.GetOneBookByIdAsync(id, trackChanges);

        return book;
    }
}