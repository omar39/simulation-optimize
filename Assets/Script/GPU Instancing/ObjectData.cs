using UnityEngine;
using System;
public class ObjectData
    {
        public Vector3 position;
        public Quaternion rotation;
        public Vector3 scale;
        public Matrix4x4 matrix 
        {
            get { return Matrix4x4.TRS(position, rotation, scale); }
        }
        public ObjectData()
        {
        }
}
