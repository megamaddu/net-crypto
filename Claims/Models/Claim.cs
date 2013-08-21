namespace Claims.Models
{
	public class Claim
	{
		public string Id { get; private set; }
		public string Kind { get; private set; }
		public string Name { get; private set; }
		public string Value { get; private set; }

		public Claim(
			string id = null,
			string kind = null,
			string name = null,
			string value = null)
		{
			Id = id ?? "0";
			Kind = kind ?? ClaimKind.Unknown;
			Name = name;
			Value = value;
		}

		internal void Merge(Claim claim)
		{
			this.Kind = claim.Kind;
			this.Name = claim.Name;
			this.Value = claim.Value;
		}
	}
}
