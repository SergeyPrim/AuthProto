using System.Security.Cryptography;
using System.Text;

namespace AuthProto.Shared.Utilities
{
    public static class HashService
    {
        const int _hashKeySize = 64;
        const int _hashIterations = 10000;
        readonly static HashAlgorithmName _hashAlgorithm = HashAlgorithmName.SHA512;

        public static string HashPasword(string password, out string salt)
        {
            var saltBytes = RandomNumberGenerator.GetBytes(_hashKeySize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(password),
                saltBytes,
                _hashIterations,
                _hashAlgorithm,
                _hashKeySize);

            salt = Convert.ToHexString(saltBytes);
            return Convert.ToHexString(hash);
        }

        public static bool CheckPasword(string pass, string hash, string salt)
        {
            var saltBytes = Convert.FromHexString(salt);
            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(pass, saltBytes, _hashIterations, _hashAlgorithm, _hashKeySize);

            return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(hash));
        }
    }
}
