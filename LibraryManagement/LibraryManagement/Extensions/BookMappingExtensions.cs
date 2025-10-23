using LibraryManagement.Models;

namespace LibraryManagement.Extensions;

public static class BookMappingExtensions
{
    public static BookModel ToModel(this Book book)
    {
        return new BookModel
        {
            Code = book.Code,
            Title = book.Title,
            Author = book.Author,
            Year = book.Year,
            BookStatus = book.BookStatus.ToString()
        };
    }
}