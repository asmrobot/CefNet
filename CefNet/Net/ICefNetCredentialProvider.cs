using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace CefNet.Net
{
	/// <summary>
	/// Provides credentials.
	/// </summary>
	public interface ICefNetCredentialProvider
	{
		/// <summary>
		/// Called when the browser needs credentials from the user.
		/// </summary>
		/// <param name="proxy">Indicates whether the <paramref name="host"/> is a proxy server.</param>
		/// <param name="host">The hostname.</param>
		/// <param name="port">The port number.</param>
		/// <param name="realm">
		/// The realm is used to describe the protected area or to indicate the scope of protection.
		/// </param>
		/// <param name="scheme">The authentication scheme.</param>
		/// <returns>
		/// The <see cref="NetworkCredential"/> that is associated with the specified host, port
		/// and authentication type, or, if no credentials are available, null.
		/// </returns>
		Task<NetworkCredential> GetCredentialAsync(bool proxy, string host, int port, string realm, string scheme, CancellationToken cancellationToken);
	}

}
