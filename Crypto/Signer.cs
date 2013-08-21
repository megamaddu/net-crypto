namespace Crypto
{
	using System;
	using System.Diagnostics.Contracts;
	using System.Security.Cryptography;
	using System.Text;
	using Extensions;

	/// <summary>
	/// PEM key RSA signer.
	/// </summary>
	public sealed class Signer : Crypto
	{
		/// <summary>
		/// Creates a signer for the given key.
		/// </summary>
		/// <param name="pemKey">PEM format key.</param>
		/// <param name="isPrivate">True if pemKey is a private key, otherwise False.</param>
		public Signer(string pemKey, bool isPrivate)
			: base(pemKey, isPrivate)
		{
		}

		/// <summary>
		/// Signs the given data using this Signer's key.
		/// </summary>
		/// <param name="data">The input data as a string to hash and sign.</param>
		/// <param name="encKind">Encoding format of the input data.</param>
		/// <param name="algKind">Hashing algorithm to use.</param>
		/// <returns>The signature of the input data as a base64 encoded string.</returns>
		public string Sign(string data, EncodingKind encKind = EncodingKind.UTF8, AlgorithmKind algKind = AlgorithmKind.SHA1)
		{
			Contract.Requires(!string.IsNullOrEmpty(data));

			return InvokeProvider(encKind, algKind, (rsa, enc, alg) => Convert.ToBase64String(rsa.SignData(enc.GetBytes(data), alg)));
		}
	}
}
