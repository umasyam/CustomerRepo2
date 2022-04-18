using System;
using System.Runtime.Serialization;

namespace CustomerAPI.Filters
{
    /// <summary>
    /// Record Not Found Exception.
    /// </summary>
    [Serializable]
    public class RecordNotFoundException : Exception
    {
        /// <summary>
        ///  Record Not Found Exception.
        /// </summary>
        protected RecordNotFoundException()
        {
        }

        /// <summary>
        /// Record Not Found Exception.
        /// </summary>
        /// <param name="message">Message.</param>
        protected RecordNotFoundException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///  Record Not Found Exception.
        /// </summary>
        /// <param name="info">Serialization Info.</param>
        /// <param name="context">Streaming Context.</param>
        protected RecordNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}