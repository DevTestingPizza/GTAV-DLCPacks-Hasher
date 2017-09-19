using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace GTAV_DLCPacks_Hasher
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private string hashList = "";
        private string[] filePath;
        private void button1_Click(object sender, EventArgs e)
        {
            //string hashList = "";
            folderBrowserDialog1.ShowDialog();
            filePath = Directory.GetFiles(folderBrowserDialog1.SelectedPath, "*", SearchOption.AllDirectories).OrderBy(p => p).ToArray();
            if (!filePath[0].Contains("Grand Theft Auto V"))
            {
                MessageBox.Show("Error: You did not select a GTA (root) folder.");
                return;
            }
            button1.Text = "Loading...";
            UseWaitCursor = true;
            //foreach (var file in filePath)
            //{
                //hashList += HashFile(file);

            backgroundWorker1.RunWorkerAsync(filePath);
            button1.Enabled = false;

            //using (var sha1 = SHA1.Create())
            //{
            //byte[] contentBytes = File.ReadAllBytes(filePath[i]);
            //sha1.TransformBlock(contentBytes, 0, contentBytes.Length, contentBytes, 0);
            //sha1.TransformFinalBlock(new byte[0], 0, 0);
            //hashList += "File: " + filePath[i] + "\nSHA1: " + BitConverter.ToString(sha1.Hash).Replace("-", "").ToLower() + "\n";
            //byte[] contentBytes = File.ReadAllBytes(file);
            //sha1.TransformBlock(contentBytes, 0, contentBytes.Length, contentBytes, 0);
            //sha1.TransformFinalBlock(new byte[0], 0, 0);
            //hashList += "File: " + file + "\nSHA1: " + BitConverter.ToString(sha1.Hash).Replace("-", "").ToLower() + "\n";
            //}

            //}

        }
        private void appendToHashString(string hash, string filepath)
        {
            hashList += "Filepath: " + filepath + "\nSHA1 hash: " + hash + "\n";
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < filePath.Length; i++) 
            {
                Console.WriteLine(filePath[i]);
                //backgroundWorker1.ReportProgress(i);
                //int progress = (i * 100) / (filePath.Count() * 100);
                //Console.WriteLine(progress + "% complete.");
                //richTextBox1.Text = richTextBox1.Text + "\nHasing: " + filePath[i];
                //backgroundWorker1.ReportProgress(progress);
                //backgroundWorker1.ReportProgress(((i + 1) / filePath.Length) * 100);
                Stream file = null;
                using (file = File.OpenRead(filePath[i].ToString()))
                {
                    byte[] fileBuffer = null;
                    int bytesRead = 0;
                    long size = file.Length;
                    long totalBytesRead = 0;
                    using (HashAlgorithm hasher = SHA1.Create())
                    {
                        do
                        {
                            fileBuffer = new byte[4096];
                            bytesRead = file.Read(fileBuffer, 0, fileBuffer.Length);
                            totalBytesRead += bytesRead;
                            hasher.TransformBlock(fileBuffer, 0, bytesRead, null, 0);
                        } while (bytesRead != 0);
                        hasher.TransformFinalBlock(fileBuffer, 0, 0);
                        StringBuilder hash2 = new StringBuilder(40);
                        byte[] hashBytes = hasher.Hash;
                        foreach (byte b in hashBytes)
                            hash2.Append(b.ToString("X2").ToUpper());
                        appendToHashString(hash2.ToString(), filePath[i]);
                    }
                }
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //progressBar1.Value = e.ProgressPercentage;
            //richTextBox1.Text = e.ProgressPercentage + "% complete.";
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Console.WriteLine(hashList);
            richTextBox2.Text = hashList;
            UseWaitCursor = false;
            button1.Text = "Done (Click me to select a new folder)";
            button1.Enabled = true;
        }
        //private string HashFile(String filepath)
        //{
        //    //string sha1Hash = "";
        //    //using (var sha1 = SHA1.Create())
        //    //{
        //    //    byte[] contentBytes = File.ReadAllBytes(filepath);
        //    //    sha1.TransformFinalBlock(contentBytes, 0, 0);
        //    //    sha1Hash = BitConverter.ToString(sha1.Hash).Replace("-", "").ToUpper();
        //    //}

        //    string hashEntry = "Filepath: " + filepath + "\nSHA1 hash: " + sha1Hash + "\n";
        //    return hashEntry;
        //}
    }
}
