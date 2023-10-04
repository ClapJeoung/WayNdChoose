using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DebugScript : MonoBehaviour
{
  public TMP_InputField Year = null;
  public TMP_InputField Turn = null;
  public TMP_InputField HP = null;
  public TMP_InputField Sanity = null;
 // public TMP_InputField MaxSanity = null;
  public TMP_InputField Gold = null;
  public TMP_InputField Movepoint = null;
  [Space(10)]
  public TMP_InputField Skill_Conversation = null;
  public TMP_InputField Skill_Force = null;
  public TMP_InputField Skill_Wild = null;
  public TMP_InputField Skill_Intelligence = null;
  [Space(10)]
  public TMP_InputField Tendency_Body = null;
  public TMP_InputField Tendency_Head = null;
  [Space(10)]
  public TMP_InputField EXP_Long_ID = null;
  public TMP_InputField EXP_Long_Turn = null;
  public TMP_InputField EXP_Short_0_ID = null;
  public TMP_InputField EXP_Short_0_Turn = null;
  public TMP_InputField EXP_Short_1_ID = null;
  public TMP_InputField EXP_Short_1_Turn = null;
  [Space(10)]
  public TMP_InputField Cult_Phase = null;
 // public TMP_InputField Cult_Type = null;
  public TMP_InputField Cult_Progress = null;
  [Space(10)]
  public TMP_InputField NextEventId = null;

  public void UpdateValues()
  {
    Year.text=GameManager.Instance.MyGameData.Year.ToString();
    Turn.text=GameManager.Instance.MyGameData.Turn.ToString();
    HP.text=GameManager.Instance.MyGameData.HP.ToString();
    Sanity.text=GameManager.Instance.MyGameData.Sanity.ToString();
  //  MaxSanity.text=GameManager.Instance.MyGameData.MaxSanity.ToString();
    Gold.text=GameManager.Instance.MyGameData.Gold.ToString();
    Movepoint.text=GameManager.Instance.MyGameData.MovePoint.ToString();

    Skill_Conversation.text=GameManager.Instance.MyGameData.Skill_Conversation.LevelByDefault.ToString();
    Skill_Force.text = GameManager.Instance.MyGameData.Skill_Force.LevelByDefault.ToString();
    Skill_Wild.text = GameManager.Instance.MyGameData.Skill_Wild.LevelByDefault.ToString();
    Skill_Intelligence.text = GameManager.Instance.MyGameData.Skill_Intelligence.LevelByDefault.ToString();

    Tendency_Body.text = GameManager.Instance.MyGameData.Tendency_Body.Level.ToString();
    Tendency_Head.text=GameManager.Instance.MyGameData.Tendency_Head.Level.ToString();

    EXP_Long_ID.text = GameManager.Instance.MyGameData.LongTermEXP != null ? GameManager.Instance.MyGameData.LongTermEXP.ID : "";
    EXP_Long_Turn.text = GameManager.Instance.MyGameData.LongTermEXP != null ? GameManager.Instance.MyGameData.LongTermEXP.Duration.ToString() : "";
    EXP_Short_0_ID.text = GameManager.Instance.MyGameData.ShortTermEXP[0] != null ? GameManager.Instance.MyGameData.ShortTermEXP[0].ID : "";
    EXP_Short_0_Turn.text = GameManager.Instance.MyGameData.ShortTermEXP[0] != null ? GameManager.Instance.MyGameData.ShortTermEXP[0].Duration.ToString() : "";
    EXP_Short_1_ID.text = GameManager.Instance.MyGameData.ShortTermEXP[1] != null ? GameManager.Instance.MyGameData.ShortTermEXP[1].ID : "";
    EXP_Short_1_Turn.text = GameManager.Instance.MyGameData.ShortTermEXP[1] != null ? GameManager.Instance.MyGameData.ShortTermEXP[1].Duration.ToString() : "";

    if (GameManager.Instance.MyGameData.QuestType == QuestType.Cult)
    {
      Cult_Phase.text = GameManager.Instance.MyGameData.Quest_Cult_Phase.ToString();
  //    Cult_Type.text = GameManager.Instance.MyGameData.Quest_Cult_Type.ToString();
      Cult_Progress.text = GameManager.Instance.MyGameData.Quest_Cult_Progress.ToString();
    }
  }
  public void ApplyValues()
  {
    if (Year.text == "") return;

    GameManager.Instance.MyGameData.Year = int.Parse(Year.text);
    GameManager.Instance.MyGameData.Turn= int.Parse(Turn.text);
    GameManager.Instance.MyGameData.HP= int.Parse(HP.text);
    GameManager.Instance.MyGameData.Sanity=int.Parse(Sanity.text);
 //   GameManager.Instance.MyGameData.MaxSanity= int.Parse(MaxSanity.text);
    GameManager.Instance.MyGameData.Gold= int.Parse(Gold.text);
    GameManager.Instance.MyGameData.MovePoint = int.Parse(Movepoint.text);

    GameManager.Instance.MyGameData.Skill_Conversation.LevelByDefault = int.Parse(Skill_Conversation.text);
    GameManager.Instance.MyGameData.Skill_Force.LevelByDefault = int.Parse(Skill_Force.text);
    GameManager.Instance.MyGameData.Skill_Wild.LevelByDefault = int.Parse(Skill_Wild.text);
    GameManager.Instance.MyGameData.Skill_Intelligence.LevelByDefault = int.Parse(Skill_Intelligence.text);

    GameManager.Instance.MyGameData.Tendency_Body.Level = int.Parse(Tendency_Body.text);
    GameManager.Instance.MyGameData.Tendency_Head.Level=int.Parse(Tendency_Head.text);

    if (EXP_Long_ID.text == "")
    {
      GameManager.Instance.MyGameData.LongTermEXP = null;
    }
    else if (GameManager.Instance.ExpDic.ContainsKey(EXP_Long_ID.text))
    {
      if (GameManager.Instance.MyGameData.LongTermEXP.ID == EXP_Long_ID.text)
      {
        GameManager.Instance.MyGameData.LongTermEXP.Duration = int.Parse(EXP_Long_Turn.text);
      }
      else
      {
        GameManager.Instance.MyGameData.LongTermEXP = GameManager.Instance.ExpDic[EXP_Long_ID.text];
        GameManager.Instance.MyGameData.LongTermEXP.Duration = EXP_Long_Turn.text != "" ? int.Parse(EXP_Long_Turn.text) : ConstValues.LongTermStartTurn;
      }
    }
    UIManager.Instance.UpdateExpLongTermIcon();
    if (EXP_Short_0_ID.text == "")
    {
      GameManager.Instance.MyGameData.ShortTermEXP[0] = null;
    }
    else if (GameManager.Instance.ExpDic.ContainsKey(EXP_Short_0_ID.text))
    {
      if (GameManager.Instance.MyGameData.ShortTermEXP[0].ID == EXP_Short_0_ID.text)
      {
        GameManager.Instance.MyGameData.ShortTermEXP[0].Duration = int.Parse(EXP_Short_0_Turn.text);
      }
      else
      {
        GameManager.Instance.MyGameData.ShortTermEXP[0] = GameManager.Instance.ExpDic[EXP_Short_0_ID.text];
        GameManager.Instance.MyGameData.ShortTermEXP[0].Duration = EXP_Short_0_Turn.text != "" ? int.Parse(EXP_Short_0_Turn.text) : ConstValues.ShortTermStartTurn;
      }
    }
    if (EXP_Short_1_ID.text == "")
    {
      GameManager.Instance.MyGameData.ShortTermEXP[1] = null;
    }
    else if (GameManager.Instance.ExpDic.ContainsKey(EXP_Short_1_ID.text))
    {
      if (GameManager.Instance.MyGameData.ShortTermEXP[1].ID == EXP_Short_1_ID.text)
      {
        GameManager.Instance.MyGameData.ShortTermEXP[1].Duration = int.Parse(EXP_Short_1_Turn.text);
      }
      else
      {
        GameManager.Instance.MyGameData.ShortTermEXP[1] = GameManager.Instance.ExpDic[EXP_Short_1_ID.text];
        GameManager.Instance.MyGameData.ShortTermEXP[1].Duration = EXP_Short_1_Turn.text != "" ? int.Parse(EXP_Short_1_Turn.text) : ConstValues.ShortTermStartTurn;
      }
    }
    UIManager.Instance.UpdateExpShortTermIcon();

    if (GameManager.Instance.MyGameData.QuestType == QuestType.Cult)
    {
      GameManager.Instance.MyGameData.Quest_Cult_Phase = int.Parse(Cult_Phase.text);
  //    GameManager.Instance.MyGameData.Quest_Cult_Type = int.Parse(Cult_Type.text);
      GameManager.Instance.MyGameData.Quest_Cult_Progress = int.Parse(Cult_Progress.text);
    }

  }
}
