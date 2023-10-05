namespace Skyline.DataMiner.Utils.Linux.Exceptions
{
	using System;
	using System.Runtime.Serialization;

	/// <summary>
	/// The exception that is thrown when a permission denied is returned.
	/// </summary>
	[Serializable]
	public class PermissionDeniedException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="PermissionDeniedException"/> class.
		/// </summary>
		public PermissionDeniedException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PermissionDeniedException"/> class with the command that failed.
		/// </summary>
		/// <param name="command">The command.</param>
		public PermissionDeniedException(string command)
			: base($"Permission denied: {command}'.")
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PermissionDeniedException"/> class with the command that failed and a reference to the inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="command">The command.</param>
		/// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
		public PermissionDeniedException(string command, Exception innerException)
			: base($"Permission denied: {command}'.", innerException)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PermissionDeniedException"/> class with serialized data.
		/// </summary>
		/// <param name="info">The serialization info.</param>
		/// <param name="context">The streaming context.</param>
		/// <exception cref="ArgumentNullException">The <paramref name="info"/> parameter is <see langword="null" />.</exception>
		/// <exception cref="SerializationException">The class name is <see langword="null" /> or HResult is zero (0).</exception>
		/// <remarks>This constructor is called during deserialization to reconstitute the exception object transmitted over a stream.</remarks>
		protected PermissionDeniedException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}