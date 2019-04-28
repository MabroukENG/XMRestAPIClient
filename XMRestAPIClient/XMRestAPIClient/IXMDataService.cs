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
    public interface IXMDataService<T, TIdentifier> where T : IXMModel<TIdentifier> where TIdentifier : struct
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
        Task<T> GetItemAsync(Func<T, bool> predicate);
        /// <summary>
        /// Gets all items asynchronous.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetAllItemsAsync(int page = 0, int count = 0);
        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        Task<IEnumerable<T>> GetItemsAsync(Func<T, bool> predicat);
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

        /// <summary>
        /// Counts items asynchronous.
        /// </summary>
        /// <returns></returns>
        Task<long> CountAsync();

        /// <summary>
        /// Counts items asynchronous.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        Task<long> CountAsync(Func<T, bool> predicate);

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
        /// <returns></returns>
        T GetItem(Func<T, bool> predicate);

        /// <summary>
        /// Gets all items.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        IEnumerable<T> GetAllItems(int page = 0, int count = 0);

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        IEnumerable<T> GetItems(Func<T, bool> predicate);


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

        /// <summary>
        /// Counts the items.
        /// </summary>
        /// <returns></returns>
        long Count();

        /// <summary>
        /// Counts the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        long Count(Func<T, bool> predicate);
    }
}

