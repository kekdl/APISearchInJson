using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APISearchInJson.Model
{
    public class Search
    {
        public string Root { get; set; }
        public List<Content> Contents { get; set; }

    }

        public class Content
    {
        public string NameField { get; set; }
        public string ContentField { get; set; }

    }


}