using AppNegocio.Class;
using AppNegocio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AppNegocio.Services
{
    public class GenericService<T> : IGenericService<T> where T : class
    {

        protected readonly HttpClient client;
        protected readonly JsonSerializerOptions options;
        protected readonly string _endpoint;

        public GenericService(HttpClient client)
        {
            this.client = client;
            this.options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            this._endpoint = ApiEndPoints.GetEndPoint(typeof(T).Name);
        }

        public GenericService()
        {
            this.client = new HttpClient();
            this.options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            string urlApi;
            if (Properties.Resources.Remoto == "false")
            {
                urlApi = Properties.Resources.urlApiLocal;
            }
            else
            {
                urlApi = Properties.Resources.urlApiRemoto;
            }

            this._endpoint = urlApi + ApiEndPoints.GetEndPoint(typeof(T).Name);
        }



        public async Task<List<T>?> GetAllAsync()
        {
            var response = await client.GetAsync(_endpoint);
            var content = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content?.ToString());
            }
            return JsonSerializer.Deserialize<List<T>>(content, options); ;
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            var response = await client.GetAsync($"{_endpoint}/{id}");
            var content = await response.Content.ReadAsStringAsync(); // Cambia a string para manejar mejor el error
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException($"Error: {response.StatusCode}, Detalle: {content}");
            }
            return JsonSerializer.Deserialize<T>(content, options);
        }

        public async Task<T?> AddAsync(T? entity)
        {
            var response = await client.PostAsJsonAsync(_endpoint, entity);
            var content = await response.Content.ReadAsStreamAsync();
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(content?.ToString());
            }
            return JsonSerializer.Deserialize<T>(content, options);
        }

        public async Task UpdateAsync(T? entity)
        {
            var idValue = entity.GetType().GetProperty("id").GetValue(entity);

            var response = await client.PutAsJsonAsync($"{_endpoint}/{idValue}", entity);
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(response?.ToString());
            }
        }

        public async Task DeleteAsync(int id)
        {
            var response = await client.DeleteAsync($"{_endpoint}/{id}");
            if (!response.IsSuccessStatusCode)
            {
                throw new ApplicationException(response.ToString());

            }
        }


    }
}

