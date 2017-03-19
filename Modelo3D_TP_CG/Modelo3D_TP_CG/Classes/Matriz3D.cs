using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace matriz3D {

    class Matriz3D {
        private float[,] m = new float[4 , 4];
        
        public float get_m(int i,int j) {
            return m[i , j];
        }

        public void set_m(int i,int j,float numero) {
            this.m[i , j] = numero;
        }

        #region construtores
        public Matriz3D() {
            for(int i = 0 ; i < 4 ; i++) {
                for(int j = 0 ; j < 4 ; j++) {
                    m[i , j]= 0.0f;
                }
            }
        }

        public Matriz3D(float m00 , float m01 , float m02 , float m03 , float m10 , float m11 , float
            m12 , float m13 , float m20 , float m21 , float m22 , float m23 , float m30 , float m31 , float
            m32 , float m33) {
            this.m[0 , 0] = m00;
            this.m[0 , 1] = m01;
            this.m[0 , 2] = m02;
            this.m[0 , 3] = m03;
            this.m[1 , 0] = m10;
            this.m[1 , 1] = m11;
            this.m[1 , 2] = m12;
            this.m[1 , 3] = m13;
            this.m[2 , 0] = m20;
            this.m[2 , 1] = m21;
            this.m[2 , 2] = m22;
            this.m[2 , 3] = m23;
            this.m[3 , 0] = m30;
            this.m[3 , 1] = m31;
            this.m[3 , 2] = m32;
            this.m[3 , 3] = m33;

        }

        public Matriz3D(float[,] m) {
            for(int i = 0 ; i < 4; i++) {
                for(int j = 0 ; j < 4 ; j++) {
                    this.m[i , j] = m[i,j];
                }
            }
        }

        #endregion

        public void Identidade() {
            for(int i = 0 ; i < 4 ; i++) {
                for(int j = 0 ; j < 4 ; j++) {
                    if(j == i) m[i , j] = 1.0f;
                    else m[i , j] = 0.0f;
                }
            }
        }

        public static Matriz3D operator *(Matriz3D m1 , Matriz3D m2) {

            Matriz3D r = new Matriz3D();
            float temp=0.0f;

            for(int i = 0 ; i < 4 ; i++) {
                for(int j = 0 ; j < 4 ; j++) {
                    temp = 0.0f;
                    for(int k = 0 ; k < 4 ; k++) {
                        temp +=m1.m[i , k] * m2.m[k , j];
                    }
                    r.m[i , j] = temp;
                }
            }
            return r;
        }

        public float[] multiplicaVector(float[] vector) {

            float[] r = new float[4];
            float temp=0.0f;

            for(int i = 0 ; i < 4 ; i++) {
                temp = 0.0f;
                for(int j = 0 ; j < 4 ; j++) {
                    temp +=m[i , j] * vector[j];
                }
                r[i] = temp;
            }
            return r;
        }

        public static Matriz3D escala(float sx , float sy , float sz) {
            Matriz3D r = new Matriz3D();
            r.Identidade();
            r.set_m(0 , 0 , sx);
            r.set_m(1 , 1 , sy);
            r.set_m(2 , 2 , sz);
            return r;
        }

        public static Matriz3D translacao(float tx , float ty , float tz) {
            Matriz3D r = new Matriz3D();
            r.Identidade();
            r.set_m(0 , 3, tx);
            r.set_m(1 , 3 , ty);
            r.set_m(2 , 3 , tz);
            return r;
        }

        public static Matriz3D rotacaoX(float theta) {
            theta = (float)(Math.PI / 180) * theta;
            Matriz3D r = new Matriz3D();
            r.Identidade();
            r.set_m(1 , 1 ,(float)Math.Cos((double)theta));
            r.set_m(1 , 2 , -(float)Math.Sin((double)theta));
            r.set_m(2 , 1 , (float)Math.Sin((double)theta));
            r.set_m(2 , 2 , (float)Math.Cos((double)theta));
            return r;
        }

        public static Matriz3D rotacaoY(float theta) {
            theta = (float)(Math.PI / 180) * theta;
            Matriz3D r = new Matriz3D();
            r.Identidade();
            r.set_m(0 , 0 , (float)Math.Cos((double)theta));
            r.set_m(0 , 2 , (float)Math.Sin((double)theta));
            r.set_m(2 , 0 , -(float)Math.Sin((double)theta));
            r.set_m(2 , 2 , (float)Math.Cos((double)theta));
            return r;
        }

        public static Matriz3D rotacaoZ(float theta) {
            theta = (float)(Math.PI / 180) * theta;
            Matriz3D r = new Matriz3D();
            r.Identidade();
            r.set_m(0 , 0 , (float)Math.Cos((double)theta));
            r.set_m(0 , 1 , -(float)Math.Sin((double)theta));
            r.set_m(1 , 0 , (float)Math.Sin((double)theta));
            r.set_m(1 , 1 , (float)Math.Cos((double)theta));
            return r;
        }

        public static Matriz3D projParalela() {
            Matriz3D r = new Matriz3D();
            r.Identidade();
            r.set_m(2 , 2 , 0);
            return r;
        }

        public static Matriz3D projPerspectiva(float d) {
            Matriz3D r = new Matriz3D();
            r.Identidade();
            r.set_m(2 , 2 , 0);
            r.set_m(3 , 2 , (1/d));
            return r;
        }

        public void printmatriz() {
            for(int i = 0 ; i < 4 ; i++) {
                for(int j = 0 ; j < 4 ; j++) {
                    Console.Write(m[i , j]+" ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

    }
}
