using System;

namespace Pixelbin.Common.Exceptions
{
    public class PixelbinInvalidCredentialError : Exception
    {
        public PixelbinInvalidCredentialError(string message = "Invalid Credentials") : base(message) { }
    }

    public class PixelbinServerResponseError : Exception
    {
        public int? StatusCode { get; }

        public PixelbinServerResponseError(string message = "", int? status_code = null) : base(message)
        {
            StatusCode = status_code;
        }
    }

    public class PixelbinInvalidUrlError : Exception
    {
        public PixelbinInvalidUrlError(string message = "") : base(message) { }
    }

    public class PixelbinIllegalArgumentError : Exception
    {
        public PixelbinIllegalArgumentError(string message = "") : base(message) { }
    }

    public class PixelbinIllegalQueryParameterError : Exception
    {
        public PixelbinIllegalQueryParameterError(string message = "") : base(message) { }
    }
}