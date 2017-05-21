using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Sipp.Data.Entity.CoreIdentity
{
    public class ApplicationUser : IdentityUser
    {

        [MaxLength(100)]
        public string FirstName { get; set; }
        [MaxLength(100)]
        public string LastName { get; set; }
        public string ProfilePic { get; set; }
        public string ServiceName { get; set; }
        public string ServiceTitle { get; set; }
        public string ServiceDescription { get; set; }
        public string ServiceRemark { get; set; }
        public string ServiceProfilePic { get; set; }
        public string ServicePhoneNumber { get; set; }

        public Nullable<bool> IsFoodServiceAvailable { get; set; }

        public string Address { get; set; }
        public string Street { get; set; }
        public string Province { get; set; }

        public string City { get; set; }
        public string District { get; set; }
        public string PostalCode { get; set; }
        
        public System.Data.Entity.Spatial.DbGeography AddressGeoPosition { get; set; }

        /* var @bankName = new SqlParameter("@BankName", p.BankName);
            var @bankAccount = new SqlParameter("@BankAccount", p.BankAccount);
            var @bankAccountName = new SqlParameter("@bankAccountName", p.bankAccountName);
            var @ownerName = new SqlParameter("@OwnerName", p.OwnerName);
*/
        //public string BankName { get; set; }
        //public string BankAccount { get; set; }
        //public string BankAccountName { get; set; }
        //public string OwnerName { get; set; }

        //public Nullable<bool> IsMarchandized { get; set; }

        public Nullable<DateTime> CreateDate { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);

            return userIdentity;
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}
