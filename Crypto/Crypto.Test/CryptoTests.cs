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
MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAwKzGUWdKLak1O1rgG6Kd
6A5e1zOwROhqEIOLxDVSs8Ojf9jUh8agx8YStXwa01divsWgnOHBgYJomnKZY6Mq
hYfzd+dvsHqzcEslsB8Q+lnCUPh6WrpKP8GFARn5XCzEC8mji1me2Fmun5w6zKcf
m4+cNSevbssE18vG1ngJu7coaQ56dqhz2LugHURKEJxcQWgR2uj9FyZRsFVWofz3
NeEunQGWOdC+S44mU7MAhR+BGFqLj0T1Uroc3UKq3zsK9D87lqVKvoLsiFlklML1
PCDNIiB5kH2XGUBEn3lmeESHIwTVu+/eBZJUm+9zrpkSHeWou99E6VZUffikP896
CQIDAQAB
-----END PUBLIC KEY-----";
		string privkey = @"-----BEGIN RSA PRIVATE KEY-----
MIIEogIBAAKCAQEAwKzGUWdKLak1O1rgG6Kd6A5e1zOwROhqEIOLxDVSs8Ojf9jU
h8agx8YStXwa01divsWgnOHBgYJomnKZY6MqhYfzd+dvsHqzcEslsB8Q+lnCUPh6
WrpKP8GFARn5XCzEC8mji1me2Fmun5w6zKcfm4+cNSevbssE18vG1ngJu7coaQ56
dqhz2LugHURKEJxcQWgR2uj9FyZRsFVWofz3NeEunQGWOdC+S44mU7MAhR+BGFqL
j0T1Uroc3UKq3zsK9D87lqVKvoLsiFlklML1PCDNIiB5kH2XGUBEn3lmeESHIwTV
u+/eBZJUm+9zrpkSHeWou99E6VZUffikP896CQIDAQABAoIBAAjbqJ5zdjiVL+pT
qjv7BPF3P6r7KF5ypPWvZoyVuwCivPCSwmqfmiK1G4UWeJFA0jxdD55GzsOqgC6h
mCW5ianr7yiTDlJ5wagm/EuVkg1AdSuN0Oo86E74EapUTF+Ne1Hjak5sx13A+cNC
xn1ELbLULmVPNsSMNuPI7ZJMoH3twE3xf0OYMmikdWdzMkWN48kqjW6g7pgCrTJW
oOnA73MZzNw49zOvWyOn6HmZP8Z9jkzC8tkj2pRTpHF8N4nHX9cZ955UFRsmdRq3
9dUVzsvwjlohhTKIs91l6XO1rG+3nBdpF1zc5Eb5QfgDhTNrk4LIbAUEsPM2dVfy
e/0/81ECgYEA6fVE/RZFVymjzSCJCRk94GElbg8+HDSb6FvZeYqg064szntAN8Co
9f/BxHAjvTj1U10ewDWkl0Zk2MIJuH6TkCUH8HA/HmvGXbFOfOShWpjfjQjcnxex
ebcZKmPg2PRBIp/k0o7LhICxyJEw7bTsttBXRE1PPbIs0f4k0+WUGM0CgYEA0tPQ
hjSBh3+xm22DoBBACZfXXOtUHDILincUWs6kgwchjx2bWoJ0RBbcbf7xHGfV03iv
S44TQ5vSA7EhPVEyr1zIxxbKeuVsagj/Ig4iH7bv1Dg0BaEqDWbChDdsBMYG+jgV
1fqQ7FmrXR4+oe8SUhig7nZ9Kyn1NXyHRAM7li0CgYAfrOZnQaGnuGqF9ja3daRs
AZsYuJWXKgRFxFHAY+V8Y2OLnSJHqhz2GWd5mhksoE6Ot9fW/CxJku1Kb5LzWKfP
50OY6QmSq9LSkpPm0umJo5L4vE9qdsG/keulUpp5pTBAaNeJLm4dhXMMjaPLOyil
Y7W8+J6W4s2wrLD9w2J2rQKBgHV+rnvcz0Ngmu7wPab653VxNgkG7b049s2YR7PD
7lNHqiBEc8whYAKCax6yMlWOWNCeNbN53qqTMJv2H3w4wHKS1yJ9RSLwsVb25Jlq
HwJlxYsUN4nbo8AMQOPGr2CJrGHO1/yExDe0UZh3vpgo0KEIMMg1KNThcVL5r7jr
xE2RAoGAIAii/G7nHhkmqBh/jIfW9wtiruuhhCLk4IleWjdVxCRpV8ye/32LLBJP
XfVv+GztzLwgIDkMZqB/Oi5zsuSbWuNqhGq578p+inGOoAfHUMIBCMPIW4oCuNAf
V3Hwj3gttWZ46A1CxDq+WzL+QoYm15btc4pFdry7guW+RcU3h8s=
-----END RSA PRIVATE KEY-----";

		string data = @"0.f,e.8,12.2;0.1:TlM=.2:WFk=.4:bnM=.8:cHdz.10:MTIzNDU=.20:GVzdEBlbWFpbC5jb20=,e.8:dGVzdFZhbHVl;3000-06-30T18:38:36.480Z";
		string sig = "ZXwVqjzrMJ7zx1U3Iy4HKgqcelYNXxALwLghS9iozwdmLQGAa4n1dk2ZcAUrox8T8x8nulHXlot9HUZYE5OrmVITDf1iMUdKcY63LB9cYf3zJlNleDvyF02vFpssGjoUhL6IEM5mZWgseGpvzozKLYR00CDwP+gR85WFhFen51NV6ua4OMYDD5eSE9pA+cBD8gox106V6V6nQHtL844P0EeBNiM0z2xTnUPD8wwD4t7PH09Q5We7N3YMoeG+fGmAUymShoOOHCDzUFWORJPrtVZiTmolDpbPsM0X5G5P8VNzyxHUGydvumcolLrVQnbSNW0jNyBIfBQ81rllp1mXOQ==";

		[TestMethod]
		public void SignAndVerifyTest()
		{
			var signer = new Signer(privkey, true);
			string newsig = signer.Sign(data);
			Assert.AreEqual(sig, newsig);
			var verifier = new Verifier(pubkey, false);
			Assert.IsTrue(verifier.Verify(data, sig));
		}
	}
}
