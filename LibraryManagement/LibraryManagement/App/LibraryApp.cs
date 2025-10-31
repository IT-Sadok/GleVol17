namespace LibraryManagement.App;

using LibraryManagement.Simulation;
using LibraryManagement.Models;
using LibraryManagement.Services;

public class LibraryApp
{
    private readonly IBookService _bookService;

    public LibraryApp(IBookService bookService)
    {
        _bookService = bookService;
    }

    public async Task Run()
    {
        while (true)
        {
            ShowMenu();
            var input = Console.ReadLine()?.Trim();
            Console.WriteLine();
            switch (input)
            {
                case "1":
                    await HandleAddBookFlowAsync();
                    break;
                case "2":
                    await HandleRemoveBookFlowAsync();
                    break;
                case "3":
                    HandleShowAllFlow();
                    break;
                case "4":
                    HandleSearchFlow();
                    break;
                case "5":
                   await HandleBorrowFlowAsync();
                    break;
                case "6":
                    await HandleReturnFlowAsync();
                    break;
                case "7":
                    await HandleLibrarySimulatorAsync();
                    break;
                case "0":
                    Console.WriteLine("exit");
                    return;
                default: Console.WriteLine("wrong choice, try again"); break;
            }
        }
    }

    private void ShowMenu()
    {
        Console.WriteLine("Library Menu");
        Console.WriteLine("1. add book");
        Console.WriteLine("2. remove book");
        Console.WriteLine("3. show all books");
        Console.WriteLine("4. search book");
        Console.WriteLine("5. borrow book");
        Console.WriteLine("6. return book");
        Console.WriteLine("7. simulator with 100 threads");
        Console.WriteLine("0. Exit?");
        Console.Write("your choice: ");
    }

    private async Task HandleAddBookFlowAsync()
    {
        Console.Write("Name book: ");
        var title = Console.ReadLine()?.Trim();

        if (string.IsNullOrEmpty(title))
        {
            Console.WriteLine("title empty");
            return;
        }

        Console.Write("author book: ");
        var author = Console.ReadLine()?.Trim();

        if (string.IsNullOrEmpty(author))
        {
            Console.WriteLine("author empty");
            return;
        }

        var year = 0;
        var validYear = false;

        while (!validYear)
        {
            Console.Write("year book: ");

            if (int.TryParse(Console.ReadLine(), out year))
            {
                validYear = true;
            }
            else
            {
                Console.WriteLine("unknown format");
            }
        }

        var createBook = new CreateBookModel
        {
            Title = title,
            Author = author,
            Year = year
        };

        var created = await _bookService.AddAsync(createBook);
        Console.WriteLine($"book added, code: {created.Code}");
    }

    private async Task HandleRemoveBookFlowAsync()
    {
        Console.Write("enter code for delete: ");
        if (!int.TryParse(Console.ReadLine(), out var code))
        {
            Console.WriteLine("unknown code");
            return;
        }

        if (await _bookService.RemoveAsync(code))
        {
            Console.WriteLine("book deleted");
        }
        else
        {
            Console.WriteLine("book is not known");
        }
    }

    private void HandleShowAllFlow()
    {
        var allBooks = _bookService.GetAll().ToList();
        if (!allBooks.Any())
        {
            Console.WriteLine("there are no books");
            return;
        }

        Console.WriteLine("Code, name book, author, year, book status");
        foreach (var book in allBooks)
        {
            Console.WriteLine($"{book.Code}, {book.Title}, {book.Author}, {book.Year}, {book.BookStatus}");
        }
    }

    private void HandleSearchFlow()
    {
        Console.Write("enter name book or author: ");
        var nameBook = Console.ReadLine() ?? string.Empty;
        var found = _bookService.Search(nameBook).ToList();

        if (!found.Any())
        {
            Console.WriteLine("there are no books");
            return;
        }

        Console.WriteLine("Found:");
        foreach (var book in found)
        {
            Console.WriteLine($"{book.Code}, {book.Title}, {book.Author}, {book.Year}, {book.BookStatus}");
        }
    }

    private async Task HandleBorrowFlowAsync()
    {
        Console.Write("book code to take: ");
        if (!int.TryParse(Console.ReadLine(), out var code))
        {
            Console.WriteLine("uncorrect code");
            return;
        }

        if (await _bookService.BorrowAsync(code))
        {
            Console.WriteLine("the book is taken");
        }
        else
        {
            Console.WriteLine("could not get the book (maybe it is not available or already taken)");
        }
    }

    private async Task HandleReturnFlowAsync()
    {
        Console.Write("Code of the book to return: ");
        if (!int.TryParse(Console.ReadLine(), out var code))
        {
            Console.WriteLine("uncorrect code");
            return;
        }

        if (await _bookService.ReturnAsync(code))
        {
            Console.WriteLine("book is returned");
        }
        else
        {
            Console.WriteLine("unable to return (maybe the book was not found or is already available)");
        }
    }

    private async Task HandleLibrarySimulatorAsync()
    {
        await LibrarySimulator.RunAsync(_bookService);
    }
}