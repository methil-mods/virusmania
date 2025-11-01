using System;
using UnityEngine;

namespace Framework.Controller
{
    public abstract class BaseController<T> : MonoBehaviour where T : BaseController<T>
    {
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = (T)this;
        }
    }
}