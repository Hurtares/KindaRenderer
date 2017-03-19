using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using ponto3d;
using System.Drawing;

namespace desenhoFaces
{
    class Face
    {
        ArrayList vertices3D = new ArrayList();
        Vector3D normal = new Vector3D();
        public Face()
        {
        }

        public void setNormal(Vector3D normal) {
            this.normal = normal;
        }

        public Vector3D getNormal() {
            return this.normal;
        }
        public Face(ArrayList vertices3d)
        {
            this.vertices3D = vertices3d;
        }

        public ArrayList getVerticesFace()
        {
            return this.vertices3D;
        }


        public void saveVertice(Vector3D v)
        {
            this.vertices3D.Add(v);
        }

        public PointF[] getVertices2D(float largura, float altura)
        {
           PointF[] pontos2D = new PointF[vertices3D.Count];
            for(int i=0;i<vertices3D.Count;i++)
            {
                Vector3D p = (Vector3D)vertices3D[i];
                pontos2D[i] = p.getPontoViewPort(largura, altura);//converte de coords mundo (janela) para o viewport
            }

            return pontos2D;
        }

        public PointF[] getNormais(float largura,float altura) {
            PointF[] pontos2D = new PointF[2];
            Vector3D a, b, c,centro;
            a= ((Vector3D)vertices3D[1] - (Vector3D)vertices3D[0])*0.5f;
            b = ((Vector3D)vertices3D[2] - (Vector3D)vertices3D[1])*0.5f;
            centro = a + b;
            pontos2D[0] = centro.getPontoViewPort(largura , altura);
            centro = centro + normal;
            pontos2D[1] = centro.getPontoViewPort(largura , altura);
            
            /*pontos2D[0] = normal.getPontoViewPort(largura , altura);
            normal.normalize();
            pontos2D[1] = normal.getPontoViewPort(largura , altura);*/
            return pontos2D;
        }
    }
}
