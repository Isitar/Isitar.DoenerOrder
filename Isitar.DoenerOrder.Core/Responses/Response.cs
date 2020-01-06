using System.Collections.Generic;

namespace Isitar.DoenerOrder.Core.Responses
{
    public abstract class Response
    {
        public virtual bool Success { get; set; }

        public virtual IDictionary<string, IList<string>> ErrorMessages { get; set; }
    }
    public abstract class Response<T> : Response
    {
        public virtual T Data { get; set; }
    }
}