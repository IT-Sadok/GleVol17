namespace LibraryManagement.Services;

using LibraryManagement.Models;

public interface IBookService
{
    IEnumerable<BookModel> GetAll();
    
    BookModel? GetByCode(int code);
    BookModel Add(CreateBookModel book);
    bool Remove(int code);
    IEnumerable<BookModel> Search(string query);
    bool Borrow(int code);
    bool Return(int code);
}