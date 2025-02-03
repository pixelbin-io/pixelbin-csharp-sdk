using System;

namespace Pixelbin.Common.Exceptions
{
    [Obsolete("Use PDKInvalidCredentialError instead")]
    public class PixelbinInvalidCredentialError : Exception
    {
        public PixelbinInvalidCredentialError(string message = "Invalid Credentials") : base(message) { }
    }

    [Obsolete("Use PDKServerResponseError instead")]
    public class PixelbinServerResponseError : Exception
    {
        public int? StatusCode { get; }

        public PixelbinServerResponseError(string message = "", int? status_code = null) : base(message)
        {
            StatusCode = status_code;
        }
    }

    [Obsolete("Use PDKInvalidUrlError instead")]
    public class PixelbinInvalidUrlError : Exception
    {
        public PixelbinInvalidUrlError(string message = "") : base(message) { }
    }

    [Obsolete("Use PDKIllegalArgumentError instead")]
    public class PixelbinIllegalArgumentError : Exception
    {
        public PixelbinIllegalArgumentError(string message = "") : base(message) { }
    }

    [Obsolete("Use PDKIllegalQueryParameterError instead")]
    public class PixelbinIllegalQueryParameterError : Exception
    {
        public PixelbinIllegalQueryParameterError(string message = "") : base(message) { }
    }

    public class PDKInvalidCredentialError : Exception
    {
        public PDKInvalidCredentialError(string message = "Invalid Credentials") : base(message) { }
    }

    public class PDKServerResponseError : Exception
    {
        public PDKServerResponseError(string message = "") : base(message) { }
    }

    public class PDKInvalidUrlError : Exception
    {
        public PDKInvalidUrlError(string message = "") : base(message) { }
    }

    public class PDKIllegalArgumentError : Exception
    {
        public PDKIllegalArgumentError(string message = "") : base(message) { }
    }

    public class PDKIllegalQueryParameterError : Exception
    {
        public PDKIllegalQueryParameterError(string message = "") : base(message) { }
    }
}