using System;

namespace SquirrelTube.Client
{
    public abstract class UniqueObject
    {
        private Guid id;

        public UniqueObject()
        {
            id = Guid.NewGuid();
        }

        public override int GetHashCode()
        {
            return id.GetHashCode();
        }
    }
}
