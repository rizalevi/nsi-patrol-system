namespace NSI.Domain.Auth;

public class RefreshToken
{
    public Guid Id { get; private set; }
    public string UserId { get; private set; }
    public string TokenHash { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public bool IsRevoked { get; private set; }
    public DateTime CreatedAt { get; set; }

    public RefreshToken()
    {

    }
    public RefreshToken(string userId, string tokenHash, DateTime expiresAt)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        TokenHash = tokenHash;
        ExpiresAt = expiresAt;
        IsRevoked = false;
    }

    public void Revoke() => IsRevoked = true;

    public bool IsExpired() => ExpiresAt < DateTime.UtcNow;
}
