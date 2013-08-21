using System;
using System.Security.Cryptography;
using System.Text;
using Crypto;
using Crypto.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Crypto.Test
{
	[TestClass]
	public class CryptoTests
	{
		string pubkey = @"-----BEGIN PUBLIC KEY-----
MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDDV5IxvBBXha3vpbLv60ZoBFwG
gwaRhI9vnD+MbBBplFPDkHBQyNF0KoPQCUaEDp9EAzvpKU7BFbJcl+ktbqDJNiZi
gdIgl/CuAUcnBWdqZc0hR7lxNa945xK0nhqtRulQq0Ie4Q+D8vFgtw2sR9Pebw1O
CZ3Lg9eXWwyLZcRnqwIDAQAB
-----END PUBLIC KEY-----";
		string privkey = @"-----BEGIN RSA PRIVATE KEY-----
MIICXAIBAAKBgQDDV5IxvBBXha3vpbLv60ZoBFwGgwaRhI9vnD+MbBBplFPDkHBQ
yNF0KoPQCUaEDp9EAzvpKU7BFbJcl+ktbqDJNiZigdIgl/CuAUcnBWdqZc0hR7lx
Na945xK0nhqtRulQq0Ie4Q+D8vFgtw2sR9Pebw1OCZ3Lg9eXWwyLZcRnqwIDAQAB
AoGAWY8pwNGncGkisO/4VRU6Z3AwPPAPr+Hl3Vb2r8vPzogpy0zKoc8gcLEZU+Uc
xmjpzkXpTuIYcGAhMWCYkLLa1fiGvn+DBoPmEz67gj2UO0YI6hLeady8F1NSZ87Q
OxrgTMJneSsBLfJ6liloZRndU4X86FAb7WK5/mSUK8E/sikCQQDfoqTr3HbMUcsN
TU2mmq0v6bdeAqVexz7FOx/sxz/sLLoofXZv85KP/5zAYm+gxnOHaYKqafNBQtq1
MGDSljy/AkEA35y0M/4L9G0lSGK0bUnJxnQvNB1r+KnpUQ27laTlXbTqHrUfBNe9
p/Okl8+n0JEQqsCZEdslwsT8DHp7WW2UFQJAQjr33sZHBJHAserP8WRjoAn2fUgJ
U71sUJsHBGep/bbtVup5NgSLxkusT6mXZ6T+N/8+bFu+Z/h1ry1pr1RBHwJBAJof
lSkc2jqPnBnnRnrpV/S8Eej2unu7CQB/2aJL9HeBAblGl2msFaGUUgb87qrXwcgf
VaG0DbPQN/WV2j0KRP0CQH6uzmSH7PAATyhIVA+CjRcV5MovKsQplHZL6Jf/u+jx
SGSQ4Hmb6WsFJkNrmV5T0t779XoBknfsR/LrxrqR5iw=
-----END RSA PRIVATE KEY-----";

		[TestMethod]
		public void SignAndVerifyTest()
		{
			var testdata = "test";
			var signer = new Signer(privkey, true);
			string sig = signer.Sign(testdata);

			var verifier = new Verifier(pubkey, false);
			Assert.IsTrue(verifier.Verify(testdata, sig));
		}
	}
}
