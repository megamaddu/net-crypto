namespace Claims.Models
{
	using System.Collections.Generic;

	public class Claimset
	{
		public string Id { get; private set; }
		public IDictionary<string, Claim> Claims { get; private set; }
		public string Name { get; private set; }
		public string Provider { get; private set; }
		internal string Signature { get; private set; }

		public Claimset(
			string id = null,
			IDictionary<string, Claim> claims = null,
			string name = null,
			string provider = null,
			string signature = null)
		{
			Id = id ?? "0";
			Claims = claims ?? new Dictionary<string, Claim>();
			Name = name;
			Provider = provider;
			Signature = signature;
		}

		internal void Merge(Claimset claimset)
		{
			this.Name = claimset.Name;
			this.Provider = claimset.Name;
			this.Signature = claimset.Name;
			foreach (var from in claimset.Claims)
			{
				Claim claim;
				if (!this.Claims.TryGetValue(from.Key, out claim))
				{
					claim = from.Value;
				}
				else
				{
					claim.Merge(from.Value);
				}
			}
		}
	}
}
