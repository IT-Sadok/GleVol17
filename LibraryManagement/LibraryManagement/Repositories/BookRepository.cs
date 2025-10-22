namespace LibraryManagement.Repositories;

using System.Text.Json;
using LibraryManagement.Models;

public class BookRepository : IBookRepository
{
    private readonly string _filePath = Path.Combine(AppContext.BaseDirectory, "Data", "books.json");
    private readonly Dictionary<int, Book> _booksByCode;

    public BookRepository()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);
        _booksByCode = LoadBooks().ToDictionary(b => b.Code, b => b);
    }

    private List<Book> LoadBooks()
    {
        if (!File.Exists(_filePath))
            return new List<Book>();

        var json = File.ReadAllText(_filePath);
        return JsonSerializer.Deserialize<List<Book>>(json) ?? new List<Book>();
    }

    private void SaveBooks()
    {
        var options = new JsonSerializerOptions();
        var json = JsonSerializer.Serialize(_booksByCode.Values, options);
        File.WriteAllText(_filePath, json);
    }

    public IEnumerable<Book> GetAll()
    {
        return _booksByCode.Values.OrderBy(book => book.Code);
    }


    public Book? GetByCode(int code)
    {
        return _booksByCode.TryGetValue(code, out var book) ? book : null;
    }

    public void Add(Book book)
    {
        if (!_booksByCode.ContainsKey(book.Code))
        {
            _booksByCode.Add(book.Code, book);
        }
        else
        {
            Console.WriteLine($"book with code: {book.Code} already exists");
        }

        SaveBooks();
    }

    public void Remove(int code)
    {
        _booksByCode.Remove(code);
        SaveBooks();
    }

    public IEnumerable<Book> Search(string query)
    {
        query = query?.Trim().ToLowerInvariant() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(query))
        {
            return Enumerable.Empty<Book>();
        }

        return _booksByCode.Values.Where(b =>
            b.Title.ToLowerInvariant().Contains(query) ||
            b.Author.ToLowerInvariant().Contains(query));
    }

    public void Update(Book book)
    {
        var exists = _booksByCode.ContainsKey(book.Code);

        if (exists)
        {
            _booksByCode.Remove(book.Code);
            _booksByCode.Add(book.Code, book);
            SaveBooks();
        }
    }

    public int GetNextCode()
    {
        return _booksByCode.Any() ? _booksByCode.Keys.Max() + 1 : 1;
    }

    public void Persist()
    {
        SaveBooks();
    }
}