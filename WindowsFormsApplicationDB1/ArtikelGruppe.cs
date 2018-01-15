using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplicationDB1
{
    class ArtikelGruppe
    {
        private Int32 artikelgruppenid;
        private String gruppenbez;

        public int Artikelgruppenid
        {
            get
            {
                return artikelgruppenid;
            }

            set
            {
                artikelgruppenid = value;
            }
        }

        public string Gruppenbez
        {
            get
            {
                return gruppenbez;
            }

            set
            {
                gruppenbez = value;
            }
        }

        public override string ToString()
        {
            return String.Format("{0}", this.Gruppenbez);
        }
    }
}
