namespace Humanidy.Generators;

internal static class SourceWriterExtensions
{
    public static void WriteSkipLocalsInitAttr(this SourceWriter builder, HumanidyCompilationOptions options)
    {
        if (options.AllowUnsafe)
        {
            builder.WriteLine("[global::System.Runtime.CompilerServices.SkipLocalsInit]");
        }
    }
}
