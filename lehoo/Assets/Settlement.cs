using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
public class Settlement
{
 public bool IsRiver=false;//�ֺ� 2ĭ�� �� ����
  public bool IsForest = false;//�ֺ� 1ĭ�� �� ����
  public bool IsMine = false;  //�ֺ� 1ĭ�� ��� ����
  public bool IsMountain = false;//�ֺ� 2ĭ�� �� ����
  public bool IsSea = false;    //�ֺ� 1ĭ�� �ٴ� ����

    public int Wealth = 0;      //��
    public int Faith = 0;       //�ž�
    public int Culture = 0;     //��ȭ
    public int Science = 0;     //����
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

