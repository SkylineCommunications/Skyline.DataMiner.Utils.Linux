namespace Skyline.DataMiner.Utils.Linux.Exceptions
{
	using System;
	using System.Runtime.Serialization;

	/// <summary>
	/// The exception is thrown if the operating system of the Linux machine is not (yet) supported by this library.
	/// </summary>
	[Serializable]
	public class LinuxNotSupportedException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="LinuxNotSupportedException"/> class.
		/// </summary>
		public LinuxNotSupportedException() : base("Operating system not (yet) support by the library.")
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LinuxNotSupportedException"/> class.
		/// </summary>
		/// <param name="message">The message.</param>
		public LinuxNotSupportedException(string message) : base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LinuxNotSupportedException"/> class with a reference to the inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="message">The message.</param>
		/// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
		public LinuxNotSupportedException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LinuxNotSupportedException"/> class with serialized data.
		/// </summary>
		/// <param name="info">The serialization info.</param>
		/// <param name="context">The streaming context.</param>
		/// <exception cref="ArgumentNullException">The <paramref name="info"/> parameter is <see langword="null" />.</exception>
		/// <exception cref="SerializationException">The class name is <see langword="null" /> or HResult is zero (0).</exception>
		/// <remarks>This constructor is called during deserialization to reconstitute the exception object transmitted over a stream.</remarks>
		protected LinuxNotSupportedException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}