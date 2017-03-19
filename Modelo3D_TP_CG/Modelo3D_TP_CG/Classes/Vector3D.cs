using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using matriz3D;

namespace ponto3d
{
    class Vector3D
    {
        //dados
        private float x, y, z, w; //cordenadas do ponto
        //métodos

        public Vector3D()
        {
            this.x = this.y = this.z = 0.0f;
            this.w = 1.0f;

        }
        public Vector3D(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = 1.0f;
        }

        public Vector3D(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public float getX()
        {
            return this.x;
        }
        public float getY()
        {
            return this.y;
        }
        public float getZ()
        {
            return this.z;
        }
        public void setX(float x)
        {
            this.x = x;
        }
        public void setY(float y)
        {
            this.y = y;
        }
        public void setZ(float z)
        {
            this.z = z;
        }
        public void set(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        //verifica se o vector recebido por parâmetro é igual ao da classe
        public bool Equals(Vector3D v)
        {

            return (this.x == v.x && this.y == v.y && this.z == v.z && this.w == v.w);
                
        }

        //usar o operador ^ para produto vectorial/extreno entre 2 objectos do tipo Vector3D
        // sobrecarga do operador ^
        public static Vector3D operator^(Vector3D v1, Vector3D v2)
        {
            Vector3D r = new Vector3D();
            r.x = v1.y * v2.z - v2.y * v1.z;
            r.y = v2.x * v1.z - v1.x * v2.z;
            r.z = v1.x * v2.y - v2.x * v1.y;

            return r;
        }

        // usar o operador * para produto escalar/interno de 2 objectos do tipo Vector3D
        //sobrecarga do operador *
        public static float operator*(Vector3D v1, Vector3D v2)
        {
            float r = v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
            return r;
        }
        public Vector3D Clone()
        {
            return new Vector3D(this.x, this.y, this.z);
        }

        //retorna a norma do vector
        public float norm()
        {
            float norma = (float)Math.Sqrt(Math.Pow(this.x, 2) + Math.Pow(this.y, 2) + Math.Pow(this.z, 2));
            return norma;
        }
        //normaliza o vector
        public void normalize()
        {
            float n = norm();
            if(n!=0.0)
            {
                this.x = this.x / n;
                this.y = this.y / n;
                this.z = this.z / n;

            }
        }
        //converter em string
        public override string ToString()
        {

            string str = "(" + this.x + "," + this.y + "," + this.z + "," + this.w + ")";
            return str;
        }

        //usar o operador * para objectos do tipo Vector3D –> sobrecarga do operador *
        public static Vector3D operator *(Vector3D v, float c)
        {
            Vector3D r = new Vector3D(c * v.x, c * v.y, c * v.z);
            return r;

        }
        //Sobrecarga do operador % para Multiplicação de 2 pontos componente a componente
        public static Vector3D operator%(Vector3D v1, Vector3D v2)
        {
            Vector3D r = new Vector3D(v1.x*v2.x,v1.y*v2.y, v1.z*v2.z);

            return r;

        }

        //usar o operador + para objectos do tipo Vector3D –> sobrecarga do operador *
        public static Vector3D operator+(Vector3D v1, Vector3D v2)
        {
            Vector3D r = new Vector3D(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);

            return r;
        }
        //Sobrecarga do operador operador binário - para Subtracção de pontos em 3D;
        public static Vector3D operator-(Vector3D v1, Vector3D v2)
        {
            Vector3D r = new Vector3D(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);

            return r;

        }

        //retorna o simétrico do vector
        //sobrecarga do operador unário -
        public static Vector3D operator-(Vector3D v)
        {
            Vector3D r = new Vector3D(-v.x, -v.y, -v.z);
            return r;
        }

        public PointF getPontoViewPort(float largura, float altura)
        {
            // xv= xvmin + (xw-xwmin)*Sx, com Sx= (xvmax-xvmin)/(xwmax-xwmin)
            // yv= yvmin - (yw-ywmin)*Sy, com Sy= (yvmax-yvmin)/(ywmax-ywmin),
            //Sx=1, Sy=1, xvmin=yvmin=xwmin=ywmin=0
            return new PointF(largura/2+this.x, altura/2 -this.y);
        }


        public void geraCoordHomogeneas(Matriz3D matriz)
        {
            float[] resultado = matriz.multiplicaVector(new float[4] { this.x, this.y, this.z, this.w });
            this.x = resultado[0];
            this.y = resultado[1];
            this.z= resultado[2];
            this.w= resultado[3];
        }

        public void geraCoordCartesianas(Matriz3D matriz)
        {
            float[] resultado = matriz.multiplicaVector(new float[4] { this.x, this.y, this.z, this.w });
            this.x = resultado[0]/resultado[3];
            this.y = resultado[1]/resultado[3];
            this.z = resultado[2];// não dividimos para preservar a informação sobre Z
            this.w = 1.0f;
        }
    }
}
