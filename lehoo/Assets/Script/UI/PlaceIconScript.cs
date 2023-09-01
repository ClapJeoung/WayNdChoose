using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;
public class PlaceIconScript : MonoBehaviour
{
  public SectorType MyType = SectorType.Residence;
  public Image MyImage = null;
  public Image Quest_Wolf_CultBlock_Effect = null;    //이건 일단 보류...
  public Image Quest_Wolf_CultBlock = null;
  public Image Quest_Wolf_HideoutToken = null;
  public Color IdleColor= Color.white;
  public Color SelectedColor= Color.white;
  public Color BlockedColor= Color.white;
  public void OpenIcon()
  {
    switch (GameManager.Instance.MyGameData.QuestType)
    {
      case QuestType.Wolf:
        if (GameManager.Instance.MyGameData.Quest_Wolf_Cult_BlockedPlaces.Contains(MyType))
        {
          MyImage.color = BlockedColor;
          if (Quest_Wolf_CultBlock.enabled == false) Quest_Wolf_CultBlock.enabled = true;
          if (Quest_Wolf_HideoutToken.enabled == true) Quest_Wolf_HideoutToken.enabled = false;
        }
        else
        {
          MyImage.color = IdleColor;
          if (Quest_Wolf_CultBlock.enabled == true) Quest_Wolf_CultBlock.enabled = false;
          if (GameManager.Instance.MyGameData.Quest_Wolf_Cult_TokenedSectors[MyType] == 0)
          {
            if (Quest_Wolf_HideoutToken.enabled == false) Quest_Wolf_HideoutToken.enabled = true;
          }
          else
          {
            if (Quest_Wolf_HideoutToken.enabled == true) Quest_Wolf_HideoutToken.enabled = false;
          }
        }
        break;
    }
  }
  public void SetSelectColor()
  {
    switch (GameManager.Instance.MyGameData.QuestType)
    {
      case QuestType.Wolf:
        if (GameManager.Instance.MyGameData.Quest_Wolf_Cult_BlockedPlaces.Contains(MyType)) { MyImage.color = BlockedColor; }
        else MyImage.color = SelectedColor;
        break;
    }
  }
    public void SetIdleColor()
  {
  switch (GameManager.Instance.MyGameData.QuestType)
  {
      case QuestType.Wolf:
        if (GameManager.Instance.MyGameData.Quest_Wolf_Cult_BlockedPlaces.Contains(MyType)) { MyImage.color = BlockedColor; }
        else MyImage.color = IdleColor;
        break;
    }
  }
}
