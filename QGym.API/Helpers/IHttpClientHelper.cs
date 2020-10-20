using System.Collections.Generic;
using System.Threading.Tasks;

namespace QGym.API.Helpers
{
    public interface IHttpClientHelper
    {

        Task<T> GetAsync<T>(string resquest, Dictionary<string, string> headers = null);
        /// <summary>
        /// Llamada Generica de un Post
        /// </summary>
        /// <typeparam name="TT">Respuesta</typeparam>
        /// <typeparam name="T">Request</typeparam>
        /// <param name="request"></param>
        /// <param name="uri"></param>
        /// <returns></returns>

        Task<TT> PostAsync<TT, T>(T request, string uri, Dictionary<string, string> headers = null);
    }
}