// Ignore Spelling: rhs lhs

namespace Skyline.DataMiner.Utils.Linux.DiskConfiguration
{
	using System;
	using System.Globalization;

	/// <summary>
	/// Represents the size of a a file item.
	/// </summary>
	public sealed class Size : IEquatable<Size>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Size"/> class.
		/// </summary>
		/// <param name="bytes">The size in bytes.</param>
		public Size(long bytes)
		{
			Bytes = bytes;
		}

		/// <summary>
		/// Get the Size in bytes.
		/// </summary>
		public long Bytes { get; private set; }

		/// <summary>
		/// Provides a human readable string of the binary size (e.g. '23.1 MiB').
		/// </summary>
		/// <param name="bytes">The size in bytes.</param>
		/// <returns>The human readable string.</returns>
		public static string HumanReadableByteCountBin(long bytes)
		{
			long absB = bytes == long.MinValue ? long.MaxValue : Math.Abs(bytes);
			if (absB < 1024)
			{
				return bytes + " B";
			}

			long value = absB;
			char[] units = new[] { 'K', 'M', 'G', 'T', 'P', 'E' };
			int unitSelection = 0;
			for (int i = 40; i >= 0 && absB > 0xfffccccccccccccL >> i; i -= 10)
			{
				value >>= 10;
				unitSelection++;
			}

			value *= Math.Sign(bytes);
			double dValue = value / 1024.0;
			return string.Format(CultureInfo.InvariantCulture, "{0:N1} {1}iB", dValue, units[unitSelection]);
		}

		/// <summary>
		/// Provides a human readable string of the size (e.g. '23.1 MB') conform with SI.
		/// </summary>
		/// <param name="bytes">The size in bytes.</param>
		/// <returns>The human readable string.</returns>
		public static string HumanReadableByteCountSI(long bytes)
		{
			if (bytes > -1000 && bytes < 1000)
			{
				return bytes + " B";
			}

			char[] units = new[] { 'k', 'M', 'G', 'T', 'P', 'E' };
			int unitSelection = 0;
			while (bytes <= -999_950 || bytes >= 999_950)
			{
				bytes /= 1000;
				unitSelection++;
			}

			double dValue = bytes / 1000.0;
			return string.Format(CultureInfo.InvariantCulture, "{0:N1} {1}B", dValue, units[unitSelection]);
		}

		/// <summary>
		/// Determines whether the two specified object are not equal.
		/// </summary>
		/// <param name="lhs">The first object to compare.</param>
		/// <param name="rhs">The second object to compare.</param>
		/// <returns><c>false</c> if the operands are equal; otherwise, <c>true</c>.</returns>
		public static bool operator !=(Size lhs, Size rhs) => !(lhs == rhs);

		/// <summary>
		/// Returns a value that indicates whether a specified <see cref="Size"/> value is less than another specified <see cref="Size"/> value.
		/// </summary>
		/// <param name="lhs">The first value to compare.</param>
		/// <param name="rhs">The second value to compare.</param>
		/// <returns><c>true</c> if <paramref name="lhs"/> is less than <paramref name="rhs"/>; otherwise, <c>false</c>.</returns>
		public static bool operator <(Size lhs, Size rhs) => lhs.Bytes < rhs.Bytes;

		/// <summary>
		/// Returns a value that indicates whether a specified <see cref="Size"/> value is less than or equal to another specified <see cref="Size"/> value.
		/// </summary>
		/// <param name="lhs">The first value to compare.</param>
		/// <param name="rhs">The second value to compare.</param>
		/// <returns><c>true</c> if <paramref name="lhs"/> is less than or equal to <paramref name="rhs"/>; otherwise, <c>false</c>.</returns>
		public static bool operator <=(Size lhs, Size rhs) => lhs.Bytes <= rhs.Bytes;

		/// <summary>
		/// Determines whether the two specified object are equal.
		/// </summary>
		/// <param name="lhs">The first object to compare.</param>
		/// <param name="rhs">The second object to compare.</param>
		/// <returns><c>true</c> if the operands are equal; otherwise, <c>false</c>.</returns>
		public static bool operator ==(Size lhs, Size rhs)
		{
			if (lhs is null)
			{
				if (rhs is null)
				{
					return true;
				}

				// Only the left side is null.
				return false;
			}

			// Equals handles case of null on right side.
			return lhs.Equals(rhs);
		}

		/// <summary>
		/// Returns a value that indicates whether a specified <see cref="Size"/> value is greater than another specified <see cref="Size"/> value.
		/// </summary>
		/// <param name="lhs">The first value to compare.</param>
		/// <param name="rhs">The second value to compare.</param>
		/// <returns><c>true</c> if <paramref name="lhs"/> is greater than <paramref name="rhs"/>; otherwise, <c>false</c>.</returns>
		public static bool operator >(Size lhs, Size rhs) => lhs.Bytes > rhs.Bytes;

		/// <summary>
		/// Returns a value that indicates whether a specified <see cref="Size"/> value is greater than or equal to another specified <see cref="Size"/> value.
		/// </summary>
		/// <param name="lhs">The first value to compare.</param>
		/// <param name="rhs">The second value to compare.</param>
		/// <returns><c>true</c> if <paramref name="lhs"/> is greater than or equal to <paramref name="rhs"/>; otherwise, <c>false</c>.</returns>
		public static bool operator >=(Size lhs, Size rhs) => lhs.Bytes >= rhs.Bytes;

		/// <summary>
		/// Compares the object to another object.
		/// </summary>
		/// <param name="obj">The object to compare against.</param>
		/// <returns><c>true</c> if the elements are equal; otherwise, <c>false</c>.</returns>
		public override bool Equals(object obj) => this.Equals(obj as Size);

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns><c>true</c> if the elements are equal; otherwise, <c>false</c>.</returns>
		public bool Equals(Size other)
		{
			if (other == null)
			{
				return false;
			}

			if (Bytes == other.Bytes)
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// Returns the hash code.
		/// </summary>
		/// <returns>The hash code.</returns>
		public override int GetHashCode() => Bytes.GetHashCode();

		/// <summary>
		/// Returns a string that represents the size (human readable).
		/// </summary>
		/// <returns>A string that represents the size.</returns>
		public override string ToString()
		{
			return HumanReadableByteCountBin(Bytes);
		}
	}
}