using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMI.Infrastructure
{
    public class ApiLog
    {
        public string CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string Application { get; set; }             // The name of application that is being called.
        public long ApiUserId { get; set; }                    // The user that made the request.
        public string Machine { get; set; }                 // The machine that made the request.
        public string RequestIpAddress { get; set; }        // The IP address that made the request.
        public string RequestContentType { get; set; }      // The request content type.
        public string RequestContentBody { get; set; }      // The request content body.
        public string RequestUri { get; set; }              // The request URI.
        public string RequestMethod { get; set; }           // The request method (GET, POST, etc).
        public string RequestRouteTemplate { get; set; }    // The request route template.
        public string RequestRouteData { get; set; }        // The request route data.
        public string RequestHeaders { get; set; }          // The request headers.
        public DateTime? RequestTimestamp { get; set; }     // The request timestamp.
        public string ResponseContentType { get; set; }     // The response content type.
        public string ResponseContentBody { get; set; }     // The response content body.
        public int? ResponseStatusCode { get; set; }        // The response status code.
        public string ResponseHeaders { get; set; }         // The response headers.
        public DateTime? ResponseTimestamp { get; set; }    // The response timestamp.
    }
}
