namespace LibraryManagement.Services;

using LibraryManagement.Models;
using LibraryManagement.Repositories;

public class BookService : IBookService
{
    private readonly IBookRepository _repository;

    public BookService(IBookRepository repository)
    {
        _repository = repository;
    }

    public IEnumerable<Book> GetAll()
    {
        return _repository.GetAll();
    }

    public Book? GetByCode(int code)
    {
        return _repository.GetByCode(code);
    }

    public Book Add(Book book)
    {
        book.Code = _repository.GetNextCode();
        book.BookStatus = BookStatus.Available;
        _repository.Add(book);
        _repository.Persist();
        return book;
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

    public IEnumerable<Book> Search(string query)
    {
        return _repository.Search(query);
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