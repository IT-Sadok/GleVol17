using LibraryManagement.Models;
using LibraryManagement.Services;

namespace LibraryManagement.App
{
    public class LibraryApp
    {
        private readonly IBookService _bookService;

        public LibraryApp(IBookService bookService)
        {
            _bookService = bookService;
        }

        public void Run()
        {
            while (true)
            {
                ShowMenu();
                var input = Console.ReadLine()?.Trim();
                Console.WriteLine();
                switch (input)
                {
                    case "1": AddBookFlow();
                        break;
                    case "2": RemoveBookFlow();
                        break;
                    case "3": ShowAllFlow();
                        break;
                    case "4": SearchFlow();
                        break;
                    case "5": BorrowFlow();
                        break;
                    case "6": ReturnFlow();
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
            Console.WriteLine("0. Exit?");
            Console.Write("your choice: ");
        }

        private void AddBookFlow()
        {
            Console.Write("Name book: ");
            var title = Console.ReadLine() ?? string.Empty;

            Console.Write("author book: ");
            var author = Console.ReadLine() ?? string.Empty;

            Console.Write("year book: ");
            if (!int.TryParse(Console.ReadLine(), out var year))
            {
                Console.WriteLine("unknown format");
                return;
            }

            var book = new Book { Title = title, Author = author, Year = year };
            var created = _bookService.Add(book);
            Console.WriteLine($"book added, code: {created.Code}");
        }

        private void RemoveBookFlow()
        {
            Console.Write("enter code for delete: ");
            if (!int.TryParse(Console.ReadLine(), out var code))
            {
                Console.WriteLine("unknown code");
                return;
            }

            if (_bookService.Remove(code))
            {
                Console.WriteLine("book deleted");
            }
            else
            {
                Console.WriteLine("book is not known");
            }
        }

        private void ShowAllFlow()
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

        private void SearchFlow()
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

        private void BorrowFlow()
        {
            Console.Write("book code to take: ");
            if (!int.TryParse(Console.ReadLine(), out var code))
            {
                Console.WriteLine("uncorrect code");
                return;
            }

            if (_bookService.Borrow(code))
            {
                Console.WriteLine("the book is taken");
            }
            else
            {
                Console.WriteLine("could not get the book (maybe it is not available or already taken)");
            }
        }

        private void ReturnFlow()
        {
            Console.Write("Code of the book to return: ");
            if (!int.TryParse(Console.ReadLine(), out var code))
            {
                Console.WriteLine("uncorrect code");
                return;
            }

            if (_bookService.Return(code))
            {
                Console.WriteLine("book is returned");
            }
            else
            {
                Console.WriteLine("unable to return (maybe the book was not found or is already available)");
            }
        }
    }
}