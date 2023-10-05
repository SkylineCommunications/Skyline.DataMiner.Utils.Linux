﻿namespace Skyline.DataMiner.Utils.Linux.Exceptions
{
	using System;
	using System.Runtime.Serialization;

	/// <summary>
	/// The exception that is thrown when a command is returning an error.
	/// </summary>
	[Serializable]
	public class ErrorResultException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ErrorResultException"/> class.
		/// </summary>
		public ErrorResultException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ErrorResultException"/> class with the command that failed.
		/// </summary>
		/// <param name="command">The command that fialed.</param>
		/// <param name="error">The error that was returned.</param>
		public ErrorResultException(string command, string error)
			: base($"The command '{command}' returned an error: {error}.")
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ErrorResultException"/> class with the command that failed and a reference to the inner exception that is the cause of this exception.
		/// </summary>
		/// <param name="command">The command that fialed.</param>
		/// <param name="error">The error that was returned.</param>
		/// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
		public ErrorResultException(string command, string error, Exception innerException)
			: base($"The command '{command}' returned an error: {error}.", innerException)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ErrorResultException"/> class with serialized data.
		/// </summary>
		/// <param name="info">The serialization info.</param>
		/// <param name="context">The streaming context.</param>
		/// <exception cref="ArgumentNullException">The <paramref name="info"/> parameter is <see langword="null" />.</exception>
		/// <exception cref="SerializationException">The class name is <see langword="null" /> or HResult is zero (0).</exception>
		/// <remarks>This constructor is called during deserialization to reconstitute the exception object transmitted over a stream.</remarks>
		protected ErrorResultException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}