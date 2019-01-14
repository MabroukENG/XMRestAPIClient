using System;
using System.Collections.Generic;
using System.Text;

namespace XMRestAPIClient
{
    /// <summary>
    /// XMRestSettings.
    /// </summary>
    public class XMRestSettings
    {
        /// <summary>
        /// The authorization key header name. By default "Authorization".
        /// </summary>
        public static string AuthorizationKeyHeaderName = "Authorization";

        /// <summary>
        /// The authorization key header value. Empty by default.
        /// </summary>
        public static string AuthorizationKeyHeaderValue = string.Empty;

        /// <summary>
        /// The base URL. Default  "http://localhost:8080/"
        /// </summary>
        public static string BaseUrl = "http://localhost:8080/";

        /// <summary>
        /// The API version. By default is 1.
        /// </summary>
        public static int ApiVersion = 1;

    }
}
