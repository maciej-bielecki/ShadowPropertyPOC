using System;
using System.Collections.Generic;
using System.Text;

namespace DaceloRex.WebApplication.SeedWork
{
    public abstract class Entity
    {
        public Guid Id { get; set; }

        public Guid TenantId { get; set; }

        public override int GetHashCode()
        {
            return (Id.GetHashCode() ^ TenantId.GetHashCode()).GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = obj as Entity;
            if (other != null)
                return Id.Equals(other.Id) && TenantId.Equals(other.TenantId);

            return base.Equals(obj);
        }

        public static bool operator ==(Entity x, Entity y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(Entity x, Entity y)
        {
            return !(x == y);
        }
    }
}
