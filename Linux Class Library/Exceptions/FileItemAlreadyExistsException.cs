namespace Skyline.DataMiner.Utils.Linux.Exceptions
{
	using System;
	using System.Runtime.Serialization;

	/// <summary>
	/// The exception that is thrown when a command is not returning any result.
	/// </summary>
	[Serializable]
	public class FileItemAlreadyExistsException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FileItemAlreadyExistsException"/> class.
		/// </summary>
		public FileItemAlreadyExistsException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FileItemAlreadyExistsException"/> class.
		/// </summary>
		/// <param name="path">The path.</param>
		public FileItemAlreadyExistsException(string path)
			: base($"The file item already exists: '{path}'")
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FileItemAlreadyExistsException"/> class with a reference to the inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
		public FileItemAlreadyExistsException(string path, Exception innerException)
			: base($"The file item already exists: '{path}'", innerException)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FileItemAlreadyExistsException"/> class with serialized data.
		/// </summary>
		/// <param name="info">The serialization info.</param>
		/// <param name="context">The streaming context.</param>
		/// <exception cref="ArgumentNullException">The <paramref name="info"/> parameter is <see langword="null" />.</exception>
		/// <exception cref="SerializationException">The class name is <see langword="null" /> or HResult is zero (0).</exception>
		/// <remarks>This constructor is called during deserialization to reconstitute the exception object transmitted over a stream.</remarks>
		protected FileItemAlreadyExistsException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}