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
    EditorGUILayout.BeginHorizontal();
    EditorGUILayout.LabelField("전체",GUILayout.MaxWidth(60));
    GUILayout.FlexibleSpace();
    if (GUILayout.Button("패널 활성화",GUILayout.Width(150)))
    {
      for (int i = 0; i < Target.PanelRects.Count; i++)
      {
        Target.PanelRects[i].Rect.anchoredPosition = Target.PanelRects[i].InsidePos;
      }
    }
    if (GUILayout.Button("패널 비활성화", GUILayout.Width(150)))
    {
      for (int i = 0; i < Target.PanelRects.Count; i++)
      {
        Target.PanelRects[i].Rect.anchoredPosition = Target.PanelRects[i].OutisdePos;
      }
    }
    EditorGUILayout.EndHorizontal();
    EditorGUILayout.Space();
    EditorGUILayout.Space();
    for(int i = 0; i < Target.PanelGroups.Count; i++)
    {
      EditorGUILayout.BeginHorizontal();
      EditorGUILayout.LabelField(Target.PanelGroups[i].Name, GUILayout.MaxWidth(60));
      GUILayout.FlexibleSpace();
      if (GUILayout.Button("패널 활성화", GUILayout.Width(150)))
      {
        for(int j=0;j< Target.PanelGroups[i].Panels.Count;j++)
        {
          for(int k = 0; k < Target.PanelRects.Count; k++)
          {
            if(Target.PanelRects[k].Name== Target.PanelGroups[i].Panels[j])
            {
              Target.PanelRects[k].Rect.anchoredPosition = Target.PanelRects[k].InsidePos;
              break;
            }
          }
        }
      }
      if (GUILayout.Button("패널 비활성화", GUILayout.Width(150)))
      {
        for (int j = 0; j < Target.PanelGroups[i].Panels.Count; j++)
        {
          for (int k = 0; k < Target.PanelRects.Count; k++)
          {
            if (Target.PanelRects[k].Name == Target.PanelGroups[i].Panels[j])
            {
              Target.PanelRects[k].Rect.anchoredPosition = Target.PanelRects[k].OutisdePos;
              break;
            }
          }
        }
      }
      EditorGUILayout.EndHorizontal();
      EditorGUILayout.Space();
    }
  }
}
