using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace XMRestAPIClient
{
    /// <summary>
    /// The XMBaseService
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="XMRestAPIClient.IXMDataService{T}" />
    public class XMBaseDataService<T,TIdentifier> : IXMDataService<T,TIdentifier> where T : IXMModel<TIdentifier> where TIdentifier:struct
    {
        /// <summary>
        /// Gets the name of the API.
        /// </summary>
        /// <value>
        /// The name of the API.
        /// </value>
        public virtual string ApiName { get; }

        /// <summary>
        /// Gets the name of the authorization header. 'Authorization' by default.
        /// </summary>
        /// <value>
        /// The name of the authorization header.
        /// </value>
        public virtual string AuthorizationHeaderName { get; } = "Authorization";

        /// <summary>
        /// Gets or sets the authorization token.
        /// </summary>
        /// <value>
        /// The authorization token.
        /// </value>
        public virtual string AuthorizationToken { get; } = string.Empty;

        /// <summary>
        /// Gets or sets the base API URL.
        /// </summary>
        /// <value>
        /// The base API URL.
        /// </value>
        public virtual string BaseAPIUrl { get; } = "http://localhost:8080/";

        /// <summary>
        /// The API version
        /// </summary>
        public virtual int ApiVersion { get; } = 1;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:XMRestAPIClient.XMBaseDataService`2"/> class.
        /// </summary>
        public XMBaseDataService()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:XMRestAPIClient.XMBaseDataService`2"/> class.
        /// </summary>
        /// <param name="baseUrl">Base URL.</param>
        /// <param name="apiVersion">API version.</param>
        /// <param name="authorizationToken">Authorization token.</param>
        public XMBaseDataService(string baseUrl, int apiVersion, string authorizationToken) : this()
        {
            BaseAPIUrl = baseUrl;
            ApiVersion = apiVersion;
            AuthorizationToken = authorizationToken;
        }




        /// <summary>
        /// Gets the API URL.
        /// </summary>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="apiParams">The API parameters.</param>
        /// <returns></returns>
        public virtual Uri GetApiUrl(HttpMethod httpMethod, TIdentifier id = default(TIdentifier), params Tuple<string, string>[] apiParams)
        {
            var _version = ApiVersion == -1 ? "" : $"/v{ApiVersion}";
            var _id = string.IsNullOrEmpty($"{id}") ? "/" : $"/{id}";
            var _uri = new Uri(new Uri($"{BaseAPIUrl}{(BaseAPIUrl.EndsWith("/",StringComparison.OrdinalIgnoreCase) ? "" : "/")}api{_version}/{ApiName}{_id}").ToString());
            return _uri;
        }

        /// <summary>
        /// Deletes the item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual async Task<bool> DeleteItemAsync(T item)
        {
            try
            {
                return await DeleteItemAsync(item.Id);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Deletes the item.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual async Task<bool> DeleteItemAsync(TIdentifier id)
        {
            try
            {
                if (string.IsNullOrEmpty($"{id}"))
                    return false;
                var _url = GetApiUrl(HttpMethod.Delete, id);
                //new Uri(new Uri($"{BaseUrl}{(BaseUrl.EndsWith("/") ? "" : "/")}api/v{ApiVersion}/{ApiName}/{id}").ToString());
                var deleteResult = await DeleteFromDataServer(string.Empty, _url.ToString());
                return deleteResult.Type == XMRestResultType.Success;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Deletes the item.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual async Task<bool> DeleteItemAsync(Func<T, bool> predicate)
        {
            try
            {
                var item = await GetItemAsync(predicate);
                return await DeleteItemAsync(item.Id);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Gets items per page. Gets all items if page=-1 (default).
        /// </summary>
        /// <param name="page">The page. </param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<T>> GetAllItemsAsync(int page = -1)
        {
            try
            {
                var _url = GetApiUrl(HttpMethod.Get);
                var getAllResult = await GetFromDataServer(string.Empty, _url.ToString());
                return DeserializeData<List<T>>(getAllResult?.JsonData);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public virtual async Task<T> GetItemAsync(TIdentifier id)
        {
            try
            {
                if (string.IsNullOrEmpty($"{id}"))
                    return default(T);
                var _url = GetApiUrl(HttpMethod.Get, id);
                var getResult = await GetFromDataServer(string.Empty, _url.ToString());
                return DeserializeData<T>(getResult?.JsonData);
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual async Task<T> GetItemAsync(Func<T, bool> predicate, int page = -1)
        {
            try
            {
                var data = await GetAllItemsAsync(page);
                if (data == null || data.Any() == false)
                    return default(T);

                return data.FirstOrDefault(predicate);
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual async Task<IEnumerable<T>> GetItemsAsync(Func<T, bool> predicate, int page = -1)
        {
            try
            {
                var data = await GetAllItemsAsync(page);
                if (data == null || data.Any() == false)
                    return null;

                return data.Where(predicate);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Updates (or saves a new) item. If the Id was not set, a new GUID will be generated.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual async Task<bool> SaveItemAsync(T item)
        {
            try
            {
                if (item == null)
                    return false;

                var jsonRequest = SerializeData(item);
                if (string.IsNullOrEmpty($"{(await GetItemAsync(item.Id))?.Id}"))
                {
                    var _postUrl = GetApiUrl(HttpMethod.Post);
                    var postResult = await PostToDataServer(jsonRequest, _postUrl.ToString());
                    return postResult.Type == XMRestResultType.Success;
                }
                var _putUrl = GetApiUrl(HttpMethod.Put);
                var putResult = await PutToDataServer(jsonRequest, _putUrl.ToString());
                return putResult.Type == XMRestResultType.Success;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        /// <summary>
        /// Posts to data server.
        /// </summary>
        /// <param name="jsonContent">Content of the json.</param>
        /// <param name="url">The URL.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns>Instance of the <see cref="XMRestResult"/> class.</returns>
        protected virtual async Task<XMRestResult> PostToDataServer(string jsonContent, string url, Dictionary<string, string> headers = null, int timeout = 20)
        {
            try
            {
                return await RestRequest(jsonContent, url, headers, timeout, HttpMethod.Post);
            }
            catch (Exception ex)
            {
                return new XMRestResult { Message = ex.Message, Type = XMRestResultType.Error };
            }
        }

        /// <summary>
        /// Puts to data server.
        /// </summary>
        /// <param name="jsonContent">Content of the json.</param>
        /// <param name="url">The URL.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns></returns>
        protected virtual async Task<XMRestResult> PutToDataServer(string jsonContent, string url, Dictionary<string, string> headers = null, int timeout = 20)
        {
            try
            {
                return await RestRequest(jsonContent, url, headers, timeout, HttpMethod.Put);
            }
            catch (Exception ex)
            {
                return new XMRestResult { Message = ex.Message, Type = XMRestResultType.Error };
            }
        }

        /// <summary>
        /// Gets from data server.
        /// </summary>
        /// <param name="jsonContent">Content of the json.</param>
        /// <param name="url">The URL.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns></returns>
        protected virtual async Task<XMRestResult> GetFromDataServer(string jsonContent, string url, Dictionary<string, string> headers = null, int timeout = 20)
        {
            try
            {
                return await RestRequest(jsonContent, url, headers, timeout, HttpMethod.Get);
            }
            catch (Exception ex)
            {
                return new XMRestResult { Message = ex.Message, Type = XMRestResultType.Error };
            }
        }

        /// <summary>
        /// Deletes from data server.
        /// </summary>
        /// <param name="jsonContent">Content of the json.</param>
        /// <param name="url">The URL.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="timeout">The timeout.</param>
        /// <returns></returns>
        protected virtual async Task<XMRestResult> DeleteFromDataServer(string jsonContent, string url, Dictionary<string, string> headers = null, int timeout = 20)
        {
            try
            {
                return await RestRequest(jsonContent, url, headers, timeout, HttpMethod.Delete);
            }
            catch (Exception ex)
            {
                return new XMRestResult { Message = ex.Message, Type = XMRestResultType.Error };
            }
        }

        /// <summary>
        /// Rests the request.
        /// </summary>
        /// <param name="jsonContent">Content of the json.</param>
        /// <param name="url">The URL.</param>
        /// <param name="headers">The headers.</param>
        /// <param name="timeout">The timeout.</param>
        /// <param name="method">The method.</param>
        /// <returns></returns>
        private async Task<XMRestResult> RestRequest(string jsonContent, string url, Dictionary<string, string> headers, int timeout, HttpMethod method)
        {
            using (HttpClient client = new HttpClient())
            {
                client.Timeout = new TimeSpan(0, 0, 0, timeout);
                var request = new HttpRequestMessage()
                {
                    RequestUri = new Uri(url),
                    Method = method
                };

                if (string.IsNullOrEmpty(jsonContent) == false)
                {
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                    request.Content = content;
                }

                if (UsesAuthorizationToken(method) == true)
                {
                    request.Headers.Add(AuthorizationHeaderName, AuthorizationToken);
                }

                if (headers != null && headers.Any())
                {
                    headers.ToList().ForEach(h => request.Headers.Add(h.Key, h.Value));
                }

                var response = await client.SendAsync(request);
                var dataResult = response.Content.ReadAsStringAsync().Result;
                //var result = JsonConvert.DeserializeObject<ConnectionResult<T>>(dataResult);
                return new XMRestResult()
                {
                    Type = response.IsSuccessStatusCode ? XMRestResultType.Success : XMRestResultType.Error,
                    Message = string.Empty,
                    JsonData = dataResult
                };
            }
        }

        /// <summary>
        /// Determines whether the specified HTTP method uses an authorization token.
        /// </summary>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <returns>
        ///   <c>true</c> if the specified HTTP method [uses authorization token] ; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool UsesAuthorizationToken(HttpMethod httpMethod)
        {
            return false;
        }

        /// <summary>
        /// Deserializes the data. It makes a json conversion by default.
        /// </summary>
        /// <typeparam name="TReturn">The type of the return.</typeparam>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public virtual TReturn DeserializeData<TReturn>(string data)
        {
            return JsonConvert.DeserializeObject<TReturn>(data);
        }

        /// <summary>
        /// Serializes the data. It makes a json conversion by default.
        /// </summary>
        /// <param name="dataObj">The data object.</param>
        /// <returns></returns>
        public virtual string SerializeData(object dataObj)
        {
            return JsonConvert.SerializeObject(dataObj);
        }

        /// <summary>
        /// Tests the service.
        /// </summary>
        /// <returns></returns>
        public XMRestResult TestService()
        {
            return Task.Run(async () =>
            {
                var _url = GetApiUrl(HttpMethod.Get);
                return await RestRequest(null, _url.ToString(), null, 20, HttpMethod.Get);
            }).GetAwaiter().GetResult();
        }

        #region SYNCHRONOUS



        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public T GetItem(TIdentifier id)
        {
            return Task.Run(async () =>
             {
                 return await GetItemAsync(id);
             }).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="page">The page.</param>
        /// <returns></returns>
        public T GetItem(Func<T, bool> predicate, int page = -1)
        {
            return Task.Run(async () =>
            {
                return await GetItemAsync(predicate, page);
            }).GetAwaiter().GetResult();
        }


        /// <summary>
        /// Gets all items.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <returns></returns>
        public IEnumerable<T> GetAllItems(int page = -1)
        {
            return Task.Run(async () =>
            {
                return await GetAllItemsAsync(page);
            }).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="page">The page.</param>
        /// <returns></returns>
        public IEnumerable<T> GetItems(Func<T, bool> predicate, int page = -1)
        {
            return Task.Run(async () =>
            {
                return await GetItemsAsync(predicate, page);
            }).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Updates or saves new item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public bool SaveItem(T item)
        {
            return Task.Run(async () =>
            {
                return await SaveItemAsync(item);
            }).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Deletes the item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public bool DeleteItem(T item)
        {
            return Task.Run(async () =>
            {
                return await DeleteItemAsync(item);
            }).GetAwaiter().GetResult();
        }
        /// <summary>
        /// Deletes the item.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public bool DeleteItem(TIdentifier id)
        {
            return Task.Run(async () =>
            {
                return await DeleteItemAsync(id);
            }).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Deletes the item.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public bool DeleteItem(Func<T, bool> predicate)
        {
            return Task.Run(async () =>
            {
                return await DeleteItemAsync(predicate);
            }).GetAwaiter().GetResult();
        }
        #endregion
    }
}
