using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Data;

namespace inscripcion
{
    public partial class buscardocente : Form
    {
        private int borderRadius = 20;
        private int borderSize = 2;
        private Color borderColor = Color.DarkOliveGreen;

        public buscardocente()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.None;
            this.Padding = new Padding(borderSize + 1);
            guna2TextBox1.PlaceholderText = "Buscar...";
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

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.Clear(this.BackColor);
            FormRegionAndBorder(this, borderRadius, e.Graphics, borderColor, borderSize);
        }

        private void CargarDatos()
        {
            string connectionString = "server=localhost;database=kinder;user=root;password=;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT CI_maestro, Nombre, Apellido FROM maestros";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    
                    guna2DataGridView1.DataSource = dataTable;

                    
                    Font regularFont = new Font("Century Gothic", 10, FontStyle.Regular);
                    guna2DataGridView1.DefaultCellStyle.Font = regularFont;
                    guna2DataGridView1.AlternatingRowsDefaultCellStyle.Font = regularFont;
                    guna2DataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Century Gothic", 10, FontStyle.Bold);
                    guna2DataGridView1.RowHeadersDefaultCellStyle.Font = regularFont;

                    
                    guna2DataGridView1.RowTemplate.Height = 25;

                    
                    guna2DataGridView1.ColumnHeadersHeight = 30; 

                   
                    guna2DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                   
                    guna2DataGridView1.ReadOnly = true;

                    
                    guna2DataGridView1.ScrollBars = ScrollBars.Both; 

                    
                    guna2DataGridView1.Refresh();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar datos: " + ex.Message);
                }
            }
        }

        private void CargarDatos(string searchTerm = "")
        {
            string connectionString = "server=localhost;database=kinder;user=root;password=;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT CI_maestro, Nombre, Apellido FROM maestros";
                    if (!string.IsNullOrEmpty(searchTerm))
                    {
                       
                        query += " WHERE CI_maestro LIKE @searchTerm OR Nombre LIKE @searchTerm OR Apellido LIKE @searchTerm";
                    }

                    MySqlCommand command = new MySqlCommand(query, connection);
                    if (!string.IsNullOrEmpty(searchTerm))
                    {
                        command.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");
                    }

                    MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                   
                    guna2DataGridView1.DataSource = dataTable;

                   
                    Font regularFont = new Font("Century Gothic", 10, FontStyle.Regular);
                    guna2DataGridView1.DefaultCellStyle.Font = regularFont;
                    guna2DataGridView1.AlternatingRowsDefaultCellStyle.Font = regularFont;
                    guna2DataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Century Gothic", 10, FontStyle.Bold);
                    guna2DataGridView1.RowHeadersDefaultCellStyle.Font = regularFont;

                   
                    guna2DataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
                    guna2DataGridView1.ColumnHeadersHeight = 35; 

                   
                    guna2DataGridView1.RowTemplate.Height = 25;

                   
                    guna2DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                   
                    guna2DataGridView1.ReadOnly = true;

                    
                    guna2DataGridView1.ScrollBars = ScrollBars.Both;

                    
                    guna2DataGridView1.Refresh();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al cargar datos: " + ex.Message);
                }
            }
        }

        private void buscardocente_Load(object sender, EventArgs e)
        {
            string searchTerm = guna2TextBox1.Text.Trim();
            CargarDatos(searchTerm);
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (guna2DataGridView1.CurrentRow != null)
            {
               
                string ciMaestro = guna2DataGridView1.CurrentRow.Cells["CI_maestro"].Value.ToString();

                
                var confirmResult = MessageBox.Show("¿Estás seguro que deseas eliminar este registro?",
                                                     "Confirmar eliminación",
                                                     MessageBoxButtons.YesNo,
                                                     MessageBoxIcon.Warning);

                if (confirmResult == DialogResult.Yes)
                {
                   
                    string connectionString = "server=localhost;database=kinder;user=root;password=;";
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        try
                        {
                            connection.Open();
                            string query = "DELETE FROM maestros WHERE CI_maestro = @ciMaestro";
                            MySqlCommand command = new MySqlCommand(query, connection);
                            command.Parameters.AddWithValue("@ciMaestro", ciMaestro);
                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Registro eliminado exitosamente.");
                                
                                CargarDatos();
                            }
                            else
                            {
                                MessageBox.Show("No se pudo eliminar el registro.");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error al eliminar el registro: " + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un docente para eliminar.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            string searchTerm = guna2TextBox1.Text.Trim();
            CargarDatos(searchTerm);
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (guna2DataGridView1.CurrentRow != null)
            {
              
                string ciMaestro = guna2DataGridView1.CurrentRow.Cells["CI_maestro"].Value.ToString();
                string nombre = guna2DataGridView1.CurrentRow.Cells["Nombre"].Value.ToString();
                string apellido = guna2DataGridView1.CurrentRow.Cells["Apellido"].Value.ToString();

               
                registrardocente formVisualizacion = new registrardocente(soloVisualizacion: true);

                
                formVisualizacion.CargarDatos(ciMaestro, nombre, apellido);

               
                formVisualizacion.ShowDialog();
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un docente para ver su perfil.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }




}