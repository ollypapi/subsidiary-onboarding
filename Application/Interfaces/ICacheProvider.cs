using Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICacheProvider
    {
        Task<T> Get<T>(string key);
        Task Set<T>(string Key, T entity, CacheOptions options);
        Task Remove(string Key);
    }
}
