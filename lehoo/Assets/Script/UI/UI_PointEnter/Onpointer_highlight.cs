using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.Rendering.DebugUI;

public class Onpointer_highlight : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
  private bool interactive = true;
  public bool Interactive
  {
    get { return interactive; }
    set
    {
      interactive = value;
      if (value == false) UIManager.Instance.HighlightManager.TurnOff();

    }
  }
  [SerializeField] private List<HighlightCallInfo> HighlightList=new List<HighlightCallInfo> ();
  public HighlightCallInfo GetCallInfo(HighlightEffectEnum effect)
  {
    foreach (HighlightCallInfo call in HighlightList)
      if (call.CallType == effect) return call;

    return null;
  }
  public void SetInfo(HighlightEffectEnum type,int value)
  {
    bool _createnew = true;

    foreach (HighlightCallInfo call in HighlightList)
    {
      if (call.CallType == type)
      {
        call.Value = value;
        _createnew = false;
      }
    }
    if (_createnew)
    {
      HighlightList.Add(new HighlightCallInfo(type,value));
    }
  }
  public void SetInfo(List<SkillTypeEnum> skilltypes)
  {
    bool _createnew = true;

    foreach (HighlightCallInfo call in HighlightList)
    {
      if (call.CallType == HighlightEffectEnum.Skill)
      {
        _createnew = false;
        call.Skilltype= skilltypes;
      }
    }
    if (_createnew)
    {
      HighlightList.Add(new HighlightCallInfo(skilltypes));
    }
  }
  public void SetInfo(HighlightEffectEnum type)
  {
    bool _createnew = true;

    foreach (HighlightCallInfo call in HighlightList)
    {
      if (call.CallType == type)
      {
        _createnew = false;
      }
    }
    if (_createnew)
    {
      HighlightList.Add(new HighlightCallInfo(type));
    }
  }
  public void RemoveAllCall()
  {
    HighlightList.Clear();
  }
  public void OnPointerEnter(PointerEventData eventData)
  {
    if (!Interactive) return;
    if (HighlightList.Count == 0) return;

    UIManager.Instance.HighlightManager.SetHighlights(HighlightList);
  }

  public void OnPointerExit(PointerEventData eventData)
  {
    if (!Interactive) return;
    if (HighlightList.Count == 0) return;

    UIManager.Instance.HighlightManager.TurnOff();
  }

}
[System.Serializable]
public class HighlightCallInfo
{
  public HighlightEffectEnum CallType;
  public int Value = 0;
  public List<SkillTypeEnum> Skilltype;
  public HighlightCallInfo(HighlightEffectEnum type)
  {
    CallType = type;
  }
  public HighlightCallInfo(HighlightEffectEnum type,int value)
  {
    CallType = type;
    Value = value;
  }
  public HighlightCallInfo(List<SkillTypeEnum> skilltype)
  {
    CallType = HighlightEffectEnum.Skill;
    Skilltype=skilltype;
  }
}
