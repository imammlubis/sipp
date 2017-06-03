using System.Collections.Generic;

namespace Sipp.Web.Utils
{
    public class EmailContent
    {
        public string Body { get; set; }
        public List<string> Destination { get; set; }
        public List<string> CC { get; set; }
        public string Subject { get; set; }
    }
}