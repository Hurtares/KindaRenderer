using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using desenhoFaces;
using System.IO;

namespace Modelo3D_TP_CG {
    public partial class Form1 : Form {

        Objecto objA;
        Pen penA;
        SolidBrush brushA;
        Objecto objB;
        Pen penB;
        SolidBrush brushB;

        string nomeFicheiroA, nomeFicheiroB;

        public Form1() {
            InitializeComponent();

            // criar e inicializar objecto
            float larguraA = this.pictureBoxA.Width;
            float alturaA = this.pictureBoxA.Height;
            objA = new Objecto(larguraA , alturaA);

            //criar e configurar a pen e o brush
            penA = new Pen(this.corContorno_A.BackColor);
            brushA = new SolidBrush(this.corObj_A.BackColor);

            // atribuir a pen e o brush ao objecto
            objA.setCores(penA , brushA);
            // criar e inicializar objecto
            float largura_B = this.pictureBoxA.Width;
            float altura_B = this.pictureBoxA.Height;
            objB = new Objecto(largura_B , altura_B);

            //criar e configurar a pen e o brush
            penB = new Pen(this.corContorno_B.BackColor);
            brushB = new SolidBrush(this.corObj_B.BackColor);

            // atribuir a pen e o brush ao objecto
            objB.setCores(penB , brushB);
        }

        private void pictureBoxA_Paint(object sender , PaintEventArgs e) {
            Graphics g = e.Graphics;
            if(objA != null)
                //desenha
                objA.desenha(g , tbX_A.Value , tbY_A.Value , tbZ_A.Value , rotX_A.Value , rotY_A.Value , rotZ_A.Value);
            total_A.Text = "Total: " + objA.getTotalFaces();
            desen_A.Text = "Desenhadas: " + objA.getFacesDesenhadas();
        }

        private void pictureBoxB_Paint(object sender , PaintEventArgs e) {
            Graphics g = e.Graphics;
            if(objB != null)
                //desenha
                objB.desenha(g , tbX_B.Value , tbY_B.Value , tbZ_B.Value , rotX_B.Value , rotY_B.Value , rotZ_B.Value);
            total_B.Text = "Total: " + objB.getTotalFaces();
            desen_B.Text = "Desenhadas: " + objB.getFacesDesenhadas();
        }

        private void btn_corObj_A_Click(object sender , EventArgs e) {
            ColorDialog dialogo = new ColorDialog();
            if(dialogo.ShowDialog() == DialogResult.OK) {

                this.corObj_A.BackColor = dialogo.Color;
                if(objA != null) {
                    setCores();
                    this.pictureBoxA.Invalidate();
                }
            }
        }

        private void btn_corContorno_A_Click(object sender , EventArgs e) {
            ColorDialog dialogo = new ColorDialog();
            if(dialogo.ShowDialog() == DialogResult.OK) {

                this.corContorno_A.BackColor = dialogo.Color;
                if(objA != null) {
                    setCores();
                    this.pictureBoxA.Invalidate();
                }
            }
        }

        private void btn_corObj_B_Click(object sender , EventArgs e) {
            ColorDialog dialogo = new ColorDialog();
            if(dialogo.ShowDialog() == DialogResult.OK) {

                this.corObj_B.BackColor = dialogo.Color;
                if(objB != null) {
                    setCores();
                    this.pictureBoxB.Invalidate();
                }
            }
        }

        private void btn_corContorno_B_Click(object sender , EventArgs e) {
            ColorDialog dialogo = new ColorDialog();
            if(dialogo.ShowDialog() == DialogResult.OK) {

                this.corContorno_B.BackColor = dialogo.Color;
                if(objB != null) {
                    setCores();
                    this.pictureBoxB.Invalidate();
                }
            }
        }

        private void Form1_Resize(object sender , EventArgs e) {
            if(objA != null) {
                objA.setJanela(this.pictureBoxA.Width , this.pictureBoxA.Height);
                this.pictureBoxA.Invalidate();
            }
            if(objB != null) {
                objB.setJanela(this.pictureBoxB.Width , this.pictureBoxB.Height);
                this.pictureBoxA.Invalidate();
            }
        }

        private void setCores() {
            if(penA != null && brushA != null) {
                brushA.Color = this.corObj_A.BackColor;
                penA.Color = this.corContorno_A.BackColor;
                penA.Width = (float)this.espec_A.Value;
                objA.setCores(penA , brushA);

            }
            if(penB != null && brushB != null) {
                brushB.Color = this.corObj_B.BackColor;
                penB.Color = this.corContorno_B.BackColor;
                penB.Width = (float)this.espec_B.Value;
                objB.setCores(penB , brushB);

            }
        }

        private void espec_A_ValueChanged(object sender , EventArgs e) {
            if(objA != null) {
                setCores();
                this.pictureBoxA.Invalidate();
            }
        }

        private void espec_B_ValueChanged(object sender , EventArgs e) {
            if(objB != null) {
                setCores();
                this.pictureBoxB.Invalidate();
            }
        }

        

        private void importarAToolStripMenuItem_Click(object sender , EventArgs e) {
            Stream str;
            OpenFileDialog fileDialog = new OpenFileDialog();

            fileDialog.Filter = "Text files (*.txt;*.obj)|*.txt;*.obj|All files (*.*)|*.*";
            fileDialog.RestoreDirectory = true;

            if(fileDialog.ShowDialog() == DialogResult.OK) 
                try {
                    if((str = fileDialog.OpenFile()) != null) {

                        using(str) {
                            if(objA != null)//se já existe um objeto, atualiza-se esse objecto
                            {
                                objA.getVertices().Clear(); // apagar as coordenadas dos vértices
                                objA.getIndicesFaces().Clear();// apagar os índices dos vértices
                                objA.getnumVertFace().Clear();    // apagar o conteúdo do array numVertFace                       
                                objA.setObjecto(this.pictureBoxA.Width , this.pictureBoxA.Height , str,fileDialog.SafeFileName); // carregar novo objecto a partir de Ficheiro

                            }
                            nomeFicheiroA = fileDialog.SafeFileName;
                            obj_A.Text = nomeFicheiroA;
                            setCores();
                            //check o wireframe

                            this.pictureBoxA.Invalidate(); // voltar a redesenhar a picture box
                        }

                    }
                }
                catch(Exception ex) {
                    MessageBox.Show("Erro: Não foi possível ler o ficheiro! \n Origem do Erro:" + ex.Message);
                }
        }

        

        private void importarBToolStripMenuItem_Click(object sender , EventArgs e) {
            Stream str;
            OpenFileDialog fileDialog = new OpenFileDialog();

            fileDialog.Filter = "Text files (*.txt;*.obj)|*.txt;*.obj|All files (*.*)|*.*";
            fileDialog.RestoreDirectory = true;

            if(fileDialog.ShowDialog() == DialogResult.OK)
                try {
                    if((str = fileDialog.OpenFile()) != null) {

                        using(str) {
                            if(objB != null)//se já existe um objeto, atualiza-se esse objecto
                            {
                                objB.getVertices().Clear(); // apagar as coordenadas dos vértices
                                objB.getIndicesFaces().Clear();// apagar os índices dos vértices
                                objB.getnumVertFace().Clear();    // apagar o conteúdo do array numVertFace                       
                                objB.setObjecto(this.pictureBoxB.Width , this.pictureBoxB.Height , str, fileDialog.SafeFileName); // carregar novo objecto a partir de Ficheiro

                            }
                            nomeFicheiroB = fileDialog.SafeFileName;
                            obj_B.Text = nomeFicheiroB;
                            setCores();
                            //check o wireframe

                            this.pictureBoxB.Invalidate(); // voltar a redesenhar a picture box
                        }

                    }
                }
                catch(Exception ex) {
                    MessageBox.Show("Erro: Não foi possível ler o ficheiro! \n Origem do Erro:" + ex.Message);
                }
        }
        private void sairToolStripMenuItem_Click(object sender , EventArgs e) {
            Application.Exit();
        }

        private void tbX_A_Scroll(object sender , EventArgs e) {
            this.pictureBoxA.Invalidate();
        }

        private void tbY_A_Scroll(object sender , EventArgs e) {
            this.pictureBoxA.Invalidate();
        }

        private void tbZ_A_Scroll(object sender , EventArgs e) {
            this.pictureBoxA.Invalidate();
        }

        private void rotX_A_Scroll(object sender , EventArgs e) {
            this.pictureBoxA.Invalidate();
        }

        private void rotY_A_Scroll(object sender , EventArgs e) {
            this.pictureBoxA.Invalidate();
        }

        private void rotZ_A_Scroll(object sender , EventArgs e) {
            this.pictureBoxA.Invalidate();
        }

        private void tbX_B_Scroll(object sender , EventArgs e) {
            this.pictureBoxB.Invalidate();
        }

        private void tbY_B_Scroll(object sender , EventArgs e) {
            this.pictureBoxB.Invalidate();
        }

        private void tbZ_B_Scroll(object sender , EventArgs e) {
            this.pictureBoxB.Invalidate();
        }

        private void rotX_B_Scroll(object sender , EventArgs e) {
            this.pictureBoxB.Invalidate();
        }

        private void rotY_B_Scroll(object sender , EventArgs e) {
            this.pictureBoxB.Invalidate();
        }

        private void rotZ_B_Scroll(object sender , EventArgs e) {
            this.pictureBoxB.Invalidate();
        }

        private void proj_A_CheckedChanged(object sender , EventArgs e) {
            objA.setPerspetiva();
            this.pictureBoxA.Invalidate();
        }

        private void proj_B_CheckedChanged(object sender , EventArgs e) {
            objB.setPerspetiva();
            this.pictureBoxB.Invalidate();
        }

        private void faces_A_CheckedChanged(object sender , EventArgs e) {
            objA.setMostrarFaces();
            this.pictureBoxA.Invalidate();
        }

        private void faces_B_CheckedChanged(object sender , EventArgs e) {
            objB.setMostrarFaces();
            this.pictureBoxB.Invalidate();
        }

        private void norm_A_CheckedChanged(object sender , EventArgs e) {

        } 
               
        private void norm_B_CheckedChanged(object sender , EventArgs e) {

        }

    }
}
