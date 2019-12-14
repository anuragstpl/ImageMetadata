using MetadataExtractor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageObjectify
{
    public partial class MainBoard : Form
    {
        public MainBoard()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                int columnIndex = 0;
                int rowIndex = 0;
                IReadOnlyList<Directory> tagDir = ImageMetadataReader.ReadMetadata(openFileDialog1.FileName);
                tabControl1.TabPages.Clear();
                foreach (Directory item in tagDir)
                {
                    TabPage tabPage = new TabPage(item.Name);
                    tabControl1.TabPages.Add(tabPage);
                    IReadOnlyList<Tag> lstTags = item.Tags;
                    TableLayoutPanel flowLayoutPanel = new TableLayoutPanel();
                    flowLayoutPanel.Width = 900;
                    flowLayoutPanel.Height = 600;
                    flowLayoutPanel.Margin = new Padding(0, 200, 0, 0);
                    flowLayoutPanel.AutoSize = false;
                    flowLayoutPanel.AutoScroll = true;
                    foreach (Tag tagItem in lstTags)
                    {
                        flowLayoutPanel.Controls.Add(new Label { Text = tagItem.Name }, 0, rowIndex);
                        flowLayoutPanel.Controls.Add(new Label { Text = tagItem.Description }, 1, rowIndex);
                        
                        rowIndex++;
                    }
                   
                    tabPage.Controls.Add(flowLayoutPanel);
                    //tabPage.Controls.Add(groupBox);
                    columnIndex++;
                }
            }
        }

        void GetCorrectionPadding(TableLayoutPanel TLP, int width)
        {
            TableLayoutColumnStyleCollection styles = TLP.ColumnStyles;
            foreach (ColumnStyle style in styles)
            {
                // Set the row height to 20 pixels.
                style.SizeType = SizeType.Absolute;
                style.Width = width;
            }
        }


        private void MainBoard_Load(object sender, EventArgs e)
        {
            //GetCorrectionPadding(tableLayoutPanel1, 90);
            //GetCorrectionPadding(tableLayoutPanel2, 450);
        }
    }
}
