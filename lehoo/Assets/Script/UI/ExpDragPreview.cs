using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpDragPreview : MonoBehaviour
{
  [SerializeField] private Image Illust = null;
  [SerializeField] private List<CanvasGroup> EffectGroups= new List<CanvasGroup>();
  [SerializeField] private RectTransform MyRect = null;

  public void Setup(Experience exp)
  {
    Illust.sprite = exp.Illust;
    for(int i=0;i<EffectGroups.Count; i++)
    {
      if (i == 6) continue;
      if (exp.Effects.Contains((EffectType)i))
      {
        if (!EffectGroups[i].gameObject.activeInHierarchy)
        {
          EffectGroups[i].gameObject.SetActive(true);
        }
        EffectGroups[i].alpha = 0.2f;
      }
      else
      {
        if (EffectGroups[i].gameObject.activeInHierarchy) EffectGroups[i].gameObject.SetActive(false);
      }
    }
    LayoutRebuilder.ForceRebuildLayoutImmediate(EffectGroups[0].transform.parent.transform as RectTransform);
  }
  private void Update()
  {
    MyRect.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    MyRect.anchoredPosition3D = new Vector3(MyRect.anchoredPosition3D.x, MyRect.anchoredPosition3D.y, 0.0f);
  }
  public void SetDown()
  {
    transform.position=Vector2.one*1000.0f;
    gameObject.SetActive(false);
  }
  public void UpdateEffectAlpha(SelectionData data)
  {
    switch (data.ThisSelectionType)
    {
      case SelectionTargetType.None:
        break;
      case SelectionTargetType.Pay:
        /*
        if (data.SelectionPayTarget == StatusTypeEnum.HP)
        {
          EffectGroups[(int)EffectType.HPLoss].alpha = 1.0f;
        }
        else if (data.SelectionPayTarget == StatusTypeEnum.Sanity)
        {
          EffectGroups[(int)EffectType.SanityLoss].alpha = 1.0f;
        }
        */
        break;
      case SelectionTargetType.Check_Single:
        switch (data.SelectionCheckSkill[0])
        {
          case SkillTypeEnum.Conversation:
            EffectGroups[(int)EffectType.Conversation].alpha = 1.0f;
            break;
          case SkillTypeEnum.Force:
            EffectGroups[(int)EffectType.Force].alpha = 1.0f;
            break;
          case SkillTypeEnum.Wild:
            EffectGroups[(int)EffectType.Wild].alpha = 1.0f;
            break;
          case SkillTypeEnum.Intelligence:
            EffectGroups[(int)EffectType.Intelligence].alpha = 1.0f;
            break;
        }
        break;
      case SelectionTargetType.Check_Multy:
        foreach(var skill in data.SelectionCheckSkill)
          switch (skill)
          {
            case SkillTypeEnum.Conversation:
              EffectGroups[(int)EffectType.Conversation].alpha = 1.0f;
              break;
            case SkillTypeEnum.Force:
              EffectGroups[(int)EffectType.Force].alpha = 1.0f;
              break;
            case SkillTypeEnum.Wild:
              EffectGroups[(int)EffectType.Wild].alpha = 1.0f;
              break;
            case SkillTypeEnum.Intelligence:
              EffectGroups[(int)EffectType.Intelligence].alpha = 1.0f;
              break;
          }

        break;
    }
  }
  public void ResetEffectAlpha()
  {
    foreach (var group in EffectGroups)
      group.alpha = 0.2f;
  }
}
