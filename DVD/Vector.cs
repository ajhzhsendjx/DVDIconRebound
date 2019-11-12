using System;
using System.Windows;

namespace DVD
{
    public class Vector
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public Vector()
        {
            X = 0;
            Y = 0;
            Z = 0;
        }
        public Vector(double x, double y)
        {
            X = x;
            Y = y;
            Z = 0;
        }
        public Vector(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public Vector(Point p)
        {
            X = p.X;
            Y = p.Y;
            Z = 0;
        }
        public Vector(Point p1, Point p2)
        {
            X = p2.X - p1.X;
            Y = p2.Y - p1.Y;
            Z = 0;
        }

        public virtual Vector Value()
        {
            return this;
        }
        public virtual Vector Apply(Func<double, double> f)
        {
            return new Vector(f(X), f(Y), f(Z));
        }
        public virtual double Square()
        {
            return X * X + Y * Y + Z * Z;
        }
        public virtual double Norm()
        {
            return Math.Sqrt(Square());
        }
        public virtual Vector UnitVector()
        {
            return Value() / Norm();
        }
        public virtual Vector DirectionVector()
        {
            return UnitVector().Apply(x => Math.Acos(x));
        }
        public virtual double DotProduct(Vector V)
        {
            return X * V.X + Y * V.Y + Z * V.Z;
        }
        public virtual Vector CrossProduct(Vector V)
        {
            return new Vector(-Z * V.Y + Y * V.Z, Z * V.X - X * V.Z, -Y * V.X + X * V.Y);
        }
        public virtual bool IsVertical(Vector V)
        {
            return Value().DotProduct(V) == 0;
        }
        public virtual bool IsParallel(Vector V)
        {
            return Value().CrossProduct(V) == new Vector();
        }

        public override string ToString()
        {
            return $"{X},{Y},{Z}";
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public Point ToPoint()
        {
            return new Point(X, Y);
        }

        public static Vector operator +(Vector R)
        {
            return R;
        }
        public static Vector operator +(Vector L, Vector R)
        {
            return new Vector(L.X + R.X, L.Y + R.Y);
        }
        public static Vector operator -(Vector R)
        {
            return new Vector(-R.X, -R.Y);
        }
        public static Vector operator -(Vector L, Vector R)
        {
            return -R + L;
        }
        public static Vector operator *(Vector L, double R)
        {
            return new Vector(L.X * R, L.Y * R);
        }
        public static Vector operator *(double L, Vector R)
        {
            return R * L;
        }
        public static Vector operator /(Vector L, double R)
        {
            return 1 / R * L;
        }
        public static bool operator ==(Vector L,Vector R)
        {
            return L.X == R.Y && L.Y == R.Y;
        }
        public static bool operator !=(Vector L,Vector R)
        {
            return !(L == R);
        }
    }
    public class Point
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"({X},{Y})";
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static Point operator +(Point L, Point R)
        {
            return new Point(L.X + R.X, L.Y + R.Y);
        }
        public static Point operator -(Point R)
        {
            return new Point(-R.X, -R.Y);
        }
        public static Point operator -(Point L, Point R)
        {
            return -R + L;
        }
        public static Point operator *(Point L, float R)
        {
            return new Point(L.X * R, L.Y * R);
        }
        public static Point operator *(float L, Point R)
        {
            return R * L;
        }
        public static Point operator /(Point L, float R)
        {
            return 1 / R * L;
        }
        public static bool operator ==(Point L, Point R)
        {
            return L.X == R.X && L.Y == R.Y;
        }
        public static bool operator !=(Point L, Point R)
        {
            return !(L == R);
        }

        public static implicit operator System.Windows.Point(Point point)
        {
            return new System.Windows.Point(point.X, point.Y);
        }
        public static implicit operator Point(System.Windows.Point point)
        {
            return new Point(point.X, point.Y);
        }
        public static implicit operator System.Drawing.Point(Point point)
        {
            return new System.Drawing.Point((int)point.X, (int)point.Y);
        }
        public static implicit operator Point(System.Drawing.Point point)
        {
            return new Point(point.X, point.Y);
        }
        public static explicit operator Size(Point point)
        {
            return new Size(point.X, point.Y);
        }
        public static implicit operator Point(Size size)
        {
            return new Point(size.Width, size.Height);
        }
    }
    public class Size
    {
        public double Width { get; set; }
        public double Height { get; set; }

        public Size(double width, double height)
        {
            Width = width;
            Height = height;
        }

        public override string ToString()
        {
            return $"({Width},{Height})";
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static Size operator +(Size L, Size R)
        {
            return new Size(L.Width + R.Width, L.Height + R.Height);
        }
        public static Size operator -(Size R)
        {
            return new Size(-R.Width, -R.Height);
        }
        public static Size operator -(Size L, Size R)
        {
            return -R + L;
        }
        public static Size operator *(Size L, double R)
        {
            return new Size(L.Width * R, L.Height * R);
        }
        public static Size operator *(double L, Size R)
        {
            return R * L;
        }
        public static Size operator /(Size L, double R)
        {
            return 1 / R * L;
        }
        public static bool operator ==(Size L, Size R)
        {
            return L.Width == R.Width && L.Height == R.Height;
        }
        public static bool operator !=(Size L, Size R)
        {
            return !(L == R);
        }

        public static implicit operator System.Drawing.Size(Size size)
        {
            return new System.Drawing.Size((int)size.Width, (int)size.Height);
        }
        public static implicit operator Size(System.Drawing.Size size)
        {
            return new Size(size.Width, size.Height);
        }
    }
}