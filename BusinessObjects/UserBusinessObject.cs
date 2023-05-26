using AlticeApi.Data;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AlticeApi.BusinessObjects
{
    public class UserBusinessObject : IUserBusinessObject
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;

        public UserBusinessObject(HttpClient httpClient, IMemoryCache cache)
        {
            _httpClient = httpClient;
            _cache = cache;
        }

        public async Task<string> GetUsers()
        {
            //try to get the cached data
            if (_cache.TryGetValue<string>("UserData", out var cachedData))
            {
                return cachedData;
            }

            try
            {
                var usersResponse = await _httpClient.GetAsync("https://jsonplaceholder.typicode.com/users");
                usersResponse.EnsureSuccessStatusCode();
                var postsResponse = await _httpClient.GetAsync("https://jsonplaceholder.typicode.com/posts");
                postsResponse.EnsureSuccessStatusCode();

                var users = await usersResponse.Content.ReadAsAsync<List<User>>();
                var posts = await postsResponse.Content.ReadAsAsync<List<Post>>();

                var result = new List<UserWithPosts>();
                //create the response model
                foreach (var user in users)
                {
                    //match posts with users ids
                    var userPosts = posts.FindAll(p => p.UserId == user.Id);
                    result.Add(new UserWithPosts
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Username = user.Username,
                        Email = user.Email,
                        UserAddress = user.Address.Street + ", " + user.Address.Suite + "- " + user.Address.Zipcode + " " + user.Address.City,
                        Phone = user.Phone,
                        Website = user.Website,
                        Company = user.Company.Name,
                        Posts = userPosts
                    });
                }
                var jsonResponse = JsonConvert.SerializeObject(result, Formatting.Indented);

                //cache data for 1 min
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(1));

                _cache.Set("UserData", jsonResponse, cacheEntryOptions);

                return jsonResponse;
            }
            catch (Exception ex)
            {
                //return the error messae
                return JsonConvert.SerializeObject(new { error = ex.Message });
            }
        }
    }
}