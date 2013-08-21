namespace Crypto
{
	using System;
	using System.Diagnostics.Contracts;
	using System.Security.Cryptography;
	using System.Text;
	using Extensions;

	/// <summary>
	/// PEM key RSA verifier.
	/// </summary>
	public sealed class Verifier : Crypto
	{
		/// <summary>
		/// Creates a verifier for the given key.
		/// </summary>
		/// <param name="pemKey">PEM format key.</param>
		/// <param name="isPrivate">True if pemKey is a private key, otherwise False.</param>
		public Verifier(string pemKey, bool isPrivate)
			: base(pemKey, isPrivate)
		{
		}

		/// <summary>
		/// Verifies the given data using this Verifier's key.
		/// </summary>
		/// <param name="data">The input data as a string to hash and sign.</param>
		/// <param name="sig">Signature to compare verify as a base64 encoded string.</param>
		/// <param name="encKind">Encoding format of the input data.</param>
		/// <param name="algKind">Hashing algorithm to use.</param>
		/// <returns>True if the given signature matches the input data, otherwise False.</returns>
		public bool Verify(string data, string sig, EncodingKind encKind = EncodingKind.UTF8, AlgorithmKind algKind = AlgorithmKind.SHA1)
		{
			Contract.Requires(!string.IsNullOrEmpty(data));
			Contract.Requires(!string.IsNullOrEmpty(sig));

			return InvokeProvider(encKind, algKind, (rsa, enc, alg) => rsa.VerifyData(enc.GetBytes(data), alg, Convert.FromBase64String(sig)));
		}
	}
}
