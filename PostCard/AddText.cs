using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PostCard
{
    public partial class AddText : Form
    {
        public AddText()
        {
            InitializeComponent();
        }

        private void AddText_Load(object sender, EventArgs e)
        {
            string targetDirectory = Application.StartupPath + @"/Post Card Images";
            DirectoryInfo files = new DirectoryInfo(targetDirectory);
            FileInfo[] fileEntries = files.GetFiles("*.jpg");
            foreach (FileInfo imageName in fileEntries)
            {
                ImageList.Items.Add(imageName.Name);
            }
            button1.Hide();
            textBox1.Hide();
            label1.Hide();
            button2.Hide();
        }

        private void EditImage_Click(object sender, EventArgs e)
        {
            try { 
                button1.Show();
            button2.Show();
            label1.Show();
            textBox1.Show();
            string targetDirectory = Application.StartupPath + @"/Post Card Images/";
            string filepath = targetDirectory + ImageList.SelectedItem.ToString().Trim();
            pictureBox1.ImageLocation = filepath;
            pictureBox1.Load(filepath);
            pictureBox1.Update();
            pictureBox1.Refresh();
            pictureBox1.Visible = true;
        }catch(NullReferenceException )
            {
                MessageBox.Show("Please Select image from list. Or take new Picture");

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            writeOnPostCard();
        }

        private void writeOnPostCard()
        {
            
           
            Bitmap bmp = new Bitmap(pictureBox1.ImageLocation);
            RectangleF rectf = new RectangleF(0,0,100, 50);

            Graphics g = Graphics.FromImage(bmp);

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.DrawString(textBox1.Text, new Font("Arial", 24), Brushes.White, rectf);

            g.Flush();

            pictureBox1.Image = bmp;
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("your_email_address@gmail.com");
                mail.To.Add("mrizwanghuman@gmail.com");
                mail.Subject = "Test Mail";
                mail.Body = "This is for testing SMTP mail from GMAIL";

                SmtpServer.Port = 465;
                SmtpServer.Credentials = new System.Net.NetworkCredential("mrizwantech@gmail.com", "guj78695");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                MessageBox.Show("mail Send");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
