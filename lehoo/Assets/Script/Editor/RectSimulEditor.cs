using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

[CustomEditor(typeof(PanelObjRectSimulator))]
public class RectSimulEditor : Editor
{
    PanelObjRectSimulator Me = null;
    UI_default Target = null;

    private void OnEnable()
    {
        Me = target as PanelObjRectSimulator;
        Target = Me.gameObject.GetComponent<UI_default>();
    }

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("·¹ÈÄ~"))
        {
            Debug.Log(Target.gameObject.name);
            Debug.Log(Target.name);
        }
    }
}
