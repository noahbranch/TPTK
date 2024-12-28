using System;
using System.Security.Cryptography;
using System.Text;

public static class PasswordHasher {
  private const int SaltSize = 16; // 128-bit salt
  private const int KeySize = 32;  // 256-bit hash
  private const int Iterations = 10000;

  public static string HashPassword(string password) {
    using var algorithm = new Rfc2898DeriveBytes(
        password,
        SaltSize,
        Iterations,
        HashAlgorithmName.SHA256
    );
    var salt = algorithm.Salt;
    var hash = algorithm.GetBytes(KeySize);

    // Combine salt and hash into a single string
    var hashBytes = new byte[SaltSize + KeySize];
    Array.Copy(salt, 0, hashBytes, 0, SaltSize);
    Array.Copy(hash, 0, hashBytes, SaltSize, KeySize);

    // Convert to Base64 for storage
    return Convert.ToBase64String(hashBytes);
  }

  public static bool VerifyPassword(string password, string hashedPassword) {
    var hashBytes = Convert.FromBase64String(hashedPassword);

    // Extract salt from stored hash
    var salt = new byte[SaltSize];
    Array.Copy(hashBytes, 0, salt, 0, SaltSize);

    // Hash the input password with the extracted salt
    using var algorithm = new Rfc2898DeriveBytes(
        password,
        salt,
        Iterations,
        HashAlgorithmName.SHA256
    );
    var hash = algorithm.GetBytes(KeySize);

    // Extract hash from stored hash
    for (int i = 0; i < KeySize; i++) {
      if (hashBytes[i + SaltSize] != hash[i]) {
        return false;
      }
    }

    return true;
  }
}
