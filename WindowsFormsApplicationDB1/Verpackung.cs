using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplicationDB1
{
    class Verpackung
    {
        private Int32 verpackungsid;
        private String verpackungsbez;

        public int Verpackungsid
        {
            get
            {
                return verpackungsid;
            }

            set
            {
                verpackungsid = value;
            }
        }

        public string Verpackungsbez
        {
            get
            {
                return verpackungsbez;
            }

            set
            {
                verpackungsbez = value;
            }
        }

        public override string ToString()
        {
            return String.Format("{0}", this.Verpackungsbez);
        }
    }
}
