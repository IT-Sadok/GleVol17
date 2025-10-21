namespace LibraryManagement.Models;

public class Book
{
    public int Code { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int Year { get; set; }
    public BookStatus BookStatus { get; set; } = BookStatus.Available;
    
}