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
    public class XMBaseDataService<T> : IXMDataService<T> where T : IXMModel
    {
        protected virtual string ApiName { get; }

        /// <summary>
        /// Gets the API URL.
        /// </summary>
        /// <param name="httpMethod">The HTTP method.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="apiParams">The API parameters.</param>
        /// <returns></returns>
        public virtual Uri GetApiUrl(HttpMethod httpMethod, string id = "", params Tuple<string, string>[] apiParams)
        {
            var _version = XMRestSettings.ApiVersion == -1 ? "" : $"/v{XMRestSettings.ApiVersion}";
            var _id = string.IsNullOrEmpty(id) ? "/" : $"/{id}";
            var _uri = new Uri(new Uri($"{XMRestSettings.BaseUrl}{(XMRestSettings.BaseUrl.EndsWith("/") ? "" : "/")}api{_version}/{ApiName}{_id}").ToString());
            return _uri;
        }

        /// <summary>
        /// Deletes the item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual async Task<bool> DeleteItem(T item)
        {
            try
            {
                return await DeleteItem(item.Id);
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
        public virtual async Task<bool> DeleteItem(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return false;
                var _url = GetApiUrl(HttpMethod.Delete, id);
                //new Uri(new Uri($"{XMRestSettings.BaseUrl}{(XMRestSettings.BaseUrl.EndsWith("/") ? "" : "/")}api/v{XMRestSettings.ApiVersion}/{ApiName}/{id}").ToString());
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
        public virtual async Task<bool> DeleteItem(Func<T, bool> predicate)
        {
            try
            {
                var item = await GetItem(predicate);
                return await DeleteItem(item.Id);
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
        public virtual async Task<IEnumerable<T>> GetAllItems(int page = -1)
        {
            try
            {
                var _url = GetApiUrl(HttpMethod.Get);
                var getAllResult = await GetFromDataServer(string.Empty, _url.ToString());
                return JsonConvert.DeserializeObject<List<T>>(getAllResult?.JsonData);
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
        /// <exception cref="NotImplementedException"></exception>
        public virtual async Task<T> GetItem(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return default(T);
                var _url = GetApiUrl(HttpMethod.Get, id);
                var getResult = await GetFromDataServer(string.Empty, _url.ToString());
                return JsonConvert.DeserializeObject<T>(getResult?.JsonData);
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
        public virtual async Task<T> GetItem(Func<T, bool> predicate, int page = -1)
        {
            try
            {
                var data = await GetAllItems(page);
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
        public virtual async Task<IEnumerable<T>> GetItems(Func<T, bool> predicate, int page = -1)
        {
            try
            {
                var data = await GetAllItems(page);
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
        public virtual async Task<bool> SaveItem(T item)
        {
            try
            {
                if (item == null)
                    return false;

                if (string.IsNullOrEmpty(item.Id) == true)
                    item.Id = Guid.NewGuid().ToString();

                var jsonRequest = JsonConvert.SerializeObject(item);
                if (string.IsNullOrEmpty((await GetItem(item.Id))?.Id))
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
        /// <param name="ConnectionUrl">The connection URL.</param>
        /// <param name="headers">The headers.</param>
        /// <returns>Instance of the <see cref="XMRestResult"/> class.</returns>
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

        private async Task<XMRestResult> RestRequest(string jsonContent, string url, Dictionary<string, string> headers, int timeout, HttpMethod method)
        {
            try
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

                    if (string.IsNullOrEmpty(XMRestSettings.AuthorizationKeyHeaderValue) == false)
                    {
                        request.Headers.Add(XMRestSettings.AuthorizationKeyHeaderName, XMRestSettings.AuthorizationKeyHeaderValue);
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
            catch (Exception ex)
            {
                return new XMRestResult { Message = ex.Message, Type = XMRestResultType.Error };
            }
        }

        #region ASYNCHRONOUS
        /// <summary>
        /// Gets the item asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public T GetItemAsync(string id)
        {
            return Task.Run(async () =>
             {
                 return await GetItem(id);
             }).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Gets the item asynchronous.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="page">The page.</param>
        /// <returns></returns>
        public T GetItemAsync(Func<T, bool> predicate, int page = -1)
        {
            return Task.Run(async () =>
            {
                return await GetItem(predicate, page);
            }).GetAwaiter().GetResult();
        }


        /// <summary>
        /// Gets all items asynchronous.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <returns></returns>
        public IEnumerable<T> GetAllItemsAsync(int page = -1)
        {
            return Task.Run(async () =>
            {
                return await GetAllItems(page);
            }).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Gets the items asynchronous.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="page">The page.</param>
        /// <returns></returns>
        public IEnumerable<T> GetItemsAsync(Func<T, bool> predicate, int page = -1)
        {
            return Task.Run(async () =>
            {
                return await GetItems(predicate, page);
            }).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Updates or saves new item asynchronous.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public bool SaveItemAsync(T item)
        {
            return Task.Run(async () =>
            {
                return await SaveItem(item);
            }).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Deletes the item asynchronous.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public bool DeleteItemAsync(T item)
        {
            return Task.Run(async () =>
            {
                return await DeleteItem(item);
            }).GetAwaiter().GetResult();
        }
        /// <summary>
        /// Deletes the item asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public bool DeleteItemAsync(string id)
        {
            return Task.Run(async () =>
            {
                return await DeleteItem(id);
            }).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Deletes the item asynchronous.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public bool DeleteItemAsync(Func<T, bool> predicate)
        {
            return Task.Run(async () =>
            {
                return await DeleteItem(predicate);
            }).GetAwaiter().GetResult();
        }
        #endregion
    }
}
