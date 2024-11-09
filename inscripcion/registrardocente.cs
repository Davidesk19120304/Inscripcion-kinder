using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace inscripcion
{
    public partial class registrardocente : Form
    {
        private int borderRadius = 20;
        private int borderSize = 2;
        private Color borderColor = Color.DarkOliveGreen;
        private bool soloVisualizacion;

        public registrardocente(bool soloVisualizacion = false)
        {
            InitializeComponent();
            this.soloVisualizacion = soloVisualizacion;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Padding = new Padding(borderSize + 1);

            // Configuración inicial dependiendo del modo
            if (soloVisualizacion)
            {
                guna2TextBox1.ReadOnly = true;  // Solo lectura
                guna2TextBox2.ReadOnly = true;
                guna2TextBox3.ReadOnly = true;

                guna2Button4.Visible = false;  // Ocultar botón de guardar en modo visualización
                guna2Button3.Text = "Modificar"; // Cambiar texto del botón
                guna2Button3.Width += guna2Button4.Width; // Ampliar ancho del botón para ocupar espacio vacío
            }
            else
            {
                // Configurar placeholder text
                guna2TextBox1.PlaceholderText = "Cedula de Identidad";
                guna2TextBox2.PlaceholderText = "Nombre";
                guna2TextBox3.PlaceholderText = "Apellido";
            }
        }

        public void CargarDatos(string ciMaestro, string nombre, string apellido)
        {
            // cargar datos en los TextBox
            guna2TextBox1.Text = ciMaestro;
            guna2TextBox2.Text = nombre;
            guna2TextBox3.Text = apellido;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.Style |= 0x20000; // WS_EX_COMPOSITED
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

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.Clear(this.BackColor);
            FormRegionAndBorder(this, borderRadius, e.Graphics, borderColor, borderSize);
        }



        private void guna2Button4_Click(object sender, EventArgs e)
        {
            // Guardar nuevos datos
            string ciMaestro = guna2TextBox1.Text.Trim();
            string nombre = guna2TextBox2.Text.Trim();
            string apellido = guna2TextBox3.Text.Trim();

            if (string.IsNullOrWhiteSpace(ciMaestro) || string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(apellido))
            {
                MessageBox.Show("Por favor, complete todos los campos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (MySqlConnection conexion = new MySqlConnection("Server=localhost;Database=kinder;Uid=root;Pwd=;"))
                {
                    conexion.Open();

                    // Verificar si el CI_maestro ya existe
                    string queryCheck = "SELECT COUNT(*) FROM maestros WHERE CI_maestro = @CI_maestro";
                    MySqlCommand comandoCheck = new MySqlCommand(queryCheck, conexion);
                    comandoCheck.Parameters.AddWithValue("@CI_maestro", ciMaestro);

                    int count = Convert.ToInt32(comandoCheck.ExecuteScalar());

                    if (count > 0)
                    {
                        MessageBox.Show("Este número de cédula ya está registrado.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Insertar nuevo registro
                    string query = "INSERT INTO maestros (CI_maestro, Nombre, Apellido) VALUES (@CI_maestro, @Nombre, @Apellido)";
                    MySqlCommand comando = new MySqlCommand(query, conexion);
                    comando.Parameters.Add(new MySqlParameter("@CI_maestro", ciMaestro));
                    comando.Parameters.Add(new MySqlParameter("@Nombre", nombre));
                    comando.Parameters.Add(new MySqlParameter("@Apellido", apellido));

                    comando.ExecuteNonQuery();
                    MessageBox.Show("Datos guardados correctamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al guardar los datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void registrardocente_Load(object sender, EventArgs e)
        {
            guna2ComboBox1.ForeColor = Color.Black; // Color del texto
            guna2ComboBox1.BackColor = Color.White; // Color de fondo
            guna2ComboBox1.BorderColor = Color.DarkOliveGreen; // Color del borde
            guna2ComboBox1.DropDownStyle = ComboBoxStyle.DropDownList;

            // Limpiar cualquier elemento existente
            guna2ComboBox1.Items.Clear();

            // Agregar elementos al ComboBox
            guna2ComboBox1.Items.Add("V-");
            guna2ComboBox1.Items.Add("E-");

            // Establecer el índice seleccionado por defecto
            if (guna2ComboBox1.Items.Count > 0)
            {
                guna2ComboBox1.SelectedIndex = 0; // Seleccionar el primer elemento
            }
        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Button3_Click_1(object sender, EventArgs e)
        {
            if (soloVisualizacion)
            {
                // Cambiar a modo edición
                guna2Button3.Text = "Guardar Cambios"; // Cambiar texto del botón
                guna2TextBox1.ReadOnly = false; // Hacer editables los TextBox
                guna2TextBox2.ReadOnly = false;
                guna2TextBox3.ReadOnly = false;

                // Ocultar el botón guna2Button4
                guna2Button4.Visible = false;

                // Ajustar el tamaño de guna2Button3 para que ocupe el espacio de ambos botones
                guna2Button3.Width += guna2Button4.Width; // Sumar anchos

                // Ajustar la posición para que quede alineado
                guna2Button3.Location = new Point(guna2Button3.Location.X - guna2Button4.Width, guna2Button3.Location.Y); // Mover a la izquierda para alinearse

                soloVisualizacion = false; // Cambiar a modo edición
            }
            else
            {
                // Confirmar antes de guardar cambios
                DialogResult result = MessageBox.Show(
                    "¿Estás seguro que deseas guardar cambios?",
                    "Confirmación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Guardar cambios en la base de datos
                    string ciMaestro = guna2TextBox1.Text.Trim();
                    string nombre = guna2TextBox2.Text.Trim();
                    string apellido = guna2TextBox3.Text.Trim();

                    if (string.IsNullOrWhiteSpace(ciMaestro) || string.IsNullOrWhiteSpace(nombre) || string.IsNullOrWhiteSpace(apellido))
                    {
                        MessageBox.Show("Por favor, complete todos los campos antes de modificar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    try
                    {
                        using (MySqlConnection conexion = new MySqlConnection("Server=localhost;Database=kinder;Uid=root;Pwd=;"))
                        {
                            conexion.Open();

                            // Actualizar la información del maestro
                            string query = "UPDATE maestros SET Nombre = @Nombre, Apellido = @Apellido WHERE CI_maestro = @CI_maestro";
                            MySqlCommand comando = new MySqlCommand(query, conexion);
                            comando.Parameters.AddWithValue("@CI_maestro", ciMaestro);
                            comando.Parameters.AddWithValue("@Nombre", nombre);
                            comando.Parameters.AddWithValue("@Apellido", apellido);

                            int rowsAffected = comando.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Datos actualizados correctamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                // Volver a modo visualización
                                guna2Button3.Text = "Modificar"; // Cambiar texto del botón
                                guna2TextBox1.ReadOnly = true; // Hacer no editables los TextBox
                                guna2TextBox2.ReadOnly = true;
                                guna2TextBox3.ReadOnly = true;

                                // Restaurar el botón guna2Button4
                                guna2Button4.Visible = true; // Hacer visible el botón de nuevo
                                guna2Button3.Width -= guna2Button4.Width; // Reducir el tamaño de guna2Button3 al tamaño original

                                // Volver a la ubicación original
                                guna2Button3.Location = new Point(guna2Button3.Location.X + guna2Button4.Width, guna2Button3.Location.Y); // Restaurar posición

                                soloVisualizacion = true; // Volver a modo visualización
                            }
                            else
                            {
                                MessageBox.Show("No se encontraron registros para actualizar.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ocurrió un error al actualizar los datos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
