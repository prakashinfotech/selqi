using Selqi.Application.Auth;
using Xunit;

namespace Selqi.Application.Tests.Auth;

public sealed class PasswordHasherTests
{
    [Fact]
    public void VerifyReturnsTrueForMatchingPassword()
    {
        var hash = PasswordHasher.Hash("Customer123!");

        Assert.True(PasswordHasher.Verify("Customer123!", hash));
    }

    [Fact]
    public void VerifyReturnsFalseForDifferentPassword()
    {
        var hash = PasswordHasher.Hash("Customer123!");

        Assert.False(PasswordHasher.Verify("WrongPassword123!", hash));
    }
}
