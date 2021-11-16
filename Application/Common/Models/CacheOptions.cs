using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Models
{
    public class CacheOptions
    {
        public string Identifier { get; set; }
        public int? SlidingExpirySeconds { get; set; }
        public int? AbsoluteExpirySeconds { get; set; }
    }
}
