using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType {
    Conversation,Force,Wild,Intelligence,
    HPLoss,SanityLoss,GoldGen,None
   }
public class Experience
{
  public string ID = "";
  public string Name { get { return GameManager.Instance.GetTextData(ID+"_Name"); } }
  public string Description { get { return GameManager.Instance.GetTextData(ID + "_Description"); } }
  public List<EffectType> Effects=new List<EffectType>();
  public int PassiveCount
  {
    get
    {
      int _count = 0;
      if (Effects.Contains(EffectType.HPLoss)) _count++;
      if(Effects.Contains(EffectType.SanityLoss)) _count++;
      if(Effects.Contains(EffectType.GoldGen)) _count++;
      return _count;
    }
  }
  public int ActiveCount
  {
    get
    {
      int _count = 0;
      if(Effects.Contains(EffectType.Conversation))_count++;
      if (Effects.Contains(EffectType.Force)) _count++;
      if (Effects.Contains(EffectType.Wild)) _count++;
      if (Effects.Contains(EffectType.Intelligence)) _count++;
      return _count;
    }
  }
  private int _duration = 0;
  public int Duration
  {
    get { return _duration; }
    set { _duration = value;
      if (_duration <= 0)
      {
        GameManager.Instance.MyGameData.DeleteExp(this);
        UIManager.Instance.SetInfoPanel(string.Format(GameManager.Instance.GetTextData("DeletedExp"), Name));
      }
    }
  }
  private Sprite illust = null;
  public Sprite Illust
  {
    get { if(illust==null)illust=GameManager.Instance.ImageHolder.GetEXPIllust(ID); return illust; }
  }
  public string EffectString_Passive
  {
    get
    {
      string _str = "";
      for(int i = 0; i < Effects.Count; i++)
      {
        string _temp = "";
        switch (Effects[i])
        {
          case EffectType.HPLoss:
            _temp = GameManager.Instance.GetTextData(StatusTypeEnum.HP, 12) + " "
        + string.Format("{0}%", WNCText.PositiveColor((ConstValues.HPLoss_Exp * 100).ToString()));
            break;
          case EffectType.SanityLoss:
            _temp = GameManager.Instance.GetTextData(StatusTypeEnum.Sanity, 12) + " "
+ string.Format("{0}%", WNCText.PositiveColor((ConstValues.SanityLoss_Exp * 100).ToString()));
            break;
          case EffectType.GoldGen:
            _temp = GameManager.Instance.GetTextData(StatusTypeEnum.Gold, 12) + " "
+ string.Format("{0}%", WNCText.PositiveColor((ConstValues.GoldGen_Exp * 100).ToString()));
            break;
        }
        if (_temp != "")
        {
          _str += _temp+"<br>";
        }
      }
      return _str;
    }
  }
  public string EffectString_Active
  {
    get
    {
      string _str = "";
      for (int i = 0; i < Effects.Count; i++)
      {
        string _temp = "";
        switch (Effects[i])
        {
          case EffectType.Conversation:
            _temp = $"{GameManager.Instance.GetTextData(SkillTypeEnum.Conversation, 1)} + {ConstValues.ExpSkillLevel}";
            break;
          case EffectType.Force:
            _temp = $"{GameManager.Instance.GetTextData(SkillTypeEnum.Force, 1)} + {ConstValues.ExpSkillLevel}";
            break;
          case EffectType.Wild:
            _temp = $"{GameManager.Instance.GetTextData(SkillTypeEnum.Wild, 1)} + {ConstValues.ExpSkillLevel}";
            break;
          case EffectType.Intelligence:
            _temp = $"{GameManager.Instance.GetTextData(SkillTypeEnum.Intelligence, 1)} + {ConstValues.ExpSkillLevel}";
            break;
        }
        if (_temp != "")
        {
          _str += _temp + "<br>";
        }
      }
      return _str;
    }
  }
  public Experience Copy()
  {
    Experience _exp=new Experience();
    _exp.ID=ID;
    _exp.Effects=Effects;
    _exp.Duration=Duration;
    return _exp;
  }
}
[System.Serializable]
public class ExperienceJsonData
{
  public string ID = "";
  public string Type;    //0~9 : 기술들  10~   체력,정신력,돈 등
  public Experience ReturnEXPClass()
  {
    Experience _exp = new Experience();
    _exp.ID = ID;
    string[] _temp = Type.Split("@");
    for (int i = 0; i < _temp.Length; i++) _exp.Effects.Add((EffectType)int.Parse(_temp[i]));

    return _exp;
  }
}

