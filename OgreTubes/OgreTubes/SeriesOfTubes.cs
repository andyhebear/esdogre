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

        //typedef std::vector<Ogre::Vector3> VVec;
        //VVec mLineVertices;
        ArrayList mLineVertices;

        uint mSideCount;
        double mRadius;
        bool mUniqueMaterial;

        uint mSphereRings;
        uint mSphereSegments;
        double mSphereRadius;
        double mSphereMaxVisDistance;

        MaterialPtr mMaterial;
        ManualObject mTubeObject;

        //typedef std::vector<Ogre::Entity*> SphereStorage;
        //SphereStorage mSpheresJoints;

        ArrayList mSpheresJoints;

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
            /* : mSceneMgr(sceneMgr),
         mSideCount(numberOfSides),
         mRadius(radius),
         mTubeObject(0),
         mUniqueMaterial(false),
         mSphereRings(sphereRings),
         mSphereSegments(sphereSegments),
         mSphereRadius(sphereRadius),
         mSphereMaxVisDistance(sphereMaxVisibilityDistance),
         mSceneNode(0)*/
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
            // mLineVertices.push_back(pos);
        }
        public void removePoint(uint pointNumber)
        {
            //if (pointNumber < mLineVertices.size())
            //{
            //    VVec::iterator it = mLineVertices.begin() + pointNumber;
            //    mLineVertices.erase(it);
            //}
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
            //mMaterial = MaterialManager::getSingleton().getByName(materialName);

            mUniqueMaterial = uniqueMaterial;

            if (mUniqueMaterial)
                mMaterial = mMaterial.Clone(materialName + "_" + name);
            //if (mUniqueMaterial)
            //	mMaterial = mMaterial->clone(materialName + "_" + name);

            mTubeObject = mSceneMgr.CreateManualObject(name);
            mTubeObject.Dynamic = isDynamic;


            //mTubeObject = mSceneMgr->createManualObject(name);
            //mTubeObject->setDynamic(isDynamic);

            _update(disableUVs, disableNormals);

            if (mSceneNode != null)
                mSceneNode.AttachObject(mTubeObject);
            //mSceneNode->attachObject(mTubeObject);


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
            //if (mTubeObject->getDynamic() == true && mTubeObject->getNumSections() > 0)
            //    mTubeObject->beginUpdate(0);
            //else
            //    mTubeObject->begin(mMaterial->getName());

            Quaternion qRotation = new Quaternion(new Degree(360.0f / (float)mSideCount), Vector3.UNIT_Z);
            uint iVertCount = mSideCount + 1;
            Vector3[] vCoreVerts = new Vector3[iVertCount];
            Vector3 vPos = Vector3.UNIT_Z * (float)mRadius;
            //Quaternion qRotation(Degree(360.0/(Real)mSideCount),Vector3::UNIT_Z);

            //const uint iVertCount = mSideCount + 1;

            //Vector3* vCoreVerts = new Vector3[iVertCount];
            //Vector3 vPos = Vector3::UNIT_Y * mRadius;

            for (int i = 0; i < iVertCount; i++)
            {
                vCoreVerts[i] = vPos;
                vPos = qRotation * vPos;
            }
            //for (int i=0;i<iVertCount;i++)
            //{
            //    vCoreVerts[i] = vPos;
            //    vPos = qRotation * vPos;
            //}


            Vector3 vLineVertA;
            Vector3 vLineVertB;
            Vector3 vLine;
            double dDistance;
            int A, B, C, D;
            int iOffset;
            //Vector3 vLineVertA, vLineVertB;
            //Vector3 vLine;
            //Real dDistance;
            //int A,B,C,D;
            //int iOffset;
            Vector3[] vCylinderVerts = new Vector3[iVertCount * 2];
            //Vector3* vCylinderVerts = new Vector3[iVertCount * 2];

            for (int i = 1; i < mLineVertices.Count; i++)
            {
                vLineVertA = (Vector3)mLineVertices[i - 1];
                vLineVertB = (Vector3)mLineVertices[i];

                vLine = vLineVertB - vLineVertA;
                dDistance = vLine.Normalise();//.normalise();

                qRotation = Vector3.UNIT_Z.GetRotationTo(vLine);//.getRotationTo(vLine);

                for (int j = 0; j < iVertCount; j++)
                {
                    vCylinderVerts[j] = (qRotation * vCoreVerts[j]);
                    vCylinderVerts[j + iVertCount] = (qRotation * (vCoreVerts[j] + (Vector3.UNIT_Z * (float)dDistance)));
                }
                //for (int j=0;j<iVertCount;j++)
                //{
                //    vCylinderVerts[j] = (qRotation * vCoreVerts[j]);
                //    vCylinderVerts[j + iVertCount] = (qRotation * (vCoreVerts[j] + (Vector3::UNIT_Z * dDistance)));
                //}

                float u, v, delta;
                delta = 1.0f / (float)(iVertCount - 1);
                u = 0.0f;
                v = 1.0f;

                for (int j = 0; j < (iVertCount * 2); j++)
                {
                    mTubeObject.Position(vCylinderVerts[j] + vLineVertA);
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
                    //mTubeObject->position(vCylinderVerts[j] + vLineVertA);
                    //if (disableNormals == false)
                    //{
                    //    mTubeObject->normal(vCylinderVerts[j].normalisedCopy());
                    //}
                    //if (disableUVs == false)
                    //{
                    //    if (j == iVertCount){
                    //        u = 0.0;
                    //        v = 0.0;
                    //    }
                    //    mTubeObject->textureCoord(u,v);
                    //    u += delta;
                    //}
                }

                iOffset = (int)((i - 1) * (iVertCount * 2));
                for (int j = 0; j < iVertCount; j++)
                {
                    // End A: 0-(Sides-1)
                    // End B: Sides-(Sides*2-1)

                    // Verts:
                    /*

                    A = (j+1)%Sides		C = A + Sides
                    B = j				D = B + Sides

                    */



                    A = (int)((j + 1) % iVertCount);
                    B = j;
                    C = (int)(A + iVertCount);
                    D = (int)(B + iVertCount);

                    A += iOffset;
                    B += iOffset;
                    C += iOffset;
                    D += iOffset;


                    // Tri #1
                    // C,B,A
                    mTubeObject.Triangle((ushort)C, (ushort)B, (ushort)A);
                    //mTubeObject->triangle(C,B,A);

                    // Tri #2
                    // C,D,B
                    mTubeObject.Triangle((ushort)C, (ushort)D, (ushort)B);
                    //mTubeObject->triangle(C,D,B);



                }


            }

            //delete[] vCoreVerts;
            //delete[] vCylinderVerts;
            //vCoreVerts = 0;
            //vCylinderVerts = 0;

            if (mSphereMesh != null)
                _createSphere(mTubeObject.Name + "_SphereMesh");

            if (mSceneNode != null)
                mSceneNode.RemoveAndDestroyAllChildren();//removeAndDestroyAllChildren();
            //if (mSphereMesh.isNull() == true)
            //    _createSphere(mTubeObject->getName() + "_SphereMesh");

            //if (mSceneNode)
            //    mSceneNode->removeAndDestroyAllChildren();


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

            //VVec::iterator it = mLineVertices.begin()+1;
            //for (int i=1; it != (mLineVertices.end()-1);++it,i++)
            //{
            //    if (mSpheresJoints.size() < i)
            //    {
            //        pEnt = mSceneMgr->createEntity(mTubeObject->getName() + "_SphereEnt" + StringConverter::toString(i),mSphereMesh->getName());
            //        pEnt->setMaterialName(mMaterial->getName());
            //        mSpheresJoints.push_back(pEnt);
            //    }
            //    else
            //    {
            //        pEnt = mSpheresJoints[i-1];
            //    }
            //    pEnt->setRenderingDistance(mSphereMaxVisDistance);

            //    if (mSceneNode)
            //    {
            //        pChildNode = mSceneNode->createChildSceneNode();
            //        pChildNode->setPosition((*it));				
            //        pChildNode->attachObject(pEnt);
            //    }
            //}


            //mTubeObject->end();

        }
        public void _destroy()
        {
            if (mTubeObject != null)
            {
                if (mTubeObject.ParentSceneNode != null)
                    mTubeObject.ParentSceneNode.DetachObject(mTubeObject);//>detachObject(mTubeObject);

                mSceneMgr.DestroyManualObject(mTubeObject);
                //mSceneMgr->destroyManualObject(mTubeObject);
            }



            if (mUniqueMaterial)
            {
                MaterialManager.Singleton.Remove(mMaterial.Name);
                //MaterialManager::getSingleton().remove(mMaterial->getName());
            }
            mMaterial.Dispose();
            mMaterial = null;
            //mMaterial.setNull();

            if (mSpheresJoints.Count > 0)
            {
                Entity pEnt;
                foreach (Entity en in mSpheresJoints)
                {
                    en.ParentSceneNode.DetachObject(en);
                    mSceneMgr.DestroyEntity(en);
                }
                //SphereStorage::iterator it = mSpheresJoints.begin();
                //for (; it != mSpheresJoints.end(); ++it)
                //{
                //    pEnt = (*it);
                //    pEnt->getParentSceneNode()->detachObject(pEnt);
                //    mSceneMgr->destroyEntity(pEnt);
                //}
            }

            if (mSphereMesh != null)
            {
                MeshManager.Singleton.Remove(mSphereMesh.Name);
                mSphereMesh = null;
                //MeshManager::getSingleton().remove(mSphereMesh->getName());
                //mSphereMesh.setNull();
            }

            if (mSceneNode != null)
            {
                mSceneNode.RemoveAndDestroyAllChildren();// removeAndDestroyAllChildren();
                mSceneNode.ParentSceneNode.RemoveAndDestroyChild(mSceneNode.Name);// removeAndDestroyChild(mSceneNode->getName());
                mSceneNode = null;
            }

        }

        public void _createSphere(string strName)
        {

        }

        public ManualObject createDebug(string name)
        {
            

            ManualObject* pObj = mSceneMgr->createManualObject(name);
		pObj->begin("BaseWhiteNoLighting",RenderOperation::OT_LINE_STRIP);

		VVec::iterator it = mLineVertices.begin();
		for (; it != mLineVertices.end();++it)
		{
			pObj->position((*it));
			pObj->colour(Math::UnitRandom(),Math::UnitRandom(),Math::UnitRandom());
		}
		pObj->end();

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
