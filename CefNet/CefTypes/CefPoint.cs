using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace CefNet
{
	public partial struct CefPoint : IEquatable<CefPoint>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CefPoint"/> class with the specified coordinates.
		/// </summary>
		/// <param name="x">The horizontal position of the point.</param>
		/// <param name="y">The vertical position of the point.</param>
		public CefPoint(int x, int y)
		{
			_instance.x = x;
			_instance.y = y;
		}

		/// <summary>
		/// Multiplies coordinates of the current point by the specified value.
		/// </summary>
		/// <param name="value">The scale factor.</param>
		public void Scale(float value)
		{
			_instance.x = (int)(_instance.x * value);
			_instance.y = (int)(_instance.y * value);
		}

		/// <summary>
		/// Translates this <see cref="CefPoint"/> by the specified amount.
		/// </summary>
		/// <param name="x">The amount to offset the x-coordinate.</param>
		/// <param name="y">The amount to offset the y-coordinate.</param>
		public void Offset(int x, int y)
		{
			_instance.x += x;
			_instance.y += y;
		}

		/// <summary>
		/// Converts the attributes of this <see cref="CefPoint"/> to a human-readable string.
		/// </summary>
		/// <returns>
		/// A string that represents this <see cref="CefPoint"/>.
		/// </returns>
		public override string ToString()
		{
			return "{X=" + X.ToString(CultureInfo.InvariantCulture) + ",Y=" + Y.ToString(CultureInfo.InvariantCulture) + "}";
		}

		/// <summary>
		/// Specifies whether this point instance contains the same coordinates as the specified <see cref="CefPoint"/>.
		/// </summary>
		/// <param name="other">The <see cref="CefPoint"/> to test for equality.</param>
		/// <returns>Returns true if <paramref name="other"/> has the same coordinates as this point instance.</returns>
		public bool Equals(CefPoint other)
		{
			return X == other.X && Y == other.Y;
		}

		/// <summary>
		/// Specifies whether this point instance contains the same coordinates as the specified object.
		/// </summary>
		/// <param name="obj">The <see cref="Object"/> to test for equality.</param>
		/// <returns>
		/// Returns true if <paramref name="obj"/> is a <see cref="CefPoint"/> and has
		/// the same coordinates as this point instance.
		/// </returns>
		public override bool Equals(object obj)
		{
			return (obj is CefPoint pt) && X == pt.X && Y == pt.Y;
		}

		/// <summary>
		/// Returns a hash code for this <see cref="CefPoint"/>.
		/// </summary>
		/// <returns>An integer value that specifies a hash value for this <see cref="CefPoint"/>.</returns>
		public override int GetHashCode()
		{
			return X ^ Y;
		}

		/// <summary>
		/// Compares two <see cref="CefPoint"/> objects. The result specifies whether
		/// the values of the X and Y properties of the two <see cref="CefPoint"/>
		/// objects are equal.
		/// </summary>
		/// <param name="a">A <see cref="CefPoint"/> to compare.</param>
		/// <param name="b">A <see cref="CefPoint"/> to compare.</param>
		/// <returns>
		/// Returns true if the <see cref="X"/> and <see cref="Y"/> values of left and
		/// right are equal; otherwise, false.
		/// </returns>
		public static bool operator ==(CefPoint a, CefPoint b)
		{
			return a.X == b.X && a.Y == b.Y;
		}

		/// <summary>
		/// Compares two <see cref="CefPoint"/> objects. The result specifies whether
		/// the values of the X or Y properties of the two <see cref="CefPoint"/>
		/// objects are unequal.
		/// </summary>
		/// <param name="a">A <see cref="CefPoint"/> to compare.</param>
		/// <param name="b">A <see cref="CefPoint"/> to compare.</param>
		/// <returns>
		/// Returns true if the values of either the <see cref="X"/> properties or the
		/// <see cref="Y"/> properties of left and right differ; otherwise, false.
		/// </returns>
		public static bool operator !=(CefPoint a, CefPoint b)
		{
			return a.X != b.X || a.Y != b.Y;
		}
	}
}
