using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType {
    Conversation,Force,Nature,Intelligence
    , Speech, Threat, Deception, Logic, Martialarts, Bow, Somatology, Survivable, Biology, Knowledge,
  HPLoss, HPGen,
  SanityLoss, SanityGen,
  GoldLoss, GoldGen }

public class Experience
{
  public string ID = "";
  public bool GoodExp = false;
  public string Name = "";
  public string Description = "";
  public string SubDescription = "";
    public Dictionary<EffectType, int> Effects=new Dictionary<EffectType, int>();
  private int _duration = 0;
  public int Duration
  {
    get { return _duration; }
    set { _duration = value;
      if (_duration.Equals(0)) GameManager.Instance.MyGameData.DeleteExp(this);
    }
  }
  public Sprite Illust = null;
  public string EffectString
  {
    get
    {
      string _str = "";
      foreach (var _data in Effects)
      {
        if (!_str.Equals("")) _str += "\n";
        TextData _textdata = GameManager.Instance.GetTextData(_data.Key);
        string _temp = "";
        string _temptemp = _data.Value > 0 ? GameManager.Instance.GetTextData("increase").Name : GameManager.Instance.GetTextData("decrease").Name;
        switch (_data.Key)
        {
          case EffectType.Conversation:
          case EffectType.Force:
          case EffectType.Nature:
          case EffectType.Intelligence:
          case EffectType.Speech:
          case EffectType.Threat:
          case EffectType.Deception:
          case EffectType.Logic:
          case EffectType.Martialarts:
          case EffectType.Bow:
          case EffectType.Somatology:
          case EffectType.Survivable:
          case EffectType.Biology:
          case EffectType.Knowledge:
            _temp = $"{_textdata.Name} +{_data.Value}";
            break;

          case EffectType.HPLoss:
          case EffectType.SanityLoss:
          case EffectType.GoldLoss:
            _temp = $"{_textdata.FailDescription} {_temptemp}";
            break;
          case EffectType.HPGen:
          case EffectType.SanityGen:
          case EffectType.GoldGen:
            _temp = $"{_textdata.SuccessDescription} {_temptemp}";
            break;
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
    _exp.GoodExp=GoodExp;
    _exp.Name=Name;
    _exp.Description=Description;
    _exp.SubDescription=SubDescription;
    _exp.Effects=Effects;
    _exp.Duration=Duration;
    _exp.Illust=Illust;
    return _exp;
  }
}
public class ExperienceJsonData
{
  public string ID = "";
  public int GoodOrBad;
  public string Type;    //0~9 : 기술들  10~   체력,정신력,돈 등
  public string Info;
  public Experience ReturnEXPClass()
  {
    Experience _exp = new Experience();
    _exp.ID= ID;
    TextData _textdata = GameManager.Instance.GetTextData(ID);
    _exp.Name = _textdata.Name;
    _exp.Description = _textdata.Description;
    _exp.SubDescription = _textdata.SelectionSubDescription;
    _exp.GoodExp = GoodOrBad == 0 ? false : true;
    _exp.Illust = GameManager.Instance.ImageHolder.GetEXPIllust(ID);
        string[] _temp = Type.Split("@");
        EffectType[] _type = new EffectType[_temp.Length];
        for (int i = 0; i < _temp.Length; i++) _type[i] = (EffectType)int.Parse(_temp[i]);

        _temp = Info.Split("@");
        int[] _infos=new int[_temp.Length];
        for (int i = 0; i < _temp.Length; i++) _infos[i] = int.Parse(_temp[i]);

        for (int i = 0; i < _temp.Length; i++) _exp.Effects.Add(_type[i], _infos[i]);
            return _exp;
    }
}

