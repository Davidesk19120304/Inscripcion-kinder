using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
namespace inscripcion
{


    public partial class Form1 : Form
    {
        MySqlConnection conexion = new MySqlConnection("Server=localhost;Database=kinder;Uid=root;pwd=;");
        string g;
        private int borderRadius = 20;
        private int borderSize = 2;
        private Color borderColor = Color.AliceBlue;

        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.Padding = new Padding(borderSize + 1);

          
            this.BackColor = Color.AliceBlue;

           
            this.guna2Panel1.BackColor = Color.AliceBlue;


            this.guna2Panel2.BackColor = Color.FromArgb(29, 135, 61);

            guna2TextBox1.PlaceholderText = "Usuario";
            guna2TextBox2.PlaceholderText = "Contraseña";
            guna2TextBox3.PlaceholderText = "Confirmar Contraseña";
            guna2TextBox2.UseSystemPasswordChar = true;
            guna2TextBox3.UseSystemPasswordChar = true;
            guna2TextBox4.PlaceholderText = "Correo Electrónico";


            guna2TextBox1.PlaceholderForeColor = Color.FromArgb(128, 128, 128); 
            guna2TextBox2.PlaceholderForeColor = Color.FromArgb(128, 128, 128);
            guna2TextBox3.PlaceholderForeColor = Color.FromArgb(128, 128, 128);
            guna2TextBox4.PlaceholderForeColor = Color.FromArgb(128, 128, 128);
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

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rectForm = this.ClientRectangle;

          
            e.Graphics.Clear(this.BackColor);

        
            FormRegionAndBorder(this, borderRadius, e.Graphics, borderColor, borderSize);
        }

        private void Form1_Load(object sender, EventArgs e)
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


        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string username = guna2TextBox1.Text.Trim();
            string password = guna2TextBox2.Text.Trim();
            string confirmPassword = guna2TextBox3.Text.Trim();

            // Validaciones de campos
            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("El campo de usuario no puede estar vacío.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (username.Length < 5 || username.Length > 20)
            {
                MessageBox.Show("El nombre de usuario debe tener entre 5 y 20 caracteres.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(username, @"^[a-zA-Z0-9@$!%*?&]+$"))
            {
                MessageBox.Show("El nombre de usuario solo puede contener letras, números y los siguientes caracteres especiales: @, $, !, %, *, ?, &.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(password) || password.Length < 8)
            {
                MessageBox.Show("La contraseña debe tener al menos 8 caracteres.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$"))
            {
                MessageBox.Show("La contraseña debe contener al menos una letra mayúscula, una minúscula, un número y un carácter especial.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Las contraseñas no coinciden. Por favor, verifica.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Verificar y registrar
            try
            {
                using (MySqlConnection conexion = new MySqlConnection("Server=localhost;Database=kinder;Uid=root;pwd=;"))
                {
                    conexion.Open();
                    string checkUserQuery = "SELECT COUNT(*) FROM login WHERE user = @user";
                    MySqlCommand checkUserCommand = new MySqlCommand(checkUserQuery, conexion);
                    checkUserCommand.Parameters.AddWithValue("@user", username);
                    int userCount = Convert.ToInt32(checkUserCommand.ExecuteScalar());

                    if (userCount > 0)
                    {
                        MessageBox.Show("El usuario ya está registrado. Por favor, elija otro nombre de usuario.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    g = guna2RadioButton1.Checked ? "MASCULINO" : "FEMENINO";

                    string hashedPassword = HashPassword(password);
                    string query = "INSERT INTO login(fecha, user, password, genero) VALUES (@fecha, @user, @password, @genero)";
                    MySqlCommand comando = new MySqlCommand(query, conexion);
                    comando.Parameters.AddWithValue("@fecha", guna2DateTimePicker1.Text);
                    comando.Parameters.AddWithValue("@user", username);
                    comando.Parameters.AddWithValue("@password", hashedPassword);
                    comando.Parameters.AddWithValue("@genero", g);

                    comando.ExecuteNonQuery();
                    MessageBox.Show("Información insertada correctamente");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al insertar los datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            Form2 formulario2 = new Form2();


            formulario2.Show(this);
            this.Hide();
        }

        private void guna2HtmlLabel2_Click(object sender, EventArgs e)
        {

        }

        private void guna2TextBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2TextBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void guna2PictureBox6_Click(object sender, EventArgs e)
        {

        }
    }
}
