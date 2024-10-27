namespace Shopzy.Application.Dtos;

public sealed record class AddressDto : Dto
{
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Street { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
}
