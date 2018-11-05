using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace RBfTool
{
    public partial class Form1 : Form
    {
        RbfManager manager = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            manager = new RbfManager();

            if (manager.LoadRbf("E:\\MSVC\\RBfTool\\Pacman2.rbf") == true)
            {
                romText.Text = "Size : "+manager.getRomLength();
                rbfText.Text = "Size : "+manager.getRbfLength();
            } 
        }

        private void OpenRbf(){
            manager = new RbfManager();
            romText.Text = "";
            rbfText.Text = "";
            injectedRBF.Text = "";
            toolStripStatusLabel1.Text = "";

            String file = ChooseOpenFile("rbf");

            if (file=="") {
                manager = null;
                return;
            }
            if (manager.LoadRbf(file) == true){
                romText.Text = "Size : " + manager.getRomLength();
                rbfText.Text = "Size : " + manager.getRbfLength();
                injectedRBF.Text = "Size : " + manager.getinjectedRbfLength();
                toolStripStatusLabel1.Text = "Injected rbf loaded.";
            }
            else{
                rbfText.Text = "Size : " + manager.getRbfLength();
                toolStripStatusLabel1.Text = "Regular Rbf loaded.";
            }
        }

        private void SaveRbf() {
            if (manager==null) {
                Console.Beep();
                toolStripStatusLabel1.Text = "No rbf to save.";                
                return;
            }
            String file = ChooseSaveFile("rbf");
            manager.SaveRbf(file);
            toolStripStatusLabel1.Text = "Rbf saved.";     
        }

        private void SaveInjectedRbf()
        {
            if (manager == null)
            {
                Console.Beep();
                toolStripStatusLabel1.Text = "No rbf to save.";
                return;
            }            
            String file = ChooseSaveFile("rbf");
            manager.SaveInjectedRbf(file);
            toolStripStatusLabel1.Text = "Injected Rbf saved.";  
        }

        private void OpenRom(){
            if (manager==null) {
                Console.Beep();
                toolStripStatusLabel1.Text = "Load rbf before rom.";
                return;
            }

            String file = ChooseOpenFile("rom");
            if (file == "")
            {
                return;
            }
            if (manager.LoadRom(file) == true){
                romText.Text = "Size : " + manager.getRomLength();
                toolStripStatusLabel1.Text = "Rom loaded.";
                // injected size
                injectedRBF.Text = "Size : " + manager.getinjectedRbfLength();
            }
        }

        private void SaveRom()
        {
            if (manager == null)
            {
                Console.Beep();
                toolStripStatusLabel1.Text = "No rom to save.";
                return;
            }
            String file = ChooseSaveFile("rom");
            if (file == "")
            {
                return;
            }
            manager.SaveRom(file);
            toolStripStatusLabel1.Text = "Rom saved.";
        }

        private String ChooseOpenFile(String fileType) {
            String retVal = "";

            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                //InitialDirectory = Application.StartupPath,
                Title = "Load " + fileType + " File",
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = fileType,
                Filter = fileType + " files (*." + fileType + ")|*." + fileType,
                FilterIndex = 2,
                RestoreDirectory = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                retVal = openFileDialog1.FileName;
            }
            return retVal;
        }

        private String ChooseSaveFile(String fileType)
        {
            String retVal = "";

            SaveFileDialog saveFileDialog1 = new SaveFileDialog
            {
                Filter = fileType + " files (*." + fileType + ")|*." + fileType,
                //InitialDirectory = Application.StartupPath,
                Title = "Save " + fileType + " File",
                RestoreDirectory = true
            };

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                retVal = saveFileDialog1.FileName;
            }
            return retVal;
        }

        void AutoInject(){
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                manager = new RbfManager();
                manager.AutoInject(fbd.SelectedPath);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenRbf();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OpenRbf();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            OpenRom();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            SaveInjectedRbf();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SaveRbf();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SaveRom();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            AutoInject();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            OpenURL("https://github.com/MiSTer-devel/Main_MiSTer/wiki");
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            OpenURL("https://github.com/funkycochise?tab=repositories");
        }

        private void OpenURL(String url)  {
            ProcessStartInfo sinfo = new ProcessStartInfo(url);
            Process.Start(sinfo);
        }
    }
}
