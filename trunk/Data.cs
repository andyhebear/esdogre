using System;
using System.Collections.Generic;
using System.IO;
using Mogre;

namespace OgreDem
{
    public class DemData
    {
        string filename;
        /// <summary>
        /// 记录数
        /// </summary>
        int recordno;
        int demcol;
       public Vector3 min = new Vector3();
        public Vector3 max = new Vector3();
        public List<List<int>> data = new List<List<int>>();
        public DemData(string filename)
        {
            using (FileStream stream = new FileStream(filename, FileMode.Open))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    //读取文件名
                    this.filename = reader.ReadLine().Trim();
                    this.recordno = Convert.ToInt32(reader.ReadLine().Trim());
                    for (int i = 0; i < recordno; i++)
                    {
                        string strline = reader.ReadLine();
                        var words = new List<string>(strline.Split(' '));
                        words.RemoveAll(r => string.IsNullOrEmpty(r));
                        demcol = Convert.ToInt32(words[1]);
                        words.RemoveRange(0, 3);
                        List<int> tempdate = new List<int>();
                        words.ForEach(r => tempdate.Add(Convert.ToInt32(r)));
                        
                        data.Add(tempdate);
                    }
                    reader.Close();
                }
                stream.Close();
            }
        }
        public float[] CreateVertices(int row, int col, int step, out int vbufCount)
        {

            int count = 0;
            // const int nVertices = 8;
            vbufCount = row * col * 5;
            float txf = 1f / row;
            float txt = 1f / col;
            float[] vertices = new float[vbufCount];
            Random r = new Random();
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    vertices[count] = j * step;//x
                    vertices[count + 1] =i * step; //r.Next(100);//y
                    vertices[count + 2] =data[i][j]/10; //z
                    if (vertices[count + 2] < -10) {
                        vertices[count + 2] = -10;
                    }


                    vertices[count + 3] = j * txf;//tx
                    vertices[count + 4] = i * txt;//ty

                    if (i == 0 && j == 0)
                    {
                        min.x = vertices[count];
                        min.y = vertices[count + 1];
                        min.z = vertices[count + 2];

                        max.x = vertices[count];
                        max.y = vertices[count + 1];
                        max.z = vertices[count + 2];
                    }
                    else
                    {
                        min.x = System.Math.Min(min.x, vertices[count]);
                        min.y = System.Math.Min(min.y, vertices[count + 1]);
                        min.z = System.Math.Min(min.z, vertices[count + 2]);

                        max.x = System.Math.Max(max.x, vertices[count]);
                        max.y = System.Math.Max(max.x, vertices[count + 1]);
                        max.z = System.Math.Max(max.x, vertices[count + 2]);
                    }

                    count += 5;
                }
            }
            return vertices;
        }
        public ushort[] CreateFaces(int row, int col, out uint ibufCount)
        {
            int count = 0;
            ibufCount = (uint)((row - 1) * (col - 1) * 2 * 3 * 2);
            ushort[] faces = new ushort[(row - 1) * (col - 1) * 2 * 3 * 2];
            for (int i = 0; i < row - 1; i++)
            {
                for (int j = 0; j < col - 1; j++)
                {
                    faces[count] = (ushort)(i * row + j);
                    faces[count + 1] = (ushort)((i + 1) * row + j);
                    faces[count + 2] = (ushort)((i + 1) * row + j + 1);

                    faces[count + 3] = (ushort)(i * row + j);
                    faces[count + 4] = (ushort)((i + 1) * row + j + 1);
                    faces[count + 5] = (ushort)(i * row + j + 1);

                    faces[count + 6] = (ushort)(i * row + j);
                    faces[count + 7] = (ushort)((i + 1) * row + j + 1);
                    faces[count + 8] = (ushort)((i + 1) * row + j);

                    faces[count + 9] = (ushort)(i * row + j);
                    faces[count + 10] = (ushort)(i * row + j + 1);
                    faces[count + 11] = (ushort)((i + 1) * row + j + 1);

                    count += 12;
                }
            }
            return faces;
        }
        public unsafe void createMesh()
        {
            /// Create the mesh via the MeshManager
            MeshPtr msh = MeshManager.Singleton.CreateManual("ColourCube", "General");

            /// Create one submesh
            SubMesh sub = msh.CreateSubMesh();

            /// Define the vertices (8 vertices, each consisting of 2 groups of 3 floats
            //const int nVertices = 8;
            //int row = recordno;
           // int col = demcol;
            int row = recordno;
            int col = 1100;
            int step = 10;
            int vbufCount;
            uint nVertices = (uint)(col * row);
            float[] vertices = CreateVertices(row, col, step, out vbufCount);


            /// Define 12 triangles (two triangles per cube face)
            /// The values in this table refer to vertices in the above table
            uint ibufCount;
            ushort[] faces = CreateFaces(row, col, out ibufCount);

            /// Create vertex data structure for 8 vertices shared between submeshes
            msh.sharedVertexData = new VertexData();
            msh.sharedVertexData.vertexCount = nVertices;

            /// Create declaration (memory format) of vertex data
            VertexDeclaration decl = msh.sharedVertexData.vertexDeclaration;
            uint offset = 0;
            // 1st buffer
            decl.AddElement(0, offset, VertexElementType.VET_FLOAT3, VertexElementSemantic.VES_POSITION);
            offset += VertexElement.GetTypeSize(VertexElementType.VET_FLOAT3);
            decl.AddElement(0, offset, VertexElementType.VET_FLOAT2, VertexElementSemantic.VES_TEXTURE_COORDINATES);
            offset += VertexElement.GetTypeSize(VertexElementType.VET_FLOAT2);
            /// Allocate vertex buffer of the requested number of vertices (vertexCount) 
            /// and bytes per vertex (offset)
            HardwareVertexBufferSharedPtr vbuf =
                HardwareBufferManager.Singleton.CreateVertexBuffer(offset, msh.sharedVertexData.vertexCount,
                HardwareBuffer.Usage.HBU_STATIC_WRITE_ONLY);


            /// Upload the vertex data to the card
            fixed (void* p = vertices)
            {
                vbuf.WriteData(0, vbuf.SizeInBytes, p, true);// writeData(0, vbuf->getSizeInBytes(), vertices, true);
            }
            /// Set vertex buffer binding so buffer 0 is bound to our vertex buffer
            VertexBufferBinding bind = msh.sharedVertexData.vertexBufferBinding;//  msh->sharedVertexData->vertexBufferBinding;
            bind.SetBinding(0, vbuf);

            /// Allocate index buffer of the requested number of vertices (ibufCount) 
            HardwareIndexBufferSharedPtr ibuf =
                HardwareBufferManager.Singleton.CreateIndexBuffer(HardwareIndexBuffer.IndexType.IT_16BIT,
                ibufCount, HardwareBuffer.Usage.HBU_STATIC_WRITE_ONLY);

            /// Upload the index data to the card
            fixed (void* p = faces)
            {
                ibuf.WriteData(0, ibuf.SizeInBytes, p, true);
            }

            /// Set parameters of the submesh
            sub.useSharedVertices = true;
            sub.indexData.indexBuffer = ibuf;
            sub.indexData.indexCount = ibufCount;
            sub.indexData.indexStart = 0;

            /// Set bounding information (for culling)
            msh._setBounds(new AxisAlignedBox(min,max));
            // msh._setBoundingSphereRadius(Mogre.Math.Sqrt(3 * 100 * 100));

            /// Notify Mesh object that it has been loaded
            msh.Load();
        }
    }
}