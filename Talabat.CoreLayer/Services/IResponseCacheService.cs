﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.CoreLayer.Services
{
    public interface IResponseCacheService
    {
        Task CacheResponseAsync(string key, object Response, TimeSpan timeToLive);

        Task<string?> GetCachedResponseAsync(string key);
    }
}
