namespace Humanidy.Generators;

internal static class EntropyCalculator
{
    /// <summary>
    /// Gets the entropy in bits of an identifier with the given length and alphabet size.
    /// The entropy is calculated as <c>length * log2(alphabetSize)</c>.
    /// </summary>
    /// <param name="length">The length of the randomly-generated string.</param>
    /// <param name="alphabetSize">The length of the alphabet used to generate the string.</param>
    /// <returns>The entropy in bits.</returns>
    public static double Entropy(int length, int alphabetSize)
    {
        return length * Math.Log(alphabetSize, 2);
    }
}
