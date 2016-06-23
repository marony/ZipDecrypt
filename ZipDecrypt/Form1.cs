using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ionic.Zip;

namespace ZipDecrypt
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            var result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtFilePath.Text = openFileDialog.FileName;
            }
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            var filePath = txtFilePath.Text;

            var start = DateTime.Now;
            // パスワード解析
            foreach (var password in new PasswordMaker().NextPassword())
            {
                //Console.WriteLine(password);
                try
                {
                    using (var zip = ZipFile.Read(filePath))
                    {
                        var entry = zip[0];
                        entry.ExtractWithPassword(Path.GetTempPath(), ExtractExistingFileAction.OverwriteSilently, password);
                    }
                }
                catch (BadPasswordException ex)
                {
                    //Console.WriteLine(ex);
                    continue;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    continue;
                }
                var time = (DateTime.Now - start).TotalMilliseconds.ToString();

                MessageBox.Show(password, $"{time} ms");
                break;
            }
        }
    }
}
