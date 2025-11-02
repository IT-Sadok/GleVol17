using LibraryManagement.Models;
using LibraryManagement.Services;

namespace LibraryManagement.Simulation;

public enum BookAction
{
    Get = 0,
    Add = 1,
    Remove = 2,
    Borrow = 3,
    Return = 4
}

public static class LibrarySimulator
{
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
            tasks.Add(Task.Run(async () =>
            {
                var threadId = Environment.CurrentManagedThreadId;
                var randomBook = books.Count > 0 ? books[Random.Shared.Next(books.Count)] : null;
                var code = randomBook?.Code ?? 0;
                var randomAction = (BookAction)Random.Shared.Next(0, 5);
                var codeNotNull = code != 0;

                Console.WriteLine($"thread {threadId} started on book {code}");

                switch (randomAction)
                {
                    case BookAction.Get:
                        if (codeNotNull)
                        {
                            var getBook = bookService.GetByCode(code);
                            Console.WriteLine($"thread {threadId} GET book {code} => {getBook?.Title ?? "not found"}");
                        }
                        break;
                    case BookAction.Add:
                        var newBook = await bookService.AddAsync(new CreateBookModel
                        {
                            Title = $"new book {Random.Shared.Next(100,300)} ", 
                            Author = $"new author",
                            Year = 2000 + Random.Shared.Next(5)
                        });
                        Console.WriteLine($"thread {threadId} ADD new book => {newBook?.Code}");
                        break;
                    case BookAction.Remove:
                        if (codeNotNull)
                        {
                            var removeBook =  await bookService.RemoveAsync(code);
                            Console.WriteLine($"thread {threadId} REMOVE book {code} => {(removeBook ? "OK" : "FAIL")}");
                        }
                        break;
                    case BookAction.Borrow:
                        if (codeNotNull)
                        {
                            var  borrowBook = await bookService.BorrowAsync(code);
                            Console.WriteLine($"thread {threadId} BORROW  book {code} => {(borrowBook ? "OK" : "FAIL")}");
                        }
                        break;
                    case BookAction.Return:
                        if (codeNotNull)
                        {
                            var returnBook = await bookService.ReturnAsync(code);
                            Console.WriteLine($"thread {threadId} RETURN  book {code} => {(returnBook ? "OK" : "FAIL")}");
                        }
                        break;
                }

                await Task.Delay(100);

                Console.WriteLine($"thread {threadId} finished work on book {code}");
            }));
        }

        await Task.WhenAll(tasks);
        Console.WriteLine("simulation completed");
    }
}