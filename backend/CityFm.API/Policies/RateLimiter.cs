using System;
using System.Collections.Generic;

namespace CityFm.API.Policies
{
    public class RateLimiter : IRateLimiter
    {
        private readonly int _maxRequests;
        private readonly TimeSpan _window;
        private readonly Queue<DateTime> _timestamps = new();

        public RateLimiter(int maxRequests, TimeSpan window)
        {
            _maxRequests = maxRequests;
            _window = window;
        }

        public bool TryEnter()
        {
            var now = DateTime.UtcNow;

            while (_timestamps.Count > 0 && now - _timestamps.Peek() > _window)
                _timestamps.Dequeue();

            if (_timestamps.Count >= _maxRequests)
                return false;

            _timestamps.Enqueue(now);
            return true;
        }

        // Simple wrapper for now, same logic as TryEnter
        public bool AllowRequest(string key)
        {
            return TryEnter();
        }
    }
}