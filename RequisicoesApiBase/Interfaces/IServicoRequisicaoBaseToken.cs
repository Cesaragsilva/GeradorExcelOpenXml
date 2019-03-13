using System.Threading.Tasks;

namespace RequisicoesApiBase
{
    public interface IServicoRequisicaoBaseToken
    {
        /// <summary>
        /// Gera um token via o flow ClientCredentials
        /// </summary>
        /// <param name="hostIdentityServer">Url do identity server</param>
        /// <param name="clientId">clientId para obter o token</param>
        /// <param name="clientSecret">secret do clientId</param>
        /// <param name="scope">Scope solicitados para obter o token</param>
        Task<string> GerarTokenClientCredentialsAsync(string hostIdentityServer, string clientId, string clientSecret = "", string scope = "");
        /// <summary>
        /// Gera um token via o flow Password
        /// </summary>
        /// <param name="hostIdentityServer">Url do identity server</param>
        /// <param name="clientId">clientId para obter o token</param>
        /// <param name="clientSecret">secret do clientId</param>
        /// <param name="scope">Scope solicitados para obter o token</param>
        Task<string> GerarTokenClientPasswordAsync(string hostIdentityServer, string clientId, string username, string password, string clientSecret = "", string scope = "");
    }
}
