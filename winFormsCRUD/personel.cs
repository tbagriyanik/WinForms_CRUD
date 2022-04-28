using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace winFormsCRUD
{
    public class personel
    {
        public int Id { get; set; }     //prop TAB TAB
        public string isim { get; set; }

        public int grupID { get; set; }
        public virtual grup personelGrubu { get; set; }
    }
}
