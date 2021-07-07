using System;
using System.Runtime.Serialization;

namespace TeaTimer
{
    [Serializable]
    internal class TeaModelException : Exception
    {
        public TeaModelException()
        {
        }

        public TeaModelException(string message) : base(message)
        {
        }

        public TeaModelException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TeaModelException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}