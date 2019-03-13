using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RequisicoesApiBase.Servico
{
    public class ServicoRequisicaoBaseHttpMetodos : IServicoRequisicaoBaseHttpMetodos
    {
        private HttpClient clienteHttp;
        private const string mediaType = "application/json"; 
        public string TokenBearer { get; set; }

        public ServicoRequisicaoBaseHttpMetodos()
        {
            clienteHttp = new HttpClient();
            clienteHttp.DefaultRequestHeaders.Accept.Clear();
            clienteHttp.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));
            clienteHttp.Timeout = TimeSpan.FromMinutes(1);
        }
        private void AdicionarTokenBearer()
        {
            if (!string.IsNullOrEmpty(TokenBearer))
                clienteHttp.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", TokenBearer);
        }

        /// <summary>
        /// Efetua uma requisição assincrona do metodo GET conforme a URI
        /// </summary>
        /// <param name="uri">Url para efetuar a request</param>
        public async Task<T> GetAsync<T>(string uri)
        {
            AdicionarTokenBearer();

            var resposta = await clienteHttp.GetAsync(uri);
            var content = await resposta.Content.ReadAsStringAsync();
            if (!resposta.IsSuccessStatusCode)
                resposta.EnsureSuccessStatusCode();

            var conteudo = await resposta.Content.ReadAsStringAsync();
            return await Task.Run(() => JsonConvert.DeserializeObject<T>(conteudo));
        }
        /// <summary>
        /// Efetua uma requisição sincrona do metodo GET conforme a URI
        /// </summary>
        /// <param name="uri">Url para efetuar a request</param>
        public T Get<T>(string url)
        {
            AdicionarTokenBearer();
            var uri = new Uri(url);

            HttpResponseMessage resposta = null;
            resposta = clienteHttp.GetAsync(uri).Result;
            var content = resposta.Content.ReadAsStringAsync().Result;

            if (!resposta.IsSuccessStatusCode)
                resposta.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<T>(content);
        }
        /// <summary>
        /// Efetua uma requisição assincrona do metodo POST conforme a URI e o corpo enviada
        /// </summary>
        /// <param name="uri">Url para efetuar a request</param>
        public async Task<T> PostAsync<T>(string uri, T corpo)
        {
            AdicionarTokenBearer();
            var parametro = new StringContent(JsonConvert.SerializeObject(corpo), Encoding.UTF8, mediaType);

            HttpResponseMessage resposta = clienteHttp.PostAsync(uri, parametro).Result;
            var content = await resposta.Content.ReadAsStringAsync();
            if (!resposta.IsSuccessStatusCode)
                resposta.EnsureSuccessStatusCode();

            return await Task.Run(() => JsonConvert.DeserializeObject<T>(content));
        }
        /// <summary>
        /// Efetua uma requisição sincrona do metodo POST conforme a URI e o corpo enviada
        /// </summary>
        /// <param name="uri">Url para efetuar a request</param>
        public T Post<T>(string uri, T corpo)
        {
            AdicionarTokenBearer();

            var json = JsonConvert.SerializeObject(corpo);
            var conteudoHttp = new StringContent(json, Encoding.UTF8, mediaType);

            HttpResponseMessage resposta = null;
            resposta = clienteHttp.PostAsync(uri, conteudoHttp).Result;
            var content = resposta.Content.ReadAsStringAsync().Result;

            if (!resposta.IsSuccessStatusCode)
                resposta.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<T>(content);
        }
        /// <summary>
        /// Efetua uma requisição assincrona do metodo PUT conforme a URI e o corpo enviada
        /// </summary>
        /// <param name="uri">Url para efetuar a request</param>
        public async Task<T> PutAsync<T>(string uri, T corpo)
        {
            AdicionarTokenBearer();
            var parametro = new StringContent(JsonConvert.SerializeObject(corpo), Encoding.UTF8, mediaType);

            HttpResponseMessage resposta = clienteHttp.PutAsync(uri, parametro).Result;
            var content = await resposta.Content.ReadAsStringAsync();
            if (!resposta.IsSuccessStatusCode)
                resposta.EnsureSuccessStatusCode();

            return await Task.Run(() => JsonConvert.DeserializeObject<T>(content));
        }
        /// <summary>
        /// Efetua uma requisição sincrona do metodo PUT conforme a URI e o corpo enviada
        /// </summary>
        /// <param name="uri">Url para efetuar a request</param>
        public T Put<T>(string uri, T corpo)
        {
            AdicionarTokenBearer();

            var json = JsonConvert.SerializeObject(corpo);
            var conteudoHttp = new StringContent(json, Encoding.UTF8, mediaType);

            HttpResponseMessage resposta = null;
            resposta = clienteHttp.PutAsync(uri, conteudoHttp).Result;
            var content = resposta.Content.ReadAsStringAsync().Result;

            if (!resposta.IsSuccessStatusCode)
                resposta.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<T>(content);
        }
        /// <summary>
        /// Efetua uma requisição assincrona do metodo DELETE conforme a URI
        /// </summary>
        /// <param name="uri">Url para efetuar a request</param>
        public async Task<T> DeleteAsync<T>(string uri)
        {
            AdicionarTokenBearer();

            HttpResponseMessage resposta = clienteHttp.DeleteAsync(uri).Result;
            var content = await resposta.Content.ReadAsStringAsync();
            if (!resposta.IsSuccessStatusCode)
                resposta.EnsureSuccessStatusCode();

            return await Task.Run(() => JsonConvert.DeserializeObject<T>(content));
        }
        /// <summary>
        /// Efetua uma requisição sincrona do metodo DELETE conforme a URI
        /// </summary>
        /// <param name="uri">Url para efetuar a request</param>
        public T Delete<T>(string uri)
        {
            AdicionarTokenBearer();

            HttpResponseMessage resposta = clienteHttp.DeleteAsync(uri).Result;
            var content = resposta.Content.ReadAsStringAsync().Result;
            if (!resposta.IsSuccessStatusCode)
                resposta.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}
