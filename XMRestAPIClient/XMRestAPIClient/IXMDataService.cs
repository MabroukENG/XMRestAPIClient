using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace XMRestAPIClient
{
    /// <summary>
    /// Defines an XMRestService.
    /// </summary>
    /// <typeparam name="T">The model</typeparam>
    public interface IXMDataService<T,TIdentifier> where T : IXMModel<TIdentifier> where TIdentifier :struct
    {
        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<T> GetItemAsync(TIdentifier id);
        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        Task<T> GetItemAsync(Func<T, bool> predicate, int page = -1);
        /// <summary>
        /// Gets all items.
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<T>> GetAllItemsAsync(int page = -1);
        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetItemsAsync(Func<T, bool> predicate, int page = -1);
        /// <summary>
        /// Saves the item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        Task<bool> SaveItemAsync(T item);
        /// <summary>
        /// Deletes the item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        Task<bool> DeleteItemAsync(T item);
        /// <summary>
        /// Deletes the item.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<bool> DeleteItemAsync(TIdentifier id);
        /// <summary>
        /// Deletes the item.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        Task<bool> DeleteItemAsync(Func<T, bool> predicate);


        // ASYNC METHODS 

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        T GetItem(TIdentifier id);

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="page">The page.</param>
        /// <returns></returns>
        T GetItem(Func<T, bool> predicate, int page = -1);

        /// <summary>
        /// Gets all items.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <returns></returns>
        IEnumerable<T> GetAllItems(int page = -1);

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="page">The page.</param>
        /// <returns></returns>
        IEnumerable<T> GetItems(Func<T, bool> predicate, int page = -1);


        /// <summary>
        /// Saves the item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        bool SaveItem(T item);

        /// <summary>
        /// Deletes the item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        bool DeleteItem(T item);

        /// <summary>
        /// Deletes the item.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        bool DeleteItem(TIdentifier id);

        /// <summary>
        /// Deletes the item.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        bool DeleteItem(Func<T, bool> predicate);
    }
}

