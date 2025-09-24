namespace GameStore.Api.Dtos;
public record class UpdateGameDto(
    string Name,
    int GenreId,      // <- Add this
    decimal Price,
    DateOnly ReleaseDate
);
