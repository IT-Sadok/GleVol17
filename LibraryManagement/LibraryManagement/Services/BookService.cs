namespace LibraryManagement.Services;

using LibraryManagement.Models;
using LibraryManagement.Repositories;
using LibraryManagement.Extensions;

public class BookService : IBookService
{
    private readonly IBookRepository _repository;

    public BookService(IBookRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<BookModel> GetAll()
    {
        return _repository.GetAll().Select(book => book.ToModel());
    }

    public BookModel? GetByCode(int code)
    {
        var book = _repository.GetByCode(code);
        var bookOrNull = book?.ToModel();
        return bookOrNull;
    }

    public async Task<BookModel?> AddAsync(CreateBookModel createBook)
    {
        var book = new Book
        {
            Code = _repository.GetNextCode(),
            Title = createBook.Title,
            Author = createBook.Author,
            Year = createBook.Year,
            BookStatus = BookStatus.Available
        };
        try
        {
            await _repository.AddAsync(book);
            await _repository.PersistAsync();

            var newBook = book.ToModel();
            return newBook;
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    public async Task<bool> RemoveAsync(int code)
    {
        var removedBookCode = _repository.GetByCode(code);
        if (removedBookCode == null)
        {
            return false;
        }

        await _repository.RemoveAsync(code);
        await _repository.PersistAsync();
        return true;
    }

    public IEnumerable<BookModel> Search(string query)
    {
        var foundBooks = _repository.Search(query).Select(book => book.ToModel());

        return foundBooks;
    }

    public async Task<bool> BorrowAsync(int code)
    {
        var book = _repository.GetByCode(code);
        if (book == null || book.BookStatus == BookStatus.Busy)
        {
            return false;
        }

        book.BookStatus = BookStatus.Busy;
        await _repository.UpdateAsync(book);
        await _repository.PersistAsync();
        return true;
    }

    public async Task<bool> ReturnAsync(int code)
    {
        var book = _repository.GetByCode(code);
        if (book == null || book.BookStatus == BookStatus.Available)
        {
            return false;
        }

        book.BookStatus = BookStatus.Available;
       await _repository.UpdateAsync(book);
       await _repository.PersistAsync();
        return true;
    }
}