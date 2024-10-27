namespace Shopzy.Application.Dtos;

public sealed record class UserDto : Dto
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string SurName { get; set; } = string.Empty;
    public IReadOnlyCollection<AddressDto> Addressess { get; set; } = new List<AddressDto>();
}
