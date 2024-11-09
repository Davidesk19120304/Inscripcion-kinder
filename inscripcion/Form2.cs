using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Drawing.Drawing2D;
namespace inscripcion
{
    public partial class Form2 : Form
    {
        private int borderRadius = 20;
        private int borderSize = 2;
        private Color borderColor = Color.FromArgb(51, 153, 255);

        MySqlConnection conexion = new MySqlConnection("Server=localhost;Database=inscripcion;Uid=root;pwd=;");

        private List<Image> _images = new List<Image>()
        {
            Properties.Resources.WhatsApp_Image_2024_10_16_at_14_41_39,
            Properties.Resources.WhatsApp_Image_2024_10_16_at_14_41_40__1_,
            Properties.Resources.WhatsApp_Image_2024_10_16_at_14_41_40__2_
        };

        private int _currentIndex = 0;
        private Timer timer1;

        public Form2()
        {
            InitializeComponent();

            timer1 = new Timer(); 
            timer1.Interval = 3000; 
            timer1.Tick += timer1_Tick;
            timer1.Start();

            LoadImage(_currentIndex);
            guna2TextBox1.PlaceholderText = "Usuario";
            guna2TextBox2.PlaceholderText = "Contraseña";
            guna2TextBox2.UseSystemPasswordChar = true;


           
            guna2TextBox1.PlaceholderForeColor = Color.FromArgb(128, 128, 128); 
            guna2TextBox2.PlaceholderForeColor = Color.FromArgb(128, 128, 128);

            this.FormBorderStyle = FormBorderStyle.None;
            this.Padding = new Padding(borderSize + 1);


            this.BackColor = Color.AliceBlue;


            this.guna2Panel1.BackColor = Color.White;


            

        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style |= 0x20000;
                return cp;
            }
        }

        private GraphicsPath GetRoundedPath(Rectangle rect, float radius)
        {
            GraphicsPath path = new GraphicsPath();
            float curveSize = radius * 2F;
            path.StartFigure();
            path.AddArc(rect.X, rect.Y, curveSize, curveSize, 180, 90);
            path.AddArc(rect.Right - curveSize, rect.Y, curveSize, curveSize, 270, 90);
            path.AddArc(rect.Right - curveSize, rect.Bottom - curveSize, curveSize, curveSize, 0, 90);
            path.AddArc(rect.X, rect.Bottom - curveSize, curveSize, curveSize, 90, 90);
            path.CloseFigure();
            return path;
        }

        private void FormRegionAndBorder(Form form, float radius, Graphics graph, Color borderColor, float borderSize)
        {
            if (this.WindowState != FormWindowState.Minimized)
            {
                using (GraphicsPath roundPath = GetRoundedPath(form.ClientRectangle, radius))
                using (Pen penBorder = new Pen(borderColor, borderSize))
                {
                    graph.SmoothingMode = SmoothingMode.AntiAlias;
                    form.Region = new Region(roundPath);


                    if (borderSize >= 1)
                    {

                        Rectangle rect = form.ClientRectangle;
                        rect.Inflate(-((int)borderSize / 2), -((int)borderSize / 2));
                        graph.DrawPath(penBorder, roundPath);
                    }
                }
            }

        }






        private void LoadImage(int index)
        {
            if (_images.Count > 0)
            {
                pictureBoxCarousel.Image = _images[index];
                pictureBoxCarousel.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            NextImage();
        }

        private void NextImage()
        {
            _currentIndex = (_currentIndex + 1) % _images.Count;
            LoadImage(_currentIndex);
        }

        private void PrevImage()
        {
            _currentIndex = (_currentIndex - 1 + _images.Count) % _images.Count;
            LoadImage(_currentIndex);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            NextImage();
            timer1.Start();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            PrevImage();
            timer1.Start();
        }
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            try
            {


                conexion.Open();
                
                conexion.Close();

            }

            catch
            {
                DialogResult = MessageBox.Show("Error");

            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

            string enteredUsername = guna2TextBox1.Text.Trim();
            string enteredPassword = guna2TextBox2.Text.Trim();
            string hashedPassword = HashPassword(enteredPassword); 

            using (MySqlConnection conexion = new MySqlConnection("Server=localhost;Database=kinder;Uid=root;pwd=;"))
            {
                string query = "SELECT user, password FROM login WHERE user = @user AND password = @password";
                MySqlCommand cmd = new MySqlCommand(query, conexion);
                cmd.Parameters.AddWithValue("@user", enteredUsername);
                cmd.Parameters.AddWithValue("@password", hashedPassword); 

                try
                {
                    conexion.Open();
                    MySqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        Form4 formulario3 = new Form4();
                        formulario3.Show(this);
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Usuario o contraseña incorrectos");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ocurrió un error: " + ex.Message);
                }
                finally
                {
                    conexion.Close();
                }
            }
        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form2_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rectForm = this.ClientRectangle;


            e.Graphics.Clear(this.BackColor);


            FormRegionAndBorder(this, borderRadius, e.Graphics, borderColor, borderSize);
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            Form1 formulario1 = new Form1();


            formulario1.Show(this);
            this.Hide();
        }

        private void pictureBoxCarousel_Click(object sender, EventArgs e)
        {

        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
