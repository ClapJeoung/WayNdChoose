using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="ImageHolder")]

public class ImageHolder : ScriptableObject
{
  public List<Sprite> EventSprites=new List<Sprite>();
  public List<Sprite> TownSprites=new List<Sprite>();
  public List<Sprite> CitySprites=new List<Sprite>();
  public List<Sprite> CastleSprites=new List<Sprite>();
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
  public List<Sprite> EXPSprites=new List<Sprite>();
  public Sprite DefaultSprite = null;
  public Sprite GetEventSprite(string _index)
  {
    foreach (Sprite _sprite in EventSprites) if (_sprite.name == _index) return _sprite;
    return DefaultSprite;
  }
  public Sprite GetTownSprite(int _index)
  {
    if(TownSprites.Count<_index)return DefaultSprite;
    return TownSprites[_index-1];
  }
  public Sprite GetCitySprite(int _index)
  {
    if (CitySprites.Count < _index) return DefaultSprite;
    return CitySprites[_index - 1];
  }
  public Sprite GetCastleSprite(int _index)
  {
    if (CastleSprites.Count < _index) return DefaultSprite;
    return CastleSprites[_index - 1];
  }
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
  public Sprite GetEXPSprite(string _index)
  {
    foreach (Sprite _sprite in EXPSprites) if (_sprite.name == _index) return _sprite;
    return DefaultSprite;
  }
}
