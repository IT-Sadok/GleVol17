using LibraryManagement.App;
using LibraryManagement.Repositories;
using LibraryManagement.Services;

var repository = new BookRepository();
var service = new BookService(repository);
var app = new LibraryApp(service);

await app.Run();