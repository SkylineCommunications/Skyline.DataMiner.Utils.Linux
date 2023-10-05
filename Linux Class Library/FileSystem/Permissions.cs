// Ignore Spelling: lhs rhs

namespace Skyline.DataMiner.Utils.Linux.FileSystem
{
	using System;

	/// <summary>
	/// Represents the permissions for a file, directory or link.
	/// </summary>
	public sealed class Permissions : IEquatable<Permissions>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Permissions"/> class.
		/// </summary>
		/// <param name="permissionPart">The permissions (e.g. rwxr--r--).</param>
		public Permissions(string permissionPart)
		{
			if (permissionPart.Length != 9)
			{
				throw new ArgumentException("expecting 9 characters (e.g. rwxr--r--).", "permissionPart");
			}

			UserPermission = new Permission(permissionPart.Substring(0, 3));
			GroupPermission = new Permission(permissionPart.Substring(3, 3));
			OtherPermission = new Permission(permissionPart.Substring(6, 3));
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Permissions"/> class.
		/// </summary>
		/// <param name="userPermissions">The permissions of the user.</param>
		/// <param name="groupPermission">The permissions of the group.</param>
		/// <param name="otherPermission">The permissions of the others.</param>
		public Permissions(Permission userPermissions, Permission groupPermission, Permission otherPermission)
		{
			UserPermission = userPermissions;
			GroupPermission = groupPermission;
			OtherPermission = otherPermission;
		}

		/// <summary>
		/// The permissions of the group owner.
		/// </summary>
		public Permission GroupPermission { get; private set; }

		/// <summary>
		/// The permissions for all other users.
		/// </summary>
		public Permission OtherPermission { get; private set; }

		/// <summary>
		/// The string used to represent the 3 digit permissions of the file.
		/// </summary>
		public string PermissionString
		{
			get
			{
				return $"{this.UserPermission.PermissionLevel}{this.GroupPermission.PermissionLevel}{this.OtherPermission.PermissionLevel}";
			}
		}

		/// <summary>
		/// The permissions of the user owner.
		/// </summary>
		public Permission UserPermission { get; private set; }

		/// <summary>
		/// Determines whether the two specified object are not equal.
		/// </summary>
		/// <param name="lhs">The first object to compare.</param>
		/// <param name="rhs">The second object to compare.</param>
		/// <returns><c>false</c> if the operands are equal; otherwise, <c>true</c>.</returns>
		public static bool operator !=(Permissions lhs, Permissions rhs) => !(lhs == rhs);

		/// <summary>
		/// Determines whether the two specified object are equal.
		/// </summary>
		/// <param name="lhs">The first object to compare.</param>
		/// <param name="rhs">The second object to compare.</param>
		/// <returns><c>true</c> if the operands are equal; otherwise, <c>false</c>.</returns>
		public static bool operator ==(Permissions lhs, Permissions rhs)
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
		/// Compares the object to another object.
		/// </summary>
		/// <param name="obj">The object to compare against.</param>
		/// <returns><c>true</c> if the elements are equal; otherwise, <c>false</c>.</returns>
		public override bool Equals(object obj) => this.Equals(obj as Permissions);

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns><c>true</c> if the elements are equal; otherwise, <c>false</c>.</returns>
		public bool Equals(Permissions other)
		{
			if (UserPermission == other.UserPermission && GroupPermission == other.GroupPermission && OtherPermission == other.OtherPermission)
			{
				return true;
			}

			return false;
		}

		/// <summary>
		/// Returns the hash code.
		/// </summary>
		/// <returns>The hash code.</returns>
		public override int GetHashCode() => (UserPermission, GroupPermission, OtherPermission).GetHashCode();

		public override string ToString()
		{
			return PermissionString;
		}

		/// <summary>
		/// Represents the permissions of a user, group or others.
		/// </summary>
		public sealed class Permission : IEquatable<Permission>
		{
			private const char ExecuteChar = 'x';
			private const int ExecuteValue = 1;
			private const char ReadChar = 'r';
			private const int ReadValue = 4;
			private const char WriteChar = 'w';
			private const int WriteValue = 2;

			/// <summary>
			/// Initializes a new instance of the <see cref="Permission"/> class.
			/// </summary>
			/// <param name="level">The level of the permission.</param>
			public Permission(int level)
			{
				if (level < 0 || level > 7)
				{
					throw new ArgumentException("Expect value from 0 to 7 for the permission level.", "level");
				}

				this.PermissionLevel = level;
			}

			/// <summary>
			/// Initializes a new instance of the <see cref="Permission"/> class.
			/// </summary>
			/// <param name="read">Whether read permission is given.</param>
			/// <param name="write">Whether write permission is given.</param>
			/// <param name="execute">Whether execute permission is given.</param>
			public Permission(bool read, bool write, bool execute)
			{
				int alarmlevel = 0;

				if (read)
				{
					alarmlevel += ReadValue;
				}

				if (write)
				{
					alarmlevel += WriteValue;
				}

				if (execute)
				{
					alarmlevel += ExecuteValue;
				}

				this.PermissionLevel = alarmlevel;
			}

			/// <summary>
			/// Initializes a new instance of the <see cref="Permission"/> class.
			/// </summary>
			/// <param name="permission">The permissions (e.g. rwx or r--).</param>
			public Permission(string permission)
			{
				if (permission.Length != 3)
				{
					throw new ArgumentException("The permission should only contain 3 characters (e.g. rwx or r--)", "permission");
				}

				int alarmlevel = 0;

				if (permission[0] == ReadChar)
				{
					alarmlevel += ReadValue;
				}

				if (permission[1] == WriteChar)
				{
					alarmlevel += WriteValue;
				}

				if (permission[2] == ExecuteChar)
				{
					alarmlevel += ExecuteValue;
				}

				this.PermissionLevel = alarmlevel;
			}

			/// <summary>
			/// Gets whether execute permission is given.
			/// </summary>
			public bool Execute
			{
				get
				{
					return (PermissionLevel & ExecuteValue) == ExecuteValue;
				}
			}

			/// <summary>
			/// Gets the permission level.
			/// </summary>
			public int PermissionLevel { get; private set; }

			/// <summary>
			/// Gets whether read permission is given.
			/// </summary>
			public bool Read
			{
				get
				{
					return (PermissionLevel & ReadValue) == ReadValue;
				}
			}

			/// <summary>
			/// Gets whether write permission is given.
			/// </summary>
			public bool Write
			{
				get
				{
					return (PermissionLevel & WriteValue) == WriteValue;
				}
			}

			/// <summary>
			/// Determines whether the two specified object are not equal.
			/// </summary>
			/// <param name="lhs">The first object to compare.</param>
			/// <param name="rhs">The second object to compare.</param>
			/// <returns><c>false</c> if the operands are equal; otherwise, <c>true</c>.</returns>
			public static bool operator !=(Permission lhs, Permission rhs) => !(lhs == rhs);

			/// <summary>
			/// Determines whether the two specified object are equal.
			/// </summary>
			/// <param name="lhs">The first object to compare.</param>
			/// <param name="rhs">The second object to compare.</param>
			/// <returns><c>true</c> if the operands are equal; otherwise, <c>false</c>.</returns>
			public static bool operator ==(Permission lhs, Permission rhs)
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
			/// Compares the object to another object.
			/// </summary>
			/// <param name="obj">The object to compare against.</param>
			/// <returns><c>true</c> if the elements are equal; otherwise, <c>false</c>.</returns>
			public override bool Equals(object obj) => this.Equals(obj as Permission);

			/// <summary>
			/// Indicates whether the current object is equal to another object of the same type.
			/// </summary>
			/// <param name="other">An object to compare with this object.</param>
			/// <returns><c>true</c> if the elements are equal; otherwise, <c>false</c>.</returns>
			public bool Equals(Permission other)
			{
				if (other is null)
				{
					return false;
				}

				if (this.PermissionLevel == other.PermissionLevel)
				{
					return true;
				}

				return false;
			}

			/// <summary>
			/// Returns the hash code.
			/// </summary>
			/// <returns>The hash code.</returns>
			public override int GetHashCode() => PermissionLevel.GetHashCode();
		}
	}
}