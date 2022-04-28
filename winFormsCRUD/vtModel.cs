using System;
using System.Data.Entity;
using System.Linq;

namespace winFormsCRUD
{
    public class vtModel : DbContext
    {       
        public vtModel()
            : base("name=vtModel")
        {
        }

        public virtual DbSet<personel> Personeller { get; set; }
        public virtual DbSet<grup> Gruplar { get; set; }
    }
}   