namespace Shopzy.Application.Dtos;

public sealed record class JwtDto(string AccessToken, int ExpiresIn) : Dto;
