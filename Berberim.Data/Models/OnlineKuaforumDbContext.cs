using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Berberim.Data.Migrations;

namespace Berberim.Data.Models
{
    public class OnlineKuaforumDbContext : DbContext

    {
        public OnlineKuaforumDbContext() : base("name=myConnectionString")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<OnlineKuaforumDbContext, Configuration>("myConnectionString"));
        }

        public DbSet<ADMIN> ADMIN { get; set; }
        public DbSet<BSACMODEL> BSACMODEL { get; set; }
        public DbSet<ISLEM> ISLEM { get; set; }
        public DbSet<KAMPANYA> KAMPANYA { get; set; }
        public DbSet<MAIL> MAIL { get; set; }
        public DbSet<MUSTERI> MUSTERİ { get; set; }
        public DbSet<PERSONEL> PERSONEL { get; set; }
        public DbSet<RANDEVU> RANDEVU { get; set; }
        public DbSet<SALON> SALON { get; set; }
        public DbSet<SALONFOTO> SALONFOTO { get; set; }
        public DbSet<SALONSAYFA> SALONSAYFA { get; set; }
        public DbSet<TRENDHAIR> TRENDHAIRS { get; set; }
        public DbSet<YORUM> YORUM { get; set; }
        public DbSet<SALONLOGO> SALONLOGO { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
        }
    }
}
