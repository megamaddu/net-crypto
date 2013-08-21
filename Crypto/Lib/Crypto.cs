namespace Crypto
{
	using System;
	using System.Diagnostics.Contracts;
	using System.Security.Cryptography;
	using System.Text;
	using Extensions;

	/// <summary>
	/// Abstract base class for PEM key RSA encryption.
	/// </summary>
	public abstract class Crypto
	{
		private string _pemKey;
		private bool _isPrivate;

		internal Crypto(string pemKey, bool isPrivate)
		{
			Contract.Requires(!string.IsNullOrEmpty(pemKey));

			_pemKey = pemKey;
			_isPrivate = isPrivate;
		}

		protected T InvokeProvider<T>(EncodingKind encodingKind, AlgorithmKind algorithmKind, Func<RSACryptoServiceProvider, Encoding, HashAlgorithm, T> method)
		{
			var encoding = GetEncoding(encodingKind);
			using (var rsa = new RSACryptoServiceProvider())
			{
				rsa.PersistKeyInCsp = false;
				if (_isPrivate)
					rsa.LoadPrivateKeyPEM(_pemKey);
				else
					rsa.LoadPublicKeyPEM(_pemKey);

				using (var algorithm = GetAlgorithm(algorithmKind))
					return method(rsa, encoding, algorithm);
			}
		}

		protected Encoding GetEncoding(EncodingKind encodingKind)
		{
			Encoding encoding;
			switch (encodingKind)
			{
				case EncodingKind.UTF8:
				default:
					encoding = Encoding.UTF8;
					break;
			}

			return encoding;
		}

		protected HashAlgorithm GetAlgorithm(AlgorithmKind algorithmKind)
		{
			HashAlgorithm algorithm;
			switch (algorithmKind)
			{
				case AlgorithmKind.SHA1:
				default:
					algorithm = new SHA1CryptoServiceProvider();
					break;
			}

			return algorithm;
		}
	}
}
