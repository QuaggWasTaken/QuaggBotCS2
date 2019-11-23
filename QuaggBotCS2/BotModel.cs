using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuaggBotCS2
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }

        public long __UserSnow { get; set; }

        [NotMapped]
        public ulong UserSnow
        {
            get
            {
                unchecked
                {
                    return (ulong)__UserSnow;
                }
            }

            set
            {
                unchecked
                {
                    __UserSnow = (long)value;
                }
            }
        }

        public string Name { get; set; }

        public string Discriminator { get; set; }

        public virtual Server Guild { get; set; }

        public int Strikes { get; set; }

        public int TotalMutes { get; set; }

        public bool Muted { get; set; }
    }

    public class Server
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ServerID { get; set; }

        public long __ServerSnow { get; set; }

        [NotMapped]
        public ulong ServerSnow
        {
            get
            {
                unchecked
                {
                    return (ulong)__ServerSnow;
                }
            }

            set
            {
                unchecked
                {
                    __ServerSnow = (long)value;
                }
            }
        }

        public string ServerName { get; set; }

        public virtual ICollection<User> Users { get; set; }

        public string SettingsJson { get; set; }
    }
}
