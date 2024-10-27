namespace Shopzy.Domain.Entities;

public sealed class CartItem : AuditableEntity
{
    private CartItem() { }

    private CartItem(Cart cart, Product product, int quantity) =>
        (Cart, Product, Quantity) = (cart, product, quantity);

    public static CartItem Create(Cart cart, Product product, int quantity)
    {
        return new(cart, product, quantity);
    }

    public Cart? Cart { get; private set; }
    public Product? Product { get; private set; }

    public int Quantity { get; private set; }

    public void IncreaseQuantity()
    {
        Quantity++;
    }
    public void DecreaseQuantity()
    {
        if (Quantity > 0)
        {
            Quantity--;
        }
    }
}
