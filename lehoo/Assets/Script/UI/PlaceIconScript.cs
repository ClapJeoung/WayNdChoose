using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlaceIconScript : MonoBehaviour
{
  public SectorTypeEnum MyType = SectorTypeEnum.Residence;
  [SerializeField] private Image MyImage = null;
  [SerializeField] private Image SabbatImage = null;
  [SerializeField] private Color IdleColor= Color.white;
  [SerializeField] private Color SelectedColor= Color.white;
  [SerializeField] private Color BlockedColor= Color.white;
  public void OpenIcon()
  {
    switch (GameManager.Instance.MyGameData.QuestType)
    {
      case QuestType.Cult:
        if (GameManager.Instance.MyGameData.Quest_Cult_Phase ==3 )
        {
          if ( GameManager.Instance.MyGameData.Cult_SabbatSector==MyType)
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
