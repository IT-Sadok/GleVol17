using System.Globalization;

namespace LibraryManagement.Repositories;

using System.Text.Json;
using LibraryManagement.Models;

public class BookRepository : IBookRepository
{
    private readonly string _filePath = Path.Combine(AppContext.BaseDirectory, "Data", "books.json");
    private readonly Dictionary<int, Book> _booksByCode;
    private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);

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

    private async Task SaveBooksAsync()
    {
        var options = new JsonSerializerOptions();
        var json = JsonSerializer.Serialize(_booksByCode.Values, options);
        await File.WriteAllTextAsync(_filePath, json);
    }

    public IEnumerable<Book> GetAll()
    {
        return _booksByCode.Values.OrderBy(book => book.Code);
    }


    public Book? GetByCode(int code)
    {
        return _booksByCode.TryGetValue(code, out var book) ? book : null;
    }

    public async Task AddAsync(Book book)
    {
        await _semaphoreSlim.WaitAsync();
        try
        {
            if (!_booksByCode.ContainsKey(book.Code))
            {
                _booksByCode.Add(book.Code, book);
            }
            else
            {
                throw new InvalidOperationException($"Book with code {book.Code} already exists");
            }

            await SaveBooksAsync();
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }

    public async Task RemoveAsync(int code)
    {
        await _semaphoreSlim.WaitAsync();
        try
        {
            _booksByCode.Remove(code);
            await SaveBooksAsync();
        }
        finally
        {
            _semaphoreSlim.Release();
        }
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

    public async Task UpdateAsync(Book book)
    {
        await _semaphoreSlim.WaitAsync();
        try
        {
            var exists = _booksByCode.ContainsKey(book.Code);

            if (exists)
            {
                _booksByCode.Remove(book.Code);
                _booksByCode.Add(book.Code, book);
                await SaveBooksAsync();
            }
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }

    public int GetNextCode()
    {
        return _booksByCode.Any() ? _booksByCode.Keys.Max() + 1 : 1;
    }
}