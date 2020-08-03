using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimplePhonebook.Data.Models
{
    public class ContactNumber
    {
        [Key]
        public long ContactId { get; set; }
        public long ProfileId { get; set; }
        public string Type { get; set; }
        public string Number { get; set; }
    }
}
