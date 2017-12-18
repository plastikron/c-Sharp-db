using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplicationDB1
{
    public partial class FormUpdate : Form
    {
        Artikel selArtikel;
        DialogResult result = DialogResult.OK;

        public DialogResult Result
        {
            get
            {
                return result;
            }

            private set
            {
                result = value;
            }
        }

        public Artikel SelArtikel
        {
            get
            {
                return selArtikel;
            }

            set
            {
                selArtikel = value;
            }
        }

        public FormUpdate()
        {
            InitializeComponent();
        }

        public FormUpdate(Artikel artikel) : this()
        {
            SelArtikel = artikel;
            this.textBoxArtikelOid.Text = SelArtikel.ArtikelOid.ToString();
            this.textBoxArtikelnr.Text = SelArtikel.ArtikelNr;
            this.textBoxArtikelgr.Text = SelArtikel.ArtikelGruppe.ToString();
            this.textBoxBezeichnung.Text = SelArtikel.Bezeichnung;
            this.textBoxBestand.Text = SelArtikel.Bestand.ToString();
            this.textBoxMeldebestand.Text = SelArtikel.Meldebestand.ToString();
            this.textBoxVerpackung.Text = SelArtikel.Verpackung.ToString();
            this.textBoxvkPreis.Text = SelArtikel.VkPreis.ToString();
            this.textBoxletzteEntnahme.Text = SelArtikel.LetzteEntnahme.ToShortDateString();
        }
        private void FormUpdate2_Load(object sender, EventArgs e)
        {

        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Result = DialogResult.Cancel;
            this.Close();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            SelArtikel.ArtikelNr = textBoxArtikelnr.Text;
            SelArtikel.Bezeichnung = textBoxBezeichnung.Text;
            SelArtikel.Bestand = Convert.ToInt16(textBoxBestand.Text);
            SelArtikel.Meldebestand = Convert.ToInt16(textBoxMeldebestand.Text);
            SelArtikel.VkPreis = Convert.ToDecimal(textBoxvkPreis.Text);
            SelArtikel.LetzteEntnahme = Convert.ToDateTime(textBoxletzteEntnahme.Text);
            this.Close();
        }
    }
}
