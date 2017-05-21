using Microsoft.AspNet.Identity.EntityFramework;
using Sipp.Data.Entity.CoreIdentity;
using Sipp.Data.Entity.Organization;
using Sipp.Data.Entity.Payment;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sipp.Core.Data.Infrastructure
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Company> Company { get; set; }
        public DbSet<CompanyHistory> CompanyHistory { get; set; }
        public DbSet<RegularPayment> RegularPayment { get; set; }
        public DbSet<PaymentHistory> PaymentHistory { get; set; }
        

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Configuration.ProxyCreationEnabled = false;
            //Configuration.LazyLoadingEnabled = false;
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
