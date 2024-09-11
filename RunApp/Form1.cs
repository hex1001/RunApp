using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace RunApp
{
    public partial class Form1 : Form
    {
        public const string FILE_NAME = "Settings.ini"; 
        public Form1()
        {
            InitializeComponent();
        }

        private void btnFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                InitialDirectory = @"D:\",
                Title = "Browse Text Files",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "*",
                Filter = "All files (*.*)|*.*",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            timer1.Start();
            btnStart.Visible = false;
            btnStop.Visible = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DateTime dateTime = DateTime.Now;
            numHour.Value = dateTime.Hour;
            numMinute.Value = dateTime.Minute;
            numSecund.Value = dateTime.Second;

            var MyIni = new IniFile(FILE_NAME);
            if (MyIni.KeyExists("Path", "textBox1"))
            {
                textBox1.Text = MyIni.ReadKey("Path", "textBox1");
            }
            if (MyIni.KeyExists("Hour", "hour"))
            {
                numHour.Value = Convert.ToDecimal(MyIni.ReadKey("Hour", "hour"));
            }
            if (MyIni.KeyExists("Minute", "minute"))
            {
                numMinute.Value = Convert.ToDecimal(MyIni.ReadKey("Minute", "minute"));
            }
            if (MyIni.KeyExists("Secund", "secund"))
            {
                numSecund.Value = Convert.ToDecimal(MyIni.ReadKey("Secund", "secund"));
            }

            btnStop.Visible = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            if (numHour.Value == now.Hour &&
                    numMinute.Value == now.Minute &&
                    numSecund.Value == now.Second)
            {
                var proc1 = new ProcessStartInfo();
                string anyCommand;
                proc1.UseShellExecute = true;

                proc1.WorkingDirectory = @"C:\Windows\System32";

                proc1.FileName = @"C:\Windows\System32\cmd.exe";
                proc1.Verb = "runas";
                string str2 = textBox1.Text;
                int last = str2.LastIndexOf(@"\") + 1;
                int len = str2.Length;
                //MessageBox.Show(last.ToString());
                //MessageBox.Show(len.ToString());

                string str = textBox1.Text.Substring(last, len - last);
                //MessageBox.Show(str);
                proc1.Arguments = "/c taskkill /IM " + str;
                proc1.WindowStyle = ProcessWindowStyle.Hidden;
                Process.Start(proc1);

                Thread.Sleep(2000);

                var proc2 = new ProcessStartInfo();
                string anyCommand2;
                proc2.UseShellExecute = true;

                proc2.WorkingDirectory = @"C:\Windows\System32";

                proc2.FileName = @"C:\Windows\System32\cmd.exe";
                proc2.Verb = "runas";

                proc2.Arguments = "/c start " + textBox1.Text;
                proc2.WindowStyle = ProcessWindowStyle.Hidden;
                Process.Start(proc2);

            } 
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            btnStart.Visible = true;
            btnStop.Visible = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!chBoxSave.Checked)
            {
                new IniFile(FILE_NAME).DeleteFile();
            }
            else
            {
                IniFile MyIni = new IniFile (FILE_NAME);
                MyIni.WriteKey("Path", textBox1.Text, "textBox1");
                MyIni.WriteKey("Hour", numHour.Value.ToString(), "hour");
                MyIni.WriteKey("Minute", numMinute.Value.ToString(), "minute");
                MyIni.WriteKey("Secund", numSecund.Value.ToString(), "secund");
            }
        }
    }
}
