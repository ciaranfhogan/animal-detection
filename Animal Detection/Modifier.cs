using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovementDetection.Modifiers
{
    public interface Modifier
    {
        float Apply(float value, float strength);
    }
}

namespace MovementDetection.Modifiers.Decay
{
    public class Linear : Modifier
    {
        private float speed = 0.01f;

        public Linear() { }

        public Linear(float speed)
        {
            this.speed = speed;
        }

        public float Apply(float value, float strength)
        {
            return Math.Max(value - (speed * strength), 0f);
        }
    }
}

namespace MovementDetection.Modifiers.Growth
{
    public class Linear : Modifier
    {
        private float speed = 0.01f;

        public Linear() { }

        public Linear(float speed)
        {
            this.speed = speed;
        }

        public float Apply(float value, float strength)
        {
            return Math.Min(value + (speed * strength), 1f);
        }
    }
}


namespace MovementDetection.Modifiers.Dropoff
{
    public class Linear : Modifier
    {
        public Linear() { }

        public float Apply(float value, float strength)
        {
            return value * strength;
        }
    }
}