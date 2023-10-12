using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType {
    Conversation,Force,Wild,Intelligence,
  HPGen, HPLoss,
  SanityGen, SanityLoss,
  GoldGen, GoldLoss }
public enum ExpTypeEnum{ Normal,Bad}
public class Experience
{
  public string ID = "";
  public string Name { get { return GameManager.Instance.GetTextData(ID+"_Name"); } }
  public ExpTypeEnum ExpType = ExpTypeEnum.Normal; 
  public string Description { get { return GameManager.Instance.GetTextData(ID + "_Description"); } }
    public List<EffectType> Effects=new List<EffectType>();
  private int _duration = 0;
  public int Duration
  {
    get { return _duration; }
    set { _duration = value;
      if (_duration<0) GameManager.Instance.MyGameData.DeleteExp(this);
    }
  }
  private Sprite illust = null;
  public Sprite Illust
  {
    get { if(illust==null)illust=GameManager.Instance.ImageHolder.GetEXPIllust(ID); return illust; }
  }
  public string EffectString
  {
    get
    {
      string _str = "";
      foreach (var _data in Effects)
      {
        if (!_str.Equals("")) _str += "\n";
        string _temp = "";
        switch (_data)
        {
          case EffectType.Conversation:
            _temp = $"{GameManager.Instance.GetTextData(SkillTypeEnum.Conversation, 1)} + 1";
            break;
          case EffectType.Force:
            _temp = $"{GameManager.Instance.GetTextData(SkillTypeEnum.Force, 1)} + 1";
            break;
          case EffectType.Wild:
            _temp = $"{GameManager.Instance.GetTextData(SkillTypeEnum.Wild, 1)} + 1";
            break;
          case EffectType.Intelligence:
            _temp = $"{GameManager.Instance.GetTextData(SkillTypeEnum.Intelligence,1)} + 1";
            break;

          case EffectType.HPLoss:
            _temp = GameManager.Instance.GetTextData(StatusTypeEnum.HP, 15); break;
          case EffectType.SanityLoss:
            _temp = GameManager.Instance.GetTextData(StatusTypeEnum.Sanity, 15); break;
          case EffectType.GoldLoss:
            _temp = GameManager.Instance.GetTextData(StatusTypeEnum.Gold, 15); break;
          case EffectType.HPGen:
            _temp = GameManager.Instance.GetTextData(StatusTypeEnum.HP, 12); break;
          case EffectType.SanityGen:
            _temp = GameManager.Instance.GetTextData(StatusTypeEnum.Sanity, 12); break;
          case EffectType.GoldGen:
            _temp = GameManager.Instance.GetTextData(StatusTypeEnum.Gold, 12); break;
        }
        _str += _temp;
      }

      return _str;
    }
  }
  public Experience Copy()
  {
    Experience _exp=new Experience();
    _exp.ID=ID;
    _exp.ExpType = ExpType;
    _exp.Effects=Effects;
    _exp.Duration=Duration;
    return _exp;
  }
}
[System.Serializable]
public class ExperienceJsonData
{
  public string ID = "";
  public int GoodOrBad;
  public string Type;    //0~9 : 기술들  10~   체력,정신력,돈 등
  public Experience ReturnEXPClass()
  {
    Experience _exp = new Experience();
    _exp.ID = ID;
    _exp.ExpType = (ExpTypeEnum)GoodOrBad;
    string[] _temp = Type.Split("@");
    for (int i = 0; i < _temp.Length; i++) _exp.Effects.Add((EffectType)int.Parse(_temp[i]));

    return _exp;
  }
}

