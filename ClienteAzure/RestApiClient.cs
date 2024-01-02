using System.Net.Http;
using System.Threading.Tasks;

namespace CteTarea8MVC.ClienteAzure
{



    public class RestApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUri;

        public RestApiClient(string baseUri)
        {
            _httpClient = new HttpClient();
            _baseUri = baseUri;
        }

        public async Task<string> PostAsync(string endpoint, string content)
        {
            string fullUri = $"{_baseUri}/{endpoint}";

            try
            {
                HttpResponseMessage response = await _httpClient.PostAsync(fullUri, new StringContent(content));

                response.EnsureSuccessStatusCode(); // Lanza una excepción en caso de error

                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException ex)
            {
                // Manejar la excepción, puedes personalizar esto según tus necesidades
                throw new Exception($"Error al realizar la solicitud POST: {ex.Message}");
            }
        }
    }

}
