namespace LibraryManagement.Repositories;

using LibraryManagement.Models;

public interface IBookRepository
{
    IEnumerable<Book> GetAll();
    Book? GetByCode(int code);
    Task RemoveAsync(int code);
    IEnumerable<Book> Search(string query);
    Task UpdateAsync(Book book);
    int GetNextCode();
    Task PersistAsync();
    Task AddAsync(Book book);
}