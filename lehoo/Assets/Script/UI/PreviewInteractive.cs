using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum PreviewPanelType { Turn,HP,Sanity,Gold,Map,Trait,Theme,EXP,Tendency,Selection}
public class PreviewInteractive :MonoBehaviour, IPointerEnterHandler,IPointerExitHandler
{
    public PreviewPanelType PanelType=PreviewPanelType.Turn;
    [Space(15)]
    private Trait MyTrait = null;
    public ThemeType MyTheme = ThemeType.Conversation;
    private Experience MyEXP = null;
    private TendencyType MyTendency = TendencyType.None;
    public TendencyType MySelectionTendency= TendencyType.None;
    public void OnPointerEnter(PointerEventData eventData)
    {

    }
    public void OnPointerExit(PointerEventData eventData) 
    {
        
    }
}
