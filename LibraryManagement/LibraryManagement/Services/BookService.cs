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

    public BookModel Add(CreateBookModel createBook)
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
            _repository.Add(book);
            _repository.Persist();

            var newBook = book.ToModel();
            return newBook;
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }

    public bool Remove(int code)
    {
        var removedBookCode = _repository.GetByCode(code);
        if (removedBookCode == null)
        {
            return false;
        }

        _repository.Remove(code);
        _repository.Persist();
        return true;
    }

    public IEnumerable<BookModel> Search(string query)
    {
        var foundBooks = _repository.Search(query).Select(book => book.ToModel());

        return foundBooks;
    }

    public bool Borrow(int code)
    {
        var book = _repository.GetByCode(code);
        if (book == null || book.BookStatus == BookStatus.Busy)
        {
            return false;
        }

        book.BookStatus = BookStatus.Busy;
        _repository.Update(book);
        _repository.Persist();
        return true;
    }

    public bool Return(int code)
    {
        var book = _repository.GetByCode(code);
        if (book == null || book.BookStatus == BookStatus.Available)
        {
            return false;
        }

        book.BookStatus = BookStatus.Available;
        _repository.Update(book);
        _repository.Persist();
        return true;
    }
}