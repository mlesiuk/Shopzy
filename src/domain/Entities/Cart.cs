namespace Shopzy.Domain.Entities;

public sealed class Cart : AuditableEntity
{
    private Cart() { }

    private Cart(User user) => (User, UserId) = (user, user.Id);

    public static Cart Create(User user)
    {
        return new Cart(user);
    }

    private readonly List<CartItem> _cartItems = new();
    public IReadOnlyCollection<CartItem> CartItems => _cartItems;

    public void AddCartItem(CartItem cartItem)
    {
        var cartItemExists = _cartItems.FirstOrDefault(c => c.Id == cartItem.Id);
        if (cartItemExists is not null)
        {
            _cartItems.Remove(cartItem);
        }

        cartItem.IncreaseQuantity();
        _cartItems.Add(cartItem);
    }

    public Guid? UserId { get; private set; }
    public User? User { get; private set; }
}
