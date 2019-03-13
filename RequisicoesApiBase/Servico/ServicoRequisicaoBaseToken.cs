using IdentityModel.Client;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace RequisicoesApiBase.Servico
{
    public class ServicoRequisicaoBaseToken : IServicoRequisicaoBaseToken
    {
        private HttpClient client;
        private DiscoveryResponse discoveryResponse;
        public ServicoRequisicaoBaseToken()
        {
            client = new HttpClient();
        }

        private async Task<DiscoveryResponse> ObterDiscoveryClient(string hostIdentityServer)
        {
            discoveryResponse = await client.GetDiscoveryDocumentAsync(hostIdentityServer);
            if (discoveryResponse.IsError) throw new Exception(discoveryResponse.Error);

            return discoveryResponse;
        }

        /// <summary>
        /// Gera um token via o flow ClientCredentials
        /// </summary>
        /// <param name="hostIdentityServer">Url do identity server</param>
        /// <param name="clientId">clientId para obter o token</param>
        /// <param name="clientSecret">secret do clientId</param>
        /// <param name="scope">Scope solicitados para obter o token</param>
        public async Task<string> GerarTokenClientCredentialsAsync(string hostIdentityServer, string clientId, string clientSecret = "", string scope = "")
        {
            try
            {
                await ObterDiscoveryClient(hostIdentityServer);

                var response = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
                {
                    Address = discoveryResponse.TokenEndpoint,

                    ClientId = clientId,
                    ClientSecret = clientSecret,
                    Scope = scope
                });

                if (response.IsError) throw new Exception(response.Error);

                return response.AccessToken;
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao obter token.\nDetalhe: " + ex.GetBaseException().Message);
            }
        }
        
        /// <summary>
        /// Gera um token via o flow Password
        /// </summary>
        /// <param name="hostIdentityServer">Url do identity server</param>
        /// <param name="clientId">clientId para obter o token</param>
        /// <param name="clientSecret">secret do clientId</param>
        /// <param name="scope">Scope solicitados para obter o token</param>
        public async Task<string> GerarTokenClientPasswordAsync(string hostIdentityServer, string clientId, string username, string password, string clientSecret = "", string scope = "")
        {
            try
            {
                await ObterDiscoveryClient(hostIdentityServer);

                var response = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
                {
                    Address = discoveryResponse.TokenEndpoint,

                    ClientId = clientId,
                    ClientSecret = clientSecret,
                    Scope = scope,
                    UserName = username,
                    Password = password
                });

                if (response.IsError) throw new Exception(response.Error);

                return response.AccessToken;
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao obter token.\nDetalhe: " + ex.GetBaseException().Message);
            }
        }
    }
}
