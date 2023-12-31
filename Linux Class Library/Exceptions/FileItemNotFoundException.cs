﻿namespace Skyline.DataMiner.Utils.Linux.Exceptions
{
	using System;
	using System.Runtime.Serialization;

	/// <summary>
	/// The exception that is thrown when a command is not returning any result.
	/// </summary>
	[Serializable]
	public class FileItemNotFoundException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="FileItemNotFoundException"/> class.
		/// </summary>
		public FileItemNotFoundException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FileItemNotFoundException"/> class with the command that failed.
		/// </summary>
		/// <param name="command">The command.</param>
		public FileItemNotFoundException(string command)
			: base($"No such file or directory: {command}")
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FileItemNotFoundException"/> class with the command that failed and a reference to the inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="command">The command.</param>
		/// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
		public FileItemNotFoundException(string command, Exception innerException)
			: base($"No such file or directory: {command}", innerException)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FileItemNotFoundException"/> class with serialized data.
		/// </summary>
		/// <param name="info">The serialization info.</param>
		/// <param name="context">The streaming context.</param>
		/// <exception cref="ArgumentNullException">The <paramref name="info"/> parameter is <see langword="null" />.</exception>
		/// <exception cref="SerializationException">The class name is <see langword="null" /> or HResult is zero (0).</exception>
		/// <remarks>This constructor is called during deserialization to reconstitute the exception object transmitted over a stream.</remarks>
		protected FileItemNotFoundException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}