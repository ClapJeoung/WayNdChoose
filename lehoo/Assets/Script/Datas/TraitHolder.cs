using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trait
{
  public string ID = "";
  public string Name = "";
  public string Description = "";
  public string SubDescription = "";
  public Sprite Illust = null;
  public Sprite Icon = null;
    public Dictionary<EffectType, int> Effects=new Dictionary<EffectType, int>();

  public string EffectString
  {
    get
    {
      string _str = "";
      foreach(var _data in Effects)
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
}
public class TraitJsonData
{
  public string ID = "";
  public string Type;    //0~9 : 기술들  10~   체력,정신력,돈 등
  public string Info;
  public Trait ReturnTraitClass()
  {
    Trait _mytrait = new Trait();
    _mytrait.ID = ID;
    TextData _textdata= GameManager.Instance.GetTextData(ID);
    _mytrait.Name = _textdata.Name;
    _mytrait.Description = _textdata.Description;
    _mytrait.SubDescription = _textdata.SelectionSubDescription;
    _mytrait.Illust = GameManager.Instance.ImageHolder.GetTraitIllust(ID);
    _mytrait.Icon = GameManager.Instance.ImageHolder.GetTraitIcon(ID);
    string[] _temp = Type.Split('@');
        EffectType[] _type=new EffectType[_temp.Length];
    for (int i = 0; i < _temp.Length; i++) _type[i] =(EffectType)int.Parse(_temp[i]);

    _temp = Info.Split('@');
    int[] _info = new int[_temp.Length];
    for (int i = 0; i < _temp.Length; i++) _info[i] =int.Parse(_temp[i]);

        for(int i = 0; i < _temp.Length; i++)
        {
            _mytrait.Effects.Add(_type[i], _info[i]);
        }
    return _mytrait;
  }
}

