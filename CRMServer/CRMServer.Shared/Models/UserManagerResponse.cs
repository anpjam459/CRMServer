using System;
using System.Collections.Generic;
using System.Text;

namespace CRMServer.Shared.Models
{
    public class UserManagerResponse
    {
        public string Message { set; get; }
        public bool IsSusscess { set; get; }
        public IEnumerable<string> Errors { set; get; }
        public DateTime ExpireDate { set; get; }
    }
}
