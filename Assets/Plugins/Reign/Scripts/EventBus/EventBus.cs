using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Reign.Events
{
    public interface IEvent { }

    public static class EventBus
    {
        private static readonly ConcurrentDictionary<Type, List<object>> _subscribers = new();

        /// <summary>
        /// Subscribe an async handler to an event
        /// </summary>
        public static void Subscribe<T>(Func<T, Task> handler) where T : IEvent
        {
            var type = typeof(T);

            _subscribers.AddOrUpdate(type, _ => new List<object> { handler },
                (_, list) =>
                {
                    lock (list)
                    {
                        list.Add(handler);
                        return list;
                    }
                });
        }

        /// <summary>
        /// Unsubscribe from async handler
        /// </summary>
        public static void Unsubscribe<T>(Func<T, Task> handler) where T : IEvent
        {
            var type = typeof(T);

            if (_subscribers.TryGetValue(type, out var list))
            {
                lock (list)
                {
                    list.Remove(handler);
                }
            }
        }

        /// <summary>
        /// Invoke an event of type T safely
        /// </summary>
        public static async Task Publish<T>(T _event, bool parallel = true) where T : IEvent
        {
            var type = typeof(T);

            if (!_subscribers.TryGetValue(type, out var list)) return;

            List<Func<T, Task>> handlers;

            // Avoid modification during iteration
            lock (list)
            {
                handlers = list.Cast<Func<T, Task>>().ToList();
            }

            if (parallel)
            {
                // Parallel execution

                var tasks = handlers.Select(handler => SafeInvoke(handler, _event));
                await Task.WhenAll(tasks);
            }
            else
            {
                // Sequential execution

                foreach (var handler in handlers)
                {
                    await SafeInvoke(handler, _event);
                }
            }
        }

        /// <summary>
        /// Wrapper to safely invoke an event
        /// </summary>
        private static async Task SafeInvoke<T>(Func<T, Task> handler, T _event)
        {
            try
            {
                await handler(_event);
            }
            catch (Exception ex)
            {
                // Replace with real logging in production
                Console.WriteLine($"Event handler error: {ex}");
            }
        }

        /// <summary>
        /// Clear all subscribers
        /// </summary>
        public static void ClearAllSubscribers()
        {
            _subscribers.Clear();
        }

        /// <summary>
        /// Clear subscribers of a specific event type
        /// </summary>
        public static void ClearSubscribersOfType<T>() where T : IEvent
        {
            _subscribers.TryRemove(typeof(T), out _);
        }
    }
}