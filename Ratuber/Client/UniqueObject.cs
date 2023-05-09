using System;

namespace Ratuber.Client
{
    public abstract class UniqueObject
    {
        private Guid id;

        public UniqueObject()
        {
            id = Guid.NewGuid();
        }

        public int GetUniqueId()
        {
            return id.GetHashCode();
        }
    }
}
