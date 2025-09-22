using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InExTrack.Domain.Commons
{
    public abstract class Entity
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime CreatedDate { get; private set; } = DateTime.UtcNow;

        public DateTime? UpdatedDate { get; set; }
    }
}
