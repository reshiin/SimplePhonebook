using SimplePhonebook.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimplePhonebook.Models
{
    public class ProfileViewModel
    {
        public Profile Profile { get; set; }
        public List<Profile> Profiles { get; set; }
    }
}