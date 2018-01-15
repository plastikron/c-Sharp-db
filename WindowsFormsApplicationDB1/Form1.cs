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
using System.Globalization;

namespace WindowsFormsApplicationDB1
{
    public partial class Form1 : Form
    {
        OleDbConnection con = null;
        OleDbCommand cmd = null;
        OleDbDataReader reader = null;
        List<Artikel> artikelList = null;
        public Form1()
        {
            InitializeComponent();
            artikelList = new List<Artikel>();
        }

        private void buttonConnection_Click(object sender, EventArgs e)
        {
            //Provider = Microsoft.ACE.OLEDB.12.0; 
            //Data Source = Bestellung.accdb;
            con = new OleDbConnection();
            //OleDbConnectionStringBuilder builder = new OleDbConnectionStringBuilder();
            //builder.Provider = "Microsoft.ACE.OLEDB.12.0";
            //builder.DataSource = "Bestellung.accdb";
            //con.ConnectionString = builder.ConnectionString;
            con.ConnectionString = Properties.Settings.Default.DbCon;

            try
            {
                con.Open();
                toolStripStatusLabel1.Text = "Verbindung erfolgreich";
            }
            catch (InvalidOperationException Inv)
            {
                MessageBox.Show("Verbindung bereits geöffnet");
            }
            catch (OleDbException ole)
            {
                MessageBox.Show(ole.Message);
            }
        }

        private void buttonCommand_Click(object sender, EventArgs e)
        {
            if(con.State != ConnectionState.Open)
            {
                con.Open();
            }
            cmd = con.CreateCommand();
            cmd.CommandText = "Select * from tArtikel;";
            //cmd = new OleDbCommand("Select * from tArtikel;", con);
            try
            {
                reader = cmd.ExecuteReader();
                toolStripStatusLabel1.Text = "Kommando erfolgreich abgesetzt";
            }
            catch (Exception)
            {

                MessageBox.Show("Zugriff auf Artikel nicht möglich");
            }
        }

        private void buttonReader_Click(object sender, EventArgs e)
        {
            while(reader.Read() == true)
            {
                //String bez = reader.GetString(3);
                Artikel a = mkArtikelObject(reader);
                artikelList.Add(a);
                //listBoxAusgabe.Items.Add(a);
                //listBoxAusgabe.Items.Add(reader["ArtikelNr"].ToString() + ": " + reader["Bezeichnung"]);
            }
            listBoxAusgabe.DataSource = artikelList;
            reader.Close();
        }

        private Artikel mkArtikelObject(OleDbDataReader reader)
        {
            Artikel a = new Artikel();
            try
            {
                int i = 0;
                // Fehler beim Umwandeln!!
                a.ArtikelOid = Convert.ToInt32(checkDBNull(reader[i++]));
                a.ArtikelNr = Convert.ToString(checkDBNull(reader[i++]));
                a.ArtikelGruppe = Convert.ToInt32(checkDBNull(reader[i++]));
                a.Bezeichnung = Convert.ToString(checkDBNull(reader[i++]));
                a.Bestand = Convert.ToInt16(checkDBNull(reader[i++]));
                a.Meldebestand = Convert.ToInt16(checkDBNull(reader[i++]));
                a.Verpackung = Convert.ToInt32(checkDBNull(reader[i++]));
                a.VkPreis = Convert.ToDecimal(checkDBNull(reader[i++]));
                a.LetzteEntnahme = Convert.ToDateTime(checkDBNull(reader[i++]));
            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message);
            }

            return a;
        }

        // Hilfsmethode zum abprüfen von "null"-Werten
        private Object checkDBNull(Object o)
        {
            if (o == DBNull.Value) return null;
            else return o;
        }

        private void buttonchange_Click(object sender, EventArgs e)
        {
            if (listBoxAusgabe.SelectedItem != null)
            {
                Artikel a = (Artikel) listBoxAusgabe.SelectedItem;
                a.onUpdateError = Meldebestand;
                FormUpdate frmUpdate = new FormUpdate(a);
                frmUpdate.ShowDialog();
                // modales Fenster 
                // ( hat den Fokus, man muss dieses Fenster erst schließen, danach kann man in anderen Fenster weiterarbeiten )
                if(frmUpdate.Result == DialogResult.OK)
                {

                    updateArtikel(a);
                    listBoxAusgabe.DataSource = null;
                    listBoxAusgabe.DataSource = artikelList;
                }
                else
                {
                    toolStripStatusLabel1.Text = ("Bearbeitung wurde abgebrochen");
                }
            }

        }
        private void updateArtikel(Artikel a)
        {
            //TODO: Command-Objekt
            OleDbCommand cmd = con.CreateCommand();
            //TODO: Parameter generieren
            cmd.Parameters.AddWithValue("ANR", a.ArtikelNr);
            cmd.Parameters.AddWithValue("BEZ", a.Bezeichnung);
            cmd.Parameters.AddWithValue("BEST", a.Bestand);
            cmd.Parameters.AddWithValue("MBEST", a.Meldebestand);
            cmd.Parameters.AddWithValue("VKP", a.VkPreis.ToString(new CultureInfo("de-DE")));
            cmd.Parameters.AddWithValue("ENT", a.LetzteEntnahme);
            //TODO: Commandtext: SQL
            String sql = "UPDATE tArtikel SET ArtikelNR = ANR, Bezeichnung = BEZ, Bestand = BEST, "; 
            sql+= "Meldebestand = MBEST, VkPreis = VKP, letzteEntnahme = ENT ";
            sql+= "WHERE ArtikelOid =" + a.ArtikelOid.ToString();
            cmd.CommandText = sql;
            //TODO: Conn open
            // Verbindung noch geöffnet, deshalb erneute öffnung nicht notwendig
            //con.Open();
            //TODO: Command ausführen
            try
            {
                cmd.ExecuteNonQuery();
                toolStripStatusLabel1.Text = "Update erfolgreich";
            }
            catch (Exception exc)
            {
                MessageBox.Show("Fehler beim Update");
                toolStripStatusLabel1.Text = exc.Message;
            }
        }

        private void buttonNeuerDatensatz_Click(object sender, EventArgs e)
        {
            Artikel a = new Artikel();
            FormInsert frmInsert = new FormInsert(con,a);
            frmInsert.ShowDialog();
            if (a != null)
            {
                insertArtikel(a);
            }
        }

        private void insertArtikel(Artikel a)
        {
            //TODO: Command-Objekt
            OleDbCommand cmd = con.CreateCommand();
            //TODO: Parameter generieren
            cmd.Parameters.AddWithValue("ANR", a.ArtikelNr);
            cmd.Parameters.AddWithValue("AGR", a.ArtikelGruppe);
            cmd.Parameters.AddWithValue("BEZ", a.Bezeichnung);
            cmd.Parameters.AddWithValue("BEST", a.Bestand);
            cmd.Parameters.AddWithValue("MBEST", a.Meldebestand);
            cmd.Parameters.AddWithValue("VPA", a.Verpackung);
            string vkp = a.VkPreis.ToString();
            cmd.Parameters.AddWithValue("VKP", a.VkPreis.ToString(new CultureInfo("de-DE")));
            cmd.Parameters.AddWithValue("ENT", a.LetzteEntnahme);
            //TODO: Commandtext: SQL
            String sql = "INSERT INTO tartikel (ArtikelNr, ArtikelGruppe, Bezeichnung, Bestand, Meldebestand,";
            sql += " Verpackung, VkPreis, letzteEntnahme) Values(ANR, AGR, BEZ, BEST, MBEST, VPA, VKP, ENT)";
            cmd.CommandText = sql;

            try
            {
                cmd.ExecuteNonQuery();
                toolStripStatusLabel1.Text = "Insert erfolgreich";
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                toolStripStatusLabel1.Text = exc.Message;
            }
        }

        private void buttonBezirk_Click(object sender, EventArgs e)
        {
            OleDbCommand cmd = con.CreateCommand();
            cmd.CommandText = "INSERT INTO tBezirk(Bezirk) Values('" + textBoxBezirk.Text + "')";
            cmd.CommandType = CommandType.Text;
            cmd.ExecuteNonQuery();

            cmd.CommandText = "SELECT @@IDENTITY FROM tBezirk";
            Int32 id = Convert.ToInt32(cmd.ExecuteScalar());
            MessageBox.Show("Satz wurde mit ID = " + id.ToString() +" eingefügt");
        }
        private void Meldebestand(String message)
        {
            MessageBox.Show(message);
        }
    }
}