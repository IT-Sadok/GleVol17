namespace LibraryManagement.Services;
using LibraryManagement.Models;

public interface IBookService
{
    IEnumerable<Book> GetAll();
    Book? GetByCode(int code);
    Book Add(Book book);
    bool Remove(int code);
    IEnumerable<Book> Search(string query);
    bool Borrow(int code);
    bool Return(int code);
}