using System;
using UnityEngine;
using Framework.Controller;

namespace Core.Cursor
{
    public class CursorController : BaseController<CursorController>
    {
        const int width = 32;
        const int height = 32;

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
