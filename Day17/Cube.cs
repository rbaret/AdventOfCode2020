using System;


namespace Day17
{
    internal class Cube : IEquatable<Cube>
    {
        private int w;
        private int x;
        private int y;
        private int z;
        Tuple<int, int, int, int> coords;
        private bool active;

        public Cube(int w, int x, int y, int z)
        {
            this.w = w;
            this.x = x;
            this.y = y;
            this.z = z;
            coords = new Tuple<int, int, int, int>(w, x, y, z);
            active = true;
        }
        public Cube(int w, int x, int y, int z, bool status)
        {
            this.w = w;
            this.x = x;
            this.y = y;
            this.z = z;
            coords = new Tuple<int, int, int, int>(w, x, y, z);
            active = status;
        }

        public bool GetActiveStatus()
        {
            return active;
        }

        public Tuple<int, int, int, int> GetCoords()
        {
            return coords;
        }

        public int GetW() { return w; }
        public int GetX() { return x; }
        public int GetY() { return y; }
        public int GetZ() { return z; }


        public void SetActiveStatus(bool status)
        {
            active = status;
        }
        public override bool Equals(object obj)
        {
            return this.Equals(obj as Cube);
        }

        public bool Equals(Cube c)
        {
            if (Object.ReferenceEquals(c, null))
            {
                return false;
            }

            // Optimization for a common success case.
            if (Object.ReferenceEquals(this, c))
            {
                return true;
            }

            // If run-time types are not exactly the same, return false.
            if (this.GetType() != c.GetType())
            {
                return false;
            }

            return (w == c.w && x == c.x && y == c.y && z == c.z);

        }

        public bool Equals(int w, int x, int y, int z)
        {
            return (this.w == w && this.x == x && this.y == y && this.z == z);
        }
        public override int GetHashCode()
        {
            return w*1000000 + x * 10000 + y*100 + z;
        }

        public static bool operator ==(Cube lhs, Cube rhs)
        {
            // Check for null on left side.
            if (Object.ReferenceEquals(lhs, null))
            {
                if (Object.ReferenceEquals(rhs, null))
                {
                    // null == null = true.
                    return true;
                }

                // Only the left side is null.
                return false;
            }
            // Equals handles case of null on right side.
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Cube lhs, Cube rhs)
        {
            return !(lhs == rhs);
        }
        public bool IsActive()
        {
            return active;
        }

        public override string ToString()
        {
            return w.ToString()+','+x.ToString() + ',' + y.ToString() + ',' + z.ToString();
        }
    }
}
