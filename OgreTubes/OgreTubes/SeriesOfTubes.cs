using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using System.Collections;

namespace OgreTubes
{
    class SeriesOfTubes : IDisposable
    {

        SceneManager mSceneMgr;


        ArrayList mLineVertices = new ArrayList();

        uint mSideCount;
        double mRadius;
        bool mUniqueMaterial;

        uint mSphereRings;
        uint mSphereSegments;
        double mSphereRadius;
        double mSphereMaxVisDistance;

        MaterialPtr mMaterial;
        ManualObject mTubeObject;


        ArrayList mSpheresJoints = new ArrayList();

        MeshPtr mSphereMesh;

        SceneNode mSceneNode;

        public SeriesOfTubes(SceneManager sceneMgr
            , uint numberOfSides
            , double radius
            , uint sphereRings
            , uint sphereSegments
            , double sphereRadius
            , double sphereMaxVisibilityDistance)
        {
           
            mSceneMgr = sceneMgr;
            mSideCount = numberOfSides;
            mRadius = radius;
            // mTubeObject
            mUniqueMaterial = false;
            mSphereRings = sphereRings;
            mSphereSegments = sphereSegments;
            mSphereRadius = sphereRadius;
            mSphereMaxVisDistance = sphereMaxVisibilityDistance;
            //mSceneNode
        }

        public void addPoint(Vector3 pos)
        {
            mLineVertices.Add(pos);
          
        }
        public void removePoint(uint pointNumber)
        {           
            if (pointNumber < mLineVertices.Count)
            {
                mLineVertices.RemoveRange(0, (int)pointNumber);
            }
        }

        public void setRadius(double radius) { mRadius = radius; }
        public void setSides(uint numberOfSides) { mSideCount = numberOfSides; }

        public double getRadius() { return mRadius; }
        public uint getSides() { return mSideCount; }

        public void setSceneNode(SceneNode sceneNode) { mSceneNode = sceneNode; }
        SceneNode getSceneNode() { return mSceneNode; }

        public MaterialPtr getMaterial() { return mMaterial; }
        public ManualObject createTubes(
              string name,
              string materialName,
              bool uniqueMaterial,
              bool isDynamic,
              bool disableUVs,
              bool disableNormals)
        {
            if (mTubeObject != null)
                return mTubeObject;
            mMaterial = MaterialManager.Singleton.GetByName(materialName);
            mUniqueMaterial = uniqueMaterial;

            if (mUniqueMaterial)
                mMaterial = mMaterial.Clone(materialName + "_" + name);

            mTubeObject = mSceneMgr.CreateManualObject(name);
            mTubeObject.Dynamic = isDynamic;

            _update(disableUVs, disableNormals);

            if (mSceneNode != null)
                mSceneNode.AttachObject(mTubeObject);
            
            return mTubeObject;
        }

        public void _update(bool disableUVs, bool disableNormals)
        {
            if (mTubeObject == null || mLineVertices.Count < 2)
                return;

            if (mTubeObject.Dynamic && mTubeObject.NumSections > 0)
                mTubeObject.BeginUpdate(0);
            else
                mTubeObject.Begin(mMaterial.Name);

            Quaternion qRotation = new Quaternion(new Degree(360.0f / (float)mSideCount), Vector3.UNIT_Z);
            uint iVertCount = mSideCount + 1;
            Vector3[] vCoreVerts = new Vector3[iVertCount];
            Vector3 vPos = Vector3.UNIT_X * (float)mRadius;
            for (int i = 0; i < iVertCount; i++)
            {
                vCoreVerts[i] = vPos;
                vPos = qRotation * vPos;
            }

            Vector3 vLineVertA;
            Vector3 vLineVertB;
            Vector3 vLine;
            double dDistance;
            int A, B, C, D;
            int iOffset;

            Vector3[] vCylinderVerts = new Vector3[iVertCount * 2];

            for (int i = 1; i < mLineVertices.Count; i++)
            {
                vLineVertA = (Vector3)mLineVertices[i - 1];
                vLineVertB = (Vector3)mLineVertices[i];

                vLine = vLineVertB - vLineVertA;
                dDistance = vLine.Normalise();

                qRotation = Vector3.UNIT_Z.GetRotationTo(vLine);

                for (int j = 0; j < iVertCount; j++)
                {
                    vCylinderVerts[j] = (qRotation * vCoreVerts[j]);
                    vCylinderVerts[j + iVertCount] = (qRotation * (vCoreVerts[j] + (Vector3.UNIT_Z * (float)dDistance)));
                }
                
                float u, v, delta;
                delta = 1.0f / (float)(iVertCount - 1);
                u = 0.0f;
                v = 1.0f;
                List<Vector3> poslist = new List<Vector3>();
                for (int j = 0; j < (iVertCount * 2); j++)
                {
                    Vector3 pos = vCylinderVerts[j] + vLineVertA;
                    mTubeObject.Position(pos);
                    poslist.Add(pos);
                    if (disableNormals == false)
                    {
                        mTubeObject.Normal(vCylinderVerts[j].NormalisedCopy);
                    }
                    if (disableUVs == false)
                    {
                        if (j == iVertCount)
                        {
                            u = 0.0f;
                            v = 0.0f;
                        }
                        mTubeObject.TextureCoord(u, v);
                        u += delta;
                    }
                }

                iOffset = (int)((i - 1) * (iVertCount * 2));
                for (int j = 0; j < iVertCount; j++)
                {
                    A = (int)((j + 1) % iVertCount);
                    B = j;
                    C = (int)(A + iVertCount);
                    D = (int)(B + iVertCount);

                    A += iOffset;
                    B += iOffset;
                    C += iOffset;
                    D += iOffset;
                    mTubeObject.Triangle((ushort)C, (ushort)B, (ushort)A);
                    mTubeObject.Triangle((ushort)C, (ushort)D, (ushort)B);
                }
            }

            if (mSphereMesh == null)
                _createSphere(mTubeObject.Name + "_SphereMesh");

            if (mSceneNode != null)
                mSceneNode.RemoveAndDestroyAllChildren();

            Entity pEnt;
            SceneNode pChildNode;

            for (int i = 1; i < mLineVertices.Count - 1; i++)
            {
                if (mSpheresJoints.Count < i)
                {
                    pEnt = mSceneMgr.CreateEntity(mTubeObject.Name + "_SphereEnt" + i.ToString(), mSphereMesh.Name);
                    pEnt.SetMaterialName(mMaterial.Name);
                    mSpheresJoints.Add(pEnt);
                }
                else
                {
                    pEnt = (Entity)mSpheresJoints[i - 1];
                }
                pEnt.RenderingDistance = (float)mSphereMaxVisDistance;
                if (mSceneNode != null)
                {
                    pChildNode = mSceneNode.CreateChildSceneNode();
                    Vector3 it = (Vector3)mLineVertices[i];
                    pChildNode.SetPosition(it.x, it.y, it.z);
                    pChildNode.AttachObject(pEnt);
                }
            }
            mTubeObject.End();
          
        }
        public void _destroy()
        {
            if (mTubeObject != null)
            {
                if (mTubeObject.ParentSceneNode != null)
                    mTubeObject.ParentSceneNode.DetachObject(mTubeObject);
                mSceneMgr.DestroyManualObject(mTubeObject);
            }

            if (mUniqueMaterial)
            {
                MaterialManager.Singleton.Remove(mMaterial.Name);
            }
            mMaterial.Dispose();
            mMaterial = null;
            if (mSpheresJoints.Count > 0)
            {
                foreach (Entity en in mSpheresJoints)
                {
                    en.ParentSceneNode.DetachObject(en);
                    mSceneMgr.DestroyEntity(en);
                }
            }

            if (mSphereMesh != null)
            {
                MeshManager.Singleton.Remove(mSphereMesh.Name);
                mSphereMesh = null;
            }

            if (mSceneNode != null)
            {
                mSceneNode.RemoveAndDestroyAllChildren();
                mSceneNode.ParentSceneNode.RemoveAndDestroyChild(mSceneNode.Name);
                mSceneNode = null;
            }

        }

        unsafe public void _createSphere(string strName)
        {
            mSphereMesh = MeshManager.Singleton.CreateManual(strName, ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME);
            SubMesh pSphereVertex = mSphereMesh.CreateSubMesh();

            mSphereMesh.sharedVertexData = new VertexData();
            VertexData vertexData = mSphereMesh.sharedVertexData;

            VertexDeclaration vertexDecl = vertexData.vertexDeclaration;
            uint currOffset = 0;
            vertexDecl.AddElement(0, currOffset, VertexElementType.VET_FLOAT3, VertexElementSemantic.VES_POSITION);
            currOffset += VertexElement.GetTypeSize(VertexElementType.VET_FLOAT3);

            vertexDecl.AddElement(0, currOffset, VertexElementType.VET_FLOAT3, VertexElementSemantic.VES_NORMAL);
            currOffset += VertexElement.GetTypeSize(VertexElementType.VET_FLOAT3);

            vertexDecl.AddElement(0, currOffset, VertexElementType.VET_FLOAT2, VertexElementSemantic.VES_TEXTURE_COORDINATES, 0);
            currOffset += VertexElement.GetTypeSize(VertexElementType.VET_FLOAT2);

            vertexData.vertexCount = (uint)((mSphereRadius + 1) * (mSphereSegments + 1));
            HardwareVertexBufferSharedPtr vBuf = HardwareBufferManager.Singleton.CreateVertexBuffer(vertexDecl.GetVertexSize(0), vertexData.vertexCount, HardwareBuffer.Usage.HBU_STATIC_WRITE_ONLY, false);
            VertexBufferBinding binding = vertexData.vertexBufferBinding;
            binding.SetBinding(0, vBuf);
            float* pVertex = (float*)vBuf.Lock(HardwareBuffer.LockOptions.HBL_DISCARD);

            pSphereVertex.indexData.indexCount = (uint)(6 * mSphereRadius * (mSphereSegments + 1));
            pSphereVertex.indexData.indexBuffer = HardwareBufferManager.Singleton.CreateIndexBuffer(HardwareIndexBuffer.IndexType.IT_16BIT, pSphereVertex.indexData.indexCount, HardwareBuffer.Usage.HBU_STATIC_WRITE_ONLY, false);
            HardwareIndexBufferSharedPtr iBuf = pSphereVertex.indexData.indexBuffer;
            ushort* pIndices = (ushort*)iBuf.Lock(HardwareBuffer.LockOptions.HBL_DISCARD);

            float fDeltaRingAngle = (Mogre.Math.PI / mSphereRings);
            float fDeltaSegAngle = (2 * Mogre.Math.PI / mSphereSegments);
            ushort wVerticeIndex = 0;
            
            for (int ring = 0; ring <= mSphereRings; ring++)
            {
                float r0 = (float)(mSphereRadius * Mogre.Math.Sin((float)(ring * fDeltaRingAngle)));
                float y0 = (float)(mSphereRadius * Mogre.Math.Cos((float)(ring * fDeltaRingAngle)));

                for (int seg = 0; seg <= mSphereSegments; seg++)
                {
                    float x0 = (float)(r0 * Mogre.Math.Sin((float)(seg * fDeltaSegAngle)));
                    float z0 = (float)(r0 * Mogre.Math.Cos((float)(seg * fDeltaSegAngle)));

                    *pVertex++ = x0;
                    *pVertex++ = y0;
                    *pVertex++ = z0;

                    Vector3 vNormal = new Vector3(x0, y0, z0).NormalisedCopy;
                    *pVertex++ = vNormal.x;
                    *pVertex++ = vNormal.y;
                    *pVertex++ = vNormal.z;

                    *pVertex++ = (float)seg / (float)mSphereSegments;
                    *pVertex++ = (float)ring / (float)mSphereRings;

                    if (ring != mSphereRings)
                    {

                        *pIndices++ = (ushort)(wVerticeIndex + mSphereSegments + 1);
                        *pIndices++ = wVerticeIndex;
                        *pIndices++ = (ushort)(wVerticeIndex + mSphereSegments);
                        *pIndices++ = (ushort)(wVerticeIndex + mSphereSegments + 1);
                        *pIndices++ = (ushort)(wVerticeIndex + 1);
                        *pIndices++ = wVerticeIndex;
                        wVerticeIndex++;
                    }
                };
            }
            // Unlock
            vBuf.Unlock();
            iBuf.Unlock();
            // Generate face list
            pSphereVertex.useSharedVertices = true;

            mSphereMesh._setBounds(new AxisAlignedBox(new Vector3((float)-mSphereRadius, (float)-mSphereRadius, (float)-mSphereRadius), new Vector3((float)mSphereRadius, (float)mSphereRadius, (float)mSphereRadius)), false);
            mSphereMesh._setBoundingSphereRadius((float)mSphereRadius);
            mSphereMesh.Load();
        }

        public ManualObject createDebug(string name)
        {
            ManualObject pObj = mSceneMgr.CreateManualObject(name);
            pObj.Begin("BaseWhiteNoLighting", RenderOperation.OperationTypes.OT_LINE_LIST);
            foreach (Vector3 it in mLineVertices)
            {
                pObj.Position(it);
                pObj.Colour(Mogre.Math.UnitRandom(), Mogre.Math.UnitRandom(), Mogre.Math.UnitRandom());
            }
            pObj.End();
            return pObj;

        }


        #region IDisposable 成员

        public void Dispose()
        {
            this._destroy();
        }

        #endregion
    }
}
