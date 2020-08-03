using SimplePhonebook.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimplePhonebook.Models
{
    public class GroupViewModel
    {
        public List<GroupSummary> Groups { get; set; }
        public Group Group { get; set; }
    }

    public class GroupSummary
    {
        public Group Group { get; set; }
        public long MembersCount { get; set; }
    }
}