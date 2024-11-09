using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using AForge.Video;
using AForge.Video.DirectShow;

namespace inscripcion
{
    public partial class registroalumno : Form
    {
        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource;

        public registroalumno()
        {
            InitializeComponent();
            IniciarCamara();
        }

        private void IniciarCamara()
        {
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (videoDevices.Count > 0)
            {
                foreach (FilterInfo device in videoDevices)
                {
                    comboBox5.Items.Add(device.Name);
                }
                if (comboBox5.Items.Count > 0)
                {
                    comboBox5.SelectedIndex = 0;
                    SeleccionarCamara(comboBox5.SelectedIndex);
                }
            }
            else
            {
                MessageBox.Show("No se encontró ninguna cámara. Asegúrate de que esté conectada.");
            }
        }

        private void SeleccionarCamara(int index)
        {
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.SignalToStop();
                videoSource.WaitForStop();
            }

            videoSource = new VideoCaptureDevice(videoDevices[index].MonikerString);
            videoSource.NewFrame += new NewFrameEventHandler(video_NewFrame);
            videoSource.Start();
        }

        private void video_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();
            guna2PictureBox1.Image = bitmap;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.SignalToStop();
                videoSource.WaitForStop();
            }
            base.OnFormClosing(e);
        }

        private void GuardarFotoEnBaseDatos(Bitmap imagen)
        {
            byte[] imagenBytes;
            using (MemoryStream ms = new MemoryStream())
            {
                imagen.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                imagenBytes = ms.ToArray();
            }

            string conexion = "Server=localhost;Database=kinder;User ID=root;Password=;";
            using (MySqlConnection conn = new MySqlConnection(conexion))
            {
                conn.Open();
                string query = "INSERT INTO ninos (doc_foto) VALUES (@imagen)";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@imagen", imagenBytes);
                    cmd.ExecuteNonQuery();
                }
            }
        }


        private void guna2Button2_Click_1(object sender, EventArgs e)
        {
            if (guna2PictureBox1.Image != null)
            {
                Bitmap imagenCapturada = (Bitmap)guna2PictureBox1.Image.Clone();
                GuardarFotoEnBaseDatos(imagenCapturada);
                MessageBox.Show("Foto guardada correctamente en la base de datos.");
            }
            else
            {
                MessageBox.Show("No hay imagen para guardar.");
            }
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            SeleccionarCamara(comboBox5.SelectedIndex);
        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
