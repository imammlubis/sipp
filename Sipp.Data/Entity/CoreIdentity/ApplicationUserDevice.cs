using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sipp.Data.Entity.CoreIdentity
{
    public class ApplicationUserDevice
    {
        [Key]
        public string ID { get; set; }
        public string DeviceID { get; set; }
        public string UniqueID { get; set; }
        public string InstanceID { get; set; }
        public Nullable<int> VersionApp { get; set; }
        public string OS { get; set; }
        [ForeignKey("AppUser")]
        public string UserID { get; set; }
        public virtual ApplicationUser AppUser { get; set; }
    }
}
