using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace deteccion_de_bordes
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "allfiles|*.jpg;*.png;*.jpeg";
            dialog.Multiselect = false;
            if(dialog.ShowDialog() == DialogResult.OK)
            {
                String path = dialog.FileName;
                Image img = new Bitmap(path);
                Bitmap source = (Bitmap)img.Clone();


                pictureBox1.Image = img;
                int ceros_height = img.Height + 2;
                int ceros_width = img.Width + 2;

                int[,] ceros = new int[ceros_height,ceros_width];
                Bitmap gray = BinaryImage(source,177);
                Bitmap bordes = (Bitmap)img.Clone();

                pictureBox2.Image = gray;
                Color col;


                for(int fila=1;fila<ceros_height-1;fila++)
                {
                    for(int columna=1;columna<ceros_width-1;columna++)
                    {
                        
                        col = gray.GetPixel(columna - 1, fila - 1);
                        
                        if(col.R==255)
                        {
                            ceros[fila, columna] = 1;
                        }
                        else
                        {
                            ceros[fila, columna] = 0;
                        }
                        
                    }
                }
               

                int acumulador = 0;

                for (int filas = 1; filas < ceros_height - 1; filas++)
                {
                    for (int columnas = 1; columnas < ceros_width - 1; columnas++)
                    {
                        acumulador = 0;
                        acumulador = (ceros[filas - 1, columnas - 1] * -1) +
                                     (ceros[filas - 1, columnas] * -1) +
                                     (ceros[filas - 1, columnas + 1] * -1) +
                                     (ceros[filas, columnas - 1] * -1) +
                                     (ceros[filas, columnas] * 8) +
                                     (ceros[filas, columnas + 1] * -1) +
                                     (ceros[filas + 1, columnas - 1] * -1) +
                                     (ceros[filas + 1, columnas] * -1) +
                                     (ceros[filas + 1, columnas + 1] * -1);

                        if (acumulador > .5)
                        {
                            bordes.SetPixel(columnas - 1, filas - 1, Color.FromArgb(255, 255, 255));
                        }
                        else
                        {
                            bordes.SetPixel(columnas - 1, filas - 1, Color.FromArgb(0, 0, 0));
                        }



                    }
                }

                Image border = bordes;
                pictureBox3.Image = border;








            }
        }


        public Bitmap BinaryImage(Bitmap source, int umb)
        {
            // Bitmap con la imagen binaria
            Bitmap target = new Bitmap(source.Width, source.Height, source.PixelFormat);
            // Recorrer pixel de la imagen
            for (int i = 0; i < source.Width; i++)
            {
                for (int e = 0; e < source.Height; e++)
                {
                    // Color del pixel
                    Color col = source.GetPixel(i, e);
                    // Escala de grises
                    byte gray = (byte)(col.R * 0.3f + col.G * 0.59f + col.B * 0.11f);
                    // Blanco o negro
                    byte value = 0;
                    if (gray > umb)
                    {
                        value = 255;
                    }
                    // Asginar nuevo color
                    Color newColor = System.Drawing.Color.FromArgb(value, value, value);
                    target.SetPixel(i, e, newColor);

                }
            }

            return target;
        }
    }
}
