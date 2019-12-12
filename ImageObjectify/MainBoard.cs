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
                foreach (Directory item in tagDir)
                {
                    tableLayoutPanel1.Controls.Add(new Button { Text = item.Name, Width = 200 }, columnIndex, 0);
                    IReadOnlyList<Tag> lstTags = item.Tags;
                    GroupBox groupBox = new GroupBox();
                    groupBox.Text = item.Name;
                    groupBox.AutoSize = true;
                    groupBox.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                    groupBox.Height = 850;
                    groupBox.MaximumSize = new Size(900, 900);
                    TableLayoutPanel flowLayoutPanel = new TableLayoutPanel();
                    flowLayoutPanel.Width = 900;
                    flowLayoutPanel.AutoSize = true;
                    foreach (Tag tagItem in lstTags)
                    {
                        flowLayoutPanel.Controls.Add(new Label { Text = tagItem.Name });
                        flowLayoutPanel.Controls.Add(new Label { Text = tagItem.Description });
                        groupBox.Controls.Add(flowLayoutPanel);
                        rowIndex++;
                    }
                    tableLayoutPanel2.Controls.Add(groupBox);
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
            GetCorrectionPadding(tableLayoutPanel1, 90);
            //GetCorrectionPadding(tableLayoutPanel2, 450);
        }
    }
}
