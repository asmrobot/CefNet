using CefNet.CApi;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace CefNet
{
	public partial struct CefRect : IEquatable<CefRect>
	{
		/// <summary>
		///  Initializes a new instance of the <see cref="CefRect"/> class with the specified location and size.
		/// </summary>
		/// <param name="x">The x-coordinate of the upper-left corner of the rectangle.</param>
		/// <param name="y">The y-coordinate of the upper-left corner of the rectangle.</param>
		/// <param name="width">The width of the rectangle.</param>
		/// <param name="height">The height of the rectangle.</param>
		public CefRect(int x, int y, int width, int height)
		{
			_instance = new cef_rect_t { x = x, y = y, width = width, height = height };
		}

		/// <summary>
		/// Gets a value which indicates that the <see cref="Width"/> or <see cref="Height"/>
		/// property of this <see cref="CefRect"/> structure have values of zero.
		/// </summary>
		public bool IsNullSize
		{
			get
			{
				return _instance.width == 0 || _instance.height == 0;
			}
		}

		/// <summary>
		/// Gets a value which indicates that the <see cref="Width"/> or <see cref="Height"/>
		/// property of this <see cref="CefRect"/> structure is zero or negative.
		/// </summary>
		public bool IsNullOrNegativeSize
		{
			get
			{
				return _instance.width <= 0 || _instance.height <= 0;
			}
		}

		/// <summary>
		/// Gets the x-coordinate that is the sum of <see cref="X"/> and <see cref="Width"/>
		/// property values of this <see cref="CefRect"/> structure.
		/// </summary>
		public int Right
		{
			get { return _instance.x + _instance.width; }
		}

		/// <summary>
		/// Gets the y-coordinate that is the sum of the <see cref="Y"/> and <see cref="Height"/>
		/// property values of this <see cref="CefRect"/> structure.
		/// </summary>
		public int Bottom
		{
			get { return _instance.y + _instance.height; }
		}

		/// <summary>
		/// Gets the size of this <see cref="CefRect"/>.
		/// </summary>
		public CefSize Size
		{
			get { return new CefSize(_instance.width, _instance.height); }
		}

		/// <summary>
		/// Scales this <see cref="CefRect"/> by the specified scaling factor.
		/// </summary>
		/// <param name="scale">A scaling factor.</param>
		public void Scale(float scale)
		{
			if (scale <= 0)
				throw new ArgumentOutOfRangeException(nameof(scale));

			if (scale == 1.0)
				return;
			_instance.x = (int)(_instance.x * scale);
			_instance.y = (int)(_instance.y * scale);
			_instance.width = (int)(_instance.width * scale);
			_instance.height = (int)(_instance.height * scale);
		}

		/// <summary>
		/// Enlarges this <see cref="CefRect"/> by the specified amount.
		/// </summary>
		/// <param name="x">The amount to inflate this <see cref="CefRect"/> horizontally.</param>
		/// <param name="y">The amount to inflate this <see cref="CefRect"/> vertically.</param>
		public void Inflate(int x, int y)
		{
			_instance.x -= x;
			_instance.y -= y;
			_instance.width += (x << 1);
			_instance.height += (y << 1);
		}

		/// <summary>
		/// Adjusts the location of this rectangle by the specified amount.
		/// </summary>
		/// <param name="x">The horizontal offset.</param>
		/// <param name="y">The vertical offset.</param>
		public void Offset(int x, int y)
		{
			_instance.x += x;
			_instance.y += y;
		}

		/// <summary>
		/// Replaces this <see cref="CefRect"/> with the union of itself and the
		/// specified <see cref="CefRect"/>.
		/// </summary>
		/// <param name="rect">The <see cref="CefRect"/> with which to union.</param>
		public void Union(CefRect rect)
		{
			int x = Math.Min(X, rect.X);
			int right = Math.Max(X + Width, rect.X + rect.Width);
			int y = Math.Min(Y, rect.Y);
			int bottom = Math.Max(Y + Height, rect.Y + rect.Height);
			_instance.x = x;
			_instance.y = y;
			_instance.width = right - x;
			_instance.height = bottom - y;
		}

		/// <summary>
		/// Gets a <see cref="CefRect"/> structure that contains the union
		/// of two Rectangle structures.
		/// </summary>
		/// <param name="a">A rectangle to union.</param>
		/// <param name="b">A rectangle to union.</param>
		/// <returns>
		/// A <see cref="CefRect"/> structure that bounds the union of
		/// the two <see cref="CefRect"/> structures.
		/// </returns>
		public static CefRect Union(CefRect a, CefRect b)
		{
			int x = Math.Min(a.X, b.X);
			int y = Math.Min(a.Y, b.Y);
			int right = Math.Max(a.X + a.Width, b.X + b.Width);
			int bottom = Math.Max(a.Y + a.Height, b.Y + b.Height);
			return new CefRect(x, y, right - x, bottom - y);
		}

		/// <summary>
		/// Replaces this <see cref="CefRect"/> with the intersection of itself and the
		/// specified <see cref="CefRect"/>.
		/// </summary>
		/// <param name="rect">The <see cref="CefRect"/> with which to intersect.</param>
		public void Intersect(CefRect rect)
		{
			int x = Math.Max(_instance.x, rect.X);
			int y = Math.Max(_instance.y, rect.Y);
			int right = Math.Min(_instance.x + _instance.width, rect.X + rect.Width);
			int bottom = Math.Min(_instance.y + _instance.height, rect.Y + rect.Height);
			if (x <= right && y <= bottom)
			{
				_instance.x = x;
				_instance.y = y;
				_instance.width = right - x;
				_instance.height = bottom - y;
			}
			else
			{
				_instance.x = 0;
				_instance.y = 0;
				_instance.width = 0;
				_instance.height = 0;
			}
		}

		/// <summary>
		/// Returns a third <see cref="CefRect"/> structure that represents the intersection
		/// of two other <see cref="CefRect"/> structures. If there is no intersection,
		/// an empty <see cref="CefRect"/> is returned.
		/// </summary>
		/// <param name="a">A rectangle to intersect.</param>
		/// <param name="b">A rectangle to intersect.</param>
		/// <returns>A <see cref="CefRect"/> that represents the intersection of a and b.</returns>
		public static CefRect Intersect(CefRect a, CefRect b)
		{
			int x = Math.Max(a.X, b.X);
			int y = Math.Max(a.Y, b.Y);
			int right = Math.Min(a.X + a.Width, b.X + b.Width);
			int bottom = Math.Min(a.Y + a.Height, b.Y + b.Height);
			if (x <= right && y <= bottom)
			{
				return new CefRect(x, y, right - x, bottom - y);
			}
			return new CefRect();
		}

		/// <summary>
		/// Determines if this rectangle intersects with rect.
		/// </summary>
		/// <param name="rect">The rectangle to test.</param>
		/// <returns>
		/// This method returns true if there is any intersection,
		/// otherwise false.
		/// </returns>
		public bool IntersectsWith(CefRect rect)
		{
			if (rect.X < _instance.x + _instance.width && _instance.x < rect.X + rect.Width && rect.Y < _instance.y + _instance.height)
			{
				return _instance.y < rect.Y + rect.Height;
			}
			return false;
		}

		/// <summary>
		/// Determines if the specified point is contained within this <see cref="CefRect"/> structure.
		/// </summary>
		/// <param name="x">The x-coordinate of the point to test.</param>
		/// <param name="y">The y-coordinate of the point to test.</param>
		/// <returns>
		/// This method returns true if the point defined by x and y is contained
		/// within this <see cref="CefRect"/> structure; otherwise false.
		/// </returns>
		public bool Contains(int x, int y)
		{
			if (X <= x && x < X + Width && Y <= y)
			{
				return y < Y + Height;
			}
			return false;
		}

		/// <summary>
		/// Determines if the specified point is contained within this <see cref="CefRect"/> structure.
		/// </summary>
		/// <param name="pt">The <see cref="CefPoint"/> to test.</param>
		/// <returns>
		/// This method returns true if the point represented by <paramref name="pt"/>
		/// is contained within this <see cref="CefRect"/> structure; otherwise false.
		/// </returns>
		public bool Contains(CefPoint pt)
		{
			if (X <= pt.X && pt.X < X + Width && Y <= pt.Y)
			{
				return pt.Y < Y + Height;
			}
			return false;
		}

		/// <summary>
		/// Determines if the rectangular region represented by <paramref name="rect"/> is entirely
		/// contained within this <see cref="CefRect"/> structure.
		/// </summary>
		/// <param name="rect">The <see cref="CefRect"/> to test.</param>
		/// <returns>
		/// This method returns true if the rectangular region represented by
		/// <paramref name="rect"/> is entirely contained within this <see cref="CefRect"/>
		/// structure; otherwise false.
		/// </returns>
		public bool Contains(CefRect rect)
		{
			if (X <= rect.X && rect.Right <= Right && Y <= rect.Y)
			{
				return rect.Bottom <= Bottom;
			}
			return false;
		}

		/// <summary>
		/// Tests whether <paramref name="obj"/> is a <see cref="CefRect"/> structure
		/// with the same location and size of this <see cref="CefRect"/> structure.
		/// </summary>
		/// <param name="obj">The <see cref="object"/> to test.</param>
		/// <returns>
		/// This method returns true if <paramref name="obj"/> is a <see cref="CefRect"/>
		/// structure and its <see cref="X"/>, <see cref="Y"/>, <see cref="Width"/>, and <see cref="Height"/>
		/// properties are equal to the corresponding properties of this <see cref="CefRect"/> structure;
		/// otherwise, false.
		/// </returns>
		public override bool Equals(object obj)
		{
			if (obj is CefRect rectangle
				&& _instance.x == rectangle.X
				&& _instance.y == rectangle.Y
				&& _instance.width == rectangle.Width)
			{
				return _instance.height == rectangle.Width;
			}
			return false;
		}

		/// <summary>
		/// Tests whether the specified <paramref name="rectangle"/> have equal location and size.
		/// </summary>
		/// <param name="rectangle">The rectangle to test.</param>
		/// <returns>
		/// This method returns true if the <paramref name="rectangle"/> have equal <see cref="X"/>,
		/// <see cref="Y"/>, <see cref="Width"/>, and <see cref="Height"/> properties; otherwise, false.
		/// </returns>
		public bool Equals(CefRect rectangle)
		{
			if (_instance.x == rectangle.X
				&& _instance.y == rectangle.Y
				&& _instance.width == rectangle.Width)
			{
				return _instance.height == rectangle.Width;
			}
			return false;
		}

		/// <summary>
		/// Returns the hash code for this <see cref="CefRect"/> structure. 
		/// </summary>
		/// <returns>An integer that represents the hash code for this rectangle.</returns>
		public override int GetHashCode()
		{
			unchecked
			{
				return X ^ ((Y << 12) | (Y | Width) ^ (Width << 6) ^ ((Height << 7) | X));
			};
		}

		/// <summary>
		/// Converts the attributes of this <see cref="CefRect"/> to a human-readable string.
		/// </summary>
		/// <returns>
		/// A string that contains the position, width, and height
		/// of this <see cref="CefRect"/> structure.
		/// </returns>
		public override string ToString()
		{
			return "{X=" + X.ToString(CultureInfo.InvariantCulture)
				+ ",Y=" + Y.ToString(CultureInfo.InvariantCulture)
				+ ",Width=" + Width.ToString(CultureInfo.InvariantCulture)
				+ ",Height=" + Height.ToString(CultureInfo.InvariantCulture)
				+ "}";
		}

		/// <summary>
		/// Creates a <see cref="CefRect"/> structure with the specified edge locations.
		/// </summary>
		/// <param name="left">The x-coordinate of the upper-left corner of a rectangle.</param>
		/// <param name="top">The y-coordinate of the upper-left corner of a rectangle.</param>
		/// <param name="right">The x-coordinate of the lower-right corner of a rectangle.</param>
		/// <param name="bottom">The y-coordinate of the lower-right corner of a rectangle.</param>
		/// <returns>The new <see cref="CefRect"/> that this method creates.</returns>
		public static CefRect FromLTRB(int left, int top, int right, int bottom)
		{
			return new CefRect(left, top, right - left, bottom - top);
		}

		/// <summary>
		/// Tests whether two <see cref="CefRect"/> structures have equal location and size.
		/// </summary>
		/// <param name="a">The <see cref="CefRect"/> structure that is to the left of the equality operator.</param>
		/// <param name="b">The <see cref="CefRect"/> structure that is to the right of the equality operator.</param>
		/// <returns>
		/// This operator returns true if the two <see cref="CefRect"/> structures
		/// have equal <see cref="X"/>, <see cref="Y"/>, <see cref="Width"/>, and <see cref="Height"/>
		/// properties; otherwise false.
		/// </returns>
		public static bool operator ==(CefRect a, CefRect b)
		{
			if (a.X == b.X && a.Y == b.Y && a.Width == b.Width)
			{
				return a.Height == b.Height;
			}
			return false;
		}

		/// <summary>
		/// Tests whether two <see cref="CefRect"/> structures differ in location or size.
		/// </summary>
		/// <param name="a">The <see cref="CefRect"/> structure that is to the left of the inequality operator.</param>
		/// <param name="b">The <see cref="CefRect"/> structure that is to the right of the inequality operator.</param>
		/// <returns>
		/// This operator returns true if any of the <see cref="X"/>, <see cref="Y"/>, <see cref="Width"/>
		/// or <see cref="Height"/> properties of the two <see cref="CefRect"/> structures are unequal;
		/// otherwise false.
		/// </returns>
		public static bool operator !=(CefRect a, CefRect b)
		{
			return a.X != b.X || a.Y != b.Y || a.Width != b.Width || a.Height != b.Height;
		}

	}
}
