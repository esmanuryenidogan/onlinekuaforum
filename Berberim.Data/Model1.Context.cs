﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Berberim.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class BerberimEntities : DbContext
    {
        public BerberimEntities()
            : base("name=BerberimEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ADMIN> ADMIN { get; set; }
        public virtual DbSet<BSACMODEL> BSACMODEL { get; set; }
        public virtual DbSet<ISLEM> ISLEM { get; set; }
        public virtual DbSet<KAMPANYA> KAMPANYA { get; set; }
        public virtual DbSet<MAIL> MAIL { get; set; }
        public virtual DbSet<MUSTERI> MUSTERI { get; set; }
        public virtual DbSet<RANDEVU> RANDEVU { get; set; }
        public virtual DbSet<SALONFOTO> SALONFOTO { get; set; }
        public virtual DbSet<SALONSAYFA> SALONSAYFA { get; set; }
        public virtual DbSet<TRENDHAIRS> TRENDHAIRS { get; set; }
        public virtual DbSet<YORUM> YORUM { get; set; }
        public virtual DbSet<PERSONEL> PERSONEL { get; set; }
        public virtual DbSet<SALON> SALON { get; set; }
    }
}
