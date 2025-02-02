using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iprox.Domain.Entities
{
    public class Base
    {
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }
}
