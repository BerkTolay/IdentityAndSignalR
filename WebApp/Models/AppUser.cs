using System;
using Microsoft.AspNetCore.Identity;

namespace WebApp.Models
{
    public class AppUser:IdentityUser
    {
        public string FirstName { get; set; }
        public string Lastname { get; set; }

        public DateTime DateTime { get; set; }
        public int StopWatch { get; set; }
    }
}