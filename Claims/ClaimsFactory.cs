namespace Claims
{
	public class ClaimsFactory
	{
		public object Options { get; set; }

		public ClaimsFactory(object options)
		{
			// module configuration
			this.Options = options;
		}

		public Claims.Models.Claims Parse(string ticket)
		{
			return Parser.Parse(ticket, Options);
		}

		public Claims.Models.Claims From(string json)
		{
			return Parser.From(json, Options);
		}
	}
}
