namespace Claims
{
	using System;
	using System.Linq;
	using System.Collections.Generic;
	using System.Diagnostics.Contracts;
	using Claims.Models;

	internal static class Parser
	{
		internal static class Conf
		{
			public const string Preamble = "mialc";
			public const char VersionAndClaimAndImpersonatorSeperator = '#';
			public const char ClaimAndSignatureSeperator = '|';
			public const char SectionSeperator = ';';
			public const char ItemSeperator = ',';
			public const char DetailSeperator = ':';
			public const char ClaimsetSeparator = '.';
			public const int ClaimIndex = 1;
			public const int ImpersonatorIndex = 2;
			public const int ClaimSectionsIndex = 0;
			public const int VersionIndex = 0;
			public const int ClaimsetIndex = 0;
			public const int ClaimsetHeaderIndex = 0;
			public const int RulesIndex = 1;
			public const int DetailsIndex = 1;
			public const int TimestampIndex = 2;
			public const int SignatureIndex = 1;
			public const int ClaimsetIdIndex = 0;
			public const int ClaimsetRulesIndex = 1;
			public const int ClaimsetDetailsIndex = 1;
			public const int DetailsRuleIdIndex = 0;
			public const int DetailsRuleIndex = 1;
		}

		internal static Claims Parse(string ticket, object options)
		{
			return TicketToJson(ticket, options);
		}

		internal static Claims From(string json, object Options)
		{
			throw new System.NotImplementedException();
		}

		private static Claims TicketToJson(string ticket, object options)
		{
			Contract.Assert(Conf.Preamble.Equals(ticket.Substring(0, Conf.Preamble.Length)), "preamble missing or corrupt");
			var versionAndClaimAndImpersonator = ticket.Substring(Conf.Preamble.Length).Split(Conf.VersionAndClaimAndImpersonatorSeperator);
			var version = versionAndClaimAndImpersonator[Conf.VersionIndex];
			var claimAndSignatureBlock = versionAndClaimAndImpersonator[Conf.ClaimIndex];
			var impersonator = versionAndClaimAndImpersonator.Length + 1 == Conf.ImpersonatorIndex ? versionAndClaimAndImpersonator[Conf.ImpersonatorIndex] : string.Empty;
			Contract.Assert(!string.IsNullOrWhiteSpace(version), "parser error -- version is required");
			Contract.Assert(!string.IsNullOrWhiteSpace(claimAndSignatureBlock), "parser error -- claim and signature are required");
			var claimAndSignature = claimAndSignatureBlock.Split(Conf.ClaimAndSignatureSeperator);
			var claimSectionsBlock = claimAndSignature[Conf.ClaimSectionsIndex];
			var signature = claimAndSignature[Conf.SignatureIndex];
			var encoded = claimSectionsBlock;
			var claimSections = claimSectionsBlock.Split(Conf.SectionSeperator);
			var claimset = claimSections[Conf.ClaimsetIndex];
			var details = claimSections[Conf.DetailsIndex];
			var expiration = claimSections[Conf.TimestampIndex];
			Contract.Assert(!string.IsNullOrWhiteSpace(claimset), "parser error -- claimset section is required");
			Contract.Assert(!string.IsNullOrWhiteSpace(details), "parser error -- details section is required");
			Contract.Assert(!string.IsNullOrWhiteSpace(expiration), "parser error -- expiration section is required");
			var detailBlocks = details.Split(Conf.ItemSeperator);
			var len = detailBlocks.Length;
			var i = -1;
			var parsedDetails = new Dictionary<string, Dictionary<string, string>>();
			while (++i < len)
			{
				var block = detailBlocks[i].Split(Conf.ClaimsetSeparator);
				var claimsetId = new ArraySegment<string>(block, Conf.ClaimsetIdIndex, Conf.ClaimsetDetailsIndex - Conf.ClaimsetIdIndex).First();
				var rawDetailsArray = new ArraySegment<string>(block, Conf.ClaimsetDetailsIndex, block.Length - Conf.ClaimsetDetailsIndex).ToArray();
				var detailsArrayLen = rawDetailsArray.Length;
				var j = -1;
				var parsedValues = new Dictionary<string, string>();
				while (++j < detailsArrayLen)
				{
					var rules = rawDetailsArray[j].Split(Conf.DetailSeperator);
					var detailRuleId = rules[Conf.DetailsRuleIdIndex];
					var detail = rules[Conf.DetailsRuleIndex];
					parsedValues[detailRuleId] = detail;
				}
				parsedDetails[claimsetId] = parsedValues;
			}
			var claimBlocks = claimset.Split(Conf.ItemSeperator);
			len = claimBlocks.Length;
			i = -1;
			var claimsets = new Dictionary<string, Claimset>();
			var knownIdentityValues = new Dictionary<string, string>();
			while (++i < len)
			{
				var block = claimBlocks[i].Split(Conf.ClaimsetSeparator);
				var claimsetId = block[Conf.ClaimsetIdIndex];
				var claimsetRules = int.Parse(block[Conf.ClaimsetRulesIndex], System.Globalization.NumberStyles.HexNumber);
				var claims = new Dictionary<string, Claim>();
				var b = 1;
				while (b <= claimsetRules)
				{
					if (b == (b & claimsetRules))
					{
						var claimId = b.ToString("x");
						var claimOptionsValue = default(string);
						if (parsedDetails.ContainsKey(claimsetId))
						{
							var claimsetDetails = parsedDetails[claimsetId];
							var encodedValue = claimsetDetails[claimId];
							claimOptionsValue = Convert.ToString(Convert.FromBase64String(encodedValue));
						}
						var claimOptionsKind = !string.IsNullOrWhiteSpace(claimOptionsValue) ? ClaimKind.Identity : ClaimKind.Unknown;
						claims[claimId] = new Claim(id: claimId, kind: claimOptionsKind, value: claimOptionsValue);
					}
					b *= 2;
				}
				claimsets[claimsetId] = new Claimset(id: claimsetId, claims: claims, signature: signature);
			}
			var result = new Claims(claimsets, Convert.ToDateTime(expiration), signature, encoded, ticket, (x, y) => false, new ClaimsAuthority());
			return result;
		}
	}
}
