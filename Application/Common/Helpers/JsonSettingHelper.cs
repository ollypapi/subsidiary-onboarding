﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Application.Common.Helpers
{
    public class JsonSettingHelper
    {
        public static JsonSerializerSettings GetJsonSerializer
        {
            get
            {
                var settings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
                settings.Converters.Add(new StringEnumConverter(true));

                return settings;
            }
        }
    }
}
