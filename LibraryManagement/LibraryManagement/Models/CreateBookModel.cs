﻿namespace LibraryManagement.Models;

public class CreateBookModel
{
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public int Year { get; set; }
}