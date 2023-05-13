using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EventHolder
{
  private const int PlacePer = 2, LevelPer = 3, EnvirPer = 2;
  public List<EventData> AvailableNormalEvents = new List<EventData>();
  public List<FollowEventData> AvailableFollowEvents = new List<FollowEventData>();
  public Dictionary<string, QuestHolder> AvailableQuests = new Dictionary<string, QuestHolder>();

  public List<EventData> AllNormalEvents = new List<EventData>();
  public List<FollowEventData> AllFollowEvents = new List<FollowEventData>();
  public Dictionary<string, QuestHolder> AllQuests = new Dictionary<string, QuestHolder>();
  private string GetSeasonString(int _index)
  {
    switch (_index)
    {
      case 1: return "spring";
      case 2: return "summer";
      case 3: return "fall";
      case 4: return "winter";
    }
    return "lehoo";
  }
  public void ConvertData_Normal(EventJsonData _data)
  {
    string[] _temp;
    string _id = _data.ID;
    int _season = 0;
    int[] _seasons = null;
    if (_data.Season.Equals("0")) { _seasons = new int[1]; _seasons[0] = 0; }
    else
    {
      _temp = _data.Season.Split('@');
      _seasons = new int[_temp.Length];
      for (int i = 0; i < _seasons.Length; i++) { _seasons[i] = int.Parse(_temp[i]); }
    }
    for (int i = 0; i < _seasons.Length; i++)
    {
      _season = _seasons[i];
      EventData Data = new EventData();
      if (_season.Equals(0)) _id = _data.ID;
      else _id = $"{_data.ID}_{GetSeasonString(_season)}";

      TextData _textdata = GameManager.Instance.GetTextData(_id);
      Data.Name = _textdata.Name;
      Data.ID = _id;
      Sprite[] _illusts = GameManager.Instance.ImageHolder.GetEventStartIllust(Data.ID);
      Data.Illust_spring = _illusts[0];
      Data.Illust_summer = _illusts[1];
      Data.Illust_fall = _illusts[2];
      Data.Illust_winter = _illusts[3];
      Data.Description = _textdata.Description;
      Data.Season = _season;
      Data.SettlementType = (SettlementType)_data.Settlement;
      switch (_data.Place)
      {
        case 0: Data.PlaceType = PlaceType.Residence; break;
        case 1: Data.PlaceType = PlaceType.Marketplace; break;
        case 2: Data.PlaceType = PlaceType.Temple; break;
      }
      if (_data.Place > 2)
      {
        if (Data.SettlementType.Equals(SettlementType.City))
        {
          switch (_data.Place)
          {
            case 3: Data.PlaceType = PlaceType.Library; break;
          }
        }
        else
        {
          switch (_data.Place)
          {
            case 3: Data.PlaceType = PlaceType.Theater; break;
            case 4: Data.PlaceType = PlaceType.Academy; break;
          }
        }
      }

      Data.TileCheckType = _data.TileCondition;
      switch (Data.TileCheckType)
      {
        case 0: //검사 없음
          break;
        case 1: //환경
          Data.EnvironmentType = (EnvironmentType)_data.TileCondition_info;
          break;
        case 2: //장소 레벨
          Data.PlaceLevel = _data.TileCondition_info;
          break;
      }

      Data.Selection_type = (SelectionType)_data.Selection_Type;

      switch (Data.Selection_type)//단일,이성육체,정신물질,기타 등등 분류별로 선택지,실패,성공 데이터 만들기
      {
        case SelectionType.Single://단일 선택지
          Data.SelectionDatas = new SelectionData[1];
          Data.SelectionDatas[0] = new SelectionData();
          Data.SelectionDatas[0].ThisSelectionType = (SelectionTargetType)int.Parse(_data.Selection_Target);
          Data.SelectionDatas[0].Description = _textdata.SelectionDescription;
          Data.SelectionDatas[0].SubDescription = _textdata.SelectionSubDescription;
          switch (int.Parse(_data.Selection_Target))
          {
            case 0: //무조건
              break;
            case 1: //지불
              Data.SelectionDatas[0].SelectionPayTarget = (PayOrLossTarget)int.Parse(_data.Selection_Info);
              break;
            case 2: //테마
              Data.SelectionDatas[0].SelectionCheckTheme = (ThemeType)int.Parse(_data.Selection_Info);
              break;
            case 3: //단일 스킬
              Data.SelectionDatas[0].SelectionCheckSkill = (SkillName)int.Parse(_data.Selection_Info);
              break;
          }
          if (Data.SelectionDatas[0].ThisSelectionType.Equals(SelectionTargetType.Check_Theme) || Data.SelectionDatas[0].ThisSelectionType.Equals(SelectionTargetType.Check_Skill))
          {
            Data.FailureDatas = new FailureData[1];
            Data.FailureDatas[0] = new FailureData();
            Data.FailureDatas[0].Description = _textdata.FailDescription;
            Data.FailureDatas[0].Panelty_target = (PenaltyTarget)int.Parse(_data.Failure_Penalty);
            switch (Data.FailureDatas[0].Panelty_target)
            {
              case PenaltyTarget.None: break;
              case PenaltyTarget.Status: Data.FailureDatas[0].Loss_target = (PayOrLossTarget)int.Parse(_data.Failure_Penalty_info); break;
              case PenaltyTarget.EXP: Data.FailureDatas[0].ExpID = _data.Failure_Penalty_info; break;
            }
            _illusts = GameManager.Instance.ImageHolder.GetEventFailIllusts(Data.ID, "0");
            Data.FailureDatas[0].Illust_spring = _illusts[0];
            Data.FailureDatas[0].Illust_summer = _illusts[1];
            Data.FailureDatas[0].Illust_fall = _illusts[2];
            Data.FailureDatas[0].Illust_winter = _illusts[3];
          }
          else if (Data.SelectionDatas[0].ThisSelectionType.Equals(SelectionTargetType.Pay) && Data.SelectionDatas[0].SelectionPayTarget.Equals(PayOrLossTarget.Gold))
          {
            Data.FailureDatas = new FailureData[1]; Data.FailureDatas[0] = GameManager.Instance.MyGameData.GoldFailData;
          }


          Data.SuccessDatas = new SuccessData[1];
          Data.SuccessDatas[0] = new SuccessData();
          Data.SuccessDatas[0].Description = _textdata.SuccessDescription;
          Data.SuccessDatas[0].Reward_Target = (RewardTarget)int.Parse(_data.Reward_Target);
          switch (Data.SuccessDatas[0].Reward_Target)
          {
            case RewardTarget.Experience:
            case RewardTarget.Trait:
              Data.SuccessDatas[0].Reward_ID = _data.Reward_Info; break;
            case RewardTarget.HP: case RewardTarget.Sanity: case RewardTarget.Gold: break;
            case RewardTarget.Theme: Data.SuccessDatas[0].Reward_Theme = (ThemeType)int.Parse(_data.Reward_Info); break;
            case RewardTarget.Skill: Data.SuccessDatas[0].Reward_Skill = (SkillName)int.Parse(_data.Reward_Info); break;
          }
          Data.SuccessDatas[0].SubReward_target = int.Parse(_data.SubReward);
          _illusts = GameManager.Instance.ImageHolder.GetEventSuccessIllusts(Data.ID, "0");
          Data.SuccessDatas[0].Illust_spring = _illusts[0];
          Data.SuccessDatas[0].Illust_summer = _illusts[1];
          Data.SuccessDatas[0].Illust_fall = _illusts[2];
          Data.SuccessDatas[0].Illust_winter = _illusts[3];

          Data.SelectionDatas[0].SelectionSuccesRewards.Add(Data.SuccessDatas[0].Reward_Target);
          switch (Data.SuccessDatas[0].SubReward_target)
          {
            case 0: break;
            case 1: if (!Data.SelectionDatas[0].SelectionSuccesRewards.Contains(RewardTarget.Sanity)) Data.SelectionDatas[0].SelectionSuccesRewards.Add(RewardTarget.Sanity); break;
            case 2: if (!Data.SelectionDatas[0].SelectionSuccesRewards.Contains(RewardTarget.Gold)) Data.SelectionDatas[0].SelectionSuccesRewards.Add(RewardTarget.Gold); break;
            case 3:
              if (!Data.SelectionDatas[0].SelectionSuccesRewards.Contains(RewardTarget.Sanity)) Data.SelectionDatas[0].SelectionSuccesRewards.Add(RewardTarget.Sanity);
              if (!Data.SelectionDatas[0].SelectionSuccesRewards.Contains(RewardTarget.Gold)) Data.SelectionDatas[0].SelectionSuccesRewards.Add(RewardTarget.Gold);
              break;
          }

          break;

        case SelectionType.Verticla://이성육체
        case SelectionType.Horizontal://정신물질
          Data.SelectionDatas = new SelectionData[2];
          Data.SelectionDatas[0] = new SelectionData(); Data.SelectionDatas[1] = new SelectionData();
          int[] _targetint = new int[2];
          int[] _infoint = new int[2];
          string[] _description = _textdata.SelectionDescription.Split('@');
          string[] _subdescriptions = _textdata.SelectionSubDescription.Split('@');
          _temp = _data.Selection_Target.Split('@');
          for (int j = 0; j < _temp.Length; j++)
          {
            _targetint[j] = int.Parse(_temp[j]);
            if (_targetint[j] != 0) _infoint[j] =int.Parse(_data.Selection_Info.Split('@')[j]);
          }

          for (int j = 0; j < Data.SelectionDatas.Length; j++)
          {
            Data.SelectionDatas[j].ThisSelectionType = (SelectionTargetType)_targetint[j];
            Data.SelectionDatas[j].Description = _description[j];
            Data.SelectionDatas[j].SubDescription = _subdescriptions[j];
            switch (_targetint[j])
            {
              case 0: break;
              case 1: Data.SelectionDatas[j].SelectionPayTarget = (PayOrLossTarget)_infoint[j]; break;
              case 2: Data.SelectionDatas[j].SelectionCheckTheme = (ThemeType)_infoint[j]; break;
              case 3: Data.SelectionDatas[j].SelectionCheckSkill = (SkillName)_infoint[j]; break;
            }
          }
          Data.FailureDatas = new FailureData[2];
          Data.FailureDatas[0] = new FailureData(); Data.FailureDatas[1] = new FailureData();
          for (int j = 0; j < Data.FailureDatas.Length; j++)
          {
            if (Data.SelectionDatas[j].ThisSelectionType.Equals(SelectionTargetType.Check_Theme) || Data.SelectionDatas[j].ThisSelectionType.Equals(SelectionTargetType.Check_Skill))
            {
              Data.FailureDatas[j].Description = _textdata.FailDescription.Split('@')[j];
              Data.FailureDatas[j].Panelty_target = (PenaltyTarget)int.Parse(_data.Failure_Penalty.Split('@')[j]);
              switch (Data.FailureDatas[j].Panelty_target)
              {
                case PenaltyTarget.None: break;
                case PenaltyTarget.Status: Data.FailureDatas[j].Loss_target = (PayOrLossTarget)int.Parse(_data.Failure_Penalty_info.Split('@')[j]); break;
                case PenaltyTarget.EXP: Data.FailureDatas[j].ExpID = _data.Failure_Penalty_info.Split('@')[j]; break;
              }//패널티 정보 넣기
              _illusts = GameManager.Instance.ImageHolder.GetEventFailIllusts(Data.ID, j.ToString());
              Data.FailureDatas[j].Illust_spring = _illusts[0];
              Data.FailureDatas[j].Illust_summer = _illusts[1];
              Data.FailureDatas[j].Illust_fall = _illusts[2];
              Data.FailureDatas[j].Illust_winter = _illusts[3];

            }//테마 혹은 스킬 체크인 경우 실패 설명, 패널티 대상 정보, 실패 일러스트 넣기
            else if (Data.SelectionDatas[j].ThisSelectionType.Equals(SelectionTargetType.Pay) && Data.SelectionDatas[j].SelectionPayTarget.Equals(PayOrLossTarget.Gold))
            {
              Data.FailureDatas[j] = GameManager.Instance.MyGameData.GoldFailData;
            }//골드 지불일 경우 미리 준비한 실패 정보를 넣기

          }//실패 정보 구현

          Data.SuccessDatas = new SuccessData[2];
          Data.SuccessDatas[0] = new SuccessData(); Data.SuccessDatas[1] = new SuccessData();
          for (int j = 0; j < Data.SuccessDatas.Length; j++)
          {
            Data.SuccessDatas[j].Description = _textdata.SuccessDescription.Split('@')[j];
            Data.SuccessDatas[j].Reward_Target = (RewardTarget)int.Parse(_data.Reward_Target.Split('@')[j]);
            switch (Data.SuccessDatas[j].Reward_Target)
            {
              case RewardTarget.Experience:
              case RewardTarget.Trait:
                Data.SuccessDatas[j].Reward_ID = _data.Reward_Info.Split('@')[j]; break;

              case RewardTarget.HP: case RewardTarget.Sanity: case RewardTarget.Gold: break;

              case RewardTarget.Theme: Data.SuccessDatas[j].Reward_Theme = (ThemeType)int.Parse(_data.Reward_Info.Split('@')[j]); break;

              case RewardTarget.Skill: Data.SuccessDatas[j].Reward_Skill = (SkillName)int.Parse(_data.Reward_Info.Split('@')[j]); break;
            }//보상 정보 넣기
            Data.SuccessDatas[j].SubReward_target = int.Parse(_data.SubReward.Split('@')[j]);

            Data.SelectionDatas[j].SelectionSuccesRewards.Add(Data.SuccessDatas[j].Reward_Target);
            switch (Data.SuccessDatas[j].SubReward_target)
            {
              case 0: break;
              case 1: if (!Data.SelectionDatas[j].SelectionSuccesRewards.Contains(RewardTarget.Sanity)) Data.SelectionDatas[j].SelectionSuccesRewards.Add(RewardTarget.Sanity); break;
              case 2: if (!Data.SelectionDatas[j].SelectionSuccesRewards.Contains(RewardTarget.Gold)) Data.SelectionDatas[j].SelectionSuccesRewards.Add(RewardTarget.Gold); break;
              case 3:
                if (!Data.SelectionDatas[j].SelectionSuccesRewards.Contains(RewardTarget.Sanity)) Data.SelectionDatas[j].SelectionSuccesRewards.Add(RewardTarget.Sanity);
                if (!Data.SelectionDatas[j].SelectionSuccesRewards.Contains(RewardTarget.Gold)) Data.SelectionDatas[j].SelectionSuccesRewards.Add(RewardTarget.Gold);
                break;
            }

            _illusts = GameManager.Instance.ImageHolder.GetEventSuccessIllusts(Data.ID, j.ToString());
            Data.SuccessDatas[j].Illust_spring = _illusts[0];
            Data.SuccessDatas[j].Illust_summer = _illusts[1];
            Data.SuccessDatas[j].Illust_fall = _illusts[2];
            Data.SuccessDatas[j].Illust_winter = _illusts[3];


          }//성공 정보 구현(설명, 보상, 일러스트)
          break;

        case SelectionType.Tendency:  //성향 체크는 성공뿐이므로 실패 정보가 없음
          Data.SelectionDatas = new SelectionData[1];
          Data.SelectionDatas[0] = new SelectionData();
          Data.SelectionDatas[0].ThisSelectionType = SelectionTargetType.Tendency;
          Data.SelectionDatas[0].Description = _textdata.SelectionDescription;
          Data.SelectionDatas[0].SubDescription = _textdata.SelectionSubDescription;
          Data.SelectionDatas[0] = Data.SelectionDatas[0];
          Data.SuccessDatas = new SuccessData[1];
          Data.SuccessDatas[0] = new SuccessData();
          Data.SuccessDatas[0].Description = _textdata.SuccessDescription;
          Data.SuccessDatas[0].Reward_Target = (RewardTarget)int.Parse(_data.Reward_Target);
          switch (Data.SuccessDatas[0].Reward_Target)
          {
            case RewardTarget.Experience:
            case RewardTarget.Trait:
              Data.SuccessDatas[0].Reward_ID = _data.Reward_Info; break;
            case RewardTarget.HP: case RewardTarget.Sanity: case RewardTarget.Gold: break;
            case RewardTarget.Theme: Data.SuccessDatas[0].Reward_Theme = (ThemeType)int.Parse(_data.Reward_Info); break;
            case RewardTarget.Skill: Data.SuccessDatas[0].Reward_Skill = (SkillName)int.Parse(_data.Reward_Info); break;
          }
          Data.SuccessDatas[0].SubReward_target = int.Parse(_data.SubReward);
          Data.SelectionDatas[0].SelectionSuccesRewards.Add(Data.SuccessDatas[0].Reward_Target);
          switch (Data.SuccessDatas[0].SubReward_target)
          {
            case 0: break;
            case 1: if (!Data.SelectionDatas[0].SelectionSuccesRewards.Contains(RewardTarget.Sanity)) Data.SelectionDatas[0].SelectionSuccesRewards.Add(RewardTarget.Sanity); break;
            case 2: if (!Data.SelectionDatas[0].SelectionSuccesRewards.Contains(RewardTarget.Gold)) Data.SelectionDatas[0].SelectionSuccesRewards.Add(RewardTarget.Gold); break;
            case 3:
              if (!Data.SelectionDatas[0].SelectionSuccesRewards.Contains(RewardTarget.Sanity)) Data.SelectionDatas[0].SelectionSuccesRewards.Add(RewardTarget.Sanity);
              if (!Data.SelectionDatas[0].SelectionSuccesRewards.Contains(RewardTarget.Gold)) Data.SelectionDatas[0].SelectionSuccesRewards.Add(RewardTarget.Gold);
              break;
          }
          _illusts = GameManager.Instance.ImageHolder.GetEventSuccessIllusts(Data.ID, "0");
          Data.SuccessDatas[0].Illust_spring = _illusts[0];
          Data.SuccessDatas[0].Illust_summer = _illusts[1];
          Data.SuccessDatas[0].Illust_fall = _illusts[2];
          Data.SuccessDatas[0].Illust_winter = _illusts[3];
          break;

        case SelectionType.Skill://기술 재배치느 실패가 없다
          Data.SelectionDatas = new SelectionData[1];
          Data.SelectionDatas[0] = new SelectionData();
          Data.SelectionDatas[0].ThisSelectionType = SelectionTargetType.Skill;
          Data.SelectionDatas[0].Description = _textdata.SelectionDescription;
          Data.SelectionDatas[0].SubDescription = _textdata.SelectionSubDescription;
          Data.SelectionDatas[0] = Data.SelectionDatas[0];
          Data.SuccessDatas = new SuccessData[1];
          Data.SuccessDatas[0] = new SuccessData();
          Data.SuccessDatas[0].Description = _textdata.SuccessDescription;
          Data.SuccessDatas[0].Reward_Target = (RewardTarget)int.Parse(_data.Reward_Target);
          switch (Data.SuccessDatas[0].Reward_Target)
          {
            case RewardTarget.Experience:
            case RewardTarget.Trait:
              Data.SuccessDatas[0].Reward_ID = _data.Reward_Info; break;
            case RewardTarget.HP: case RewardTarget.Sanity: case RewardTarget.Gold: break;
            case RewardTarget.Theme: Data.SuccessDatas[0].Reward_Theme = (ThemeType)int.Parse(_data.Reward_Info); break;
            case RewardTarget.Skill: Data.SuccessDatas[0].Reward_Skill = (SkillName)int.Parse(_data.Reward_Info); break;
          }
          Data.SuccessDatas[0].SubReward_target = int.Parse(_data.SubReward);
          Data.SelectionDatas[0].SelectionSuccesRewards.Add(Data.SuccessDatas[0].Reward_Target);
          switch (Data.SuccessDatas[0].SubReward_target)
          {
            case 0: break;
            case 1: if (!Data.SelectionDatas[0].SelectionSuccesRewards.Contains(RewardTarget.Sanity)) Data.SelectionDatas[0].SelectionSuccesRewards.Add(RewardTarget.Sanity); break;
            case 2: if (!Data.SelectionDatas[0].SelectionSuccesRewards.Contains(RewardTarget.Gold)) Data.SelectionDatas[0].SelectionSuccesRewards.Add(RewardTarget.Gold); break;
            case 3:
              if (!Data.SelectionDatas[0].SelectionSuccesRewards.Contains(RewardTarget.Sanity)) Data.SelectionDatas[0].SelectionSuccesRewards.Add(RewardTarget.Sanity);
              if (!Data.SelectionDatas[0].SelectionSuccesRewards.Contains(RewardTarget.Gold)) Data.SelectionDatas[0].SelectionSuccesRewards.Add(RewardTarget.Gold);
              break;
          }
          _illusts = GameManager.Instance.ImageHolder.GetEventSuccessIllusts(Data.ID, "0");
          Data.SuccessDatas[0].Illust_spring = _illusts[0];
          Data.SuccessDatas[0].Illust_summer = _illusts[1];
          Data.SuccessDatas[0].Illust_fall = _illusts[2];
          Data.SuccessDatas[0].Illust_winter = _illusts[3];
          break;

        case SelectionType.Experience:  //경험 재배치는 실패가 없다
          Data.SelectionDatas = new SelectionData[1];
          Data.SelectionDatas[0] = new SelectionData();
          Data.SelectionDatas[0].ThisSelectionType = SelectionTargetType.Exp;
          Data.SelectionDatas[0].Description = _textdata.SelectionDescription;
          Data.SelectionDatas[0].SubDescription = _textdata.SelectionSubDescription;
          Data.SelectionDatas[0] = Data.SelectionDatas[0];
          Data.SuccessDatas = new SuccessData[1];
          Data.SuccessDatas[0] = new SuccessData();
          Data.SuccessDatas[0].Description = _textdata.SuccessDescription;
          Data.SuccessDatas[0].Reward_Target = (RewardTarget)int.Parse(_data.Reward_Target);
          switch (Data.SuccessDatas[0].Reward_Target)
          {
            case RewardTarget.Experience:
            case RewardTarget.Trait:
              Data.SuccessDatas[0].Reward_ID = _data.Reward_Info; break;
            case RewardTarget.HP: case RewardTarget.Sanity: case RewardTarget.Gold: break;
            case RewardTarget.Theme: Data.SuccessDatas[0].Reward_Theme = (ThemeType)int.Parse(_data.Reward_Info); break;
            case RewardTarget.Skill: Data.SuccessDatas[0].Reward_Skill = (SkillName)int.Parse(_data.Reward_Info); break;
          }
          Data.SuccessDatas[0].SubReward_target = int.Parse(_data.SubReward);
          Data.SelectionDatas[0].SelectionSuccesRewards.Add(Data.SuccessDatas[0].Reward_Target);
          switch (Data.SuccessDatas[0].SubReward_target)
          {
            case 0: break;
            case 1: if (!Data.SelectionDatas[0].SelectionSuccesRewards.Contains(RewardTarget.Sanity)) Data.SelectionDatas[0].SelectionSuccesRewards.Add(RewardTarget.Sanity); break;
            case 2: if (!Data.SelectionDatas[0].SelectionSuccesRewards.Contains(RewardTarget.Gold)) Data.SelectionDatas[0].SelectionSuccesRewards.Add(RewardTarget.Gold); break;
            case 3:
              if (!Data.SelectionDatas[0].SelectionSuccesRewards.Contains(RewardTarget.Sanity)) Data.SelectionDatas[0].SelectionSuccesRewards.Add(RewardTarget.Sanity);
              if (!Data.SelectionDatas[0].SelectionSuccesRewards.Contains(RewardTarget.Gold)) Data.SelectionDatas[0].SelectionSuccesRewards.Add(RewardTarget.Gold);
              break;
          }
          _illusts = GameManager.Instance.ImageHolder.GetEventSuccessIllusts(Data.ID, "0");
          Data.SuccessDatas[0].Illust_spring = _illusts[0];
          Data.SuccessDatas[0].Illust_summer = _illusts[1];
          Data.SuccessDatas[0].Illust_fall = _illusts[2];
          Data.SuccessDatas[0].Illust_winter = _illusts[3];
          break;
      }

      AllNormalEvents.Add(Data);
    }
  }

  public void ConvertData_Follow(FollowEventJsonData _data)
  {
    string[] _temp;
    string _id = _data.ID;
    int _season = 0;
    int[] _seasons = null;
    if (_data.Season[0].Equals("0")) { _seasons = new int[1]; _seasons[0] = 0; }
    else
    {
      _temp = _data.Season.Split('@');
      _seasons = new int[_temp.Length];
      for (int i = 0; i < _seasons.Length; i++) { _seasons[i] = int.Parse(_temp[i]); }
    }
    for (int i = 0; i < _seasons.Length; i++)
    {
      _season = _seasons[i];
      FollowEventData Data = new FollowEventData();
      if (_season.Equals(0)) _id = _data.ID;
      else _id = $"{_data.ID}_{GetSeasonString(_season)}";
      TextData _textdata = GameManager.Instance.GetTextData(_id);

      Data.FollowType = (FollowType)_data.FollowType; //선행 대상 이벤트,경험,특성,테마,기술
      Data.FollowTarget = _data.FollowTarget;         //선행 대상- 이벤트,경험,특성이면 Id   테마면 0,1,2,3  기술이면 0~9
      if (Data.FollowType == FollowType.Event)
      {
        Data.FollowTargetSuccess = _data.FollowTargetSuccess == 0 ? true : false;//선행 대상이 이벤트일 경우 성공 혹은 실패
        Data.FollowTendency = _data.FollowTendency;                              //선행 대상이 이벤트일 경우 선택지 형식
      }
      else if (Data.FollowType.Equals(FollowType.Theme) || Data.FollowType.Equals(FollowType.Skill))
        Data.FollowTargetLevel = _data.FollowTargetSuccess;

      Data.Name = _textdata.Name;
      Data.ID = _id;
      Sprite[] _illusts = GameManager.Instance.ImageHolder.GetEventStartIllust(Data.ID);
      Data.Illust_spring = _illusts[0];
      Data.Illust_summer = _illusts[1];
      Data.Illust_fall = _illusts[2];
      Data.Illust_winter = _illusts[3];
      Data.Description = _textdata.Description;
      Data.Season = _season;
      Data.SettlementType = (SettlementType)_data.Settlement;
      switch (_data.Place)
      {
        case 0: Data.PlaceType = PlaceType.Residence; break;
        case 1: Data.PlaceType = PlaceType.Marketplace; break;
        case 2: Data.PlaceType = PlaceType.Temple; break;
      }
      if (_data.Place > 2)
      {
        if (Data.SettlementType.Equals(SettlementType.City))
        {
          switch (_data.Place)
          {
            case 3: Data.PlaceType = PlaceType.Library; break;
          }
        }
        else
        {
          switch (_data.Place)
          {
            case 3: Data.PlaceType = PlaceType.Theater; break;
            case 4: Data.PlaceType = PlaceType.Academy; break;
          }
        }
      }

      Data.TileCheckType = _data.TileCondition;
      switch (Data.TileCheckType)
      {
        case 0: //검사 없음
          break;
        case 1: //환경
          Data.EnvironmentType = (EnvironmentType)_data.TileCondition_info;
          break;
        case 2: //장소 레벨
          Data.PlaceLevel = _data.TileCondition_info;
          break;
      }

      Data.Selection_type = (SelectionType)_data.Selection_Type;

      switch (Data.Selection_type)
      {
        case SelectionType.Single:
          Data.SelectionDatas = new SelectionData[1];
          Data.SelectionDatas[0] = new SelectionData();
          Data.SelectionDatas[0].ThisSelectionType = (SelectionTargetType)int.Parse(_data.Selection_Target);
          Data.SelectionDatas[0].Description = _textdata.SelectionDescription;
          Data.SelectionDatas[0].SubDescription = _textdata.SelectionSubDescription;
          switch (int.Parse(_data.Selection_Target))
          {
            case 0: //무조건
              break;
            case 1: //지불
              Data.SelectionDatas[0].SelectionPayTarget = (PayOrLossTarget)int.Parse(_data.Selection_Info);
              break;
            case 2: //테마
              Data.SelectionDatas[0].SelectionCheckTheme = (ThemeType)int.Parse(_data.Selection_Info);
              break;
            case 3: //단일 스킬
              Data.SelectionDatas[0].SelectionCheckSkill = (SkillName)int.Parse(_data.Selection_Info);
              break;
          }

          if (Data.SelectionDatas[0].ThisSelectionType.Equals(SelectionTargetType.Check_Theme) || Data.SelectionDatas[0].ThisSelectionType.Equals(SelectionTargetType.Check_Skill))
          {
            Data.FailureDatas = new FailureData[1];
            Data.FailureDatas[0] = new FailureData();
            Data.FailureDatas[0].Description = _textdata.FailDescription;
            Data.FailureDatas[0].Panelty_target = (PenaltyTarget)int.Parse(_data.Failure_Penalty);
            switch (Data.FailureDatas[0].Panelty_target)
            {
              case PenaltyTarget.None: break;
              case PenaltyTarget.Status: Data.FailureDatas[0].Loss_target = (PayOrLossTarget)int.Parse(_data.Failure_Penalty_info); break;
              case PenaltyTarget.EXP: Data.FailureDatas[0].ExpID = _data.Failure_Penalty_info; break;
            }
            _illusts= GameManager.Instance.ImageHolder.GetEventFailIllusts(Data.ID, "0");
            Data.FailureDatas[0].Illust_spring = _illusts[0];
            Data.FailureDatas[0].Illust_summer = _illusts[1];
            Data.FailureDatas[0].Illust_fall = _illusts[2];
            Data.FailureDatas[0].Illust_winter = _illusts[3];
          }
          else if (Data.SelectionDatas[0].ThisSelectionType.Equals(SelectionTargetType.Pay) && Data.SelectionDatas[0].SelectionPayTarget.Equals(PayOrLossTarget.Gold))
          {
            Data.FailureDatas = new FailureData[1]; Data.FailureDatas[0] = GameManager.Instance.MyGameData.GoldFailData;
          }

          Data.SuccessDatas = new SuccessData[1];
          Data.SuccessDatas[0] = new SuccessData();
          Data.SuccessDatas[0].Description = _textdata.SuccessDescription;
          Data.SuccessDatas[0].Reward_Target = (RewardTarget)int.Parse(_data.Reward_Target);
          switch (Data.SuccessDatas[0].Reward_Target)
          {
            case RewardTarget.Experience:
            case RewardTarget.Trait:
              Data.SuccessDatas[0].Reward_ID = _data.Reward_Info; break;
            case RewardTarget.HP: case RewardTarget.Sanity: case RewardTarget.Gold: break;
            case RewardTarget.Theme: Data.SuccessDatas[0].Reward_Theme = (ThemeType)int.Parse(_data.Reward_Info); break;
            case RewardTarget.Skill: Data.SuccessDatas[0].Reward_Skill = (SkillName)int.Parse(_data.Reward_Info); break;
          }
          Data.SuccessDatas[0].SubReward_target = int.Parse(_data.SubReward);
          Data.SelectionDatas[0].SelectionSuccesRewards.Add(Data.SuccessDatas[0].Reward_Target);
          switch (Data.SuccessDatas[0].SubReward_target)
          {
            case 0: break;
            case 1: if (!Data.SelectionDatas[0].SelectionSuccesRewards.Contains(RewardTarget.Sanity)) Data.SelectionDatas[0].SelectionSuccesRewards.Add(RewardTarget.Sanity); break;
            case 2: if (!Data.SelectionDatas[0].SelectionSuccesRewards.Contains(RewardTarget.Gold)) Data.SelectionDatas[0].SelectionSuccesRewards.Add(RewardTarget.Gold); break;
            case 3:
              if (!Data.SelectionDatas[0].SelectionSuccesRewards.Contains(RewardTarget.Sanity)) Data.SelectionDatas[0].SelectionSuccesRewards.Add(RewardTarget.Sanity);
              if (!Data.SelectionDatas[0].SelectionSuccesRewards.Contains(RewardTarget.Gold)) Data.SelectionDatas[0].SelectionSuccesRewards.Add(RewardTarget.Gold);
              break;
          }
          _illusts = GameManager.Instance.ImageHolder.GetEventSuccessIllusts(Data.ID, "0");
          Data.SuccessDatas[0].Illust_spring = _illusts[0];
          Data.SuccessDatas[0].Illust_summer = _illusts[1];
          Data.SuccessDatas[0].Illust_fall = _illusts[2];
          Data.SuccessDatas[0].Illust_winter = _illusts[3];

          break;

        case SelectionType.Verticla:
        case SelectionType.Horizontal:
          Data.SelectionDatas = new SelectionData[2];
          Data.SelectionDatas[0] = new SelectionData(); Data.SelectionDatas[1] = new SelectionData();
          int[] _targetint = new int[2];
          int[] _infoint = new int[2];
          string[] _description = _textdata.SelectionDescription.Split('@');
          string[] _subdescriptions = _textdata.SelectionSubDescription.Split('@');
          _temp = _data.Selection_Target.Split('@');
          for (int j = 0; j < _temp.Length; j++)
          {
            _targetint[j] = int.Parse(_temp[j]);
            if (_targetint[j] != 0) _infoint[j] = int.Parse(_data.Selection_Info.Split('@')[j]);
          }
          for (int j = 0; j < Data.SelectionDatas.Length; j++)
          {
            Data.SelectionDatas[j].ThisSelectionType = (SelectionTargetType)_targetint[j];
            Data.SelectionDatas[j].Description = _description[j];
            Data.SelectionDatas[j].SubDescription = _subdescriptions[j];
            switch (_targetint[j])
            {
              case 0: break;
              case 1: Data.SelectionDatas[j].SelectionPayTarget = (PayOrLossTarget)_infoint[j]; break;
              case 2: Data.SelectionDatas[j].SelectionCheckTheme = (ThemeType)_infoint[j]; break;
              case 3: Data.SelectionDatas[j].SelectionCheckSkill = (SkillName)_infoint[j]; break;
            }
          }
          Data.FailureDatas = new FailureData[2];
          Data.FailureDatas[0] = new FailureData(); Data.FailureDatas[1] = new FailureData();
          for (int j = 0; j < Data.FailureDatas.Length; j++)
          {
            if (Data.SelectionDatas[j].ThisSelectionType.Equals(SelectionTargetType.Check_Theme) || Data.SelectionDatas[j].ThisSelectionType.Equals(SelectionTargetType.Check_Skill))
            {
              Data.FailureDatas[j].Description = _textdata.FailDescription.Split('@')[j];
              Data.FailureDatas[j].Panelty_target = (PenaltyTarget)int.Parse(_data.Failure_Penalty.Split('@')[j]);
              switch (Data.FailureDatas[j].Panelty_target)
              {
                case PenaltyTarget.None: break;
                case PenaltyTarget.Status: Data.FailureDatas[j].Loss_target = (PayOrLossTarget)int.Parse(_data.Failure_Penalty_info.Split('@')[j]); break;
                case PenaltyTarget.EXP: Data.FailureDatas[j].ExpID = _data.Failure_Penalty_info.Split('@')[j]; break;
              }
              _illusts = GameManager.Instance.ImageHolder.GetEventSuccessIllusts(Data.ID, j.ToString());
              Data.FailureDatas[j].Illust_spring = _illusts[0];
              Data.FailureDatas[j].Illust_summer = _illusts[1];
              Data.FailureDatas[j].Illust_fall = _illusts[2];
              Data.FailureDatas[j].Illust_winter = _illusts[3];
            }
            else if (Data.SelectionDatas[j].ThisSelectionType.Equals(SelectionTargetType.Pay) && Data.SelectionDatas[j].SelectionPayTarget.Equals(PayOrLossTarget.Gold))
            {
              Data.FailureDatas[j] = GameManager.Instance.MyGameData.GoldFailData;
            }

          }

          Data.SuccessDatas = new SuccessData[2];
          Data.SuccessDatas[0] = new SuccessData(); Data.SuccessDatas[1] = new SuccessData();
          for (int j = 0; j < Data.SuccessDatas.Length; j++)
          {
            Data.SuccessDatas[j].Description = _textdata.SuccessDescription.Split('@')[j];
            Data.SuccessDatas[j].Reward_Target = (RewardTarget)int.Parse(_data.Reward_Target.Split('@')[j]);
            switch (Data.SuccessDatas[j].Reward_Target)
            {
              case RewardTarget.Experience:
              case RewardTarget.Trait:
                Data.SuccessDatas[j].Reward_ID = _data.Reward_Info.Split('@')[j]; break;

              case RewardTarget.HP: case RewardTarget.Sanity: case RewardTarget.Gold: break;

              case RewardTarget.Theme: Data.SuccessDatas[j].Reward_Theme = (ThemeType)int.Parse(_data.Reward_Info.Split('@')[j]); break;

              case RewardTarget.Skill: Data.SuccessDatas[j].Reward_Skill = (SkillName)int.Parse(_data.Reward_Info.Split('@')[j]); break;
            }
            Data.SuccessDatas[j].SubReward_target = int.Parse(_data.SubReward.Split('@')[j]);
            Data.SelectionDatas[j].SelectionSuccesRewards.Add(Data.SuccessDatas[j].Reward_Target);
            switch (Data.SuccessDatas[j].SubReward_target)
            {
              case 0: break;
              case 1: if (!Data.SelectionDatas[j].SelectionSuccesRewards.Contains(RewardTarget.Sanity)) Data.SelectionDatas[j].SelectionSuccesRewards.Add(RewardTarget.Sanity); break;
              case 2: if (!Data.SelectionDatas[j].SelectionSuccesRewards.Contains(RewardTarget.Gold)) Data.SelectionDatas[j].SelectionSuccesRewards.Add(RewardTarget.Gold); break;
              case 3:
                if (!Data.SelectionDatas[j].SelectionSuccesRewards.Contains(RewardTarget.Sanity)) Data.SelectionDatas[j].SelectionSuccesRewards.Add(RewardTarget.Sanity);
                if (!Data.SelectionDatas[j].SelectionSuccesRewards.Contains(RewardTarget.Gold)) Data.SelectionDatas[j].SelectionSuccesRewards.Add(RewardTarget.Gold);
                break;
            }
            _illusts = GameManager.Instance.ImageHolder.GetEventSuccessIllusts(Data.ID, j.ToString());
            Data.SuccessDatas[j].Illust_spring = _illusts[0];
            Data.SuccessDatas[j].Illust_summer = _illusts[1];
            Data.SuccessDatas[j].Illust_fall = _illusts[2];
            Data.SuccessDatas[j].Illust_winter = _illusts[3];
          }
          break;

        case SelectionType.Tendency:
          Data.SelectionDatas = new SelectionData[1];
          Data.SelectionDatas[0] = new SelectionData();
          Data.SelectionDatas[0] = new SelectionData();
          Data.SelectionDatas[0].ThisSelectionType = SelectionTargetType.Tendency;
          Data.SelectionDatas[0].Description = _textdata.SelectionDescription;
          Data.SelectionDatas[0].SubDescription = _textdata.SelectionSubDescription;
          Data.SelectionDatas[0] = Data.SelectionDatas[0];
          Data.SuccessDatas = new SuccessData[1];
          Data.SuccessDatas[0] = new SuccessData();
          Data.SuccessDatas[0].Description = _textdata.SuccessDescription;
          Data.SuccessDatas[0].Reward_Target = (RewardTarget)int.Parse(_data.Reward_Target);
          switch (Data.SuccessDatas[0].Reward_Target)
          {
            case RewardTarget.Experience:
            case RewardTarget.Trait:
              Data.SuccessDatas[0].Reward_ID = _data.Reward_Info; break;
            case RewardTarget.HP: case RewardTarget.Sanity: case RewardTarget.Gold: break;
            case RewardTarget.Theme: Data.SuccessDatas[0].Reward_Theme = (ThemeType)int.Parse(_data.Reward_Info); break;
            case RewardTarget.Skill: Data.SuccessDatas[0].Reward_Skill = (SkillName)int.Parse(_data.Reward_Info); break;
          }
          Data.SuccessDatas[0].SubReward_target = int.Parse(_data.SubReward);
          Data.SelectionDatas[0].SelectionSuccesRewards.Add(Data.SuccessDatas[0].Reward_Target);
          switch (Data.SuccessDatas[0].SubReward_target)
          {
            case 0: break;
            case 1: if (!Data.SelectionDatas[0].SelectionSuccesRewards.Contains(RewardTarget.Sanity)) Data.SelectionDatas[0].SelectionSuccesRewards.Add(RewardTarget.Sanity); break;
            case 2: if (!Data.SelectionDatas[0].SelectionSuccesRewards.Contains(RewardTarget.Gold)) Data.SelectionDatas[0].SelectionSuccesRewards.Add(RewardTarget.Gold); break;
            case 3:
              if (!Data.SelectionDatas[0].SelectionSuccesRewards.Contains(RewardTarget.Sanity)) Data.SelectionDatas[0].SelectionSuccesRewards.Add(RewardTarget.Sanity);
              if (!Data.SelectionDatas[0].SelectionSuccesRewards.Contains(RewardTarget.Gold)) Data.SelectionDatas[0].SelectionSuccesRewards.Add(RewardTarget.Gold);
              break;
          }
          _illusts = GameManager.Instance.ImageHolder.GetEventSuccessIllusts(Data.ID,"0");
          Data.SuccessDatas[0].Illust_spring = _illusts[0];
          Data.SuccessDatas[0].Illust_summer = _illusts[1];
          Data.SuccessDatas[0].Illust_fall = _illusts[2];
          Data.SuccessDatas[0].Illust_winter = _illusts[3];

          break;

        case SelectionType.Skill:
          Data.SelectionDatas = new SelectionData[1];
          Data.SelectionDatas[0].ThisSelectionType = SelectionTargetType.Skill;
          Data.SelectionDatas[0].Description = _textdata.SelectionDescription;
          Data.SelectionDatas[0].SubDescription = _textdata.SelectionSubDescription;
          Data.SelectionDatas[0] = Data.SelectionDatas[0];
          Data.SuccessDatas = new SuccessData[1];
          Data.SuccessDatas[0] = new SuccessData();
          Data.SuccessDatas[0].Description = _textdata.SuccessDescription;
          Data.SuccessDatas[0].Reward_Target = (RewardTarget)int.Parse(_data.Reward_Target);
          switch (Data.SuccessDatas[0].Reward_Target)
          {
            case RewardTarget.Experience:
            case RewardTarget.Trait:
              Data.SuccessDatas[0].Reward_ID = _data.Reward_Info; break;
            case RewardTarget.HP: case RewardTarget.Sanity: case RewardTarget.Gold: break;
            case RewardTarget.Theme: Data.SuccessDatas[0].Reward_Theme = (ThemeType)int.Parse(_data.Reward_Info); break;
            case RewardTarget.Skill: Data.SuccessDatas[0].Reward_Skill = (SkillName)int.Parse(_data.Reward_Info); break;
          }
          Data.SuccessDatas[0].SubReward_target = int.Parse(_data.SubReward);
          Data.SelectionDatas[0].SelectionSuccesRewards.Add(Data.SuccessDatas[0].Reward_Target);
          switch (Data.SuccessDatas[0].SubReward_target)
          {
            case 0: break;
            case 1: if (!Data.SelectionDatas[0].SelectionSuccesRewards.Contains(RewardTarget.Sanity)) Data.SelectionDatas[0].SelectionSuccesRewards.Add(RewardTarget.Sanity); break;
            case 2: if (!Data.SelectionDatas[0].SelectionSuccesRewards.Contains(RewardTarget.Gold)) Data.SelectionDatas[0].SelectionSuccesRewards.Add(RewardTarget.Gold); break;
            case 3:
              if (!Data.SelectionDatas[0].SelectionSuccesRewards.Contains(RewardTarget.Sanity)) Data.SelectionDatas[0].SelectionSuccesRewards.Add(RewardTarget.Sanity);
              if (!Data.SelectionDatas[0].SelectionSuccesRewards.Contains(RewardTarget.Gold)) Data.SelectionDatas[0].SelectionSuccesRewards.Add(RewardTarget.Gold);
              break;
          }
          _illusts = GameManager.Instance.ImageHolder.GetEventSuccessIllusts(Data.ID, "0");
          Data.SuccessDatas[0].Illust_spring = _illusts[0];
          Data.SuccessDatas[0].Illust_summer = _illusts[1];
          Data.SuccessDatas[0].Illust_fall = _illusts[2];
          Data.SuccessDatas[0].Illust_winter = _illusts[3];
          break;

        case SelectionType.Experience:
          Data.SelectionDatas = new SelectionData[1];
          Data.SelectionDatas[0] = new SelectionData();
          Data.SelectionDatas[0].ThisSelectionType = SelectionTargetType.Exp;
          Data.SelectionDatas[0].Description = _textdata.SelectionDescription;
          Data.SelectionDatas[0].SubDescription = _textdata.SelectionSubDescription;
          Data.SelectionDatas[0] = Data.SelectionDatas[0];
          Data.SuccessDatas = new SuccessData[1];
          Data.SuccessDatas[0] = new SuccessData();
          Data.SuccessDatas[0].Description = _textdata.SuccessDescription;
          Data.SuccessDatas[0].Reward_Target = (RewardTarget)int.Parse(_data.Reward_Target);
          switch (Data.SuccessDatas[0].Reward_Target)
          {
            case RewardTarget.Experience:
            case RewardTarget.Trait:
              Data.SuccessDatas[0].Reward_ID = _data.Reward_Info; break;
            case RewardTarget.HP: case RewardTarget.Sanity: case RewardTarget.Gold: break;
            case RewardTarget.Theme: Data.SuccessDatas[0].Reward_Theme = (ThemeType)int.Parse(_data.Reward_Info); break;
            case RewardTarget.Skill: Data.SuccessDatas[0].Reward_Skill = (SkillName)int.Parse(_data.Reward_Info); break;
          }
          Data.SuccessDatas[0].SubReward_target = int.Parse(_data.SubReward);
          Data.SelectionDatas[0].SelectionSuccesRewards.Add(Data.SuccessDatas[0].Reward_Target);
          switch (Data.SuccessDatas[0].SubReward_target)
          {
            case 0: break;
            case 1: if (!Data.SelectionDatas[0].SelectionSuccesRewards.Contains(RewardTarget.Sanity)) Data.SelectionDatas[0].SelectionSuccesRewards.Add(RewardTarget.Sanity); break;
            case 2: if (!Data.SelectionDatas[0].SelectionSuccesRewards.Contains(RewardTarget.Gold)) Data.SelectionDatas[0].SelectionSuccesRewards.Add(RewardTarget.Gold); break;
            case 3:
              if (!Data.SelectionDatas[0].SelectionSuccesRewards.Contains(RewardTarget.Sanity)) Data.SelectionDatas[0].SelectionSuccesRewards.Add(RewardTarget.Sanity);
              if (!Data.SelectionDatas[0].SelectionSuccesRewards.Contains(RewardTarget.Gold)) Data.SelectionDatas[0].SelectionSuccesRewards.Add(RewardTarget.Gold);
              break;
          }
          _illusts = GameManager.Instance.ImageHolder.GetEventSuccessIllusts(Data.ID, "0");
          Data.SuccessDatas[0].Illust_spring = _illusts[0];
          Data.SuccessDatas[0].Illust_summer = _illusts[1];
          Data.SuccessDatas[0].Illust_fall = _illusts[2];
          Data.SuccessDatas[0].Illust_winter = _illusts[3];
          break;
      }

      AvailableFollowEvents.Add(Data);
    }
  }
  public void ConvertData_Quest(QuestEventDataJson _data)
  {
    string[] _temp;
    string _id = _data.ID;
    int _season = 0;
    int[] _seasons = null;
    if (_data.Season[0].Equals("0")) { _seasons = new int[1]; _seasons[0] = 0; }
    else
    {
      _temp = _data.Season.Split('@');
      _seasons = new int[_temp.Length];
      for (int i = 0; i < _seasons.Length; i++) { _seasons[i] = int.Parse(_temp[i]); }
    }
    for (int i = 0; i < _seasons.Length; i++)
    {
      _season = _seasons[i];
      QuestEventData Data = new QuestEventData();
      if (_season.Equals(0)) _id = _data.ID;
      else _id = $"{_data.ID}_{GetSeasonString(_season)}";
      TextData _textdata = GameManager.Instance.GetTextData(_id);
      Data.Name = _textdata.Name;
      Data.ID = _id;
      Sprite[] _startillusts = GameManager.Instance.ImageHolder.GetEventStartIllust(Data.ID);
      Data.Illust_spring = _startillusts[0];
      Data.Illust_summer = _startillusts[1];
      Data.Illust_fall = _startillusts[2];
      Data.Illust_winter = _startillusts[3];
      Data.Description = _textdata.Description;
      Data.Season = _season;
      switch (_data.Settlement)
      {
        case 0:
          Data.SettlementType = SettlementType.None;
          break;
        case 1:
          Data.SettlementType = SettlementType.Town;
          break;
        case 2:
          Data.SettlementType = SettlementType.City;
          break;
        case 3:
          Data.SettlementType = SettlementType.Castle;
          break;
        case 4:
          Data.SettlementType = SettlementType.Outer;
          break;
      }
      switch (_data.Place)
      {
        case 0: Data.PlaceType = PlaceType.Residence; break;
        case 1: Data.PlaceType = PlaceType.Marketplace; break;
        case 2: Data.PlaceType = PlaceType.Temple; break;
      }
      if (_data.Place > 2)
      {
        if (Data.SettlementType.Equals(SettlementType.City))
        {
          switch (_data.Place)
          {
            case 3: Data.PlaceType = PlaceType.Library; break;
          }
        }
        else
        {
          switch (_data.Place)
          {
            case 3: Data.PlaceType = PlaceType.Theater; break;
            case 4: Data.PlaceType = PlaceType.Academy; break;
          }
        }
      }

      Data.TileCheckType = _data.Environment_Type.Equals(0) ? 0 : 1;
      switch (Data.TileCheckType)
      {
        case 0: //검사 없음
          break;
        case 1: //환경
          Data.EnvironmentType = (EnvironmentType)(_data.Environment_Type + 1);
          break;
      }

      Data.Selection_type = (SelectionType)_data.Selection_Type;

      switch (Data.Selection_type)
      {
        case SelectionType.Single:
          Data.SelectionDatas = new SelectionData[1];
          Data.SelectionDatas[0] = new SelectionData();
          Data.SelectionDatas[0].ThisSelectionType = (SelectionTargetType)int.Parse(_data.Selection_Target);
          Data.SelectionDatas[0].Description = _textdata.Description;
          Data.SelectionDatas[0].SubDescription = _textdata.SelectionSubDescription;
          switch (int.Parse(_data.Selection_Target))
          {
            case 0: //무조건
              break;
            case 1: //지불
              Data.SelectionDatas[0].SelectionPayTarget = (PayOrLossTarget)int.Parse(_data.Selection_Info);
              break;
            case 2: //테마
              Data.SelectionDatas[0].SelectionCheckTheme = (ThemeType)int.Parse(_data.Selection_Info);
              break;
            case 3: //단일 스킬
              Data.SelectionDatas[0].SelectionCheckSkill = (SkillName)int.Parse(_data.Selection_Info);
              break;
          }
          if (Data.SelectionDatas[0].ThisSelectionType.Equals(SelectionTargetType.Check_Theme) || Data.SelectionDatas[0].ThisSelectionType.Equals(SelectionTargetType.Check_Skill))
          {
            Data.FailureDatas = new FailureData[1];
            Data.FailureDatas[0] = new FailureData();
            Data.FailureDatas[0].Description = _textdata.FailDescription;
            Data.FailureDatas[0].Panelty_target = (PenaltyTarget)int.Parse(_data.Failure_Penalty);
            switch (Data.FailureDatas[0].Panelty_target)
            {
              case PenaltyTarget.None: break;
              case PenaltyTarget.Status: Data.FailureDatas[0].Loss_target = (PayOrLossTarget)int.Parse(_data.Failure_Penalty_info); break;
              case PenaltyTarget.EXP: Data.FailureDatas[0].ExpID = _data.Failure_Penalty_info; break;
            }
            _startillusts = GameManager.Instance.ImageHolder.GetEventFailIllusts(Data.ID,"0");
            Data.FailureDatas[0].Illust_spring = _startillusts[0];
            Data.FailureDatas[0].Illust_summer = _startillusts[1];
            Data.FailureDatas[0].Illust_fall = _startillusts[2];
            Data.FailureDatas[0].Illust_winter = _startillusts[3];
          }
          else if (Data.SelectionDatas[0].ThisSelectionType.Equals(SelectionTargetType.Pay) && Data.SelectionDatas[0].SelectionPayTarget.Equals(PayOrLossTarget.Gold))
          {
            Data.FailureDatas = new FailureData[1]; Data.FailureDatas[0] = GameManager.Instance.MyGameData.GoldFailData;
          }

          Data.SuccessDatas = new SuccessData[1];
          Data.SuccessDatas[0] = new SuccessData();
          Data.SuccessDatas[0].Description = _textdata.SuccessDescription;
          Data.SuccessDatas[0].Reward_Target = (RewardTarget)int.Parse(_data.Reward_Target);
          switch (Data.SuccessDatas[0].Reward_Target)
          {
            case RewardTarget.Experience:
            case RewardTarget.Trait:
              Data.SuccessDatas[0].Reward_ID = _data.Reward_Info; break;
            case RewardTarget.HP: case RewardTarget.Sanity: case RewardTarget.Gold: break;
            case RewardTarget.Theme: Data.SuccessDatas[0].Reward_Theme = (ThemeType)int.Parse(_data.Reward_Info); break;
            case RewardTarget.Skill: Data.SuccessDatas[0].Reward_Skill = (SkillName)int.Parse(_data.Reward_Info); break;
          }
          Data.SuccessDatas[0].SubReward_target = int.Parse(_data.SubReward);
          Data.SelectionDatas[0].SelectionSuccesRewards.Add(Data.SuccessDatas[0].Reward_Target);
          switch (Data.SuccessDatas[0].SubReward_target)
          {
            case 0: break;
            case 1: if (!Data.SelectionDatas[0].SelectionSuccesRewards.Contains(RewardTarget.Sanity)) Data.SelectionDatas[0].SelectionSuccesRewards.Add(RewardTarget.Sanity); break;
            case 2: if (!Data.SelectionDatas[0].SelectionSuccesRewards.Contains(RewardTarget.Gold)) Data.SelectionDatas[0].SelectionSuccesRewards.Add(RewardTarget.Gold); break;
            case 3:
              if (!Data.SelectionDatas[0].SelectionSuccesRewards.Contains(RewardTarget.Sanity)) Data.SelectionDatas[0].SelectionSuccesRewards.Add(RewardTarget.Sanity);
              if (!Data.SelectionDatas[0].SelectionSuccesRewards.Contains(RewardTarget.Gold)) Data.SelectionDatas[0].SelectionSuccesRewards.Add(RewardTarget.Gold);
              break;
          }
          _startillusts = GameManager.Instance.ImageHolder.GetEventSuccessIllusts(Data.ID, "0");
          Data.SuccessDatas[0].Illust_spring = _startillusts[0];
          Data.SuccessDatas[0].Illust_summer = _startillusts[1];
          Data.SuccessDatas[0].Illust_fall = _startillusts[2];
          Data.SuccessDatas[0].Illust_winter = _startillusts[3];
          break;

        case SelectionType.Verticla:
        case SelectionType.Horizontal:
          Data.SelectionDatas = new SelectionData[2];
          Data.SelectionDatas[0] = new SelectionData(); Data.SelectionDatas[1] = new SelectionData();
          int[] _targetint = new int[2];
          int[] _infoint = new int[2];
          string[] _description = _textdata.SelectionDescription.Split('@');
          string[] _subdescriptions = _textdata.SelectionSubDescription.Split('@');
          _temp = _data.Selection_Target.Split('@');
          for (int j = 0; j < _temp.Length; j++)
          {
            _targetint[j] = int.Parse(_temp[j]);
            if (_targetint[j] != 0) _infoint[j] = int.Parse(_data.Selection_Info.Split('@')[j]);
          }
          for (int j = 0; j < Data.SelectionDatas.Length; j++)
          {
            Data.SelectionDatas[j].ThisSelectionType = (SelectionTargetType)_targetint[j];
            Data.SelectionDatas[j].Description = _description[j];
            Data.SelectionDatas[j].SubDescription = _subdescriptions[j];
            switch (_targetint[j])
            {
              case 0: break;
              case 1: Data.SelectionDatas[j].SelectionPayTarget = (PayOrLossTarget)_infoint[j]; break;
              case 2: Data.SelectionDatas[j].SelectionCheckTheme = (ThemeType)_infoint[j]; break;
              case 3: Data.SelectionDatas[j].SelectionCheckSkill = (SkillName)_infoint[j]; break;
            }
          }
          Data.FailureDatas = new FailureData[2];
          Data.FailureDatas[0] = new FailureData(); Data.FailureDatas[1] = new FailureData();
          for (int j = 0; j < Data.FailureDatas.Length; j++)
          {
            if (Data.SelectionDatas[j].ThisSelectionType.Equals(SelectionTargetType.Check_Theme) || Data.SelectionDatas[j].ThisSelectionType.Equals(SelectionTargetType.Check_Skill))
            {
              Data.FailureDatas[j].Description = _textdata.FailDescription.Split('@')[j];
              Data.FailureDatas[j].Panelty_target = (PenaltyTarget)int.Parse(_data.Failure_Penalty.Split('@')[j]);
              switch (Data.FailureDatas[j].Panelty_target)
              {
                case PenaltyTarget.None: break;
                case PenaltyTarget.Status: Data.FailureDatas[j].Loss_target = (PayOrLossTarget)int.Parse(_data.Failure_Penalty_info.Split('@')[j]); break;
                case PenaltyTarget.EXP: Data.FailureDatas[j].ExpID = _data.Failure_Penalty_info.Split('@')[j]; break;
              }
              _startillusts = GameManager.Instance.ImageHolder.GetEventFailIllusts(Data.ID, j.ToString());
              Data.FailureDatas[j].Illust_spring = _startillusts[0];
              Data.FailureDatas[j].Illust_summer = _startillusts[1];
              Data.FailureDatas[j].Illust_fall = _startillusts[2];
              Data.FailureDatas[j].Illust_winter = _startillusts[3];
            }
            else if (Data.SelectionDatas[j].ThisSelectionType.Equals(SelectionTargetType.Pay) && Data.SelectionDatas[j].SelectionPayTarget.Equals(PayOrLossTarget.Gold))
            {
              Data.FailureDatas[j] = GameManager.Instance.MyGameData.GoldFailData;
            }

          }

          Data.SuccessDatas = new SuccessData[2];
          Data.SuccessDatas[0] = new SuccessData(); Data.SuccessDatas[1] = new SuccessData();
          for (int j = 0; j < Data.SuccessDatas.Length; j++)
          {
            Data.SuccessDatas[j].Description = _textdata.SuccessDescription.Split('@')[j];
            Data.SuccessDatas[j].Reward_Target = (RewardTarget)int.Parse(_data.Reward_Target.Split('@')[j]);
            switch (Data.SuccessDatas[j].Reward_Target)
            {
              case RewardTarget.Experience:
              case RewardTarget.Trait:
                Data.SuccessDatas[j].Reward_ID = _data.Reward_Info.Split('@')[j]; break;

              case RewardTarget.HP: case RewardTarget.Sanity: case RewardTarget.Gold: break;

              case RewardTarget.Theme: Data.SuccessDatas[j].Reward_Theme = (ThemeType)int.Parse(_data.Reward_Info.Split('@')[j]); break;

              case RewardTarget.Skill: Data.SuccessDatas[j].Reward_Skill = (SkillName)int.Parse(_data.Reward_Info.Split('@')[j]); break;
            }
            Data.SuccessDatas[j].SubReward_target = int.Parse(_data.SubReward.Split('@')[j]);
            Data.SelectionDatas[j].SelectionSuccesRewards.Add(Data.SuccessDatas[j].Reward_Target);
            switch (Data.SuccessDatas[j].SubReward_target)
            {
              case 0: break;
              case 1: if (!Data.SelectionDatas[j].SelectionSuccesRewards.Contains(RewardTarget.Sanity)) Data.SelectionDatas[j].SelectionSuccesRewards.Add(RewardTarget.Sanity); break;
              case 2: if (!Data.SelectionDatas[j].SelectionSuccesRewards.Contains(RewardTarget.Gold)) Data.SelectionDatas[j].SelectionSuccesRewards.Add(RewardTarget.Gold); break;
              case 3:
                if (!Data.SelectionDatas[j].SelectionSuccesRewards.Contains(RewardTarget.Sanity)) Data.SelectionDatas[j].SelectionSuccesRewards.Add(RewardTarget.Sanity);
                if (!Data.SelectionDatas[j].SelectionSuccesRewards.Contains(RewardTarget.Gold)) Data.SelectionDatas[j].SelectionSuccesRewards.Add(RewardTarget.Gold);
                break;
            }
            _startillusts = GameManager.Instance.ImageHolder.GetEventSuccessIllusts(Data.ID, j.ToString());
            Data.SuccessDatas[j].Illust_spring = _startillusts[0];
            Data.SuccessDatas[j].Illust_summer = _startillusts[1];
            Data.SuccessDatas[j].Illust_fall = _startillusts[2];
            Data.SuccessDatas[j].Illust_winter = _startillusts[3];
          }
          break;

        case SelectionType.Tendency:
          Data.SelectionDatas = new SelectionData[1];
          Data.SelectionDatas[0] = new SelectionData();
          Data.SelectionDatas[0] = new SelectionData();
          Data.SelectionDatas[0].ThisSelectionType = SelectionTargetType.Tendency;
          Data.SelectionDatas[0].Description = _textdata.Description;
          Data.SelectionDatas[0].SubDescription = _textdata.SelectionSubDescription;
          Data.SelectionDatas[0] = Data.SelectionDatas[0];
          Data.SuccessDatas = new SuccessData[1];
          Data.SuccessDatas[0] = new SuccessData();
          Data.SuccessDatas[0].Description = _textdata.SuccessDescription;
          Data.SuccessDatas[0].Reward_Target = (RewardTarget)int.Parse(_data.Reward_Target);
          switch (Data.SuccessDatas[0].Reward_Target)
          {
            case RewardTarget.Experience:
            case RewardTarget.Trait:
              Data.SuccessDatas[0].Reward_ID = _data.Reward_Info; break;
            case RewardTarget.HP: case RewardTarget.Sanity: case RewardTarget.Gold: break;
            case RewardTarget.Theme: Data.SuccessDatas[0].Reward_Theme = (ThemeType)int.Parse(_data.Reward_Info); break;
            case RewardTarget.Skill: Data.SuccessDatas[0].Reward_Skill = (SkillName)int.Parse(_data.Reward_Info); break;
          }
          Data.SuccessDatas[0].SubReward_target = int.Parse(_data.SubReward);
          Data.SelectionDatas[0].SelectionSuccesRewards.Add(Data.SuccessDatas[0].Reward_Target);
          switch (Data.SuccessDatas[0].SubReward_target)
          {
            case 0: break;
            case 1: if (!Data.SelectionDatas[0].SelectionSuccesRewards.Contains(RewardTarget.Sanity)) Data.SelectionDatas[0].SelectionSuccesRewards.Add(RewardTarget.Sanity); break;
            case 2: if (!Data.SelectionDatas[0].SelectionSuccesRewards.Contains(RewardTarget.Gold)) Data.SelectionDatas[0].SelectionSuccesRewards.Add(RewardTarget.Gold); break;
            case 3:
              if (!Data.SelectionDatas[0].SelectionSuccesRewards.Contains(RewardTarget.Sanity)) Data.SelectionDatas[0].SelectionSuccesRewards.Add(RewardTarget.Sanity);
              if (!Data.SelectionDatas[0].SelectionSuccesRewards.Contains(RewardTarget.Gold)) Data.SelectionDatas[0].SelectionSuccesRewards.Add(RewardTarget.Gold);
              break;
          }
          _startillusts = GameManager.Instance.ImageHolder.GetEventSuccessIllusts(Data.ID, "0");
          Data.SuccessDatas[0].Illust_spring = _startillusts[0];
          Data.SuccessDatas[0].Illust_summer = _startillusts[1];
          Data.SuccessDatas[0].Illust_fall = _startillusts[2];
          Data.SuccessDatas[0].Illust_winter = _startillusts[3];
          break;

        case SelectionType.Skill:
          Data.SelectionDatas = new SelectionData[1];
          Data.SelectionDatas[0].ThisSelectionType = SelectionTargetType.Skill;
          Data.SelectionDatas[0].Description = _textdata.Description;
          Data.SelectionDatas[0].SubDescription = _textdata.SelectionSubDescription;
          Data.SelectionDatas[0] = Data.SelectionDatas[0];
          Data.SuccessDatas = new SuccessData[1];
          Data.SuccessDatas[0] = new SuccessData();
          Data.SuccessDatas[0].Description = _textdata.SuccessDescription;
          Data.SuccessDatas[0].Reward_Target = (RewardTarget)int.Parse(_data.Reward_Target);
          switch (Data.SuccessDatas[0].Reward_Target)
          {
            case RewardTarget.Experience:
            case RewardTarget.Trait:
              Data.SuccessDatas[0].Reward_ID = _data.Reward_Info; break;
            case RewardTarget.HP: case RewardTarget.Sanity: case RewardTarget.Gold: break;
            case RewardTarget.Theme: Data.SuccessDatas[0].Reward_Theme = (ThemeType)int.Parse(_data.Reward_Info); break;
            case RewardTarget.Skill: Data.SuccessDatas[0].Reward_Skill = (SkillName)int.Parse(_data.Reward_Info); break;
          }
          Data.SuccessDatas[0].SubReward_target = int.Parse(_data.SubReward);
          Data.SelectionDatas[0].SelectionSuccesRewards.Add(Data.SuccessDatas[0].Reward_Target);
          switch (Data.SuccessDatas[0].SubReward_target)
          {
            case 0: break;
            case 1: if (!Data.SelectionDatas[0].SelectionSuccesRewards.Contains(RewardTarget.Sanity)) Data.SelectionDatas[0].SelectionSuccesRewards.Add(RewardTarget.Sanity); break;
            case 2: if (!Data.SelectionDatas[0].SelectionSuccesRewards.Contains(RewardTarget.Gold)) Data.SelectionDatas[0].SelectionSuccesRewards.Add(RewardTarget.Gold); break;
            case 3:
              if (!Data.SelectionDatas[0].SelectionSuccesRewards.Contains(RewardTarget.Sanity)) Data.SelectionDatas[0].SelectionSuccesRewards.Add(RewardTarget.Sanity);
              if (!Data.SelectionDatas[0].SelectionSuccesRewards.Contains(RewardTarget.Gold)) Data.SelectionDatas[0].SelectionSuccesRewards.Add(RewardTarget.Gold);
              break;
          }
          _startillusts = GameManager.Instance.ImageHolder.GetEventSuccessIllusts(Data.ID, "0");
          Data.SuccessDatas[0].Illust_spring = _startillusts[0];
          Data.SuccessDatas[0].Illust_summer = _startillusts[1];
          Data.SuccessDatas[0].Illust_fall = _startillusts[2];
          Data.SuccessDatas[0].Illust_winter = _startillusts[3];
          break;

        case SelectionType.Experience:
          Data.SelectionDatas = new SelectionData[1];
          Data.SelectionDatas[0] = new SelectionData();
          Data.SelectionDatas[0].ThisSelectionType = SelectionTargetType.Exp;
          Data.SelectionDatas[0].Description = _textdata.Description;
          Data.SelectionDatas[0].SubDescription = _textdata.SelectionSubDescription;
          Data.SelectionDatas[0] = Data.SelectionDatas[0];
          Data.SuccessDatas = new SuccessData[1];
          Data.SuccessDatas[0] = new SuccessData();
          Data.SuccessDatas[0].Description = _textdata.SuccessDescription;
          Data.SuccessDatas[0].Reward_Target = (RewardTarget)int.Parse(_data.Reward_Target);
          switch (Data.SuccessDatas[0].Reward_Target)
          {
            case RewardTarget.Experience:
            case RewardTarget.Trait:
              Data.SuccessDatas[0].Reward_ID = _data.Reward_Info; break;
            case RewardTarget.HP: case RewardTarget.Sanity: case RewardTarget.Gold: break;
            case RewardTarget.Theme: Data.SuccessDatas[0].Reward_Theme = (ThemeType)int.Parse(_data.Reward_Info); break;
            case RewardTarget.Skill: Data.SuccessDatas[0].Reward_Skill = (SkillName)int.Parse(_data.Reward_Info); break;
          }
          Data.SuccessDatas[0].SubReward_target = int.Parse(_data.SubReward);
          Data.SelectionDatas[0].SelectionSuccesRewards.Add(Data.SuccessDatas[0].Reward_Target);
          switch (Data.SuccessDatas[0].SubReward_target)
          {
            case 0: break;
            case 1: if (!Data.SelectionDatas[0].SelectionSuccesRewards.Contains(RewardTarget.Sanity)) Data.SelectionDatas[0].SelectionSuccesRewards.Add(RewardTarget.Sanity); break;
            case 2: if (!Data.SelectionDatas[0].SelectionSuccesRewards.Contains(RewardTarget.Gold)) Data.SelectionDatas[0].SelectionSuccesRewards.Add(RewardTarget.Gold); break;
            case 3:
              if (!Data.SelectionDatas[0].SelectionSuccesRewards.Contains(RewardTarget.Sanity)) Data.SelectionDatas[0].SelectionSuccesRewards.Add(RewardTarget.Sanity);
              if (!Data.SelectionDatas[0].SelectionSuccesRewards.Contains(RewardTarget.Gold)) Data.SelectionDatas[0].SelectionSuccesRewards.Add(RewardTarget.Gold);
              break;
          }
          _startillusts = GameManager.Instance.ImageHolder.GetEventSuccessIllusts(Data.ID, "0");
          Data.SuccessDatas[0].Illust_spring = _startillusts[0];
          Data.SuccessDatas[0].Illust_summer = _startillusts[1];
          Data.SuccessDatas[0].Illust_fall = _startillusts[2];
          Data.SuccessDatas[0].Illust_winter = _startillusts[3];
          break;
      }

      QuestHolder _quest = null;
      if (AllQuests.ContainsKey(_data.QuestId))
      {
        _quest = AllQuests[_data.QuestId];
      }//딕셔너리에 퀘스트가 이미 만들어졌을 경우
      else
      {
        _quest = new QuestHolder();
      }//딕셔너리에 퀘스트가 없을 경우 새로 하나 만들기

      switch (_data.Sequence)
      {
        case 0://기
          _quest.QuestID = _data.QuestId; //퀘스트 ID는 QuestID
          _quest.QuestName = _textdata.Name;  //이름은 시작 이벤트의 이름
          _quest.StartDialogue = _textdata.Description; //설명(게임 내부)는 시작 이벤트의 설명
          _quest.PreDescription = _textdata.SelectionDescription;//설명(게임 외부)는 시작 이벤트의 선택지 설명
          _quest.Illust = Data.Illust_spring;//대표 일러스트는 퀘스트 ID

          _quest.QuestPreview_rising = GameManager.Instance.GetTextData(_quest.QuestID + "rising").Name;
          List<string> _climaxlist=new List<string>();
          foreach(var _textdatas in GameManager.Instance.TextDic)
          {
            if (_textdatas.Key.Contains(_quest.QuestID + "climax")) _climaxlist.Add(_textdatas.Value.Name);
          }
          _quest.QuestPreview_Climax=_climaxlist.ToArray();
          TextData _fallingtextdata = GameManager.Instance.GetTextData(_quest.QuestID + "falling");
          _quest.QuestPreview_Falling = _fallingtextdata.Name;
          _quest.StopSelectionName = _fallingtextdata.Description.Split('@')[0];
          _quest.StopSelectionSub = _fallingtextdata.SelectionDescription.Split('@')[0];
          _quest.ContinueSelectionName = _fallingtextdata.Description.Split('@')[1];
          _quest.ContinueSelectionSub = _fallingtextdata.SelectionDescription.Split('@')[1];
          break;
        case 1://승
          _quest.Eventlist_Rising.Add(Data);
          break;
        case 2://전
          _quest.Eventlist_Climax.Add(Data);
          break;
        case 3://결
          _quest.Event_Falling = Data;
          break;
      }
      if (!AllQuests.ContainsKey(_data.QuestId)) AllQuests.Add(_data.QuestId, _quest);
    }

  }

  public void LoadAllEvents()
  {
    bool _isenable = true;
    foreach (var _event in AllNormalEvents)
    {
      _isenable = true;
      foreach (var _removeid in GameManager.Instance.MyGameData.RemoveEvent)
      {
        if (_event.ID.Contains(_removeid))
        {
          _isenable = false;
          break;
        }
      }
      if (_isenable) AvailableNormalEvents.Add(_event);
    }
    foreach (var _event in AllFollowEvents)
    {
      _isenable = true;
      foreach (var _removeid in GameManager.Instance.MyGameData.RemoveEvent)
      {
        if (_event.ID.Contains(_removeid))
        {
          _isenable = false;
          break;
        }
      }
      if (_isenable) AvailableFollowEvents.Add(_event);
    }
    foreach (var _quest in AllQuests)
    {
      if (GameManager.Instance.MyGameData.ClearQuest.Contains(_quest.Key)) continue;
      if (GameManager.Instance.MyGameData.CurrentQuest == _quest.Value) continue;
      AvailableQuests.Add(_quest.Key, _quest.Value);
    }
  }//Gamemanager.instance.GameData를 기반으로 이미 클리어한 이벤트 빼고 다 활성화 리스트에 넣기
  public void RemoveEvent(string _ID)
  {
    string _defaultid = _ID;
    if (_ID.Contains("spring") || _ID.Contains("summer") || _ID.Contains("fall") || _ID.Contains("winter"))
    {
      string[] _temp = _defaultid.Split("_");
      _defaultid = "";
      for (int i = 0; i < _temp.Length - 2; i++)
      {
        _defaultid += _temp[i];
        if (i != _temp.Length - 1) _defaultid += "_";
      }
      //봄,여름,가을,겨울이 들어가 있는 ID면 계절 텍스트 뺀 아이디를 기준으로 삼음
    }

    if (_defaultid.Contains("town") || _defaultid.Contains("city") || _defaultid.Contains("castke"))
    {
      string[] _temp = _defaultid.Split("_");
      _defaultid = "";
      for (int i = 0; i < _temp.Length - 2; i++)
      {
        _defaultid += _temp[i];
        if (i != _temp.Length - 1) _defaultid += "_";
      }
      //마을,도시,성채가 들어가 있는 ID면 계절 빼고 정착지도 뺀 아이디를 기준으로 삼음
    }

    List<EventData> _normals = new List<EventData>();
    List<FollowEventData> _follows = new List<FollowEventData>();

    foreach (var _data in AvailableNormalEvents)
      if (_data.ID.Contains(_defaultid))
        _normals.Add(_data);

    foreach (var _data in AvailableFollowEvents)
      if (_data.ID.Contains(_defaultid))
        _follows.Add(_data);

    foreach (var _deletenormal in _normals) AvailableNormalEvents.Remove(_deletenormal);
    foreach (var _deletfollow in _follows) AvailableFollowEvents.Remove(_deletfollow);
    GameManager.Instance.MyGameData.RemoveEvent.Add(_ID);
  }
    /// <summary>
    /// 외부 이벤트 반환
    /// </summary>
    /// <param name="envir"></param>
    /// <returns></returns>
    public EventDataDefulat ReturnOutsideEvent(List<EnvironmentType> envir)
    {
        List<EventDataDefulat> _temp = new List<EventDataDefulat>();
        List<EventDataDefulat> _followevents = new List<EventDataDefulat>();
        foreach (var _follow in AvailableFollowEvents)
        {
            switch (_follow.FollowType)
            {
                case FollowType.Event:  //이벤트 연계일 경우 
                    List<string> _checktarget = new List<string>();
                    if (_follow.FollowTargetSuccess == true)
                    {
                        switch (_follow.FollowTendency)
                        {
                            case 0: _checktarget = GameManager.Instance.MyGameData.SuccessEvent_None; break;
                            case 1: _checktarget = GameManager.Instance.MyGameData.SuccessEvent_Rational; break;
                            case 2: _checktarget = GameManager.Instance.MyGameData.SuccessEvent_Physical; break;
                            case 3: _checktarget = GameManager.Instance.MyGameData.SuccessEvent_Mental; break;
                            case 4: _checktarget = GameManager.Instance.MyGameData.SuccessEvent_Material; break;
                            case 5: _checktarget = GameManager.Instance.MyGameData.SuccessEvent_All; break;
                        }
                    }
                    else
                    {
                        switch (_follow.FollowTendency)
                        {
                            case 0: _checktarget = GameManager.Instance.MyGameData.FailEvent_None; break;
                            case 1: _checktarget = GameManager.Instance.MyGameData.FailEvent_Rational; break;
                            case 2: _checktarget = GameManager.Instance.MyGameData.FailEvent_Physical; break;
                            case 3: _checktarget = GameManager.Instance.MyGameData.FailEvent_Mental; break;
                            case 4: _checktarget = GameManager.Instance.MyGameData.FailEvent_Material; break;
                            case 5: _checktarget = GameManager.Instance.MyGameData.FailEvent_All; break;
                        }
                    }
                    if (_checktarget.Contains(_follow.FollowTarget)) _temp.Add(_follow);
                    break;
                case FollowType.EXP://경험 연계일 경우 현재 보유한 경험 ID랑 맞는지 확인
                    foreach (var _data in GameManager.Instance.MyGameData.LongTermEXP)
                        if (_follow.FollowTarget.Equals(_data.ID)) _temp.Add(_follow);
                    foreach (var _data in GameManager.Instance.MyGameData.ShortTermEXP)
                        if (_follow.FollowTarget.Equals(_data.ID)) _temp.Add(_follow);
                    break;
                case FollowType.Trait://특성 연계일 경우 현재 보유한 특성 ID랑 맞는지 확인
                    foreach (var _data in GameManager.Instance.MyGameData.Traits)
                        if (_follow.FollowTarget.Equals(_data.ID)) _temp.Add(_follow);
                    break;
                case FollowType.Theme://테마 연계일 경우 현재 테마의 레벨이 기준 이상인지 확인
                    int _targetlevel = 0;
                    ThemeType _type = ThemeType.Conversation; ;
                    switch (_follow.FollowTarget)
                    {
                        case "0"://대화 테마
                            _type = ThemeType.Conversation; break;
                        case "1"://무력 테마
                            _type = ThemeType.Force; break;
                        case "2"://생존 테마
                            _type = ThemeType.Wild; break;
                        case "3"://학식 테마
                            _type = ThemeType.Intelligence; break;
                    }
                    _targetlevel = GameManager.Instance.MyGameData.GetThemeLevelBySkill(_type) + GameManager.Instance.MyGameData.GetEffectThemeCount_Trait(_type) +
                                GameManager.Instance.MyGameData.GetEffectThemeCount_Exp(_type) + GameManager.Instance.MyGameData.GetThemeLevelByTendency(_type);
                    if (_follow.FollowTargetLevel <= _targetlevel) _temp.Add(_follow);
                    break;
                case FollowType.Skill://기술 연계일 경우 현재 기술의 레벨이 기준 이상인지 확인
                    SkillName _skill = (SkillName)int.Parse(_follow.FollowTarget);
                    if (_follow.FollowTargetLevel <= GameManager.Instance.MyGameData.Skills[_skill].Level) _temp.Add(_follow);
                    break;
            }
        }
        foreach(var _event in _temp)
        {
            if (_event.SettlementType.Equals(SettlementType.Outer))
            {
                if (envir.Contains(_event.EnvironmentType) || _event.TileCheckType.Equals(0)) _followevents.Add(_event);
            }
        }//연계 충족된 이벤트들 중 환경 가능한 것들 리스트
        _temp.Clear();
        List<EventDataDefulat> _normalevents = new List<EventDataDefulat>();
        foreach(var _event in AvailableNormalEvents)
        {
            if (_event.SettlementType.Equals(SettlementType.Outer))
                if (envir.Contains(_event.EnvironmentType) || _event.TileCheckType.Equals(0)) _normalevents.Add(_event);
        }//일반 야외 이벤트 중 환경 가능한 것 들 리스트

        if (_followevents.Count > 0 && _normalevents.Count > 0)
        {
            if (Random.Range(0, 100) < 80) return _followevents[Random.Range(0, _followevents.Count)];
            else return _normalevents[Random.Range(0, _normalevents.Count)];
        }//두 풀 다 있ㅇ면 80로 연계
        else if (_followevents.Count > 0 && _normalevents.Count.Equals(0)) return _followevents[Random.Range(0, _followevents.Count)];
        else return _normalevents[Random.Range(0, _normalevents.Count)];
    }
    /// <summary>
    /// 정착지의 장소 이벤트 반환
    /// </summary>
    /// <param name="settletype"></param>
    /// <param name="placetype"></param>
    /// <param name="placelevel"></param>
    /// <param name="envir"></param>
    /// <returns></returns>
    public EventDataDefulat ReturnPlaceEvent(SettlementType settletype,PlaceType placetype,int placelevel,List<EnvironmentType> envir)
    {
        List<EventDataDefulat> _temp = new List<EventDataDefulat>();
        List<EventDataDefulat> _follows = new List<EventDataDefulat>();
        foreach (var _follow in AvailableFollowEvents)
        {
            switch (_follow.FollowType)
            {
                case FollowType.Event:  //이벤트 연계일 경우 
                    List<string> _checktarget = new List<string>();
                    if (_follow.FollowTargetSuccess == true)
                    {
                        switch (_follow.FollowTendency)
                        {
                            case 0: _checktarget = GameManager.Instance.MyGameData.SuccessEvent_None; break;
                            case 1: _checktarget = GameManager.Instance.MyGameData.SuccessEvent_Rational; break;
                            case 2: _checktarget = GameManager.Instance.MyGameData.SuccessEvent_Physical; break;
                            case 3: _checktarget = GameManager.Instance.MyGameData.SuccessEvent_Mental; break;
                            case 4: _checktarget = GameManager.Instance.MyGameData.SuccessEvent_Material; break;
                            case 5: _checktarget = GameManager.Instance.MyGameData.SuccessEvent_All; break;
                        }
                    }
                    else
                    {
                        switch (_follow.FollowTendency)
                        {
                            case 0: _checktarget = GameManager.Instance.MyGameData.FailEvent_None; break;
                            case 1: _checktarget = GameManager.Instance.MyGameData.FailEvent_Rational; break;
                            case 2: _checktarget = GameManager.Instance.MyGameData.FailEvent_Physical; break;
                            case 3: _checktarget = GameManager.Instance.MyGameData.FailEvent_Mental; break;
                            case 4: _checktarget = GameManager.Instance.MyGameData.FailEvent_Material; break;
                            case 5: _checktarget = GameManager.Instance.MyGameData.FailEvent_All; break;
                        }
                    }
                    if (_checktarget.Contains(_follow.FollowTarget)) _temp.Add(_follow);
                    break;
                case FollowType.EXP://경험 연계일 경우 현재 보유한 경험 ID랑 맞는지 확인
                    foreach (var _data in GameManager.Instance.MyGameData.LongTermEXP)
                        if (_follow.FollowTarget.Equals(_data.ID)) _temp.Add(_follow);
                    foreach (var _data in GameManager.Instance.MyGameData.ShortTermEXP)
                        if (_follow.FollowTarget.Equals(_data.ID)) _temp.Add(_follow);
                    break;
                case FollowType.Trait://특성 연계일 경우 현재 보유한 특성 ID랑 맞는지 확인
                    foreach (var _data in GameManager.Instance.MyGameData.Traits)
                        if (_follow.FollowTarget.Equals(_data.ID)) _temp.Add(_follow);
                    break;
                case FollowType.Theme://테마 연계일 경우 현재 테마의 레벨이 기준 이상인지 확인
                    int _targetlevel = 0;
                    ThemeType _type = ThemeType.Conversation; ;
                    switch (_follow.FollowTarget)
                    {
                        case "0"://대화 테마
                            _type = ThemeType.Conversation; break;
                        case "1"://무력 테마
                            _type = ThemeType.Force; break;
                        case "2"://생존 테마
                            _type = ThemeType.Wild; break;
                        case "3"://학식 테마
                            _type = ThemeType.Intelligence; break;
                    }
                    _targetlevel = GameManager.Instance.MyGameData.GetThemeLevelBySkill(_type) + GameManager.Instance.MyGameData.GetEffectThemeCount_Trait(_type) +
                                GameManager.Instance.MyGameData.GetEffectThemeCount_Exp(_type) + GameManager.Instance.MyGameData.GetThemeLevelByTendency(_type);
                    if (_follow.FollowTargetLevel <= _targetlevel) _temp.Add(_follow);
                    break;
                case FollowType.Skill://기술 연계일 경우 현재 기술의 레벨이 기준 이상인지 확인
                    SkillName _skill = (SkillName)int.Parse(_follow.FollowTarget);
                    if (_follow.FollowTargetLevel <= GameManager.Instance.MyGameData.Skills[_skill].Level) _temp.Add(_follow);
                    break;
            }
        }
        foreach(var _event in _temp)
        {
            if (isenable(_event)) _follows.Add(_event);
        }//해당 장소의 적합한 연계 이벤트 리스트

        List<EventDataDefulat> _normals = new List<EventDataDefulat>();
        foreach (var _event in AvailableNormalEvents)
        {
            if (isenable(_event)) _normals.Add(_event);
        }//해당 장소의 적합한 일반 이벤트 리스트

        if (_follows.Count > 0 && _normals.Count > 0)
        {
            if (Random.Range(0, 100) < 80) return _follows[Random.Range(0, _follows.Count)];
            else return _normals[Random.Range(0, _normals.Count)];
        }
        else if(_follows.Count>0&&_normals.Count.Equals(0)) return _follows[Random.Range(0, _follows.Count)];
        else return _normals[Random.Range(0, _normals.Count)];

        bool isenable(EventDataDefulat eventdata)
        {
            if (!eventdata.SettlementType.Equals(settletype)) return false;     //해당 이벤트의 정착지가 맞지 않으면 X

            if (!eventdata.PlaceType.Equals(placetype)) return false;           //해당 이벤트의 장소가 맞지 않으면 X

            if (eventdata.TileCheckType.Equals(1))                              //환경 검사일 때
                if (!envir.Contains(eventdata.EnvironmentType))return false;    //해당 이벤트의 환경이 맞지 않으면 X

            if (eventdata.TileCheckType.Equals(2))                              //장소 레벨 검사일 때
                if (!eventdata.PlaceLevel.Equals(placelevel)) return false;     //해당 이벤트의 장소 레벨이 맞지 않으면 X

            //여기까지 왔으면 다 통과했으므로 O 반환
            return true;
        }
    }
    /// <summary>
    /// 해당 정착지에서 사용 가능한 퀘스트 이벤트 하나 반환
    /// </summary>
    /// <param name="tiledata"></param>
    /// <returns></returns>
    public EventDataDefulat ReturnQuestEvent(TargetTileEventData tiledata)
    {
        QuestHolder _currentquest = GameManager.Instance.MyGameData.CurrentQuest;
        if (_currentquest == null) return null;
        switch (_currentquest.CurrentSequence)
        {
            case QuestSequence.Falling: //마지막 단계에서는 마지막 이벤트 딱 하나 
                if (isenable(_currentquest.Event_Falling))return _currentquest.Event_Falling;
                else return null;
            case QuestSequence.Climax:
                EventDataDefulat _targetevent = _currentquest.Eventlist_Climax[_currentquest.FinishedClimaxCount];
                if (isenable(_targetevent)) return _targetevent;
                else return null;
            default:
                List<EventDataDefulat> _temp = new List<EventDataDefulat>();
                foreach(var _event in _currentquest.Eventlist_Rising)
                {
                    if (_currentquest.Eventlist_Rising_clear.Contains(_event.ID)) continue;
                    if(isenable(_event)) _temp.Add(_event);
                }
                if (_temp.Count.Equals(0)) return null;
                else return _temp[Random.Range(0, _temp.Count)];

        }

        bool isenable(EventDataDefulat eventdata)
        {
            if (eventdata.SettlementType.Equals(SettlementType.Outer))
            {
                if (!tiledata.SettlementType.Equals(SettlementType.Outer)) return false;
            }   //야외 이벤트일 경우 야외만 받음
            else
            {
                if (eventdata.SettlementType.Equals(SettlementType.None))
                {
                    if (tiledata.SettlementType.Equals(SettlementType.Outer)) return false;
                    //야외 빼고 다 받음
                }   //전역 정착지일 경우
                else if (!eventdata.SettlementType.Equals(tiledata.SettlementType)) return false;
                //그 외 동일한 것 아닐 시 X 반환

            }   //정착지 이벤트일 경우

            if (!tiledata.PlaceData.ContainsKey(eventdata.PlaceType)) return false;           //해당 이벤트의 장소가 맞지 않으면 X

            if (eventdata.TileCheckType.Equals(1))                              //환경 검사일 때
                if (!tiledata.EnvironmentType.Contains(eventdata.EnvironmentType)) return false;    //해당 이벤트의 환경이 맞지 않으면 X

            if (eventdata.TileCheckType.Equals(2))                              //장소 레벨 검사일 때
                if (!eventdata.PlaceLevel.Equals(tiledata.PlaceData[eventdata.PlaceType])) return false;     //해당 이벤트의 장소 레벨이 맞지 않으면 X

            //여기까지 왔으면 다 통과했으므로 O 반환
            return true;
        }

    }
}
public class TargetTileEventData
{
  public SettlementType SettlementType; //정착지 타입
  public Dictionary<PlaceType, int> PlaceData = new Dictionary<PlaceType, int>(); //(정착지일 경우) 장소 타입과 장소 레벨
  public List<EnvironmentType> EnvironmentType = new List<EnvironmentType>();//주위 환경 타입
  public int Season;
}
#region 이벤트 정보에 쓰는 배열들
public enum FollowType { Event,EXP,Trait,Theme,Skill}
public enum SettlementType { Town,City,Castle,Outer,None}
public enum PlaceType { Residence,Marketplace,Temple,Library,Theater,Academy,NULL}
public enum EnvironmentType { River,Forest,Highland,Mountain,Sea }
public enum SelectionType { Single,Verticla, Horizontal,Tendency,Experience,Skill }// Vertical : 위 이성 아래 육체    Horizontal : 좌 정신 우 물질    
public enum CheckTarget { None,Pay,Theme,Skill}
public enum PenaltyTarget { None,Status,EXP }
public enum RewardTarget { Experience,HP,Sanity,Gold,Theme,Skill,Trait,None}
public enum EventSequence { Progress,Clear}//Suggest: 3개 제시하는 단계  Progress: 선택지 버튼 눌러야 하는 단계  Clear: 보상 수령해야 하는 단계
public enum QuestSequence { Start,Rising,Climax,Falling}
#endregion  
public class EventDataDefulat
{
  public string ID = "";        //계절 별로 분화된 ID(ID_(계절)), 텍스트랑 일러스트 아이디로 사용함
  public Sprite Illust_spring = null;
  public Sprite Illust_summer = null;
  public Sprite Illust_fall = null;
  public Sprite Illust_winter = null;
  public Sprite Illust
  {
    get
    {
      switch (GameManager.Instance.MyGameData.Turn)
      {
        case 0:return Illust_spring;
        case 1:return Illust_summer;
        case 2:return Illust_fall;
        default:return Illust_winter;
      }
    }
  }
  public string Name;
  public string Description;
  public int Season = 0;  //0,1,2,3,4
  public SettlementType SettlementType;
  public PlaceType PlaceType;
  public int TileCheckType = 0;//0:없음 1: 환경요구 2: 레벨요구
  public EnvironmentType EnvironmentType = EnvironmentType.River;
  public int PlaceLevel = 0;  //0:전역 1,2,3

  public SelectionType Selection_type;
  public SelectionData[] SelectionDatas;

  public FailureData[] FailureDatas;

  public SuccessData[] SuccessDatas;
}
public class SelectionData
{
    public SelectionTargetType ThisSelectionType = SelectionTargetType.None;
  public string Description = "";
  public string SubDescription = "";
  public PayOrLossTarget SelectionPayTarget = PayOrLossTarget.HP;
    public ThemeType SelectionCheckTheme = ThemeType.Conversation;
    public SkillName SelectionCheckSkill = SkillName.Speech;
  public List<RewardTarget> SelectionSuccesRewards=new List<RewardTarget>();
}    

public class FailureData
{
  public string Description = "";
  public PenaltyTarget Panelty_target;
  public PayOrLossTarget Loss_target= PayOrLossTarget.HP;
  public string ExpID;
  public Sprite Illust_spring = null;
  public Sprite Illust_summer = null;
  public Sprite Illust_fall = null;
  public Sprite Illust_winter = null;
  public Sprite Illust
  {
    get
    {
      switch (GameManager.Instance.MyGameData.Turn)
      {
        case 0: return Illust_spring;
        case 1: return Illust_summer;
        case 2: return Illust_fall;
        default: return Illust_winter;
      }
    }
  }
}
public enum SelectionTargetType { None, Pay, Check_Theme, Check_Skill, Tendency, Skill, Exp }//선택지 개별 내용
public enum PayOrLossTarget { HP,Sanity,Gold}
public class SuccessData
{
  public string Description = "";
  public Sprite Illust_spring = null;
  public Sprite Illust_summer = null;
  public Sprite Illust_fall = null;
  public Sprite Illust_winter = null;
  public Sprite Illust
  {
    get
    {
      switch (GameManager.Instance.MyGameData.Turn)
      {
        case 0: return Illust_spring;
        case 1: return Illust_summer;
        case 2: return Illust_fall;
        default: return Illust_winter;
      }
    }
  }
  public RewardTarget Reward_Target;
  public ThemeType Reward_Theme;
  public SkillName Reward_Skill;
  public string Reward_ID;
  private int reward_value_origin = -1;
  public int Reward_Value_Origin
  {
    get 
    { if (reward_value_origin.Equals(-1))
        switch (Reward_Target)
        {
          case RewardTarget.HP: reward_value_origin = GameManager.Instance.MyGameData.RewardHPValue_origin; break;
          case RewardTarget.Sanity: reward_value_origin = GameManager.Instance.MyGameData.RewardSanityValue_origin; break;
          case RewardTarget.Gold: reward_value_origin = GameManager.Instance.MyGameData.RewardGoldValue_origin; break;
        }
      return reward_value_origin;
    }
    set { reward_value_origin = value; }
  }

  private int reward_value_modified = -1;
  public int Reward_Value_Modified
  {
    get
    {
      if (reward_value_modified.Equals(-1))
        switch (Reward_Target)
        {
          case RewardTarget.HP: reward_value_modified = GameManager.Instance.MyGameData.RewardHPValue_modified; break;
          case RewardTarget.Sanity: reward_value_modified = GameManager.Instance.MyGameData.RewardSanityValue_modified; break;
          case RewardTarget.Gold: reward_value_modified = GameManager.Instance.MyGameData.RewardGoldValue_modified; break;
        }
      return reward_value_modified;
    }
    set { reward_value_modified = value; }
  }
  public int SubReward_target;

}

public class EventData:EventDataDefulat
{}//기본 이벤트
public class FollowEventData:EventDataDefulat
{
  public FollowType FollowType = 0;
  public string FollowTarget = "";
  public int FollowTargetLevel = 0;
  public bool FollowTargetSuccess = false;
  public int FollowTendency = 0;          //이벤트일 경우 기타,이성,육체,정신,물질 선택지 여부
}//연계 이벤트
public class QuestEventData : EventDataDefulat
{
  public string QuestID = "";
  public QuestSequence TargetQuestSequence= QuestSequence.Start;
  public int ClimaxIndex = 0;
}
public class QuestHolder
{
    public string QuestID = "";     //퀘스트 ID
  public string QuestName = "";     //퀘스트의 이름
  public int QuestClearCount
  {
    get { return SuccessRisingCount + SuccesClimaxCount; }
  } //성공한 이벤트(rising,climax) 개수
  public int FinishedRisingCount = 0;  //실패+성공한 Rising 개수
  public int FinishedClimaxCount = 0;  //실패+성공한 Climax 개수
  public int SuccessRisingCount = 0, SuccesClimaxCount = 0;//실패한 Rising,Climax 개수
  public void AddClearEvent(QuestEventData _eventdata)
  {
    switch (_eventdata.TargetQuestSequence)
    {
      case QuestSequence.Start:
        CurrentSequence = QuestSequence.Rising;
        break;
      case QuestSequence.Rising:
                Eventlist_Rising_clear.Add(_eventdata.ID);
        SuccessRisingCount++;
        FinishedRisingCount++;

        if (Eventlist_Rising.Count - 2 < FinishedRisingCount)
        {
          if (Random.Range(0, 100) < 80) CurrentSequence = QuestSequence.Climax;
        }
        else if(Eventlist_Rising.Count.Equals(FinishedRisingCount))CurrentSequence = QuestSequence.Climax;
        //슬슬 개수 다 찬듯 하면 높은 확률로 다음장, 개수 꽉 채웠으면 즉시 다음장
        break;
      case QuestSequence.Climax:

        SuccesClimaxCount++;
        FinishedClimaxCount++;
        if (Eventlist_Climax.Count.Equals(FinishedClimaxCount)) CurrentSequence = QuestSequence.Falling;
        //Climax 전부 완료했으면 최종장으로

        break;
      case QuestSequence.Falling:
        break;
    }

  }
  public void AddFailEvent(QuestEventData _eventdata)
  {
    switch (_eventdata.TargetQuestSequence)
    {
      case QuestSequence.Start:
        CurrentSequence = QuestSequence.Rising;
        break;
      case QuestSequence.Rising:
                Eventlist_Rising_clear.Add(_eventdata.ID);

                FinishedRisingCount++;
        if (Eventlist_Rising.Count - 2 < FinishedRisingCount)
        {
          if (Random.Range(0, 100) < 80) CurrentSequence = QuestSequence.Climax;
        }
        break;
      case QuestSequence.Climax:

        FinishedClimaxCount++;
        if (Eventlist_Climax.Count.Equals(FinishedClimaxCount)) CurrentSequence = QuestSequence.Falling;
        break;
      case QuestSequence.Falling:
        break;
    }
  }

  public QuestSequence CurrentSequence=QuestSequence.Start; //현재 퀘스트 단계

  public string StartDialogue = "";   //게임 내부에서 퀘스트 조우할 시 나오는 텍스트
  public string PreDescription = "";  //시작 화면 미리보기 텍스트
  public Sprite Illust;

  public string QuestPreview_rising = "";
  public string[] QuestPreview_Climax = null;
  public string QuestPreview_Falling;
  public string StopSelectionName = "", StopSelectionSub = "", ContinueSelectionName = "", ContinueSelectionSub = "";
  public List<QuestEventData> Eventlist_Rising=new List<QuestEventData>();
    public List<string> Eventlist_Rising_clear = new List<string>();
  public List<QuestEventData> Eventlist_Climax = new List<QuestEventData>();
  public QuestEventData Event_Falling = null;
  public string QuestPreview
  {
    get
    {
      switch (CurrentSequence)
      {
        case QuestSequence.Start: case QuestSequence.Rising: return QuestPreview_rising;
        case QuestSequence.Climax: return QuestPreview_Climax[FinishedClimaxCount];
        default: return QuestPreview_Falling;
      }
    }
  }
}
public class EventJsonData
{
  public string ID = "";              //ID
  public int Settlement = 0;          //0,1,2,3
  public int Place = 0;               //0,1,2,3,4
    public int TileCondition = 0;       //0: 조건없음  1: 환경  2: 레벨
    public int TileCondition_info = 0;  //환경 : 숲,강,언덕,산,바다      레벨: 0,1,2
  public string Season ;              //전역,봄,여름,가을,겨울

  public int Selection_Type;           //0.단일 1.이성+육체 2.정신+물질 3.성향 4.경험 5.기술
  public string Selection_Target;           //0.무조건 1.지불 2.테마 3.기술
  public string Selection_Info;             //0:정보 없음  1:체력,정신력,돈
                                            //2:대화,무력,생존,정신
                                            //3: 0.설득 1.협박  2.기만  3.논리 4.격투 5.활술 6.인체 7.생존 8.생물 9.잡학

  public string Failure_Penalty;            //없음,손실,경험
  public string Failure_Penalty_info;       //(체력,정신력,돈),경험 ID

  public string Reward_Target;              //경험,체력,정신력,돈,기술-테마,기술-개별,특성
  public string Reward_Info;                //경험 :ID  체력,정신력,돈:X  테마:대화,무력,생존,학식  개별기술:위 참조  특성:ID

  public string SubReward;                  //없음,돈,정신력,돈+정신력
}
public class FollowEventJsonData
{
  public string ID = "";              //ID

  public int FollowType = 0;              //이벤트,경험,특성,테마,기술
  public string FollowTarget = "";            //해당 ID 혹은 0,1,2,3 혹은 0~9
  public int FollowTargetSuccess = 0;            //(이벤트) 성공/실패
  public int FollowTendency = 0;          //이벤트일 경우 기타,이성,육체,정신,물질 선택지 여부

    public string Season;              //전역,봄,여름,가을,겨울
    public int Settlement = 0;          //0,1,2,3
  public int Place = 0;               //0,1,2,3,4

  public int Selection_Type;           //0.단일 1.이성+육체 2.정신+물질 3.성향 4.경험 5.기술
  public string Selection_Target;           //0.무조건 1.지불 2.테마 3.기술
  public string Selection_Info;             //0:정보 없음  1:체력,정신력,돈
                                            //2:대화,무력,생존,정신
                                            //3: 0.설득 1.협박  2.기만  3.논리 4.격투 5.활술 6.인체 7.생존 8.생물 9.잡학
    public int TileCondition = 0;       //0: 조건없음  1: 환경  2: 레벨
    public int TileCondition_info = 0;  //환경 : 숲,강,언덕,산,바다      레벨: 0,1,2

  public string Failure_Penalty;            //없음,손실,경험
  public string Failure_Penalty_info;       //(체력,정신력,돈),경험 ID

  public string Reward_Target;              //경험,체력,정신력,돈,기술-테마,기술-개별,특성
  public string Reward_Info;                //경험 :ID  체력,정신력,돈:X  테마:대화,무력,생존,학식  개별기술:위 참조  특성:ID

  public string SubReward;                  //없음,돈,정신력,돈+정신력
}
public class QuestEventDataJson
{
  public string QuestId = "";                 //퀘스트 ID
  public string ID = "";
  public int Sequence = 0;                   //0:기  1:승   2:전   3:결
  public string Season = "";

  public string Name = "";              //이름
  public int Settlement = 0;          //0(아무 정착지),1,2,3,4(외부)
  public int Place = 0;               //0,1,2,3,4
  public int Environment_Type = 0;    //0:전역 1:숲 2:강 3:언덕 4:산 5:바다

  public int Selection_Type;           //0.단일 1.이성+육체 2.정신+물질 3.성향 4.경험 5.기술
  public string Selection_Target;           //0.무조건 1.지불 2.테마 3.기술
  public string Selection_Info;             //0:정보 없음  1:체력,정신력,돈
                                            //2:대화,무력,생존,정신
                                            //3: 0.설득 1.협박  2.기만  3.논리 4.격투 5.활술 6.인체 7.생존 8.생물 9.잡학

  public string Failure_Penalty;            //없음,손실,경험
  public string Failure_Penalty_info;       //(체력,정신력,돈),경험 ID

  public string Reward_Target;              //경험,체력,정신력,돈,기술-테마,기술-개별,특성
  public string Reward_Info;                //경험 :ID  체력,정신력,돈:X  테마:대화,무력,생존,학식  개별기술:위 참조  특성:ID

  public string SubReward;                  //없음,돈,정신력,돈+정신력
}

