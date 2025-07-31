    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    namespace CityFm.API.Policies
    {
        public interface IRateLimiter
        {
            bool TryEnter();
            bool AllowRequest(string key);
        }
    }