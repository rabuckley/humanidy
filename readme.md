# Humanidy

Humanidy makes it easy to create strongly typed, human-readable identifiers in C#.

The identifiers are of the form,

```
pi_3LKQhvGUcADgqoEM3bh6pslE
└─┘└──────────────────────┘
 └─ Prefix    └─ Randomly generated alphanumeric characters
```

Inspired by [Stripe's identifiers](https://dev.to/stripe/designing-apis-for-humans-object-ids-3o5a), Humanidy is built to,

- Be **human-decipherable** - if you see `3e08d1dc-de54-40e8-a490-33082b7b3e34` in your diagnostics where multiple objects are identified by `Guid`, you're none-the-wiser. If you see `user_3LKQhvGUcADgqoEM3bh6pslE`, you know straight away that it's a user identifier. [Stripe](https://docs.stripe.com/api/authentication) also distinguish between live and test API keys with `sk_live_` and `sk_test_` prefixes.
- Be **type-safe and avoid ["primitive obsession"](https://grabbagoft.blogspot.com/2007/12/dealing-with-primitive-obsession.html)** - If you've got multiple objects identified by an identifier of the same type (commonly `Guid` or `int`), they're easily mixed up in method signatures. Strongly-typed identifiers [prevent these bugs at compile time](https://www.youtube.com/watch?v=0arFPIQatCU&t=1650s).
- **Avoid coupling and enumeration attacks** - Using the row number from your SQL database as the identifier ties you to the database implementation and can expose you to enumeration attacks. Humanidy identifiers are decoupled from the underlying storage and don't leak information about your database.

This format is commonly used by companies other than Stripe, too, like [GitHub](https://github.blog/engineering/platform-security/behind-githubs-new-authentication-token-formats/), OpenAI, and Anthropic, to name a few.

## Installation

```sh
dotnet add package Humanidy
```

You can add `PrivateAssets="All"` and `ExcludeAssets="runtime"` to the `PackageReference` to prevent the package being included in other projects and the build output.

## Usage

```csharp
using Humanidy;

[Humanidy("user")]
public partial struct UserId;

var userId = UserId.NewId();

Console.WriteLine(userId); // Something like "user_Zum7hYK9w0bLE"
```

The source generator implements equality, comparison, and serialization, including 

- `IEquatable<T>` and `IComparable<T>`, as well as their operator overloads.
- `ISpanParsable<T>` and `ISpanFormattable` which is used by ASP.NET Core's route and model binding, as well as enabling efficient formatting into interpolated strings.
- `IUtf8SpanParsable<T>` and `IUtf8SpanFormattable` which is used to efficiently read and write JSON.