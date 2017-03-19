using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

using matriz3D;

using System.Drawing;

using ponto3d; //para usar a class Vector3D (definida no namespace ponto3d)

using System.IO; // para usar as streams

namespace desenhoFaces
{
    class Objecto
    {
        // Classe para desenho de objectos representados por faces poligonais; 
        //neste exemplo apenas está a desenhar uma face (cujos valores são inicializados no construtor)
       // mas pode ser estendida para mais faces

        ArrayList vertices; //guarda os vértices
        ArrayList indicesFaces;//índices das faces
        ArrayList numVertFace; //guarda o nº de vértices por cada face
        Pen pen;
        SolidBrush brush;
        float largura;//largura da janela
        float altura;//altura da janela

        Stream s; // stream para carregar o ficheiro, no caso do objecto ser lido a partir de um ficheiro 

        bool perspetiva = false;
        bool mostrarFaces = false;
        int facesDesenhadas = 0;
        int totalFaces = 0;

        public Objecto()
        {
            inicializaObjecto();
        }

       public Objecto(float largura, float altura)
        {
            setJanela(largura, altura);
            inicializaObjecto();
            
        }
        void inicializaObjecto()
        {

            vertices = new ArrayList();
            indicesFaces = new ArrayList();
            numVertFace = new ArrayList();

            pen = new Pen(Color.Black);
            brush = new SolidBrush(Color.AliceBlue);

            //valores atribuídos manualmente

            vertices.Add(new Vector3D(50.0f , -50.0f , 0.0f));
            vertices.Add(new Vector3D(25.0f , -50.0f , -43.0f));
            vertices.Add(new Vector3D(-25.0f , -50.0f , -43.0f));
            vertices.Add(new Vector3D(-50.0f , -50.0f , 0.0f));
            vertices.Add(new Vector3D(-25.0f , -50.0f , 43.0f));
            vertices.Add(new Vector3D(25.0f , -50.0f , 43.0f));
            vertices.Add(new Vector3D(0.0f , 50.0f , 0.0f));


            indicesFaces.Add(0);
            indicesFaces.Add(5);
            indicesFaces.Add(4);
            indicesFaces.Add(3);
            indicesFaces.Add(2);
            indicesFaces.Add(1);

            indicesFaces.Add(0);
            indicesFaces.Add(1);
            indicesFaces.Add(6);

            indicesFaces.Add(1);
            indicesFaces.Add(2);
            indicesFaces.Add(6);

            indicesFaces.Add(2);
            indicesFaces.Add(3);
            indicesFaces.Add(6);

            indicesFaces.Add(3);
            indicesFaces.Add(4);
            indicesFaces.Add(6);

            indicesFaces.Add(4);
            indicesFaces.Add(5);
            indicesFaces.Add(6);

            indicesFaces.Add(5);
            indicesFaces.Add(0);
            indicesFaces.Add(6);

            numVertFace.Add(6);
            numVertFace.Add(3);
            numVertFace.Add(3);
            numVertFace.Add(3);
            numVertFace.Add(3);
            numVertFace.Add(3);
            numVertFace.Add(3);


        }

        public ArrayList getVertices()
        {
            return this.vertices;
        }

        public ArrayList getIndicesFaces()
        {
            return this.indicesFaces;
        }

        public ArrayList getnumVertFace()
        {
            return this.numVertFace;
        }

        public void setJanela(float largura, float altura)
        {
            this.largura = largura;
            this.altura = altura;
        }

        public void setCores(Pen p1, SolidBrush b1)
        {
            this.pen = p1;
            this.brush = b1;
        }

        public void setPerspetiva() {
            this.perspetiva = !this.perspetiva;
        }

        public void setMostrarFaces() {
            this.mostrarFaces = !this.mostrarFaces;
        }

        public int getTotalFaces() {
            return this.totalFaces;
        }

        public int getFacesDesenhadas() {
            return this.facesDesenhadas;
        }

        public ArrayList transforma(float translacaoX, float translacaoY, float translacaoZ,float rotX,float rotY,float rotZ)
        {
            ArrayList res = new ArrayList(vertices.Count);
            int distancia = 400;
            //cria uma cópia dos vértices originais
            foreach(Vector3D p in this.vertices)
            {
                res.Add(p.Clone());
            }

            // Matriz3D mTrans = Matriz3D.translacao(translacaoX,translacaoY,translacaoz);//construir a matriz translaçao
            // Matriz3D mProjParalela = Matriz3D.projParalela();

            Matriz3D mTrans = Matriz3D.translacao(translacaoX, translacaoY, translacaoZ);
            Matriz3D mProjParalela = Matriz3D.projParalela();
            Matriz3D mProjPerspetiva = Matriz3D.projPerspectiva(distancia);//a câmara está em z=400
            Matriz3D rot_X = Matriz3D.rotacaoX(rotX);
            Matriz3D rot_Y = Matriz3D.rotacaoY(rotY);
            Matriz3D rot_Z = Matriz3D.rotacaoZ(rotZ);
            rot_X.printmatriz();
            rot_Y.printmatriz();
            rot_Z.printmatriz();
            for (int i=0; i<res.Count;i++)
            {
                Vector3D p = (Vector3D)res[i];
                p.geraCoordHomogeneas(mTrans);
                p.geraCoordHomogeneas(rot_X);
                p.geraCoordHomogeneas(rot_Y);
                p.geraCoordHomogeneas(rot_Z);
                if(perspetiva)
                    p.geraCoordCartesianas(mProjPerspetiva);
                else
                    p.geraCoordCartesianas(mProjParalela);

            }
            return res;
        }

        public ArrayList geraFaces(ArrayList vertTransformados)
        {
            ArrayList faces = new ArrayList();
            int k = 0;
            facesDesenhadas = 0;
            totalFaces = 0;
            for (int i=0;i<numVertFace.Count;i++)// até nº total faces
            {
                ArrayList vertices = new ArrayList();
                Vector3D normal = new Vector3D();
                Face f = new Face();
                for(int j=0;j<(int)numVertFace[i]; j++)//para cada face, até ao nº vértices face
                {
                    //é aqui
                    Vector3D p = (Vector3D)vertTransformados[(int)indicesFaces[k++]];
                    f.saveVertice(p);

                }
                vertices = f.getVerticesFace();
                normal = ((Vector3D)vertices[1] - (Vector3D)vertices[0]) ^ ((Vector3D)vertices[2] - (Vector3D)vertices[1]);
                f.setNormal(normal);
                if(normal.getZ() >= 0 || mostrarFaces) {
                    faces.Add(f);
                    facesDesenhadas++;
                }

                totalFaces++;   
                    
            }
            Face temp = new Face();

            for(int write = 0 ; write < faces.Count ; write++) {
                for(int sort = 0 ; sort < faces.Count - 1 ; sort++) {
                    Face f =(Face)faces[sort];
                    Face f1 = (Face)faces[sort+1];
                    if(f.getNormal().getZ() > f.getNormal().getZ()) {
                        temp = (Face)faces[sort + 1];
                        faces[sort + 1] = faces[sort];
                        faces[sort] = temp;
                    }
                }
            }
            return faces;
        }

        public void desenha(Graphics g, float transX, float transY, float transZ,float rotX,float rotY,float rotZ)
        {
            ArrayList vTransf=transforma(transX, transY, transZ,rotX,rotY,rotZ);// calcula os novos vértices

            ArrayList faces=geraFaces(vTransf);// gera as faces a desenhar com base nos vértices atualizados

            for(int i=0; i<faces.Count;i++)//percorre face a face e desenha
            {

                Face f = (Face)faces[i];
                    g.FillPolygon(brush, f.getVertices2D(largura, altura));//preenche
                g.DrawPolygon(pen, f.getVertices2D(largura, altura));//desenha as linhas
                g.DrawLine(pen , f.getNormais(largura , altura)[0], f.getNormais(largura , altura)[1]);

            }
        }

        /* ******************************************************************** */
        
        //LEITURA do objecto a partir de um ficheiro txt

        //cria um objeto a partir dos dados do ficheiro
        public Objecto(float largura, float altura, Stream s)
        {
            // construtor para criar objecto a partir de um ficheiro
            this.s = s;
            leFicheiro();  // função que lê a estrutura do objecto a partir de um ficheiro
            setJanela(largura, altura);

        }
        //para atualizar um objeto já criado
        public void setObjecto(float largura, float altura, Stream s,string nomeficheiro) // construtor para criar objecto a partir de um ficheiro
        {
            this.s = s;
            if(nomeficheiro.Last() == 'j')
                leOBJ();
            else
                leFicheiro();  // função que lê a estrutura do objecto a partir de um ficheiro
            setJanela(largura, altura);

        }

        //ler os dados do ficheiro (que contém a estrutura do objeto)
        private void leFicheiro()
        {
            StreamReader readerObjecto = new StreamReader(this.s);
            //ler, do ficheiro, a estrutura do objecto (coordenadas vértices, índices faces, ...) para as estruturas de dados da classe Objecto
            
            string linha = "";
            ArrayList pontos = new ArrayList();
            ArrayList indices = new ArrayList();

            while (linha != null)
            {
                linha = readerObjecto.ReadLine(); // lê uma linha do ficheiro
                if (linha != null)
                {
                    if (linha.StartsWith("v")) // se a linha inicia por "v"
                        pontos.Add(linha);     // adiciona ao array para guardar os vértices
                    if (linha.StartsWith("f")) // se a linha inicia por "f"
                        indices.Add(linha);      // adiciona ao array para guardar os índices dos vértices das faces
                }

            }
            readerObjecto.Close();

            // agora que todas as linhas estão guardadas, é necessário extrair delas as coordenadas individuais

            char[] separador = { ' ' }; // definir o separador: neste caso é o caractere espaço
            string[] coordenadas;

            foreach (string l in pontos)
            {
                coordenadas = l.Split(separador);

                // System.Windows.Forms.MessageBox.Show("" + float.Parse(coordenadas[1], System.Globalization.CultureInfo.InvariantCulture));

                this.vertices.Add(new Vector3D(float.Parse(coordenadas[1], System.Globalization.CultureInfo.InvariantCulture),
                float.Parse(coordenadas[2], System.Globalization.CultureInfo.InvariantCulture),
                float.Parse(coordenadas[3], System.Globalization.CultureInfo.InvariantCulture)));
            }

            string[] indicesVert;
            foreach (string l in indices)
            {
                indicesVert = l.Split(separador);
                numVertFace.Add(indicesVert.Length - 1);
                for(int i = 1 ; i < indicesVert.Length ; i++) {
                    //indicesFaces
                    this.indicesFaces.Add(int.Parse(indicesVert[i]));
                }
            }

        }
        private void leOBJ() {
            StreamReader readerObjecto = new StreamReader(this.s);
            //ler, do ficheiro, a estrutura do objecto (coordenadas vértices, índices faces, ...) para as estruturas de dados da classe Objecto
            string linha = "";
            ArrayList pontos = new ArrayList();
            ArrayList indices = new ArrayList();

            while(linha != null) {
                linha = readerObjecto.ReadLine(); // lê uma linha do ficheiro
                if(linha != null) {
                    if(linha.StartsWith("v ")) // se a linha inicia por "v"
                        pontos.Add(linha);     // adiciona ao array para guardar os vértices
                    if(linha.StartsWith("f ")) // se a linha inicia por "f"
                        indices.Add(linha);      // adiciona ao array para guardar os índices dos vértices das faces
                }

            }
            readerObjecto.Close();
            // agora que todas as linhas estão guardadas, é necessário extrair delas as coordenadas individuais
            char[] separador = { ' ' }; // definir o separador: neste caso é o caractere espaço
            string[] coordenadas;

            foreach(string l in pontos) {
                coordenadas = l.Split(separador);
                // System.Windows.Forms.MessageBox.Show("" + float.Parse(coordenadas[1], System.Globalization.CultureInfo.InvariantCulture));
                this.vertices.Add(new Vector3D(float.Parse(coordenadas[1] , System.Globalization.CultureInfo.InvariantCulture) ,
                float.Parse(coordenadas[2] , System.Globalization.CultureInfo.InvariantCulture) ,
                float.Parse(coordenadas[3] , System.Globalization.CultureInfo.InvariantCulture)));
            }

            string[] indicesVert;
            string[] splited;
            foreach(string l in indices) {
                indicesVert = l.Split(separador);
                numVertFace.Add(indicesVert.Length - 1);
                for(int i = indicesVert.Length-1 ; i > 0 ; i--) {
                    splited = indicesVert[i].Split('/');
                    //indicesFaces
                    this.indicesFaces.Add(int.Parse(splited[0])-1);
                }
            }

        }

    }//fecha classe
}//fecha namespace
