using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplePhonebook.Data.Models
{
    public class Profile
    {
        [Key]
        public long Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string HomeAddress { get; set; }

        public byte[] ProfilePicture { get; set; }

        [ForeignKey("ProfileId")]
        public ICollection<GroupMember> GroupMembers { get; set; }

        [ForeignKey("ProfileId")]
        public ICollection<ContactNumber> ContactNumbers { get; set; }
    }
}
