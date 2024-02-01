using System.Security.Cryptography;

namespace H3___Secure_Login_With_GUI.Security
{
	public class EncryptionService
	{
		private const int SaltSize = 16; // 128 bit
		private const int KeySize = 32; // 256 bit
		private const int Iterations = 10000;

		/// <summary>
		/// Return a hashed version of a plaintext string.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string Encrypt(string value)
		{
			using (var algorithm = new Rfc2898DeriveBytes(
				value,
				SaltSize,
				Iterations,
				HashAlgorithmName.SHA256))
			{
				var key = Convert.ToBase64String(algorithm.GetBytes(KeySize));
				var salt = Convert.ToBase64String(algorithm.Salt);

				return $"{Iterations}.{salt}.{key}";
			}
		}

		public static bool Verify(string hash, string value)
		{
			var parts = hash.Split('.', 3);

			if (parts.Length != 3)
			{
				throw new FormatException($"Invalid hash format: {parts}");
			}

			var iterations = Convert.ToInt32(parts[0]);
			var salt = Convert.FromBase64String(parts[1]);
			var key = Convert.FromBase64String(parts[2]);

			using (var algorithm = new Rfc2898DeriveBytes(
				value,
				salt,
				iterations,
				HashAlgorithmName.SHA256))
			{
				var keyToCheck = algorithm.GetBytes(KeySize);
				var verified = keyToCheck.SequenceEqual(key);

				return verified;
			}
		}
	}
}
