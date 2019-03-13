using System.Threading.Tasks;

namespace RequisicoesApiBase
{
    public interface IServicoRequisicaoBaseHttpMetodos
    {
        /// <summary>
        /// Efetua uma requisição assincrona do metodo GET conforme a URI
        /// </summary>
        /// <param name="uri">Url para efetuar a request</param>
        Task<T> GetAsync<T>(string uri);
        /// <summary>
        /// Efetua uma requisição assincrona do metodo POST conforme a URI e o corpo enviada
        /// </summary>
        /// <param name="uri">Url para efetuar a request</param>
        Task<T> PostAsync<T>(string uri, T corpo);
        /// <summary>
        /// Efetua uma requisição assincrona do metodo PUT conforme a URI e o corpo enviada
        /// </summary>
        /// <param name="uri">Url para efetuar a request</param>
        Task<T> PutAsync<T>(string uri, T corpo);
        /// <summary>
        /// Efetua uma requisição assincrona do metodo DELETE conforme a URI
        /// </summary>
        /// <param name="uri">Url para efetuar a request</param>
        Task<T> DeleteAsync<T>(string uri);
        /// <summary>
        /// Efetua uma requisição sincrona do metodo GET conforme a URI
        /// </summary>
        /// <param name="uri">Url para efetuar a request</param>
        T Get<T>(string uri);
        /// <summary>
        /// Efetua uma requisição sincrona do metodo POST conforme a URI e o corpo enviada
        /// </summary>
        /// <param name="uri">Url para efetuar a request</param>
        T Post<T>(string uri, T corpo);
        /// <summary>
        /// Efetua uma requisição sincrona do metodo PUT conforme a URI e o corpo enviada
        /// </summary>
        /// <param name="uri">Url para efetuar a request</param>
        T Put<T>(string uri, T corpo);
        /// <summary>
        /// Efetua uma requisição sincrona do metodo DELETE conforme a URI
        /// </summary>
        /// <param name="uri">Url para efetuar a request</param>
        T Delete<T>(string uri);
        string TokenBearer { get; set; }
    }
}
