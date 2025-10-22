namespace LibraryManagement.Repositories;

using LibraryManagement.Models;

public interface IBookRepository
{
    IEnumerable<Book> GetAll();
    Book? GetByCode(int code);
    void Remove(int code);
    IEnumerable<Book> Search(string query);
    void Update(Book book);
    int GetNextCode();
    void Persist();
    void Add(Book book);
}