namespace Claims.Models
{
	using System;
	using System.Diagnostics.Contracts;
	using System.Net.Http;
	using System.Net.Http.Headers;
	using System.Threading.Tasks;

	public class ClaimsAuthority
	{
		public HttpClient Client { get; private set; }
		public Uri Server { get; private set; }

		public ClaimsAuthority()
		{
			this.Server = new Uri("http://localhost:3000");
			this.Client = new HttpClient();
			this.Client.BaseAddress = this.Server;
			this.Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		}

		public async Task Expand(Claims claims, string cids)
		{
			var realm = await claims.Get(KnownIdentities.Realm, resolve: false);
			var uid = await claims.Get(KnownIdentities.Uid, resolve: false);
			Contract.Assert(!string.IsNullOrWhiteSpace(realm), "claims error -- invalid realm");
			Contract.Assert(!string.IsNullOrWhiteSpace(uid), "claims error -- invalid uid");
			var path = string.Format("/{0}/claims-ticket/{1}/expand?cids={2}", realm, uid, cids);
			var res = await this.Client.GetAsync(path);
			Contract.Assert(res.IsSuccessStatusCode, string.Format("claims error -- failed to expand: '{0} ({1})'", (int)res.StatusCode, res.ReasonPhrase));
			var parsed = await res.Content.ReadAsAsync<Claims>();
			claims.Merge(parsed);
		}
	}
}
