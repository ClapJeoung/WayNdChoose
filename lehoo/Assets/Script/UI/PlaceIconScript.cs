using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlaceIconScript : MonoBehaviour
{
  public SectorTypeEnum MyType = SectorTypeEnum.Residence;
  public Image MyImage = null;
  public Image SabbatImage = null;
  public Color IdleColor= Color.white;
  public Color SelectedColor= Color.white;
  public Color BlockedColor= Color.white;
  public void OpenIcon()
  {
    switch (GameManager.Instance.MyGameData.QuestType)
    {
      case QuestType.Cult:
        if (GameManager.Instance.MyGameData.Quest_Cult_Phase > 0 )
        {
          if (GameManager.Instance.MyGameData.Cult_SabbatSector_CoolDown==0&&
            GameManager.Instance.MyGameData.Cult_SabbatSector==MyType)
          {
            MyImage.color = BlockedColor;
            if (SabbatImage.enabled == false) SabbatImage.enabled = true;
          }
          else
          {
            MyImage.color = IdleColor;
            if (SabbatImage.enabled == true) SabbatImage.enabled = false;
          }
        }
        else
        {
          MyImage.color = IdleColor;
          if (SabbatImage.enabled == true) SabbatImage.enabled = false;
        }
        break;
    }
  }
  public void SetSelectColor()
  {
    switch (GameManager.Instance.MyGameData.QuestType)
    {
      case QuestType.Cult:
        MyImage.color = SelectedColor; break;
    }
  }
    public void SetIdleColor()
  {
  switch (GameManager.Instance.MyGameData.QuestType)
  {
      case QuestType.Cult:
        MyImage.color = IdleColor;
          break;
    }
  }
}
