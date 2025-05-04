using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace custom_attribute_odevi_alperen_saricayir
{
    public partial class Form1: Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lblSonuc.Text = "";
            txtAd.BackColor = txtSoyad.BackColor = txtBolum.BackColor = System.Drawing.Color.White;
        }

        [AttributeUsage(AttributeTargets.Property)]
        public class ZorunluAlanAttribute : Attribute
        {
            public string HataMesaji { get; set; }

            public ZorunluAlanAttribute(string hataMesaji)
            {
                HataMesaji = hataMesaji;
            }
        }

        public class Ogrenci
        {
            [ZorunluAlan("Öğrenci Adı boş bırakılamaz!")]
            public string Ad { get; set; }

            [ZorunluAlan("Öğrenci Soyadı boş bırakılamaz!")]
            public string Soyad { get; set; }

            [ZorunluAlan("Öğrenci Bölümü boş bırakılamaz!")]
            public string Bolum { get; set; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Ogrenci ogr = new Ogrenci
            {
                Ad = txtAd.Text.Trim(),
                Soyad = txtSoyad.Text.Trim(),
                Bolum = txtBolum.Text.Trim()
            };

            bool hataVarMi = false;
            string hataMesajlari = "";

            PropertyInfo[] ozellikler = typeof(Ogrenci).GetProperties();

            foreach (var ozellik in ozellikler)
            {
                var deger = ozellik.GetValue(ogr) as string;

                var attribute = (ZorunluAlanAttribute)ozellik
                    .GetCustomAttributes(typeof(ZorunluAlanAttribute), false)
                    .FirstOrDefault();

                if (attribute != null && string.IsNullOrWhiteSpace(deger))
                {
                    hataMesajlari += $"{attribute.HataMesaji}\n";
                    hataVarMi = true;

                    // Boş olan alanları renklendir
                    if (ozellik.Name == "Ad") txtAd.BackColor = System.Drawing.Color.MistyRose;
                    if (ozellik.Name == "Soyad") txtSoyad.BackColor = System.Drawing.Color.MistyRose;
                    if (ozellik.Name == "Bolum") txtBolum.BackColor = System.Drawing.Color.MistyRose;
                }
            }

            if (hataVarMi)
            {
                MessageBox.Show(hataMesajlari, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                lblSonuc.Text = $"Ad: {ogr.Ad}\nSoyad: {ogr.Soyad}\nBölüm: {ogr.Bolum}";
                txtAd.BackColor = txtSoyad.BackColor = txtBolum.BackColor = System.Drawing.Color.White;
            }
        }
    }
}
