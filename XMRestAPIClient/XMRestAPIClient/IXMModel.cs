using System;
using System.Collections.Generic;
using System.Text;

namespace XMRestAPIClient
{
    public interface IXMModel<T> where T:struct
    {
        T Identifier { get; set; }
    }
}
