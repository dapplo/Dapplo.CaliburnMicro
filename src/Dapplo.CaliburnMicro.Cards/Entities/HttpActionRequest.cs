using System.Collections.Generic;
using Dapplo.HttpExtensions.Support;

namespace Dapplo.CaliburnMicro.Cards.Entities
{
    /// <summary>
    /// A container class to post the HttpAction information
    /// </summary>
    [HttpRequest]
    public class HttpActionRequest
    {
        /// <summary>
        /// Headers for the request
        /// </summary>
        [HttpPart(HttpParts.RequestHeaders)]
        public IDictionary<string, string> Headers { get; } = new Dictionary<string, string>();

        /// <summary>
        /// Body of the HttpAction
        /// </summary>
        [HttpPart(HttpParts.RequestContent)]
        public string Body { get; set; }
    }
}
