using Application.Common.Enums;
using Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Interfaces
{
    public interface ICacheOptionsProvider
    {
        CacheOptions GetOptions(CacheEnum key);
    }
}
