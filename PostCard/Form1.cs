using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using System.IO;
using System.Reflection;

namespace PostCard
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();

        }
        private FilterInfoCollection webcam;
        private VideoCaptureDevice cam;

        private void Form1_Load(object sender, EventArgs e)
        {
            webcam = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo VideoCaptureDevice in webcam)
            {
                comboBox1.Items.Add(VideoCaptureDevice.Name);
            }
            string postCardFolder = "Post Card Images";
            string startupPath = Environment.CurrentDirectory;
            string filepath = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
            string folderPath = Directory.CreateDirectory(postCardFolder).ToString();
            pictureBox1.Image = Image.FromFile(@"defaultimage.png");
            string targetDirectory = Application.StartupPath + @"/Post Card Images";
            if (targetDirectory != null)
            {
                DirectoryInfo files = new DirectoryInfo(targetDirectory);
                FileInfo[] fileEntries = files.GetFiles("*.jpg");
                foreach (FileInfo imageName in fileEntries)
                {
                    listBox1.Items.Add(imageName.Name);
                }
            }

        }

        private void startButton_Click(object sender, EventArgs e)
        {
            try
            {
                cam = new VideoCaptureDevice(webcam[comboBox1.SelectedIndex].MonikerString);
                cam.NewFrame += new NewFrameEventHandler(cam_NewFrame);
                cam.Start();
            }
            catch (ArgumentOutOfRangeException ) {
                MessageBox.Show("Please select Cam to start from drop down list.");
            }
        }

        private void cam_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap bit = (Bitmap)eventArgs.Frame.Clone();
            pictureBox1.Image = bit;
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (cam.IsRunning)
                {
                    cam.Stop();
                }
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message + " Webcam is not running");
            }
        
        }

        private void takePic_Click(object sender, EventArgs e)
        {
            try
            {
                if (cam.IsRunning) { 
                var myUniqueFileName = string.Format(@"postCard{0}.jpg", DateTime.Now.ToString("-yyyy-MM-dd-HH-mm-ss"));
                string postCardFolder = "Post Card Images";
                string startupPath = Environment.CurrentDirectory;
                string filepath = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
                string folderPath = Directory.CreateDirectory(postCardFolder).ToString();

                string fileName = System.IO.Path.Combine(folderPath, myUniqueFileName);

                pictureBox1.Image.Save(fileName);
                MessageBox.Show("You successfuly took your picture");
                string output = fileName.Substring(fileName.IndexOf('\\') + 1);
                listBox1.Items.Add(output);
                listBox1.Update();
                }
               
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message+ " Please start webcam to take picture");
            }
            
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void selectedImageShow_Click(object sender, EventArgs e)
        {
            string targetDirectory = Application.StartupPath + @"/Post Card Images/";
            string filepath =targetDirectory + listBox1.SelectedItem.ToString().Trim();
            pictureBox1.ImageLocation = filepath;
            pictureBox1.Load(filepath);
            pictureBox1.Update();
            pictureBox1.Refresh();
            pictureBox1.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            AddText newForm = new AddText();
            newForm.Show();
        }
    }
}
