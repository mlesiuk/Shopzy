using System.Collections.Immutable;

namespace Shopzy.Notifier.Models;

public class MailContext
{
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public IDictionary<string, string> FromAddresses { get; set; } = ImmutableDictionary<string, string>.Empty;
    public IDictionary<string, string> ToAddresses { get; set; } = ImmutableDictionary<string, string>.Empty;
}
