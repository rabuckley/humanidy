namespace Humanidy;

[AttributeUsage(AttributeTargets.Struct)]
public sealed class HumanidyAttribute : Attribute
{
    public HumanidyAttribute(string prefix)
    {
        Prefix = prefix switch
        {
            string when string.IsNullOrWhiteSpace(prefix) => throw new ArgumentException("Prefix cannot be null or whitespace.", nameof(prefix)),
            { Length: < 2 } => throw new ArgumentException("Prefix length must be at least 2 characters.", nameof(prefix)),
            { Length: > 8 } => throw new ArgumentException("Prefix length must be less than or equal to 8 characters.", nameof(prefix)),
            _ => prefix
        };
    }

    /// <summary>
    /// The prefix used for the identifier.
    /// </summary>
    public string Prefix { get; }

    /// <summary>
    /// The length of the randomly generated part of the identifier.
    /// </summary>
    public int RandomLength { get; set; } = 14;
}
