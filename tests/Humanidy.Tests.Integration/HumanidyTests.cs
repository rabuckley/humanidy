namespace Humanidy;

[Humanidy("test", RandomLength = 24)]
public partial struct TestId;

public sealed class HumanidyTests
{
    [Fact]
    public void NewId_Twice_ShouldBeDifferent()
    {
        var id1 = TestId.NewId();
        var id2 = TestId.NewId();
        Assert.NotEqual(id1, id2);
    }

    [Fact]
    public void NewId_ShouldHaveCorrectLength()
    {
#pragma warning disable xUnit2000 // Constant value should be expected. Testing that the constant is generated correctly.
        Assert.Equal("test_".Length + 24, TestId.Length);
#pragma warning restore xUnit2000
    }

    [Fact]
    public void NewId_ShouldHaveCorrectPrefix()
    {
        var id = TestId.NewId();
        var str = id.ToString();
        Assert.StartsWith("test_", str, StringComparison.Ordinal);
    }

    [Fact]
    public void NewId_ShouldHaveValidCharacters()
    {
        var id = TestId.NewId();
        var str = id.ToString();
        Assert.All(str, c => Assert.True(char.IsAsciiLetterOrDigit(c) || c == '_'));
    }

    [Fact]
    public void GetHashCode_ShouldBeConsistent()
    {
        var id = TestId.NewId();
        var hash1 = id.GetHashCode();
        var hash2 = id.GetHashCode();
        Assert.Equal(hash1, hash2);
    }

    [Fact]
    public void GetHashCode_WithDifferentIds_ShouldBeDifferent()
    {
        // Arrange
        var id1 = TestId.NewId();
        var id2 = TestId.NewId();

        // Act
        var hash1 = id1.GetHashCode();
        var hash2 = id2.GetHashCode();

        // Assert
        Assert.NotEqual(hash1, hash2);
    }

    [Fact]
    public void SpanOfByte_Roundtrip()
    {
        var id = TestId.NewId();
        Span<byte> span = stackalloc byte[TestId.Length];
        Assert.True(id.TryFormat(span, out var charsWritten));
        Assert.Equal(TestId.Length, charsWritten);
        Assert.True(TestId.TryParse(span, out var parsedId));
        Assert.Equal(id, parsedId);
    }

    [Fact]
    public void String_Roundtrip()
    {
        var id = TestId.NewId();
        var str = id.ToString();
        Assert.NotEmpty(str);
        Assert.True(TestId.TryParse(str, out var parsedId));
        Assert.Equal(id, parsedId);
    }

    [Fact]
    public void SpanOfChar_Roundtrip()
    {
        var id = TestId.NewId();
        Span<char> span = stackalloc char[TestId.Length];
        Assert.True(id.TryFormat(span, out var charsWritten));
        Assert.Equal(TestId.Length, charsWritten);
        Assert.True(TestId.TryParse(span, out var parsedId));
        Assert.Equal(id, parsedId);
    }

    [Fact]
    public void IFormattable_IParsable_Roundtrip()
    {
        var id = TestId.NewId();

        var str = ((IFormattable)id).ToString("G", null);

        Assert.NotEmpty(str);
        Assert.True(TryParse(str, out TestId parsedId));
        Assert.Equal(id, parsedId);

        static bool TryParse<T>(string s, out T? result)
            where T : IParsable<T>
        {
            return T.TryParse(s, null, out result);
        }
    }

    [Fact]
    public void ISpanFormattable_ISpanParsable_Roundtrip()
    {
        var id = TestId.NewId();

        Span<char> span = stackalloc char[TestId.Length];

        var result = ((ISpanFormattable)id).TryFormat(span, out var charsWritten, null, null);
        Assert.True(result);
        Assert.Equal(TestId.Length, charsWritten);

        Assert.True(TryParse(span, out TestId parsedId));
        Assert.Equal(id, parsedId);

        static bool TryParse<T>(ReadOnlySpan<char> s, out T? result)
            where T : ISpanParsable<T>
        {
            return T.TryParse(s, null, out result);
        }
    }

    [Fact]
    public void IUtf8SpanFormattable_IUtf8SpanParsable_Roundtrip()
    {
        var id = TestId.NewId();

        Span<byte> span = stackalloc byte[TestId.Length];

        var result = ((IUtf8SpanFormattable)id).TryFormat(span, out var bytesWritten, null, null);
        Assert.True(result);
        Assert.Equal(TestId.Length, bytesWritten);

        Assert.True(TryParse(span, out TestId parsedId));
        Assert.Equal(id, parsedId);

        static bool TryParse<T>(ReadOnlySpan<byte> s, out T? result)
            where T : IUtf8SpanParsable<T>
        {
            return T.TryParse(s, null, out result);
        }
    }

    [Fact]
    public void EqualityOperator()
    {
        var id = TestId.NewId();
        AssertEquality(id, id);

        // To confuse the "you're comparing the same thing" analyzers.
        static void AssertEquality(TestId left, TestId right)
        {
            Assert.True(left == right);
            Assert.False(left != right);
        }
    }


    [Fact]
    public void EqualityOperator_DifferentIds()
    {
        var left = TestId.NewId();
        var right = TestId.NewId();
        Assert.False(left == right);
        Assert.True(left != right);
    }
}
