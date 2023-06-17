using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType {
    Speech, Threat, Deception, Logic, Kombat, Bow, Somatology, Survivable, Biology, Knowledge,
  HPGen, HPLoss,
  SanityGen, SanityLoss,
  GoldGen, GoldLoss }
public enum ExpTypeEnum{ Normal,Bad,Mad}
public class Experience
{
  private TextData textdata = null;
  public TextData TextData
  {
    get { if (textdata == null) textdata = GameManager.Instance.GetTextData(ID); return textdata; }
  }
  public string ID = "";
  public string Name { get { return TextData.Name; } }
  public ExpTypeEnum ExpType = ExpTypeEnum.Normal; 
  public string Description { get { return TextData.Description; } }
  public string SubDescription { get { return TextData.SelectionSubDescription; } }
    public List<EffectType> Effects=new List<EffectType>();
  private int _duration = 0;
  public int Duration
  {
    get { return _duration; }
    set { _duration = value;
      if (_duration.Equals(0)) GameManager.Instance.MyGameData.DeleteExp(this);
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
        TextData _textdata = GameManager.Instance.GetTextData(_data);
        string _temp = "";
        switch (_data)
        {
          case EffectType.Speech:
          case EffectType.Threat:
          case EffectType.Deception:
          case EffectType.Logic:
          case EffectType.Kombat:
          case EffectType.Bow:
          case EffectType.Somatology:
          case EffectType.Survivable:
          case EffectType.Biology:
          case EffectType.Knowledge:
            _temp = $"{_textdata.Icon} {_textdata.Name} + 1";
            break;

          case EffectType.HPLoss:
            _temp = $"{_textdata.Name}"; break;
          case EffectType.SanityLoss:
            _temp = $"{_textdata.Name}"; break;
          case EffectType.GoldLoss:
            _temp = $"{_textdata.Name}"; break;
          case EffectType.HPGen:
            _temp = $"{_textdata.Name}"; break;
          case EffectType.SanityGen:
            _temp = $"{_textdata.Name}"; break;
          case EffectType.GoldGen:
            _temp = $"{_textdata.Name}"; break;
        }
        _str += _temp;
      }

      return _str;
    }
  }
  public string ShortEffectString
  {
    get
    {
      string _str = "";
      foreach (var _data in Effects)
      {
        TextData _textdata = GameManager.Instance.GetTextData(_data);
        _str += _textdata.Icon+" ";
      }

      return _str;
    }
  }//짧게 아이콘만 주는거
  public Experience Copy()
  {
    Experience _exp=new Experience();
    _exp.ID=ID;
    _exp.ExpType = ExpType;
    _exp.Effects=Effects;
    _exp.Duration=Duration;
    _exp.textdata=textdata;
    return _exp;
  }
}
public class ExperienceJsonData
{
  public string ID = "";
  public int GoodOrBad;
  public string Type;    //0~9 : 기술들  10~   체력,정신력,돈 등
  public Experience ReturnEXPClass()
  {
    Experience _exp = new Experience();
    _exp.ID = ID;
    TextData _textdata = GameManager.Instance.GetTextData(ID);
    _exp.ExpType = (ExpTypeEnum)GoodOrBad;
    string[] _temp = Type.Split("@");
    EffectType[] _type = new EffectType[_temp.Length];
    for (int i = 0; i < _temp.Length; i++) _type[i] = (EffectType)int.Parse(_temp[i]);

    return _exp;
  }
}

