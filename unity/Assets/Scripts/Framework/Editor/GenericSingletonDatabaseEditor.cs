using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;

[CustomEditor(typeof(ScriptableObject), true)]
public class GenericSingletonDatabaseEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var type = target.GetType();
        var baseType = type.BaseType;
        if (baseType == null || !baseType.IsGenericType) return;
        var genericBase = baseType.GetGenericTypeDefinition();
        if (genericBase.FullName != "Framework.ScriptableObjects.SingletonScriptableDatabase`2") return;
        if (GUILayout.Button("Get All Data"))
        {
            var method = type.GetMethod("GetAllData", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (method != null)
            {
                method.Invoke(target, null);
            }
        }
    }
}