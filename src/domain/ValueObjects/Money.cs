using Shopzy.Domain.Enums;

namespace Shopzy.Domain.ValueObjects;

public record Money(decimal Amount, Currency Currency);
