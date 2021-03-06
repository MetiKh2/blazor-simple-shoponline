using Newtonsoft.Json;
using ShopOnline.models.DTOs;
using ShopOnline.web.Services.Contracts;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;

namespace ShopOnline.web.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly HttpClient _httpClient;

        public event Action<int> OnShoppingCartChanged;
        public ShoppingCartService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<CartItemDto> AddItem(CartItemToAddDto cartItemToAddDto)
        {
            try
            {
                var response = await _httpClient.
                    PostAsJsonAsync<CartItemToAddDto>("api/ShoppingCart", cartItemToAddDto);
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == HttpStatusCode.NoContent)
                    {
                        return default(CartItemDto);
                    }
                    return await response.Content.ReadFromJsonAsync<CartItemDto>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception(message);
                }
            }
            catch (Exception e)
            {
                return null;
                throw ;
            }
        }

        public async Task<CartItemDto> DeleteItem(int id)
        {
            try
            {
                var response = await _httpClient.
                    DeleteAsync($"api/ShoppingCart/{id}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<CartItemDto>();
                }
                else
                {
                    return default(CartItemDto);
                }
            }
            catch (Exception e)
            {
                return null;
                throw;
            }
        }

        public async Task<List<CartItemDto>> GetItems(int userId)
        {
            try
            {
                var response = await _httpClient
                    .GetAsync($"api/ShoppingCart/{userId}/GetItems");
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<CartItemDto>().ToList();
                    }
                    return await response.Content.ReadFromJsonAsync<List<CartItemDto>>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception(message);
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public void RaiseEventOnShoppingCartChanged(int totalQuantity)
        {
            if (OnShoppingCartChanged != null)
            {
                OnShoppingCartChanged.Invoke(totalQuantity);
            }
        }

        public async Task<CartItemDto> UpdateQuantity(CartItemQuantityUpdateDto cartItemQuantityUpdateDto)
        {
            try
            {
                var jsonRequest =JsonConvert.SerializeObject(cartItemQuantityUpdateDto);
                var content=new StringContent(jsonRequest,Encoding.UTF8,"application/json-patch+json");
                var response = await _httpClient
                    .PatchAsync($"api/ShoppingCart/{cartItemQuantityUpdateDto.CartItemId}",content);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<CartItemDto>();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
