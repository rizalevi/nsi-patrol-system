using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NSI.Domain.Models.Auth
{
    public class ApiResponse<T>
    {
        public int Code { get; set; }
        public bool Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
    }

    public class MessageModel
    {
        public const string InvalidToken = "Invalid Token";
    }
}
