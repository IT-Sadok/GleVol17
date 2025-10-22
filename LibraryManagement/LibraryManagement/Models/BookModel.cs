namespace LibraryManagement.Models;

public class BookModel
{
    public int Code { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int Year { get; set; }
    public string BookStatus { get; set; }  = string.Empty;
}