using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class BaseEntity
    {
        public BaseEntity()
        {
            this.DateCreated = DateTime.Now;
        }
        public long Id { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
