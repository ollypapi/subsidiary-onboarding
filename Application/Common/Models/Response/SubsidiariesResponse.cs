using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Models.Response
{
    public class SubsidiariesResponse: ResponseBase
    {
        public List<Subsidiary> Subsidiaries { get; set; }

    }

    public class Subsidiary
    {
        public string CountryId { get; set; }
        public string Name { get; set; }
        public string CurrencyCode { get; set; }
    }
}
