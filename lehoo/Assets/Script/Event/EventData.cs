using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SettlementType { Town,City,Castle,Outer}
public enum PlaceType { Residence,Marketplace,Temple,Library,Theater,Campus}
public enum EnvironmentType { None,River,Forest,Mine,Mountain,Sea }
public enum SelectionType { Single,Verticla, Horizontal,Tendency,Experience,Skill }//Horizontal : 좌 물질 우 정신     Vertical : 위 이성 아래 육체
public enum CheckTarget { None,Pay,Theme,Skill}
public enum PenaltyTarget { HP,Sanity,Gold }
public enum RewardTarget { Experience,GoldAndExperience,Gold,HP,Sanity,Theme,Skill,Trait}
public class EventData  //기본적인 무작위 풀에서 나오는 이벤트
{
    public string Index = "";
    public string Name = "";
    public string Description = "";
    public int PlaceLevel = 0;
    public EnvironmentType Environment;

    public SelectionType Selection_type;
    public string[] Selection_description;
    public int[] Selection_target;
    public int[] Selection_info;

    public bool Failure_stop;
    public string[] Failure_description;
    public int[] Faillure_penalty;

    public string[] Success_description;
    public int[] Success_target;
    public int[] Success_info;
}
public class EventJsonData
{
    public string Index = "";
    public string Name = "";
    public string Description = "";
    public bool[] Season = new bool[4];
    public int Settlement = 0;          //0,1,2,3
    public int Place = 0;               //0,1,2,3,4
    public int PlaceLevel = 0;          //0(전부) 1(낮) 2(중) 3(높)
    public int Environment_Type = 0;         //0,1,2,3,4,5

    public int Selection_Type;           //0,1,2,3,4
    public string[] Selection_Description = new string[2];
    public int[] Selection_Target = new int[2];
    public int[] Selection_Info= new int[2];

    public bool Failure_Stop;
    public string[] Failure_Description=new string[2];
    public int[] Failure_Penalty=new int[2];

    public string[] Success_Description=new string[2];
    public int[] Success_Target=new int[2];
    public int[] Success_Info = new int[2];

    public int Quest;
}
