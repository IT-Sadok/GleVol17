using LibraryManagement.Models;
using LibraryManagement.Services;

namespace LibraryManagement.Simulation;

public static class LibrarySimulator
{
    private static readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);

    public static async Task RunAsync(IBookService bookService, int tasksCount = 100)
    {
        Console.WriteLine($"starting simulation {tasksCount} threads");

        if (!bookService.GetAll().Any())
        {
            Console.WriteLine("creating 10 books");
            for (int i = 1; i <= 10; i++)
            {
                await bookService.AddAsync(new CreateBookModel
                {
                    Title = $"Book {i}",
                    Author = $"Author {i}",
                    Year = 2000 + i
                });
            }
        }

        var books = bookService.GetAll().ToList();
        var tasks = new List<Task>(tasksCount);

        Console.WriteLine("simulation started");

        for (int i = 0; i < tasksCount; i++)
        {
            int localIndex = i;

            tasks.Add(Task.Run(async () =>
            {
                var threadId = Environment.CurrentManagedThreadId;
                var book = books[localIndex % books.Count];
                var code = book.Code;

                await _semaphoreSlim.WaitAsync();
                try
                {
                    Console.WriteLine($"tthread {threadId} started on book {code}");

                    if (book.BookStatus == "Available")
                    {
                        await bookService.BorrowAsync(code);
                        Console.WriteLine($"thread {threadId} Book {code} => Busy");
                    }
                    else
                    {
                        await bookService.ReturnAsync(code);
                        Console.WriteLine($"thread {threadId} Book {code} => Available");
                    }

                    await Task.Delay(100);

                    Console.WriteLine($"thread {threadId} finished work on book {code}");
                }
                finally
                {
                    _semaphoreSlim.Release();
                }
            }));
        }

        await Task.WhenAll(tasks);

        Console.WriteLine("simulation completed");
    }
}