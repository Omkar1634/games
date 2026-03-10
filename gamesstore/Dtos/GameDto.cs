namespace gamesstore.Dtos;

//A DTO (Data Transfer Object) is a simple object that is used to transfer data between different layers of an application.
// It is often used to encapsulate data and to reduce the number of method calls when transferring data over a network or between different parts of an application. 
//In this case, the GameDto class is likely intended to represent a game object that can be transferred between different layers of the application, such as between the database and the API layer.

public record  GameDto (
    int Id,
    string Name,
    string Genre,
    decimal Price,
    DateOnly ReleaseDate

);

