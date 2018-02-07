namespace Utility
{
    [System.Serializable]
    public class int2
    {
        public int x, y;

        public int2()
        {
            this.x = 0;
            this.y = 0;
        }

        public int2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static int2 one
        {
            get { return new int2(1, 1); }
        }

        public static int2 zero
        {
            get { return new int2(0, 0); }
        }

        public static int2 operator +(int2 i1, int2 i2)
        {
            return new int2(i1.x + i2.x, i1.y + i2.y);
        }

        public static int2 operator -(int2 i1, int2 i2)
        {
            return new int2(i1.x - i2.x, i1.y - i2.y);
        }

        public static int2 operator *(int2 i1, int i2)
        {
            return new int2(i1.x * i2, i1.y * i2);
        }

        public static bool operator == (int2 i1, int2 i2)
        {
            if (ReferenceEquals(i1, null) && ReferenceEquals(i2, null)) return true; // Both are null
            else if (ReferenceEquals(i1, null) || ReferenceEquals(i2, null)) return false; // Only one was null
            return i1.x == i2.x && i1.y == i2.y;
        }

        public static bool operator !=(int2 i1, int2 i2)
        {
            return i1.x != i2.x || i1.y != i2.y;
        }
    }
}