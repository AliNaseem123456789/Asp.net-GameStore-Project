namespace GameStore.Api.Dtos;

using System.ComponentModel.DataAnnotations;
public record class CreateGameDto(
    [Required][StringLength(50)] string Name,
    [Required] int GenreId,  
    [Range(1,10000)] decimal Price,
    DateOnly ReleaseDate);
