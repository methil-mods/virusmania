using System;
using UnityEngine;

namespace Framework
{
    [Serializable]
    public abstract class Updatable<T> where T : MonoBehaviour
    {
        public abstract void Start(T controller);
        public abstract void Update(T controller);
        
        // Virtual cause there is no obligation to override it in fact
        public virtual void OnDrawGizmos() {}
        public virtual void OnDestroy() {}
    }
}