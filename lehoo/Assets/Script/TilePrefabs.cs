using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Tile prefabs", menuName = "TileprefabScriptable")]
public class TilePrefabs : ScriptableObject
{
    public Tile[] Sea;
    [Space(10)]
    public Tile[] Land;
    public Tile[] Highland, Mountain;
    public Tile[] Source_5, Source_0, Source_1, Source_2;
    public Tile[] River_1, River_2, River_3;
    public Tile[] Beach_1, Beach_2, Beach_3,Beach_4;
    public Tile[] RB_1_2, RB_1_3, RB_1_4;
    public Tile[] RB_2_3, RB_2_4;
    public Tile[] RB_3_4;

    [Space(10)]
    public Tile[] Desert;
    public Tile[] d_Highland,d_Mountain;
    public Tile[] d_Source_5, d_Source_0, d_Source_1, d_Source_2;
    public Tile[] d_River_1, d_River_2, d_River_3;
    public Tile[] d_Beach_1, d_Beach_2, d_Beach_3, d_Beach_4;
    public Tile[] d_RB_1_2, d_RB_1_3, d_RB_1_4;
    public Tile[] d_RB_2_3, d_RB_2_4;
    public Tile[] d_RB_3_4;
    [Space(10)]
    public Tile[] Town;
    public Tile[] City, Castle;
    public Tile[] Forest, Forest_river;
    public Tile[] d_Town, d_City, d_Castle;
    public Tile[] d_Forest, d_Forest_river;

  public Sprite GetTile_bottom(int code)
  {
    Tile[] _temp = new Tile[0];
    switch (code)
    {
      case TileCode.sea: _temp = Sea; break;
      case TileCode.land: _temp = Land; break;
      case TileCode.highland: _temp = Highland; break;
      case TileCode.mountain: _temp = Mountain; break;
      case TileCode.source_5: _temp = Source_5; break;
      case TileCode.source_0: _temp = Source_0; break;
      case TileCode.source_1: _temp = Source_1; break;
      case TileCode.source_2: _temp = Source_2; break;
      case TileCode.river_1: _temp = River_1; break;
      case TileCode.river_2: _temp = River_2; break;
      case TileCode.river_3: _temp = River_3; break;
      case TileCode.beach_1: _temp = Beach_1; break;
      case TileCode.beach_2: _temp = Beach_2; break;
      case TileCode.beach_3: _temp = Beach_3; break;
      case TileCode.beach_4: _temp = Beach_4; break;
      case TileCode.rb_1_2: _temp = RB_1_2; break;
      case TileCode.rb_1_3: _temp = RB_1_3; break;
      case TileCode.rb_1_4: _temp = RB_1_4; break;
      case TileCode.rb_2_3: _temp = RB_2_3; break;
      case TileCode.rb_2_4: _temp = RB_2_4; break;
      case TileCode.rb_3_4: _temp = RB_3_4; break;
      case TileCode.desert: _temp = Desert; break;
      case TileCode.d_highland: _temp = d_Highland; break;
      case TileCode.d_mountain: _temp = d_Mountain; break;
      case TileCode.d_source_5: _temp = d_Source_5; break;
      case TileCode.d_source_0: _temp = d_Source_0; break;
      case TileCode.d_source_1: _temp = d_Source_1; break;
      case TileCode.d_source_2: _temp = d_Source_2; break;
      case TileCode.d_river_1: _temp = d_River_1; break;
      case TileCode.d_river_2: _temp = d_River_2; break;
      case TileCode.d_river_3: _temp = d_River_3; break;
      case TileCode.d_beach_1: _temp = d_Beach_1; break;
      case TileCode.d_beach_2: _temp = d_Beach_2; break;
      case TileCode.d_beach_3: _temp = d_Beach_3; break;
      case TileCode.d_beach_4: _temp = d_Beach_4; break;
      case TileCode.d_rb_1_2: _temp = d_RB_1_2; break;
      case TileCode.d_rb_1_3: _temp = d_RB_1_3; break;
      case TileCode.d_rb_1_4: _temp = d_RB_1_4; break;
      case TileCode.d_rb_2_3: _temp = d_RB_2_3; break;
      case TileCode.d_rb_2_4: _temp = d_RB_2_4; break;
      case TileCode.d_rb_3_4: _temp = d_RB_3_4; break;
    }
    if (_temp.Length < 1) Debug.Log($"뭐냐고시발{code}");
    int _length = _temp.Length;
    int _maxlength = 0;
    for (int i = 0; i < _length; i++) _maxlength += (i+1) * (i+1);
    int _target = Random.Range(0, _maxlength);
    Tile _tile = null;
    int _checkarea = 0;
    for (int i = 0; i < _length; i++)
    {
      _checkarea += (int)Mathf.Pow(_length-i, 2);
      if (_target<_checkarea) { _tile = _temp[i]; break; }
    }
    return _tile.sprite;
    }
    public Sprite GetTile_top(int code)
    {
        if (code == 0) return null;
        Tile[] _temp = new Tile[0];
    if (code == TileCode.d_castle) Debug.Log("레후?!");
        switch (code)
        {
            case TileCode.town: _temp = Town;break;
            case TileCode.city: _temp = City; break;
            case TileCode.castle: _temp = Castle; break;
            case TileCode.forest: _temp = Forest; break;
            case TileCode.forest_river: _temp = Forest_river; break;
            case TileCode.d_town: _temp = d_Town; break;
            case TileCode.d_city: _temp = d_City; break;
            case TileCode.d_castle: _temp = d_Castle; break;
            case TileCode.d_forest: _temp = d_Forest; break;
            case TileCode.d_forest_river: _temp = d_Forest_river; break;
        }
    int _length = _temp.Length;
    int _maxlength = 0;
    for (int i = 0; i < _length; i++) _maxlength += (i + 1) * (i + 1);
    int _target = Random.Range(0, _maxlength);
    Tile _tile = null;
    int _checkarea = 0;
    for (int i = 0; i < _length; i++)
    {
      _checkarea +=  (int)Mathf.Pow(_length-i, 2);
      if (_target < _checkarea) { _tile = _temp[i]; break; }
    }
    return _tile.sprite;
  }

  #region GetCode
  public int GetSoure(int dir)
    {
        switch (dir)
        {
            case 5: {  return TileCode.source_5;}
            case 0: {  return TileCode.source_0; }
            case 1: {  return TileCode.source_1; }
            case 2: {  return TileCode.source_2;  }
        }
        return 0;
    }//4,5,6,7
    public int GetRiver(int start, int end)
    {
        int _sum = start - end;
        if (_sum == 1 || _sum == -1) {  return TileCode.river_1;  }
        if (_sum == 2 || _sum == -2) {  return TileCode.river_2; }
        if (_sum == 3 || _sum == -3) {  return TileCode.river_3;  }
        Debug.Log($"이상한 숫자 들어옴 {start}  {end}");
        return  0;
    }//8,9,10
    public int GetBeach(int sidecount)
    {
        if (sidecount == 1)
        {
             return TileCode.beach_1;
        }
        if (sidecount == 2)
        {
             return TileCode.beach_2;
        }
        if (sidecount == 3)
        {
             return TileCode.beach_3;
        }
        if (sidecount == 4)
        {
            return TileCode.beach_4;
        }
        Debug.Log($"이상한 숫자 들어옴 {sidecount}");
        return 0;
    }//11,12,13
    public int GetBeachRiver(int sidecount, int dir )
    {
        if (sidecount == 1)
        {
            if (dir == 2)
            {
                 return TileCode.rb_1_2;
            }
            if (dir == 3)
            {
                 return TileCode.rb_1_3;
            }
            if (dir == 4)
            {
                 return TileCode.rb_1_4;
            }
        }
        if (sidecount == 2)
        {
            if (dir == 3)
            {
                 return TileCode.rb_2_3;
            }
            if (dir == 4)
            {
                 return TileCode.rb_2_4;
            }
        }
        if (sidecount == 3)
        {
            if (dir == 4)
            {
                 return TileCode.rb_3_4;
            }
        }
        if (sidecount == 4) return TileCode.rb_3_4;

        return 0;
    }
    public int Getd_Soure(int dir)
    {
        switch (dir)
        {
            case 5: {  return TileCode.d_source_5; }
            case 0: {  return TileCode.d_source_0;  }
            case 1: {  return TileCode.d_source_1; }
            case 2: {  return TileCode.d_source_2; }
        }
        return 0;
    }//23,24,25,26
    public int Getd_River(int start, int end)
    {
        int _sum = start - end;
        if (_sum == 1 || _sum == -1) {  return TileCode.d_river_1; }
        if (_sum == 2 || _sum == -2) {  return TileCode.d_river_2;  }
        if (_sum == 3 || _sum == -3) {  return TileCode.d_river_3;  }
        Debug.Log($"이상한 숫자 들어옴 {start}  {end}");
        return 0;
    }//27,28,29
    public int Getd_Beach(int sidecount)
    {
        if (sidecount == 1)
        {
             return TileCode.d_beach_1;
        }
        if (sidecount == 2)
        {
             return TileCode.d_beach_2;
        }
        if (sidecount == 3)
        {
             return TileCode.d_beach_3;
        }
        if (sidecount == 4)
        {
            return TileCode.d_beach_4;
        }
        Debug.Log($"이상한 숫자 들어옴 {sidecount}");
        return 0;
    }//30,31,32
    public int Getd_BeachRiver(int sidecount, int dir)
    {
        if (sidecount == 1)
        {
            if (dir == 2)
            {
                 return TileCode.d_rb_1_2;
            }
            if (dir == 3)
            {
                 return TileCode.d_rb_1_3;
            }
            if (dir == 4)
            {
                 return TileCode.d_rb_1_4;
            }
        }
        if (sidecount == 2)
        {
            if (dir == 3)
            {
                 return TileCode.d_rb_2_3;
            }
            if (dir == 4)
            {
                 return TileCode.d_rb_2_4;
            }
        }
        if (sidecount == 3)
        {
            if (dir == 4)
            {
                 return TileCode.d_rb_3_4;
            }
        }
        if (sidecount == 4) return TileCode.d_rb_3_4;
        return 0;
    }//33,34,35,36,37,38,39


    #endregion
}

public static class MapCode
{
    public const int b_sea = 0;               //바다
    public const int b_land = 1;              //평지
    public const int b_highland = 2;          //고원
    public const int b_mountain = 3;          //산
    public const int b_source = 4;           //발원지
    public const int b_river = 5;             //강
    public const int b_beach = 6;             //해변
    public const int b_riverbeach = 7;
    public const int b_desert = 8;            //사막
    public const int b_desert_highland = 9;     //사막 고원
    public const int b_desert_mountain = 10;   //사막 산
    public const int b_desert_source = 11;     //사막 발원지
    public const int b_desert_river = 12;       //사막 간
    public const int b_desert_beach = 13;     //사막 해변
    public const int b_desert_riverbeach = 14;

    public const int t_null = 0;              //없음
    public const int t_forest = 1;            //숲
    public const int t_desertforest = 2;      //사막 숲
    public const int t_town = 3;              //마을
    public const int t_city = 4;              //도시
    public const int t_castle = 5;           //성채

}
public static class TileCode
{
    public const int sea = 0,
    land = 1,
    highland = 2,
    mountain = 3,
    source_5 = 4,
    source_0 = 5,
    source_1 = 6,
    source_2 = 7,
    river_1 = 8,
    river_2 = 9,
    river_3 = 10,
    beach_1 = 11,
    beach_2 = 12,
    beach_3 = 13,
    beach_4=14,
    rb_1_2 = 15,
    rb_1_3 = 16,
    rb_1_4 = 17,
    rb_2_3 = 18,
    rb_2_4 = 19,
    rb_3_4 = 20,
    desert = 21,
    d_highland = 22,
    d_mountain = 23,
    d_source_5 = 24,
    d_source_0 = 25,
    d_source_1 = 26,
    d_source_2 = 27,
    d_river_1 = 28,
    d_river_2 = 29,
    d_river_3 = 30,
    d_beach_1 = 31,
    d_beach_2 = 32,
    d_beach_3 = 33,
        d_beach_4=34,
    d_rb_1_2 = 35,
    d_rb_1_3 = 36,
    d_rb_1_4 = 37,
    d_rb_2_3 = 38,
    d_rb_2_4 = 39,
    d_rb_3_4 = 40;
    public const int
    town = 1,
    city = 2,
    castle = 3,
    forest = 4,
    forest_river = 5,
    d_town = 6,
    d_city = 7,
    d_castle = 8,
    d_forest = 9,
    d_forest_river = 10;

}