using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Models.Response
{
    public class OccupationResponse: ResponseBase
    {
        public List<Occupation> Occupations { get; set; }
    }

    public class Occupation
    {
        public string Id { get; set; }
        public string Value { get; set; }
    }
}
