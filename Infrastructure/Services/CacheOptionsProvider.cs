using Application.Common.Enums;
using Application.Common.Models;
using Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Services
{
    public class CacheOptionsProvider : ICacheOptionsProvider
    {
        readonly IDictionary<CacheEnum, CacheOptions> _cacheOptions = new Dictionary<CacheEnum, CacheOptions>();


        public CacheOptionsProvider()
        {
            var slidingExpiration = 864000;
            _cacheOptions.Add(CacheEnum.Nationalities, new CacheOptions()
            {
                Identifier = "nationality",
                AbsoluteExpirySeconds = slidingExpiration,
                SlidingExpirySeconds = slidingExpiration
            });


            _cacheOptions.Add(CacheEnum.Categories, new CacheOptions()
            {
                Identifier = "biller-categories",
                AbsoluteExpirySeconds = slidingExpiration * 30,
                SlidingExpirySeconds = slidingExpiration
            });
            _cacheOptions.Add(CacheEnum.Cities, new CacheOptions()
            {
                Identifier = "cities",
                AbsoluteExpirySeconds = slidingExpiration * 30,
                SlidingExpirySeconds = slidingExpiration
            });
            _cacheOptions.Add(CacheEnum.States, new CacheOptions()
            {
                Identifier = "states",
                AbsoluteExpirySeconds = slidingExpiration,
                SlidingExpirySeconds = slidingExpiration
            });

            _cacheOptions.Add(CacheEnum.Salutations, new CacheOptions()
            {
                Identifier = "salutations",
                AbsoluteExpirySeconds = slidingExpiration,
                SlidingExpirySeconds = slidingExpiration
            });
            _cacheOptions.Add(CacheEnum.Branches, new CacheOptions()
            {
                Identifier = "branchInfo",
                AbsoluteExpirySeconds = slidingExpiration,
                SlidingExpirySeconds = slidingExpiration
            });
            _cacheOptions.Add(CacheEnum.MeansOfId, new CacheOptions()
            {
                Identifier = "meanOfId",
                AbsoluteExpirySeconds = slidingExpiration,
                SlidingExpirySeconds = slidingExpiration
            });

            _cacheOptions.Add(CacheEnum.MaritalStatus, new CacheOptions()
            {
                Identifier = "marital",
                AbsoluteExpirySeconds = slidingExpiration,
                SlidingExpirySeconds = slidingExpiration
            });



        }
        public CacheOptions GetOptions(CacheEnum key)
        {
            if (!_cacheOptions.ContainsKey(key))
                throw new Exception("Cache item not found");

            return _cacheOptions[key];
        }


    }
}
