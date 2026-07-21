using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Reign.Events
{
    /// <summary>
    /// Define query and expected return type
    /// </summary>
    public interface IQuery<TResponse> { }

    public static class QueryBus
    {
        // Thread safe
        private static readonly ConcurrentDictionary<Type, object> _handlers = new();

        /// <summary>
        /// Register handler for a specific query type and prevents type mismatch
        /// </summary>
        public static void Register<TQuery, TResponse>(Func<TQuery, Task<TResponse>> handler) where TQuery : IQuery<TResponse>
        {
            // Get type of query and store in handler dictionary
            // If a handler already exists: it gets overwritten without notice
            _handlers[typeof(TQuery)] = handler;
        }

        /// <summary>
        /// Execute a query
        /// </summary>
        public static async Task<TResponse> Query<TQuery, TResponse>(TQuery query) where TQuery : IQuery<TResponse>
        {
            // Look for type of query in handlers
            if (_handlers.TryGetValue(typeof(TQuery), out var handler))
            {
                // Execute casted handler 
                return await ((Func<TQuery, Task<TResponse>>)handler)(query);
            }

            throw new InvalidOperationException
            (
                // No handler found or registered (failsafe)
                $"No handler registered for query {typeof(TQuery).Name}"
            );
        }
    }
}