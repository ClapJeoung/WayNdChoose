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
        if (GUILayout.Button("활성화 위치"))
        {
      for (int i = 0; i < Target.PanelRects.Count; i++)
      {
        Target.PanelRects[i].Rect.anchoredPosition = Target.PanelRects[i].InsidePos;
      }
    }
    EditorGUILayout.Space();
    if(GUILayout.Button("비활성화 위치"))
    {
      for(int i = 0; i < Target.PanelRects.Count; i++)
      {
        Target.PanelRects[i].Rect.anchoredPosition = Target.PanelRects[i].OutisdePos;
      }
    }
    }
}
