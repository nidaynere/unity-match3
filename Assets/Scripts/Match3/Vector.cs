namespace Match3
{
    public struct Vector
    {
        public Vector(int _X, int _Y)
        {
            X = _X;
            Y = _Y;
        }
    
        public Vector Normalize()
        {
            var _X = X;
            if (_X < 0)
                _X = -1;
            else if (_X > 0)
                _X = 1;
    
            var _Y = Y;
            if (_Y < 0)
                _Y = -1;
            else if (_Y > 0)
                _Y = 1;
    
            return new Vector(_X, _Y);
        }

        public Vector Reverse()
        {
            return new Vector(X * -1, Y * -1);
        }
    
        public int X, Y;
    
        public override string ToString()
        {
            return "(X=" + X + "," + "Y=" + Y + ")";
        }
    
        public static Vector operator +(Vector a, Vector b)
        => new Vector(a.X + b.X, a.Y + b.Y);
    
        public static Vector operator -(Vector a, Vector b)
        => new Vector(a.X - b.X, a.Y - b.Y);

        public static bool operator ==(Vector a, Vector b)
        => a.X == b.X && a.Y == b.Y;
    
        public static bool operator !=(Vector a, Vector b)
        => a.X != b.X || a.Y != b.Y;
    
        public static Vector operator *(Vector a, int b)
        => new Vector(a.X * b, a.Y * b);
    
        public override bool Equals(object obj)
        {
            return obj is Vector position &&
                   X == position.X &&
                   Y == position.Y;
        }
    
        public override int GetHashCode()
        {
            int hashCode = 1861411795;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
        }
    }
}
