using System;
using System.Collections.Generic;
using System.Text;

namespace XMRestAPIClient
{
    /// <summary>
    /// XMM odel.
    /// </summary>
    public interface IXMModel<T> where T:struct
    {
        T Id { get; set; }
    }
}
