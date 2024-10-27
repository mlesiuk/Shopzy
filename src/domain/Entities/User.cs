using Shopzy.Domain.ValueObjects;

namespace Shopzy.Domain.Entities;

public class User : AuditableEntity
{
    private User()
    {
        Cart = Cart.Create(this);
    }

    public static User Create(string username, string email, string passwordHash)
    {
        return new User
        {
            Username = username,
            Email = email,
            PasswordHash = passwordHash
        };
    }

    public string Username { get; private set; } = string.Empty;

    public string Name { get; private set; } = string.Empty;

    public string SurName { get; private set; } = string.Empty;

    public string PasswordHash { get; private set; } = string.Empty;
    public void SetPasswordHash(string passwordHash)
    {
        if (!string.IsNullOrEmpty(passwordHash))
        {
            PasswordHash = passwordHash;
        }
    }

    public string Email { get; private set; } = string.Empty;
    public void SetEmail(string email)
    {
        if (!string.IsNullOrEmpty(email))
        {
            Email = email;
        }
    }

    #region Address
    private readonly List<Address> _addressess = new();
    public IReadOnlyCollection<Address> Addressess => _addressess;

    public void AddAddressess(IEnumerable<Address> addressess)
    {
        foreach (var address in addressess)
        {
            if (address is not null)
            {
                AddAddress(address);
            }
        }
    }

    public void AddAddress(Address address)
    {
        var existingAddress = _addressess.FirstOrDefault(a => a.Equals(address));
        if (existingAddress is null)
        {
            _addressess.Add(address);
        }
    }

    public void RemoveAddress(Address address)
    {
        var existingAddress = _addressess.FirstOrDefault(a => a.Equals(address));
        if (existingAddress is not null)
        {
            _addressess.Remove(address);
        }
    }
    #endregion

    #region Role
    private readonly List<Role> _roles = new();
    public IReadOnlyCollection<Role> Roles => _roles.ToList();

    public void AddRole(Role role)
    {
        if (role is not null)
        {
            var existingRole = _roles.FirstOrDefault(r => r.Id == role.Id);
            if (existingRole is null)
            {
                _roles.Add(role);
            }
        }
    }
    #endregion

    public Cart? Cart { get; private set; }
}
