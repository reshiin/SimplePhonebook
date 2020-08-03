using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplePhonebook.Data.Models
{
    public class GroupMember
    {
        [Key]
        public long GroupMemberId { get; set; }
        public long ProfileId { get; set; }
        public long GroupId { get; set; }
    }
}
