using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Tile prefabs", menuName = "TileprefabScriptable")]
public class TilePrefabs : ScriptableObject
{
    public Sprite[] Sea;
    [Space(10)]
    public Sprite[] Land;
    public Sprite[] Highland, Mountain;
    public Sprite[] Source_5, Source_0, Source_1, Source_2;
    public Sprite[] River_1, River_2, River_3;
  public Sprite[] Beach_1, Beach_2, Beach_3, Beach_4, Beach_middle;
    public Sprite[] RB_1_2, RB_1_3, RB_1_4;
    public Sprite[] RB_2_3, RB_2_4;
    public Sprite[] RB_3_4;

    [Space(10)]
    public Sprite[] Village;
    public Sprite[] Town, City;
    public Sprite[] Forest, Forest_river;
  [Space(10)]
  public Sprite[] RitualProgress;
  public Sprite[] Ritual;


  private Sprite RandomTile(Sprite[] tile)
  {
    if (tile == null) return null;
    return tile[Random.Range(0, tile.Length)];
  }
  public Sprite GetTile(TileSpriteType tiletype)
  {
    Sprite[] _target = null;
    switch (tiletype)
    {
      case TileSpriteType.NULL:_target = null;break;
      case TileSpriteType.Sea: _target = Sea;break;
      case TileSpriteType.Land: _target = Land;break;
      case TileSpriteType.HighLand: _target = Highland; break;
      case TileSpriteType.Mountain: _target = Mountain; break;
      case TileSpriteType.Source_5: _target = Source_5; break;
      case TileSpriteType.Source_0: _target = Source_0; break;
      case TileSpriteType.Source_1: _target = Source_1; break;
      case TileSpriteType.Source_2: _target = Source_2; break;
      case TileSpriteType.River_1: _target = River_1; break;
      case TileSpriteType.River_2: _target = River_2; break;
      case TileSpriteType.River_3: _target = River_3; break;
      case TileSpriteType.Beach_1: _target = Beach_1; break;
      case TileSpriteType.Beach_2: _target = Beach_2; break;
      case TileSpriteType.Beach_3: _target = Beach_3; break;
      case TileSpriteType.Beach_4: _target = Beach_4; break;
      case TileSpriteType.Beach_middle: _target=Beach_middle; break;
      case TileSpriteType.RiverBeach_1_2: _target = RB_1_2; break;
      case TileSpriteType.RiverBeach_1_3: _target = RB_1_3; break;
      case TileSpriteType.RiverBeach_1_4: _target = RB_1_4; break;
      case TileSpriteType.RiverBeach_2_3: _target = RB_2_3; break;
      case TileSpriteType.RiverBeach_2_4: _target = RB_2_4; break;
      case TileSpriteType.RiverBeach_3_4: _target = RB_3_4; break;
      case TileSpriteType.Forest: _target = Forest;break;
      case TileSpriteType.RiverForest: _target = Forest_river; break;
      case TileSpriteType.Village: _target = Village; break;
      case TileSpriteType.Town: _target = Town; break;
      case TileSpriteType.City: _target = City; break;
      case TileSpriteType.RitualProgress: _target = RitualProgress; break;
      case TileSpriteType.Ritual: _target = Ritual; break;
    }
    return RandomTile(_target);
  }

  public TileSpriteType GetRiver(int maxdir)
  {
    switch (maxdir)
    {
      case 1: return TileSpriteType.River_1;
      case 2: return TileSpriteType.River_2;
        case 3: return TileSpriteType.River_3;
    }
    Debug.Log(maxdir);
    return TileSpriteType.NULL;
  }
  public TileSpriteType GetBeachTile(int seacount)
  {
    switch (seacount)
    {
      case 1:return TileSpriteType.Beach_1;
      case 2: return TileSpriteType.Beach_2;
      case 3: return TileSpriteType.Beach_3;
      case 4: return TileSpriteType.Beach_4;
    }
    return TileSpriteType.NULL;
  }
  public TileSpriteType GetRiverBeach(int seacount,int riverdir)
  {
    switch (seacount)
    {
      case 1:
        switch (riverdir)
        {
          case 2:return TileSpriteType.RiverBeach_1_2;
          case 3:return TileSpriteType.RiverBeach_1_3;
          case 4:return TileSpriteType.RiverBeach_1_4;
        }
        break;
      case 2:
        switch (riverdir)
        {
          case 3:return TileSpriteType.RiverBeach_2_3;
          case 4:return TileSpriteType.RiverBeach_2_4;
        }
        break;
      case 3:
        return TileSpriteType.RiverBeach_3_4;
    }
    Debug.Log($"wrongRiverBeach {seacount}/{riverdir}");
    return TileSpriteType.NULL;
  }
  public TileSpriteType GetSource(int dir)
  {
    switch (dir)
    {
      case 5:return TileSpriteType.Source_5;
      case 0:return TileSpriteType.Source_0;
        case 1:return TileSpriteType.Source_1;
        case 2:return TileSpriteType.Source_2;
    }
    return TileSpriteType.NULL;
  }
}

public enum TileSpriteType
{
  NULL,
  Sea,
  Land,
  HighLand,
  Mountain,
  Source_5, Source_0, Source_1, Source_2,
  River_1, River_2, River_3,
  Beach_1,Beach_2, Beach_3, Beach_4,Beach_middle,
  RiverBeach_1_2, RiverBeach_1_3, RiverBeach_1_4,
  RiverBeach_2_3, RiverBeach_2_4,
  RiverBeach_3_4,
  Forest,
  RiverForest,
  Village, Town, City,
  RitualProgress,Ritual
}
