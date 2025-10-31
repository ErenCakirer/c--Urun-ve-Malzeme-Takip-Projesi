using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace maliyet.test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
        SqlConnection baglanti = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=testmaliyet;Integrated Security=True");
        void malzemeliste()
        {
            SqlDataAdapter da = new SqlDataAdapter("Select *  From tblamalzemeler", baglanti);
            DataTable dt = new DataTable();
            da.Fill (dt);
            dataGridView1.DataSource = dt;


        }
        void Urunlistesi()
        {
            SqlDataAdapter da2 = new SqlDataAdapter("Select * From tblurunler", baglanti);
            DataTable dt2= new DataTable();
            da2.Fill (dt2);
            dataGridView1.DataSource = dt2;
        }
        void kasa()
        {
            SqlDataAdapter da3= new SqlDataAdapter("Select * From tblkasa",baglanti);
            DataTable dt3= new DataTable();
            da3.Fill (dt3);
            dataGridView1.DataSource = dt3;
        }
        void urunler()
        {
            baglanti.Open();
            SqlDataAdapter da = new SqlDataAdapter("Select * From tblurunler", baglanti);
            DataTable dt = new DataTable();
            da.Fill (dt);
            cmbürün.ValueMember = "UrunId";
            cmbürün.DisplayMember = "AD";
            cmbürün.DataSource= dt;
            baglanti.Close ();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
              malzemeliste();
            urunler();
            malzemeler();
        }
        void malzemeler()
        {
            baglanti.Open();
            SqlDataAdapter da = new SqlDataAdapter("Select * From tblamalzemeler", baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cmbmalzeme.ValueMember = "MalzemeID";
            cmbmalzeme.DisplayMember = "AD";
            cmbmalzeme.DataSource = dt;
            baglanti.Close();
        }

        private void btnürünL_Click(object sender, EventArgs e)
        {
            Urunlistesi();
        }

        private void btnmalzemeL_Click(object sender, EventArgs e)
        {
            malzemeliste ();
        }

        private void btnkasaL_Click(object sender, EventArgs e)
        {
            kasa();
        }

        private void btnmalekle_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("Insert Into tblamalzemeler(AD,STOK,FIYAT) values (@p1,@p2,@p3)", baglanti);
            komut.Parameters.AddWithValue("@p1",txtadm.Text);
            komut.Parameters.AddWithValue("@p2",decimal.Parse( txtstokm.Text));
            komut.Parameters.AddWithValue("@p3",decimal.Parse( txtfiyatm.Text));
          
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("MalzemE Eklendi");
            malzemeliste();
        }

        private void btnÜrünekle_Click(object sender, EventArgs e)
        {
            baglanti.Open ();
            SqlCommand komut = new SqlCommand("Insert Into tblurunler (AD) values (@p1)", baglanti);
            komut.Parameters.AddWithValue("@p1",txtADÜ.Text);
            komut.ExecuteNonQuery ();
            baglanti.Close();
            MessageBox.Show("Ürün Sisteme Eklendi");
            Urunlistesi();

        }

        private void txtürünoluştur_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("Insert Into tblfırın ( URUNID,MalzemeID,Mıktar,Malıyet) values(@p1,@p2,@p3,@p4)", baglanti);
            komut.Parameters.AddWithValue("@p1", cmbürün.SelectedValue);
            komut.Parameters.AddWithValue("@p2", cmbmalzeme.SelectedValue);
            komut.Parameters.AddWithValue("@p3",  decimal.Parse(txtmiktar.Text));
            komut.Parameters.AddWithValue("@p4", decimal.Parse(txtmaliyet.Text));
            komut.ExecuteNonQuery () ;
            baglanti.Close();
            MessageBox.Show("Malzeme Eklendi");
            listBox1.Items.Add(cmbmalzeme.Text + "-" + txtmaliyet.Text);
        }

        private void txtmiktar_TextChanged(object sender, EventArgs e)
        {
            double maliyet;
            if (txtmiktar.Text == "") ;
            {
               
            }
            baglanti.Open();
            SqlCommand komut = new SqlCommand("Select * From tblamalzemeler where MalzemeID=@p1", baglanti);
            komut.Parameters.AddWithValue("@p1",cmbmalzeme.SelectedValue);
            SqlDataReader dr= komut.ExecuteReader();
            while(dr.Read())
            {
                txtmaliyet. Text = dr[3].ToString();

            }
            baglanti.Close();
            maliyet = Convert.ToDouble(txtmaliyet.Text) / 1000 * Convert.ToDouble(txtmiktar.Text);
            txtmaliyet.Text=maliyet.ToString();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView1.SelectedCells[0].RowIndex;
            txtIDÜ.Text = dataGridView1.Rows[secilen].Cells[0].Value.ToString();
            txtADÜ.Text = dataGridView1.Rows[secilen].Cells[1].Value.ToString();
            baglanti.Open();
            SqlCommand komut = new SqlCommand("Select sum(maliyet) From tblfırın where urunıd=@p1");
            komut.Parameters.AddWithValue("@p1", txtIDÜ.Text);
            SqlDataReader dr = komut.ExecuteReader();
            while (dr.Read())
            {
                txtMALÜ.Text = dr[0].ToString();
            }
            baglanti.Close();
        }
    }
}
