using Selqi.Application.Auth;
using Selqi.Application.Email;
using Selqi.Domain.Identity;
using Xunit;

namespace Selqi.Application.Tests.Auth;

public sealed class AuthServiceTests
{
    // ── Positive: Register ──────────────────────────────────────────────────

    [Fact]
    public async Task RegisterAsync_WithValidData_ReturnsAuthResponse()
    {
        var service = BuildService();

        var response = await service.RegisterAsync(
            new RegisterRequest("new@selqi.local", "Password1!", "Alice", "Smith"));

        Assert.NotNull(response);
        Assert.Equal("new@selqi.local", response.User.Email);
        Assert.Equal("Alice", response.User.FirstName);
        Assert.Equal("stub-token", response.Token);
    }

    [Fact]
    public async Task RegisterAsync_NormalisesEmailToLowercase()
    {
        var service = BuildService();

        var response = await service.RegisterAsync(
            new RegisterRequest("UPPER@SELQI.LOCAL", "Password1!", "Bob", "Jones"));

        Assert.Equal("upper@selqi.local", response.User.Email);
    }

    // ── Negative: Register ──────────────────────────────────────────────────

    [Fact]
    public async Task RegisterAsync_WithDuplicateEmail_ThrowsInvalidOperationException()
    {
        var userRepo = new FakeUserRepository();
        await userRepo.AddAsync(BuildUser("taken@selqi.local"), default);
        await userRepo.SaveChangesAsync();

        var service = BuildService(userRepo);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.RegisterAsync(
                new RegisterRequest("taken@selqi.local", "Password1!", "Eve", "Evil")));
    }

    // ── Positive: Login ─────────────────────────────────────────────────────

    [Fact]
    public async Task LoginAsync_WithCorrectCredentials_ReturnsAuthResponse()
    {
        const string email = "login@selqi.local";
        const string password = "Password1!";

        var userRepo = new FakeUserRepository();
        await userRepo.AddAsync(BuildUser(email, password), default);
        await userRepo.SaveChangesAsync();

        var service = BuildService(userRepo);

        var response = await service.LoginAsync(new LoginRequest(email, password));

        Assert.Equal(email, response.User.Email);
        Assert.NotEmpty(response.Token);
    }

    // ── Negative: Login ─────────────────────────────────────────────────────

    [Fact]
    public async Task LoginAsync_WithWrongPassword_ThrowsUnauthorizedAccessException()
    {
        const string email = "user@selqi.local";

        var userRepo = new FakeUserRepository();
        await userRepo.AddAsync(BuildUser(email, "CorrectPass1!"), default);
        await userRepo.SaveChangesAsync();

        var service = BuildService(userRepo);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            service.LoginAsync(new LoginRequest(email, "WrongPass1!")));
    }

    [Fact]
    public async Task LoginAsync_WithNonExistentEmail_ThrowsUnauthorizedAccessException()
    {
        var service = BuildService();

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            service.LoginAsync(new LoginRequest("ghost@selqi.local", "Password1!")));
    }

    // ── Positive: GetCurrentUser ────────────────────────────────────────────

    [Fact]
    public async Task GetCurrentUserAsync_WithValidId_ReturnsUserDto()
    {
        var user = BuildUser("me@selqi.local");
        var userRepo = new FakeUserRepository();
        await userRepo.AddAsync(user, default);
        await userRepo.SaveChangesAsync();

        var service = BuildService(userRepo);

        var dto = await service.GetCurrentUserAsync(user.Id);

        Assert.Equal(user.Id, dto.Id);
        Assert.Equal("me@selqi.local", dto.Email);
    }

    // ── Negative: GetCurrentUser ────────────────────────────────────────────

    [Fact]
    public async Task GetCurrentUserAsync_WithUnknownId_ThrowsKeyNotFoundException()
    {
        var service = BuildService();

        await Assert.ThrowsAsync<KeyNotFoundException>(() =>
            service.GetCurrentUserAsync(Guid.NewGuid()));
    }

    // ── Positive: ResetPassword ─────────────────────────────────────────────

    [Fact]
    public async Task ResetPasswordAsync_WithValidToken_UpdatesPasswordHash()
    {
        var user = BuildUser("reset@selqi.local", "OldPass1!");
        var userRepo = new FakeUserRepository();
        await userRepo.AddAsync(user, default);
        await userRepo.SaveChangesAsync();

        var token = new PasswordResetToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = "valid-token",
            ExpiresAtUtc = DateTime.UtcNow.AddHours(1),
            CreatedAtUtc = DateTime.UtcNow
        };
        var resetRepo = new FakePasswordResetRepository(token);

        var service = BuildService(userRepo, resetRepo);

        await service.ResetPasswordAsync(
            new ResetPasswordRequest("valid-token", "NewPass1!", "NewPass1!"));

        Assert.True(PasswordHasher.Verify("NewPass1!", user.PasswordHash));
    }

    // ── Negative: ResetPassword ─────────────────────────────────────────────

    [Fact]
    public async Task ResetPasswordAsync_WithMismatchedPasswords_ThrowsInvalidOperationException()
    {
        var service = BuildService();

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.ResetPasswordAsync(
                new ResetPasswordRequest("any-token", "NewPass1!", "Different1!")));
    }

    [Fact]
    public async Task ResetPasswordAsync_WithExpiredToken_ThrowsInvalidOperationException()
    {
        var user = BuildUser("expired@selqi.local");
        var userRepo = new FakeUserRepository();
        await userRepo.AddAsync(user, default);
        await userRepo.SaveChangesAsync();

        var token = new PasswordResetToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = "expired-token",
            ExpiresAtUtc = DateTime.UtcNow.AddHours(-1),
            CreatedAtUtc = DateTime.UtcNow.AddHours(-2)
        };
        var resetRepo = new FakePasswordResetRepository(token);

        var service = BuildService(userRepo, resetRepo);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.ResetPasswordAsync(
                new ResetPasswordRequest("expired-token", "NewPass1!", "NewPass1!")));
    }

    [Fact]
    public async Task ResetPasswordAsync_WithAlreadyUsedToken_ThrowsInvalidOperationException()
    {
        var user = BuildUser("used@selqi.local");
        var userRepo = new FakeUserRepository();
        await userRepo.AddAsync(user, default);
        await userRepo.SaveChangesAsync();

        var token = new PasswordResetToken
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Token = "used-token",
            ExpiresAtUtc = DateTime.UtcNow.AddHours(1),
            UsedAtUtc = DateTime.UtcNow.AddMinutes(-10),
            CreatedAtUtc = DateTime.UtcNow.AddHours(-1)
        };
        var resetRepo = new FakePasswordResetRepository(token);

        var service = BuildService(userRepo, resetRepo);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.ResetPasswordAsync(
                new ResetPasswordRequest("used-token", "NewPass1!", "NewPass1!")));
    }

    [Fact]
    public async Task ResetPasswordAsync_WithInvalidToken_ThrowsInvalidOperationException()
    {
        var service = BuildService(resetRepo: new FakePasswordResetRepository(null));

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            service.ResetPasswordAsync(
                new ResetPasswordRequest("no-such-token", "NewPass1!", "NewPass1!")));
    }

    // ── Helpers ─────────────────────────────────────────────────────────────

    private static AuthService BuildService(
        FakeUserRepository? userRepo = null,
        FakePasswordResetRepository? resetRepo = null) =>
        new(
            userRepo ?? new FakeUserRepository(),
            new FakeTokenService(),
            resetRepo ?? new FakePasswordResetRepository(null),
            new FakeEmailService());

    private static ApplicationUser BuildUser(string email, string? password = null) => new()
    {
        Id = Guid.NewGuid(),
        Email = email,
        PasswordHash = PasswordHasher.Hash(password ?? "Default1!"),
        FirstName = "Test",
        LastName = "User",
        Role = UserRoles.Customer,
        CreatedAtUtc = DateTime.UtcNow
    };

    // ── Fakes ────────────────────────────────────────────────────────────────

    private sealed class FakeUserRepository : IUserRepository
    {
        private readonly List<ApplicationUser> _store = [];

        public Task<ApplicationUser?> FindByEmailAsync(string email, CancellationToken ct = default)
            => Task.FromResult(_store.FirstOrDefault(u => u.Email == email));

        public Task<ApplicationUser?> FindByIdAsync(Guid id, CancellationToken ct = default)
            => Task.FromResult(_store.FirstOrDefault(u => u.Id == id));

        public Task<bool> EmailExistsAsync(string email, CancellationToken ct = default)
            => Task.FromResult(_store.Any(u => u.Email == email));

        public Task AddAsync(ApplicationUser user, CancellationToken ct = default)
        {
            _store.Add(user);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(ApplicationUser user, CancellationToken ct = default)
            => Task.CompletedTask;

        public Task SaveChangesAsync(CancellationToken ct = default)
            => Task.CompletedTask;
    }

    private sealed class FakeTokenService : ITokenService
    {
        public (string Token, DateTime ExpiresAt) Generate(ApplicationUser user)
            => ("stub-token", DateTime.UtcNow.AddHours(1));
    }

    private sealed class FakePasswordResetRepository(PasswordResetToken? stored) : IPasswordResetRepository
    {
        private readonly List<PasswordResetToken> _store = stored is null ? [] : [stored];

        public Task AddAsync(PasswordResetToken token, CancellationToken ct = default)
        {
            _store.Add(token);
            return Task.CompletedTask;
        }

        public Task<PasswordResetToken?> FindByTokenAsync(string token, CancellationToken ct = default)
            => Task.FromResult(_store.FirstOrDefault(t => t.Token == token));

        public Task SaveChangesAsync(CancellationToken ct = default)
            => Task.CompletedTask;
    }

    private sealed class FakeEmailService : IEmailService
    {
        public Task SendAsync(string to, string subject, string htmlBody, CancellationToken ct = default)
            => Task.CompletedTask;

        public Task SendWithAttachmentAsync(string to, string subject, string htmlBody,
            string attachmentName, byte[] attachment, CancellationToken ct = default)
            => Task.CompletedTask;
    }
}
