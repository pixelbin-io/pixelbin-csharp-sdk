using System;

namespace Pixelbin.Common.Exceptions
{
    [Obsolete("Use PDKInvalidCredentialError instead", true)]
    public class PixelbinInvalidCredentialError : Exception
    {
        public PixelbinInvalidCredentialError(string message = "Invalid Credentials") : base(message) { }
    }

    [Obsolete("Use PDKServerResponseError instead", true)]
    public class PixelbinServerResponseError : Exception
    {
        public int? StatusCode { get; }

        public PixelbinServerResponseError(string message = "", int? status_code = null) : base(message)
        {
            StatusCode = status_code;
        }
    }

    [Obsolete("Use PDKInvalidUrlError instead", true)]
    public class PixelbinInvalidUrlError : Exception
    {
        public PixelbinInvalidUrlError(string message = "") : base(message) { }
    }

    [Obsolete("Use PDKIllegalArgumentError instead", true)]
    public class PixelbinIllegalArgumentError : Exception
    {
        public PixelbinIllegalArgumentError(string message = "") : base(message) { }
    }

    [Obsolete("Use PDKIllegalQueryParameterError instead", true)]
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
        public int? StatusCode { get; }

        public PDKServerResponseError(string message = "", int? status_code = null) : base(message)
        {
            StatusCode = status_code;
        }
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