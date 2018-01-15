using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace WindowsFormsApplicationDB1
{
    public partial class FormInsert : Form
    {
        OleDbConnection con = null;
        Artikel a;
        DialogResult result = DialogResult.OK;
        public FormInsert()
        {
            InitializeComponent();
        }

        public FormInsert(Artikel a)
        {
            this.a = a;
        }

        public FormInsert(OleDbConnection con, Artikel artikel) : this()
        {
            this.con = con;
            this.a = artikel;
            fuelleCombobox();
        }
        public void fuelleCombobox()
        {
            OleDbCommand cmd = con.CreateCommand();
            cmd.CommandText = "Select * from tArtGruppe";

                OleDbDataReader reader = cmd.ExecuteReader();
                while(reader.Read())
                {
                    ArtikelGruppe gruppe = new ArtikelGruppe();
                    gruppe.Artikelgruppenid = reader.GetInt32(0);
                    gruppe.Gruppenbez = reader.GetString(1);
                    comboBoxArtikelgruppe.Items.Add(gruppe);
                }
                reader.Close();

            cmd.CommandText = "Select * from tVerpackung";
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                Verpackung ver = new Verpackung();
                ver.Verpackungsid = reader.GetInt32(0);
                ver.Verpackungsbez = reader.GetString(1);
                comboBoxVerpackung.Items.Add(ver);
            }
            reader.Close();
        }

        public DialogResult Result
        {
            get
            {
                return result;
            }

            set
            {
                result = value;
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.result = DialogResult.Cancel;
            this.Close();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            try
            {
                a.ArtikelNr = textBoxArtikelnr.Text;
                //a.ArtikelGruppe = Convert.ToInt32(comboBoxArtikelgruppe.SelectedIndex +1);
                a.ArtikelGruppe = ((ArtikelGruppe)comboBoxArtikelgruppe.SelectedItem).Artikelgruppenid;
                a.Bezeichnung = textBoxBezeichnung.Text;
                a.Bestand = Convert.ToInt16(textBoxBestand.Text);
                a.Meldebestand = Convert.ToInt16(textBoxMeldebestand.Text);
                //a.Verpackung = Convert.ToInt32(comboBoxVerpackung.SelectedIndex + 1);
                a.Verpackung = ((Verpackung)comboBoxVerpackung.SelectedItem).Verpackungsid;
                a.VkPreis = Convert.ToDecimal(textBoxvkPreis.Text);
                a.LetzteEntnahme = Convert.ToDateTime(dateTimePicker.Value);
            }
            catch (Exception)
            {
                a = null;
            }
            this.Close();
        }
    }
}
