using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
public class Settlement
{
 public bool IsRiver=false;//주변 2칸에 강 여부
  public bool IsForest = false;//주변 1칸에 숲 여부
  public bool IsMine = false;  //주변 1칸에 언덕 여부
  public bool IsMountain = false;//주변 2칸에 산 여부
  public bool IsSea = false;    //주변 1칸에 바다 여부

    public int Wealth = 0;      //부
    public int Faith = 0;       //신앙
    public int Culture = 0;     //문화
    public int Science = 0;     //과학
}
public class MapData
{
    public int Size = 0;
    public int[] BottomMapCode;
    public int[] BottomTileCode;
    public int[] TopMapCode;
    public int[] TopTileCode;
    public int[] RotCode;
    public Vector3Int[] Town_Pos;
    public Vector3Int[] City_Pos;
    public Vector3Int[] Castle_Pos;
    public int TownCount, CityCount, CastleCount;
    public int[]   Wealth_town, Faith_town, Culture_town, Science_town;
  public bool[] Isriver_town, Isforest_town, Ismine_town, Ismountain_town,Issea_town;
  public int[] Wealth_city, Faith_city, Culture_city, Science_city;
  public bool[] Isriver_city, Isforest_city, Ismine_city, Ismountain_city,Issea_city;
  public int[]  Wealth_castle, Faith_castle, Culture_castle, Science_castle;
  public bool[] Isriver_castle, Isforest_castle, Ismine_castle, Ismountain_castle,Issea_castle;
}

