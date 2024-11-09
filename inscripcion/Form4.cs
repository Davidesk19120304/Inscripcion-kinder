using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;
using Guna.UI2.AnimatorNS;

namespace inscripcion
{
    public partial class Form4 : Form
    {
        private List<Image> images = new List<Image>
      {
        Properties.Resources.WhatsApp_Image_2024_10_30_at_16_29_14,
        Properties.Resources.WhatsApp_Image_2024_10_19_at_14_20_42,
        Properties.Resources.WhatsApp_Image_2024_10_23_at_14_30_10
      };

        private int currentIndex = 0; // Índice de la imagen actual  
        private Timer timer; // Temporizador para el carrusel  

        public Form4()
        {
            InitializeComponent();

            // Configurar el temporizador  
            timer = new Timer();
            timer.Interval = 3000; // Cambia la imagen cada 3 segundos  
            timer.Tick += Timer_Tick; // Evento cuando el temporizador "tic"  
            timer.Start(); // Inicia el temporizador  

            // Muestra la primera imagen  
            LoadImage(currentIndex);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Cambia a la siguiente imagen  
            NextImage();
        }

        private void LoadImage(int index)
        {
            // Carga la nueva imagen en el PictureBox  
            pictureBox1.Image = images[index];
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void NextImage()
        {
            // Incrementa el índice y lo reinicia si supera la cantidad de imágenes  
            currentIndex = (currentIndex + 1) % images.Count;
            LoadImage(currentIndex); // Carga la nueva imagen  
        }

        private void constanciaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Implementar lógica si es necesario
        }

        private void guna2ContextMenuStrip1_Click(object sender, EventArgs e)
        {
            // Implementar lógica si es necesario
        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            guna2ContextMenuStrip1.Show(guna2GradientButton1, new Point(0, guna2GradientButton1.Height));
        }

        private void guna2GradientButton2_Click(object sender, EventArgs e)
        {
            guna2ContextMenuStrip2.Show(guna2GradientButton2, new Point(0, guna2GradientButton2.Height));
        }

        private void guna2GradientButton3_Click(object sender, EventArgs e)
        {
            guna2ContextMenuStrip3.Show(guna2GradientButton3, new Point(0, guna2GradientButton3.Height));
        }

        private void guna2GradientButton4_Click(object sender, EventArgs e)
        {
            guna2ContextMenuStrip4.Show(guna2GradientButton4, new Point(0, guna2GradientButton4.Height));
        }

        private void registrarToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            registroalumno registroalumno = new registroalumno();
            registroalumno.Show();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
           
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // se debe actualizar la hora en los labels (si es necesario)
            guna2HtmlLabel1.Text = DateTime.Now.ToString("hh:mm:ss");
            guna2HtmlLabel2.Text = DateTime.Now.ToLongDateString();
        }

        private void guna2GradientPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void timer3_Tick(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

        }

        private void guna2ContextMenuStrip2_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void registrarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            registrardocente registrardocente = new registrardocente();
            registrardocente.Show();
        }

        private void guna2ContextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
        }

        private void buscarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            buscardocente buscardocente = new buscardocente();
            buscardocente.Show();
        }
    }
}
