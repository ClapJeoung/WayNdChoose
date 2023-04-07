using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="ImageHolder")]

public class ImageHolder : ScriptableObject
{
  public List<Sprite> TownSprites=new List<Sprite>();                 //마을 일러스트
  public List<Sprite> CitySprites=new List<Sprite>();                 //도시 일러스트
  public List<Sprite> CastleSprites=new List<Sprite>();               //성채 일러스트

  public List<Sprite> SkillSprites_Speech = new List<Sprite>();       
  public List<Sprite> SkillSprites_Threat = new List<Sprite>();
  public List<Sprite> SkillSprites_Deception = new List<Sprite>();
  public List<Sprite> SkillSprites_Logic = new List<Sprite>();
  public List<Sprite> SkillSprites_Martialarts = new List<Sprite>();
  public List<Sprite> SkillSprites_Bow = new List<Sprite>();
  public List<Sprite> SkillSprites_Somatology = new List<Sprite>();
  public List<Sprite> SkillSprites_Survivable = new List<Sprite>();
  public List<Sprite> SkillSprites_Biology = new List<Sprite>();
  public List<Sprite> SkillSprites_Knowledge = new List<Sprite>();

  public List<Sprite> TendencySprites=new List<Sprite>();

  public List<Sprite> EventIllust = new List<Sprite>();              //모든 이벤트 일러스트
  public List<Sprite> EXPIllust = new List<Sprite>();                  //모든 경험 일러스트
  public List<Sprite> TraitIllust = new List<Sprite>();                //모든 특성 일러스트
  public Sprite DefaultSprite = null;                                 //빈 일러스트
  public Sprite GetTownSprite(int _name)
  {
    Sprite _targetsprite = DefaultSprite;
    foreach (Sprite _spr in TownSprites)
    {
      if (_spr.name.Equals(_name))
      {
        _targetsprite = _spr;
        break;
      }
    }
    return _targetsprite;
  }//마을 이름에 해당하는 일러스트 가져오기
  public Sprite GetCitySprite(int _name)
  {
    Sprite _targetsprite = DefaultSprite;
    foreach (Sprite _spr in CitySprites)
    {
      if (_spr.name.Equals(_name))
      {
        _targetsprite = _spr;
        break;
      }
    }
    return _targetsprite;
  }//도시 이름에 해당하는 일러스트 가져오기
  public Sprite GetCastleSprite(int _name)
  {
    Sprite _targetsprite = DefaultSprite;
    foreach (Sprite _spr in CastleSprites)
    {
      if (_spr.name.Equals(_name))
      {
        _targetsprite = _spr;
        break;
      }
    }
    return _targetsprite;
  }//성채 이름에 해당하는 일러스트 가져오기

  public Sprite GetSkillSprite(SkillName _skillname,int _level)
  {
    List<Sprite> _temp =new List<Sprite>();
    switch (_skillname)
    {
      case SkillName.Biology:_temp = SkillSprites_Biology;break;
      case SkillName.Bow: _temp = SkillSprites_Bow; break;
      case SkillName.Deception: _temp = SkillSprites_Deception; break;
      case SkillName.Knowledge: _temp = SkillSprites_Knowledge; break;
      case SkillName.Logic: _temp = SkillSprites_Logic; break;
      case SkillName.Martialarts: _temp = SkillSprites_Martialarts; break;
      case SkillName.Somatology: _temp = SkillSprites_Somatology; break;
      case SkillName.Speech: _temp = SkillSprites_Speech; break;
      case SkillName.Survivable: _temp = SkillSprites_Survivable; break;
      case SkillName.Threat: _temp = SkillSprites_Threat; break;
    }
    if (_temp.Count < _level) return DefaultSprite;
    return _temp[_level - 1];
  }
  public Sprite GetEventIllust(string _illustid)
  {
    Sprite _targetsprite = DefaultSprite;
    foreach (Sprite _spr in EventIllust)
    {
      if (_spr.name.Equals(_illustid))
      {
        _targetsprite = _spr;
        break;
      }
    }
    return _targetsprite;
  }//ID로 이벤트 일러스트 가져오기
  public Sprite GetEXPIllust(string _illustid)
  {
    Sprite _targetsprite = DefaultSprite;
    foreach (Sprite _spr in EXPIllust)
    {
      if (_spr.name.Equals(_illustid))
      {
        _targetsprite = _spr;
        break;
      }
    }
    return _targetsprite;
  }//ID로 경험 일러스트 가져오기
  public Sprite GetTraitIllust(string _illustid)
  {
    Sprite _targetsprite = DefaultSprite;
    foreach (Sprite _spr in TraitIllust)
    {
      if (_spr.name.Equals(_illustid))
      {
        _targetsprite = _spr;
        break;
      }
    }
    return _targetsprite;
  }//ID로 특성 일러스트 가져오기
}
