using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Microsoft.VisualBasic.FileIO;

namespace form1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private static DataTable GetDataTabletFromCSVFile(string csv_file_path)
        {
            DataTable csvData = new DataTable();

            using (TextFieldParser csvReader = new TextFieldParser(csv_file_path))
            {
                csvReader.SetDelimiters(new string[] { "," });
                csvReader.HasFieldsEnclosedInQuotes = true;
                string[] colFields = csvReader.ReadFields();
                foreach (string column in colFields)
                {
                    DataColumn datecolumn = new DataColumn(column);
                    datecolumn.AllowDBNull = true;
                    csvData.Columns.Add(datecolumn);
                }
                while (!csvReader.EndOfData)
                {
                    string[] fieldData = csvReader.ReadFields();
                    //Making empty value as null
                    for (int i = 0; i < fieldData.Length; i++)
                    {
                        if (fieldData[i] == "")
                        {
                            fieldData[i] = null;
                        }
                    }
                    csvData.Rows.Add(fieldData);
                }
            }
            return csvData;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            try // main event
            {

                TreeNode l1node, l2node, l3node, l4node, l5node;

                string l1prev="", l2prev="", l3prev="", l4prev = "", l5prev = "";
                int l1i = -1, l2i = -1, l3i = -1, l4i = -1, l5i = -1;
                int i = 0;

                treeView1.Nodes.Clear();                                      
                DataTable dt1 = GetDataTabletFromCSVFile(textBox1.Text);

                if (dt1 != null) // only process if there are rows loaded
                {
                    treeView1.ShowNodeToolTips = true;
                    textBox2.Text = "No of rows loaded: " + dt1.Rows.Count.ToString();

                    for (i = 0; i < dt1.Rows.Count; i++)
                    {
                        //node.ToolTipText = "abcvalue";
                        //subnode.Text = "subText";
                        //node.Nodes.Add(subnode);
                        //node.ToolTipText = "tooltip";
                        //node.Tag = "tag";
                        //treeView1.ShowNodeToolTips = true;

                        if (l1prev != dt1.Rows[i][0].ToString()) // new level 1
                        {
                            l1i++;
                            l1node = new TreeNode();
                            l1node.Text = dt1.Rows[i][0].ToString();
                            l1node.ToolTipText = dt1.Rows[i][1].ToString();
                            treeView1.Nodes.Add(l1node);
                            l2i = -1;
                            l3i = -1;
                            l4i = -1;
                            l5i = -1;
                            l2prev = "";
                            l3prev = "";
                            l4prev = "";
                            l5prev = "";
                        }

                        if ((dt1.Columns.Count > 2 ) && (l2prev != dt1.Rows[i][2].ToString())) // new level 2
                        {
                            l2i++;
                            l2node = new TreeNode();
                            l2node.Text = dt1.Rows[i][2].ToString();
                            l2node.ToolTipText = dt1.Rows[i][3].ToString();
                            treeView1.Nodes[l1i].Nodes.Add(l2node);
                            l3i = -1;
                            l4i = -1;
                            l5i = -1;
                            l3prev = "";
                            l4prev = "";
                            l5prev = "";
                        }

                        if ((dt1.Columns.Count > 4 ) && (l3prev != dt1.Rows[i][4].ToString()) && (dt1.Rows[i][4].ToString() != "")) // new level 3
                        {
                            l3i++;
                            l3node = new TreeNode();
                            l3node.Text = dt1.Rows[i][4].ToString();
                            l3node.ToolTipText = dt1.Rows[i][5].ToString();
                            treeView1.Nodes[l1i].Nodes[l2i].Nodes.Add(l3node);
                            l4i = -1;
                            l5i = -1;
                            l4prev = "";
                            l5prev = "";
                        }

                        if ((dt1.Columns.Count > 6 ) && (l4prev != dt1.Rows[i][6].ToString()) && (dt1.Rows[i][6].ToString() != "")) // new level 3
                        {
                            l4i++;
                            l4node = new TreeNode();
                            l4node.Text = dt1.Rows[i][6].ToString();
                            l4node.ToolTipText = dt1.Rows[i][7].ToString();
                            treeView1.Nodes[l1i].Nodes[l2i].Nodes[l3i].Nodes.Add(l4node);
                            l5i = -1;
                            l5prev = "";
                        }

                        if ((dt1.Columns.Count > 8) && (l5prev != dt1.Rows[i][8].ToString()) && (dt1.Rows[i][8].ToString() != "")) // new level 3
                        {
                            l5i++;
                            l5node = new TreeNode();
                            l5node.Text = dt1.Rows[i][8].ToString();
                            l5node.ToolTipText = dt1.Rows[i][9].ToString();
                            treeView1.Nodes[l1i].Nodes[l2i].Nodes[l3i].Nodes[l4i].Nodes.Add(l5node);
                        }

                        l1prev = dt1.Rows[i][0].ToString();
                        if (dt1.Columns.Count > 2) l2prev = dt1.Rows[i][2].ToString();
                        if (dt1.Columns.Count > 4) l3prev = dt1.Rows[i][4].ToString();
                        if (dt1.Columns.Count > 6) l4prev = dt1.Rows[i][6].ToString();
                        if (dt1.Columns.Count > 8) l5prev = dt1.Rows[i][8].ToString();

                    }
                }

            }
            catch (Exception ex)
            {
                textBox2.Text = ex.Message;
            }
             
        }
        private void button2_Click(object sender, EventArgs e)
        {
            treeView1.ExpandAll();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            treeView1.CollapseAll();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // put under initialization event
            textBox1.Text = @"C:\Vicko\WDP\projects\mlorg\form1\branchhierarchy.csv";
            label3.Text = "Note: \n" +
                "1. Source file in Name,Id,Name,Id...format\n" +
                "2. Order by Name, and max 5 levels(pairs)\n" +
                "3. csv generated via saved query in Cinchy\n" +
                "4. See Get Wealth Branch Hierarchy HR/WBR..";
        }

        private void label3_Click(object sender, EventArgs e)
        {
            // none
        }
    }
}
