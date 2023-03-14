using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventData  //기본적인 무작위 풀에서 나오는 이벤트
{
  public int ID;

  public int PlaceCode = 0; //0:거주지  1:시장 2:사원 3:도서관(혹은 극장) 4:캠퍼스
  public int Season = 0;  //분기(1,2,3,4)
  public int[] Conditions;  //나오는 조건 0:노상관  1:강 필요  2:숲 필요   3:언덕 필요   4:산 필요   5:바다 필요
  public string Description = ""; //설명 텍스트
                                  //기본

  //과정
  public int ProgressType;        //과정 종류 0:일반적 1:성향 반전,증가(무작위)  2:경험 소비   3:기술 재분배

  //과정 0에서 사용하는 정보
  public bool IsCheck;      //기술 체크인지
  public bool IsTendency;   //성향 선택지인지
  public int[] Tendencies;  //성향 목록

  public bool Runnable;     //도망 가능?
  public int RunPenalty;    //도망 시 패널티(체력 or 정신력 or 돈)

  public int[] CheckTargets;//검사 목록
  public int[] CheckLevels; //검사 난이도
  public string[] FailureDialogues; //실패시 출력 텍스트
  public int[] FailurePenalty;      //실패시 패털티 종류(체력 or 정신력 or 돈 or 경험 or 특성)
  public bool ProgressAfterFailure; //실패시 이후 진행 여부


  //과업 제시   최대 1개
  public bool IsWork;       //과업 줌?
  public int WorkID;        //제시할 과업 ID


  //보상        최대 4개?
  public bool IsReward;     //보상 줌?
  public int DefaultReward_health, DefaultReward_sanity, DefaultReward_money;
  //기본 보상 양 0:없음 1:적음 2:중간 3:많음
  public int[] ExtraRewardType;
}
