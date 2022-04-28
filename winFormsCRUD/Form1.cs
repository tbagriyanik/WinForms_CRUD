using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace winFormsCRUD
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        vtModel _vt = new vtModel();

        private void Form1_Load(object sender, EventArgs e)
        {
            groupBox2.Visible = false; //personel ekle 
            groupBox3.Visible = false; //personel güncelle
            veriYukle();
            comboBox3.SelectedIndex = 0;
            toolStripStatusLabel1.Text = "Hoş geldiniz...";
        }

        private void veriYukle()
        {
            _vt.Gruplar.Load();
            _vt.Personeller.Load();

            listBox1.DataSource = _vt.Gruplar.ToList();  //1 alana bağlı liste
            listBox1.DisplayMember = "grupAdi";         //görünürde ismi gelir
            listBox1.ValueMember = "Id";                //seçildiğinde kimlik gelir

            comboBox1.DataSource = _vt.Gruplar.ToList();
            comboBox1.DisplayMember = "grupAdi";
            comboBox1.ValueMember = "Id";
            comboBox2.DataSource = _vt.Gruplar.ToList();
            comboBox2.DisplayMember = "grupAdi";
            comboBox2.ValueMember = "Id";

            var data = _vt.Personeller.Join(
                           _vt.Gruplar,
                           a => a.grupID,
                           b => b.Id,
                           (a, b) => new
                           {
                               Kimlik = a.Id,
                               Ad = a.isim,
                               GrupAdi = b.grupAdi,
                               GrupID = b.Id
                           }
                       ).ToList();

            dataGridView1.DataSource = data;
            dataGridView1.Refresh();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //formdakileri sıfırla
            textBox3.Text = "";
            comboBox3.SelectedIndex = 0;
            groupBox2.Visible = false;
            groupBox3.Visible = false;

            //değişiklikleri kaydet
            _vt.SaveChanges();
            veriYukle();

            toolStripStatusLabel1.Text = "Tazeleme yapıldı, " + _vt.Personeller.Count().ToString() + " kişi vardır.";
        }

        private void listBox1_Click(object sender, EventArgs e)
        {
            var data = _vt.Personeller.Join(
                          _vt.Gruplar,
                          a => a.grupID,
                          b => b.Id,
                          (a, b) => new
                          {
                              Kimlik = a.Id,
                              Ad = a.isim,
                              GrupAdi = b.grupAdi,
                              GrupID = b.Id
                          }
                      ).Where(c => c.GrupID == (int)listBox1.SelectedValue).ToList();

            dataGridView1.DataSource = data;
            //dataGridView1.Columns["GrupID"].Visible = false; //fazla sütun var
            dataGridView1.Refresh();

            if (data.Count > 0)
            {
                toolStripStatusLabel1.Text = data[0].GrupAdi + " grubu seçildi, " + data.Count.ToString() + " kişi bulundu";
            }
            else
            {
                toolStripStatusLabel1.Text = "Seçili grupta 0 kişi vardır.";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //tek personel silme - onay yani sormadan siliyor
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dataGridView1.SelectedRows[0];
                int silinecekID = (int)row.Cells[0].Value;
                string silinecekAd = row.Cells[1].Value.ToString();
                var silinecek = _vt.Personeller.Where(w => w.Id == silinecekID).FirstOrDefault();

                _vt.Personeller.Remove(silinecek);
                _vt.SaveChanges();
                veriYukle();

                toolStripStatusLabel1.Text = silinecekAd + " personeli silindi";
            }
            else
                MessageBox.Show("Personel seçilerek silinebilir");

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //yeni personel
            groupBox2.Visible = true;
            groupBox3.Visible = false;
            textBox1.Text = "";
            comboBox1.SelectedIndex = -1;
            textBox1.Focus();

            toolStripStatusLabel1.Text = "Yeni personel ekleniyor";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //personel ekle tamam
            if (textBox1.Text != "" && comboBox1.SelectedIndex > -1)
            {
                personel _p = new personel();
                _p.isim = textBox1.Text;
                _p.grupID = (int)comboBox1.SelectedValue;

                _vt.Personeller.Add(_p);
                _vt.SaveChanges();
                veriYukle();
                groupBox2.Visible = false;

                toolStripStatusLabel1.Text = textBox1.Text + " personeli eklendi";
            }
            else
            {
                MessageBox.Show("İsim veya grup bilgisi boş geçildi...");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //yeni personel vazgeç
            groupBox2.Visible = false;

            toolStripStatusLabel1.Text = "Yeni personel eklenmedi";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //personel güncelleme
            if (dataGridView1.SelectedRows.Count > 0)
            {
                groupBox3.Visible = true;
                groupBox2.Visible = false;
                textBox2.Text = dataGridView1.SelectedCells[1].Value.ToString();
                comboBox2.SelectedValue = (int)dataGridView1.SelectedCells[3].Value;
                textBox2.Focus();

                toolStripStatusLabel1.Text = textBox2.Text + " personeli güncelleniyor";
            }
            else
                MessageBox.Show("Personel seçilerek güncellenebilir");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //personel güncelle tamam
            if (textBox2.Text != "" && comboBox2.SelectedIndex > -1)
            {
                int rowIndex = dataGridView1.CurrentCell.RowIndex;
                DataGridViewRow row = dataGridView1.Rows[rowIndex];
                int guncellenecekID = (int)row.Cells[0].Value;
                string guncellenecekGrup = row.Cells[2].Value.ToString();
                var guncellenecek = _vt.Personeller.Where(w => w.Id == guncellenecekID).FirstOrDefault();
                guncellenecek.isim = textBox2.Text;
                guncellenecek.grupID = (int)comboBox2.SelectedValue;
                textBox2.Text = "";
                comboBox2.SelectedIndex = 0;

                _vt.SaveChanges();
                veriYukle();
                groupBox3.Visible = false;

                toolStripStatusLabel1.Text = textBox2.Text + " personeli güncellendi";
            }
            else
            {
                MessageBox.Show("İsim veya grup bilgisi boş geçildi...");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //personel güncelleme vazgeç
            groupBox3.Visible = false;
            toolStripStatusLabel1.Text = "Personel güncellemeden vazgeçildi";
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (textBox3.Text != "")
            {
                var data = _vt.Personeller.Join(
                          _vt.Gruplar,
                          a => a.grupID,
                          b => b.Id,
                          (a, b) => new
                          {
                              Kimlik = a.Id,
                              Ad = a.isim,
                              GrupAdi = b.grupAdi,
                              GrupID = b.Id
                          }
                      ).Where(c => c.Ad.Contains(textBox3.Text)).ToList();

                dataGridView1.DataSource = data;
                dataGridView1.Refresh();

                toolStripStatusLabel1.Text = textBox3.Text + " personeli arandı, " + data.Count.ToString() + " kişi bulundu.";
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox3.SelectedIndex)
            {
                case 0:
                    var data1 = _vt.Personeller.Join(
                          _vt.Gruplar,
                          a => a.grupID,
                          b => b.Id,
                          (a, b) => new
                          {
                              Kimlik = a.Id,
                              Ad = a.isim,
                              GrupAdi = b.grupAdi,
                              GrupID = b.Id
                          }
                      ).OrderBy(c => c.Kimlik).ToList();
                    dataGridView1.DataSource = data1;
                    break;
                case 1:
                    var data2 = _vt.Personeller.Join(
                          _vt.Gruplar,
                          a => a.grupID,
                          b => b.Id,
                          (a, b) => new
                          {
                              Kimlik = a.Id,
                              Ad = a.isim,
                              GrupAdi = b.grupAdi,
                              GrupID = b.Id
                          }
                      ).OrderByDescending(c => c.Kimlik).ToList();
                    dataGridView1.DataSource = data2;
                    break;
                case 2:
                    var data3 = _vt.Personeller.Join(
                          _vt.Gruplar,
                          a => a.grupID,
                          b => b.Id,
                          (a, b) => new
                          {
                              Kimlik = a.Id,
                              Ad = a.isim,
                              GrupAdi = b.grupAdi,
                              GrupID = b.Id
                          }
                      ).OrderBy(c => c.Ad).ToList();
                    dataGridView1.DataSource = data3;
                    break;
                case 3:
                    var data4 = _vt.Personeller.Join(
                          _vt.Gruplar,
                          a => a.grupID,
                          b => b.Id,
                          (a, b) => new
                          {
                              Kimlik = a.Id,
                              Ad = a.isim,
                              GrupAdi = b.grupAdi,
                              GrupID = b.Id
                          }
                      ).OrderByDescending(c => c.Ad).ToList();
                    dataGridView1.DataSource = data4;
                    break;
                case 4:
                    var data5 = _vt.Personeller.Join(
                          _vt.Gruplar,
                          a => a.grupID,
                          b => b.Id,
                          (a, b) => new
                          {
                              Kimlik = a.Id,
                              Ad = a.isim,
                              GrupAdi = b.grupAdi,
                              GrupID = b.Id
                          }
                      ).OrderBy(c => c.GrupAdi).ToList();
                    dataGridView1.DataSource = data5;
                    break;
                case 5:
                    var data6 = _vt.Personeller.Join(
                          _vt.Gruplar,
                          a => a.grupID,
                          b => b.Id,
                          (a, b) => new
                          {
                              Kimlik = a.Id,
                              Ad = a.isim,
                              GrupAdi = b.grupAdi,
                              GrupID = b.Id
                          }
                      ).OrderByDescending(c => c.GrupAdi).ToList();
                    dataGridView1.DataSource = data6;
                    break;
                default:
                    break;
            }

            toolStripStatusLabel1.Text = "Sıralama yapıldı";
            dataGridView1.Refresh();
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                button9_Click(sender, e);
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            grup _grup = new grup();
            string giris = Interaction.InputBox("Grup Adı", "Ad Giriniz", "", 500, 300);
            if (giris != "")
            {
                _grup.grupAdi = giris;
                _vt.Gruplar.Add(_grup);
                _vt.SaveChanges();
                veriYukle();

                toolStripStatusLabel1.Text = giris + " grubu eklendi";
            }
            else
            {
                MessageBox.Show("Grup adını girmediniz.");
            }

        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > -1)
            {
                int silinecekID = (int)listBox1.SelectedValue;
                var silinecek = _vt.Gruplar.Where(w => w.Id == silinecekID).FirstOrDefault();

                if (silinecek != null)
                {
                    _vt.Gruplar.Remove(silinecek);
                    _vt.SaveChanges();
                    veriYukle();

                    toolStripStatusLabel1.Text = "Grub silindi";
                }
                else
                    MessageBox.Show("Grup bulunamadı");
            }
            else
            {
                MessageBox.Show("Grup seçilmedi.");
            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > -1)
            {
                int guncelleID = (int)listBox1.SelectedValue;
                var guncellenecek = _vt.Gruplar.Where(w => w.Id == guncelleID).FirstOrDefault();

                if (guncellenecek != null)
                {
                    string giris = Interaction.InputBox("Grup Adı", "Ad Giriniz", guncellenecek.grupAdi, 500, 300);
                    if (giris != "")
                    {
                        guncellenecek.grupAdi = giris;

                        toolStripStatusLabel1.Text = guncellenecek.grupAdi + " grubu güncellendi";

                        _vt.SaveChanges();
                        veriYukle();
                    }
                }
                else
                    MessageBox.Show("Grup bulunamadı");
            }
            else
            {
                MessageBox.Show("Grup seçilmedi.");
            }
        }
    }
}
