using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplePhonebook.Data.Models
{
    public class Group
    {
        [Key]
        public long GroupId { get; set; }
        public string GroupName { get; set; }

        [ForeignKey("GroupId")]
        public List<GroupMember> GroupMembers { get; set; }
    }
}
