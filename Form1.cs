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

namespace Capracu3iezi
{
    public partial class Form1 : Form
    {
        List<Imagine> imagini = new List<Imagine>();
        string[] files;
        int x = 20, y = 0;
        PictureBox pic;
        int i = 0;
        int corecte;
        public Form1()
        {
            InitializeComponent();
            
        }

        

        private void Form1_Load(object sender, EventArgs e)
        {
            Initialisation();

        }
        private void Initialisation()
        {
            panel1.Controls.Clear();
           
            x = 20; y = 0;
            pic = null;
            i = 0;
            corecte = 0;
            foreach (Control c in this.Controls)
            {
                if (c is PictureBox)
                {
                    PictureBox pictureBox = (PictureBox)c;
                    pictureBox.AllowDrop = true;
                    pictureBox.DragEnter += pictureBox_DragEnter;
                    pictureBox.DragDrop += pictureBox_DragDrop;
                    pictureBox.Image = null;
                }
            }
            string workingdirectory = Environment.CurrentDirectory;
            string projectdirectory = Directory.GetParent(workingdirectory).Parent.FullName;
            string path = Path.Combine(projectdirectory, "Resurse");
            files = Directory.GetFiles(path);
            Random random = new Random();
            files = files.OrderBy(r => random.Next()).ToArray();
            foreach (string file in files)
            {
                string[] line = file.Split('\\');
                string nr = line[line.Length - 1].Split('.')[0];
                Imagine imagine = new Imagine()
                {
                    image = Image.FromFile(file),
                    initialloc = new Point(x, y),
                    nr = Convert.ToInt32(nr)
                };
                imagini.Add(imagine);
                x += 90 + 30;
            }
            foreach (Imagine imagine in imagini)
            {
                PictureBox pictureBox = new PictureBox()
                {
                    Image = imagine.image,
                    Name = "raspuns" + i,
                    Location = imagine.initialloc,
                    BorderStyle = BorderStyle.FixedSingle,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Width = 90,
                    Height = 70,
                    Tag = imagine.nr
                }; i++;
                pictureBox.MouseDown += pictureBox_MouseDown;
                panel1.Controls.Add(pictureBox);
            }
        }
        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            var img = (sender as PictureBox).Image;
            pic = sender as PictureBox;
            if (img == null) return;
            if (DoDragDrop(img, DragDropEffects.Move) == DragDropEffects.Move)
            {
                (sender as PictureBox).Image = null;
            }
        }
        void pictureBox_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Bitmap))
                e.Effect = DragDropEffects.Move;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach(var c in this.Controls)
            {
                if(c is PictureBox)
                {
                    PictureBox pb = (PictureBox)c;
                    if (!pb.Name.Contains("raspuns")) 
                    {
                        string name= pb.Name;
                        string n =Convert.ToString( name[name.Length-1]);
                        int nr = Convert.ToInt32(n);
                        if(nr == Convert.ToInt32(pb.Tag))
                        {
                            corecte++;
                        }
                    }
                }
            }
            if(corecte == 6)
            {
                MessageBox.Show("Bravo!", "Ai asezat corect!");
                Initialisation();

            }
            else
            {
                MessageBox.Show("Ups!", "Mai incearca o data!");
                Initialisation();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Initialisation();
        }

        void pictureBox_DragDrop(object sender, DragEventArgs e)
        {
            var bmp = (Bitmap)e.Data.GetData(DataFormats.Bitmap);
            if ((sender as PictureBox).Image == null)
            {
                (sender as PictureBox).Tag = pic.Tag;
            }
            else
            {
                Image image = (sender as PictureBox).Image;
                int nr = Convert.ToInt32((sender as PictureBox).Tag);
                (sender as PictureBox).Tag = pic.Tag;
               
                foreach (Control c in panel1.Controls) {
                    if (c is PictureBox && c.Tag!=null) {
                        if (c.Tag.ToString() == nr.ToString()) { PictureBox p =  (PictureBox)c;
                            p.Image = image; }
                    }
                }
                
            }
           (sender as PictureBox).Image = bmp;
        }
    }
}
