namespace LibraryManagement.Services;

using LibraryManagement.Models;

public interface IBookService
{
    IEnumerable<BookModel> GetAll();
    
    BookModel? GetByCode(int code);
    Task<BookModel?> AddAsync(CreateBookModel createBook);
    Task<bool> RemoveAsync(int code);
    IEnumerable<BookModel> Search(string query);
    Task<bool> BorrowAsync(int code);
    Task<bool> ReturnAsync(int code);
}