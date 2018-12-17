using System;
using System.Collections.Generic;
using System.Text;

namespace XMRestAPIClient
{
    public enum XMRestResultType : long
    {
        /// <summary>
        /// Success
        /// </summary>
        Success = 1,
        /// <summary>
        /// Error
        /// </summary>
        Error = 2,
        /// <summary>
        /// None
        /// </summary>
        None = 0,
    }
    public class XMRestResult
    {
        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>
        /// The result.
        /// </value>
        public XMRestResultType Type { get; set; } = XMRestResultType.None;

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        /// <value>
        /// The data.
        /// </value>
        public string JsonData { get; set; }
    }
}
