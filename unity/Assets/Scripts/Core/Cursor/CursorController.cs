using System;
using UnityEngine;
using Framework.Controller;

namespace Core.Cursor
{
    public class CursorController : BaseController<CursorController>
    {
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);

            var srcTex = CursorDatabase.Instance.cursorTexture;

            try
            {
                UnityEngine.Cursor.SetCursor(srcTex, CursorDatabase.Instance.hotspot, CursorDatabase.Instance.cursorMode);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Init()
        {
            var obj = new GameObject("CustomCursor");
            obj.AddComponent<CursorController>();
        }
    }
}
