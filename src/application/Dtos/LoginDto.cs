namespace Shopzy.Application.Dtos;

public sealed record class LoginDto(string AccessToken, int ExpiresIn) : Dto;
