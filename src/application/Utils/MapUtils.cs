using Shopzy.Application.Dtos;
using Shopzy.Domain.ValueObjects;

namespace Shopzy.Application.Utils;

public static class MapUtils
{
    public static IEnumerable<Address> ToAddressList(this IEnumerable<AddressDto> addressDtos)
    {
        foreach (var addressDto in addressDtos)
        {
            if (addressDto == null)
            {
                continue;
            }

            yield return Address.Create(
                addressDto.Street,
                addressDto.City,
                addressDto.State,
                addressDto.ZipCode,
                addressDto.Country);
        }
    }
}
