using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActionTrack.Core
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string UserToken { get; set; }
        public string Department { get; set; }
    }
    public class MemberInfo
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string Rank { get; set; }
        public string BloodType { get; set; }
        public DateTime DOB { get; set; }
        public string EmergencyContact { get; set; }
        public string Status { get; set; }
    }
}
