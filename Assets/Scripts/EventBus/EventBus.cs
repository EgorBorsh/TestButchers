using System;
using System.Collections.Generic;

public class EventBus<TEvent> : IEventBus<TEvent>
{
    private readonly Dictionary<string, List<Action<TEvent>>> _handlers = new();
    private readonly Dictionary<string, List<Func<TEvent, object>>> _resultHandlers = new();
    private bool _disposed;
    public IDisposable Subscribe(string eventName, Action<TEvent> handler)
    {
        if (!_handlers.ContainsKey(eventName))
            _handlers[eventName] = new List<Action<TEvent>>();

        _handlers[eventName].Add(handler);

        return new Subscription(() => Unsubscribe(eventName, handler));
    }

    public IDisposable Subscribe<TResult>(string eventName, Func<TEvent, TResult> handler)
    {
        var key = GetResultKey(eventName, typeof(TResult));

        if (!_resultHandlers.ContainsKey(key))
            _resultHandlers[key] = new List<Func<TEvent, object>>();

        _resultHandlers[key].Add(ev => handler(ev));

        return new Subscription(() => UnsubscribeResult(key, handler));
    }

    public void Publish(string eventName, TEvent eventData)
    {
        if (_handlers.TryGetValue(eventName, out var handlers))
        {
            var handlersCopy = new List<Action<TEvent>>(handlers);
            foreach (var handler in handlersCopy)
                handler?.Invoke(eventData);
        }
    }

    public void Publish<TResult>(string eventName, TEvent eventData, Action<TResult> callback)
    {
        var key = GetResultKey(eventName, typeof(TResult));

        if (_resultHandlers.TryGetValue(key, out var handlers) && handlers.Count > 0)
        {
            var lastHandler = handlers[^1];
            var result = (TResult)lastHandler(eventData);
            callback?.Invoke(result);
        }
        else
        {
            callback?.Invoke(default);
        }
    }

    public void PublishAll<TResult>(string eventName, TEvent eventData, Action<TResult[]> callback)
    {
        var key = GetResultKey(eventName, typeof(TResult));

        if (_resultHandlers.TryGetValue(key, out var handlers) && handlers.Count > 0)
        {
            var results = new List<TResult>();
            var handlersCopy = new List<Func<TEvent, object>>(handlers);

            foreach (var handler in handlersCopy)
                results.Add((TResult)handler(eventData));

            callback?.Invoke(results.ToArray());
        }
        else
        {
            callback?.Invoke(Array.Empty<TResult>());
        }
    }

    public void Clear()
    {
        _handlers.Clear();
        _resultHandlers.Clear();
    }

    private void Unsubscribe(string eventName, Action<TEvent> handler)
    {
        if (_disposed) return;

        if (_handlers.TryGetValue(eventName, out var handlers))
        {
            handlers.Remove(handler);
            if (handlers.Count == 0)
                _handlers.Remove(eventName);
        }
    }

    private void UnsubscribeResult<TResult>(string key, Func<TEvent, TResult> handler)
    {
        if (_disposed) return;

        if (_resultHandlers.TryGetValue(key, out var handlers))
        {
            handlers.RemoveAll(h => h.Method == handler.Method);
            if (handlers.Count == 0)
                _resultHandlers.Remove(key);
        }
    }

    private string GetResultKey(string eventName, Type resultType)
    {
        return $"{eventName}_{resultType.FullName}";
    }

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        Clear();
    }

    private class Subscription : IDisposable
    {
        private readonly Action _unsubscribe;
        private bool _disposed;

        public Subscription(Action unsubscribe)
        {
            _unsubscribe = unsubscribe;
        }

        public void Dispose()
        {
            if (_disposed) return;
            _disposed = true;
            try
            {
                _unsubscribe?.Invoke();
            }
            catch (ObjectDisposedException)
            {
                // Игнорируем, объект уже уничтожен
            }
        }
    }
}
