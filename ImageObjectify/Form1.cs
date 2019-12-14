using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageObjectify
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }



        public void EqualizeHistogram()
        {
            Bitmap bmp = new Bitmap(openFileDialog1.FileName);
            int[] histogram_r = new int[256];
            float max = 0;

            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    int redValue = bmp.GetPixel(i, j).R;
                    histogram_r[redValue]++;
                    if (max < histogram_r[redValue])
                        max = histogram_r[redValue];
                }
            }

            int histHeight = 128;
            Bitmap img = new Bitmap(256, histHeight + 10);
            using (Graphics g = Graphics.FromImage(img))
            {
                for (int i = 0; i < histogram_r.Length; i++)
                {
                    float pct = histogram_r[i] / max;   // What percentage of the max is this value?
                    g.DrawLine(Pens.Black,
                        new Point(i, img.Height - 5),
                        new Point(i, img.Height - 5 - (int)(pct * histHeight))  // Use that percentage of the height
                        );
                }
            }
            pictureBox2.Image = img;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        public void ClearTabs()
        {
            int tabCounter = tabControl1.TabPages.Count;
            for (int i = 1; i < tabCounter; i++)
            {
                if (tabControl1.TabPages.Count != 1)
                {
                    tabControl1.TabPages.RemoveAt(1);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // display image in picture box  
                OriginalImage.Image = new Bitmap(openFileDialog1.FileName);
                // image file path  
                label2.Text = Path.GetFileName(openFileDialog1.FileName);
                label3.Text = (Convert.ToDecimal(new FileInfo(openFileDialog1.FileName).Length) / 1024 / 1024).ToString("N3") + " MB";
                label5.Text = new FileInfo(openFileDialog1.FileName).Extension;
                label7.Text = new FileInfo(openFileDialog1.FileName).LastWriteTime.ToString();
                label9.Text = new FileInfo(openFileDialog1.FileName).CreationTime.ToString();
                label11.Text = new FileInfo(openFileDialog1.FileName).LastAccessTime.ToString();
                label13.Text = new FileInfo(openFileDialog1.FileName).IsReadOnly.ToString();
                textBox1.Text = openFileDialog1.FileName;
                EqualizeHistogram();
                ClearTabs();
                int rowIndex = 0;
                IReadOnlyList<MetadataExtractor.Directory> tagDir = ImageMetadataReader.ReadMetadata(openFileDialog1.FileName);
                foreach (MetadataExtractor.Directory item in tagDir)
                {
                    if (item.Name != "File")
                    {
                        TabPage tabPage = new TabPage(item.Name);
                        tabPage.AutoScroll = true;
                        tabPage.BackColor = Color.Transparent;
                        tabControl1.TabPages.Add(tabPage);
                        IReadOnlyList<Tag> lstTags = item.Tags;
                        TableLayoutPanel flowLayoutPanel = new TableLayoutPanel();
                        flowLayoutPanel.Width = 700;
                        flowLayoutPanel.Height = 600;
                        flowLayoutPanel.Margin = new Padding(0, 200, 0, 0);
                        flowLayoutPanel.AutoSize = false;
                        //flowLayoutPanel.AutoScroll = true;
                        foreach (Tag tagItem in lstTags)
                        {
                            flowLayoutPanel.Controls.Add(new Label { Text = tagItem.Name }, 0, rowIndex);
                            flowLayoutPanel.Controls.Add(new Label { Text = tagItem.Description }, 1, rowIndex);
                            rowIndex++;
                        }
                        tabPage.Controls.Add(flowLayoutPanel);
                    }
                }

                //var gps = ImageMetadataReader.ReadMetadata(openFileDialog1.FileName)
                //             .OfType<GpsDirectory>()
                //             .FirstOrDefault();

                //var tagDir = ImageMetadataReader.ReadMetadata(openFileDialog1.FileName);


                //var location = gps.GetGeoLocation();
                //IReadOnlyList<Tag> lstTags = gps.Tags;
                //int rowNum = 0, rowNum1 = 0, rowNum2 = 0, rowNum3 = 0, rowNum4 = 0,rowNum5=0, rowNum6 = 0, rowNum7 = 0;
                //foreach (Tag tag in lstTags)
                //{
                //    tableLayoutPanel1.Controls.Add(new Label { Text = tag.Name }, 0, rowNum);
                //    tableLayoutPanel1.Controls.Add(new Label { Text = tag.Description }, 1, rowNum);
                //    rowNum++;
                //}

                ////tagDir[2].Tags
                //foreach (Tag tag in tagDir[2].Tags)
                //{
                //    tableLayoutPanel2.Controls.Add(new Label { Text = tag.Name }, 0, rowNum1);
                //    tableLayoutPanel2.Controls.Add(new Label { Text = tag.Description }, 1, rowNum1);
                //    rowNum1++;
                //}

                //foreach (Tag tag in tagDir[0].Tags)
                //{
                //    tableLayoutPanel3.Controls.Add(new Label { Text = tag.Name }, 0, rowNum2);
                //    tableLayoutPanel3.Controls.Add(new Label { Text = tag.Description }, 1, rowNum2);
                //    rowNum2++;
                //}

                //foreach (Tag tag in tagDir[1].Tags)
                //{
                //    tableLayoutPanel4.Controls.Add(new Label { Text = tag.Name }, 0, rowNum3);
                //    tableLayoutPanel4.Controls.Add(new Label { Text = tag.Description }, 1, rowNum3);
                //    rowNum3++;
                //}

                //foreach (Tag tag in tagDir[4].Tags)
                //{
                //    if (rowNum4 < 14)
                //    {
                //        tableLayoutPanel5.Controls.Add(new Label { Text = tag.Name }, 0, rowNum4);
                //        tableLayoutPanel5.Controls.Add(new Label { Text = tag.Description }, 1, rowNum4);
                //    }
                //    else
                //    {
                //        tableLayoutPanel6.Controls.Add(new Label { Text = tag.Name }, 0, rowNum5);
                //        tableLayoutPanel6.Controls.Add(new Label { Text = tag.Description }, 1, rowNum5);
                //        rowNum5++;
                //    }
                //    rowNum4++;
                //}

                //foreach (Tag tag in tagDir[8].Tags)
                //{
                //    tableLayoutPanel7.Controls.Add(new Label { Text = tag.Name }, 0, rowNum6);
                //    tableLayoutPanel7.Controls.Add(new Label { Text = tag.Description }, 1, rowNum6);
                //    rowNum6++;
                //}

                //foreach (Tag tag in tagDir[6].Tags)
                //{
                //    tableLayoutPanel8.Controls.Add(new Label { Text = tag.Name }, 0, rowNum7);
                //    tableLayoutPanel8.Controls.Add(new Label { Text = tag.Description }, 1, rowNum7);
                //    rowNum7++;
                //}
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }
    }
}
