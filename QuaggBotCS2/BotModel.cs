using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuaggBotCS2
{
    [Serializable]
    public class User
    {

        public int UserID { get; set; }

        public ulong UserSnow { get; set; }

        public string Name { get; set; }

        public string Discriminator { get; set; }

        public virtual Server Guild { get; set; }

        public int Strikes { get; set; }

        public int TotalMutes { get; set; }

        public bool Muted { get; set; }
    }

    [Serializable]
    public class Server
    {
        public int ServerID { get; set; }

        public ulong ServerSnow { get; set; }

        public string ServerName { get; set; }

        public virtual ICollection<User> Users { get; set; }

        public string SettingsJson { get; set; }
    }
}
