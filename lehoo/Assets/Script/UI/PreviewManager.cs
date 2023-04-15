using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using OpenCvSharp.Tracking;
using JetBrains.Annotations;

public class PreviewManager : MonoBehaviour
{
    [SerializeField] private GameObject TurnPreview = null;
    [SerializeField] private Image TurnIcon = null;
    [SerializeField] private TextMeshProUGUI TurnDescription = null;
    [Space(10)]
    [SerializeField] private GameObject HPPreview = null;
    [SerializeField] private TextMeshProUGUI HPDescription = null;
    [SerializeField] private TextMeshProUGUI HPGenDescriptoin = null;
    [SerializeField] private TextMeshProUGUI HPDecreaseDescription = null;
    [Space(10)]
    [Space(10)]
    [SerializeField] private GameObject SanityPreview = null;
    [SerializeField] private TextMeshProUGUI SanityDescription = null;
    [SerializeField] private TextMeshProUGUI SanityGenDescriptoin = null;
    [SerializeField] private TextMeshProUGUI SanityDecreaseDescription = null;
    [Space(10)]
    [SerializeField] private GameObject GoldPreview = null;
    [SerializeField] private TextMeshProUGUI GoldDescription = null;
    [SerializeField] private TextMeshProUGUI GoldGenDescriptoin = null;
    [SerializeField] private TextMeshProUGUI GoldDecreaseDescription = null;
    [Space(10)]
    [SerializeField] private GameObject MapPreview = null;
    [SerializeField] private TextMeshProUGUI MapMovableDescription = null;
    [Space(10)]
    [SerializeField] private GameObject QuestPreview = null;
    [SerializeField] private TextMeshProUGUI QuestName = null;
    [SerializeField] private Image QuestIllust = null;
    [SerializeField] private TextMeshProUGUI NextQuestEventDescription = null;
    [Space(10)]
    [SerializeField] private GameObject TraitPreview = null;
    [SerializeField] private Image TraitIllust = null;
    [SerializeField] private TextMeshProUGUI TraitName = null;
    [Space(10)]
    [SerializeField] private GameObject ThemePreview = null;
    [SerializeField] private TextMeshProUGUI ThemeName = null;
    [SerializeField] private TextMeshProUGUI ThemeLevel = null;
    [SerializeField] private TextMeshProUGUI ThemeLevelDescription = null;
    [Space(10)]
    [SerializeField] private GameObject ExpPreview = null;
    [SerializeField] private TextMeshProUGUI ExpName = null;
    [SerializeField] private TextMeshProUGUI ExpDuration = null;
    [Space(10)]
    [SerializeField] private GameObject TendencyPreview = null;
    [SerializeField] private Image TendencyIcon = null;
    [SerializeField] private TextMeshProUGUI TendencyName = null;
    [SerializeField] private TextMeshProUGUI TendencyLevel = null;
    [SerializeField] private TextMeshProUGUI TendencyDescription = null;
    [Space(10)]
    [SerializeField] private GameObject SelectionNonePanel = null;
    [SerializeField] private TextMeshProUGUI SelectionNoneText = null;
    private RectTransform CurrentPreview = null;
    public void OpenTurnPreview()
    {
        int _currentturn = GameManager.Instance.MyGameData.Turn;
        Sprite _turnsprite = null;
        switch (_currentturn)
        {
            case 0:_turnsprite = GameManager.Instance.ImageHolder.SpringSprite;break;
            case 1:_turnsprite = GameManager.Instance.ImageHolder.SummerSprite;break;
            case 2: _turnsprite = GameManager.Instance.ImageHolder.FallSprite; break;
            case 3: _turnsprite = GameManager.Instance.ImageHolder.WinterSprite; break;
        }
        TurnIcon.sprite = _turnsprite;
        TextData _textdata = GameManager.Instance.GetTextData("summer");
        TurnDescription.text = _textdata.Description;
        CurrentPreview = TurnPreview.GetComponent<RectTransform>();
        TurnPreview.SetActive(true);
    }//�� �̸����� �г� ���� �� ����
    public void OpenHPPreview()
    {
        TextData _textddata = GameManager.Instance.GetTextData("hp");
        string _str = _textddata.Description;
        HPDescription.text = _str;
        _str = string.Format(_textddata.SuccessDescription, GameManager.Instance.MyGameData.GetHPGenModify().ToString());
        HPGenDescriptoin.text = _str;
        _str = string.Format(_textddata.FailDescription, GameManager.Instance.MyGameData.GetHPLossModify().ToString());
        HPDecreaseDescription.text = _str;
        CurrentPreview = HPPreview.GetComponent<RectTransform>();
        HPPreview.SetActive(true);
    }//ü�� ����, ������ ǥ�� �� ����
    public void OpenSanityPreview()
    {
        TextData _textddata = GameManager.Instance.GetTextData("sanity");
        string _str = _textddata.Description;
        SanityDescription.text = _str;
        _str = string.Format(_textddata.SuccessDescription, GameManager.Instance.MyGameData.GetSanityGenModify().ToString());
        SanityGenDescriptoin.text = _str;
        _str = string.Format(_textddata.FailDescription, GameManager.Instance.MyGameData.GetSanityLossModify().ToString());
        SanityDecreaseDescription.text = _str;
        CurrentPreview = SanityPreview.GetComponent<RectTransform>();
        SanityPreview.SetActive(true);
    }//���ŷ� ����,������ ǥ�� �� ����
    public void OpenGoldPreview()
    {
        TextData _textddata = GameManager.Instance.GetTextData("gold");
        string _str = _textddata.Description;
        GoldDescription.text = _str;
        _str = string.Format(_textddata.SuccessDescription, GameManager.Instance.MyGameData.GetGoldGenModify().ToString());
        GoldGenDescriptoin.text = _str;
        _str = string.Format(_textddata.FailDescription, GameManager.Instance.MyGameData.GetGoldPayModify().ToString());
        GoldDecreaseDescription.text = _str;
        CurrentPreview = GoldPreview.GetComponent<RectTransform>();
        GoldPreview.SetActive(true);
    }//��� ����,������ ǥ�� �� ����
    public void OpenMapPreview()
    {
        TextData _textdata = GameManager.Instance.GetTextData("movedescription");
        if (UIManager.Instance.MyMap.CanMove) MapMovableDescription.text = _textdata.SuccessDescription;
        else MapMovableDescription.text = _textdata.FailDescription;

        CurrentPreview = MapPreview.GetComponent<RectTransform>();
        MapPreview.SetActive(true);
    }//���� �̵� ���� ���ο� ���� �ؽ�Ʈ�� ���
    public void OpenQuestPreview()
    {
        if (GameManager.Instance.MyGameData.CurrentQuest == null)
        {
            TextData _textdata = GameManager.Instance.GetTextData("donthaveanyquest");
            QuestName.text = _textdata.Name;
            QuestIllust.sprite = GameManager.Instance.ImageHolder.NothingQuestIllust;
            NextQuestEventDescription.text = _textdata.Description;
        }
        else
        {
            QuestHolder _currentquest = GameManager.Instance.MyGameData.CurrentQuest;
            QuestName.text = _currentquest.QuestName;
            QuestIllust.sprite = GameManager.Instance.ImageHolder.GetEventIllust(_currentquest.StartIllustID);
            string _strid = string.Format(_currentquest.QuestID, "_");
            switch (_currentquest.CurrentSequence)
            {
                case QuestSequence.Start:  case QuestSequence.Rising:
                    _strid = string.Format(_strid, "rising");
                    break;//����Ʈid_rising�� �� ��������
                case QuestSequence.Climax:
                    _strid = string.Format(_strid, "climax","_",_currentquest.CurrentClimaxIndex.ToString());
                    break;//id_climax_���� �� �� ��������
                case QuestSequence.Falling:
                    _strid = string.Format(_strid, "falling");
                    break;//id_falling�� �� ��������
            }//����Ʈ�� �����Ұ��
            NextQuestEventDescription.text = GameManager.Instance.GetTextData(_strid).Name;
        }

        CurrentPreview = QuestPreview.GetComponent<RectTransform>();
        QuestPreview.SetActive(true);
    }//���� ����Ʈ �̸�, �Ϸ���Ʈ, ���� ����
    public void OpenTraitPreview(Trait _trait)
    {
        TraitIllust.sprite = GameManager.Instance.ImageHolder.GetTraitIllust(_trait.ID);
        TraitName.text = GameManager.Instance.GetTextData(_trait.ID).Name;

        CurrentPreview = TraitPreview.GetComponent<RectTransform>();
        TraitPreview.SetActive(true);
    }
    public void OpenThemePreview(ThemeType _theme)
    {
        TextData _themename = null;
        switch (_theme)
        {
            case ThemeType.Conversation: _themename = GameManager.Instance.GetTextData("conversation");break;
            case ThemeType.Force:_themename = GameManager.Instance.GetTextData("force");break;
            case ThemeType.Nature:_themename = GameManager.Instance.GetTextData("nature");break;
            case ThemeType.Intelligence:_themename = GameManager.Instance.GetTextData("intelligence");break;
        }
        ThemeName.text = _themename.Name;
        int _onlytheme = GameManager.Instance.MyGameData.GetThemeLevelBySkill(_theme);
        //����κ��� ���� ��
        int _onlytrait=GameManager.Instance.MyGameData.GetEffectThemeCount_Trait(_theme);
        //Ư������ ���� ��
        int _onlyexp = GameManager.Instance.MyGameData.GetEffectThemeCount_Exp(_theme);
        //���迡�� ���� ��
        int _onlytendency = GameManager.Instance.MyGameData.GetThemeLevelByTendency(_theme);
        int _sum = _onlytheme + _onlytrait + _onlyexp + _onlytendency;
        ThemeLevel.text = _sum.ToString();
        string _description = $"{GameManager.Instance.GetTextData("byskill").Name} {_onlytheme}\n" +
            $"{GameManager.Instance.GetTextData("bytrait")} {_onlytrait}\n" +
            $"{GameManager.Instance.GetTextData("byexp")} {_onlyexp}\n" +
            $"{GameManager.Instance.GetTextData("bytendency")} {_onlytendency}";
        ThemeLevelDescription.text = _description;
        CurrentPreview = ThemePreview.GetComponent<RectTransform>();
        ThemePreview.SetActive(true);
    }
    public void OpenExpPreview(Experience _exp)
    {
        ExpName.text = GameManager.Instance.GetTextData(_exp.ID).Name;
        ExpDuration.text = string.Format(_exp.AcquireData.Duration.ToString(), GameManager.Instance.GetTextData("expduration").Name);

        CurrentPreview = ExpPreview.GetComponent<RectTransform>();
        ExpPreview.SetActive(true);
    }
    public void OpenTendencyPreview(TendencyType _type)
    {
        Sprite _tendencyicon = null;
        string _tendencyname = "";
        string _tendencydescription = "";
        int _tendencylevel = 0;
        switch (_type)
        {
            case TendencyType.Rational:
                _tendencyicon = GameManager.Instance.ImageHolder.GetTendencyIcon(TendencyType.Rational);
                _tendencyname = GameManager.Instance.GetTextData("rational").Name;
                switch (GameManager.Instance.MyGameData.Tendency_RP.Level)
                {
                    case 3:
                        _tendencydescription = $"{GameManager.Instance.GetTextData("rationalselection")} {GameManager.Instance.GetTextData("sanity").FailDescription}\n" +
                            $"{GameManager.Instance.GetTextData("conversation").Name}, {GameManager.Instance.GetTextData("intelligence")} " +
                            $"{GameManager.Instance.MyGameData.GetThemeLevelByTendency(ThemeType.Conversation)} {GameManager.Instance.GetTextData("decrease")}";
                        //��ü ������ ���ŷ� �Ҹ�\n��ü, �ڿ� (�������� ���� ��ü ���ҷ�) ����
                        break;
                    case 2:
                        _tendencydescription = $"{GameManager.Instance.GetTextData("rationalselection")} {GameManager.Instance.GetTextData("sanity").FailDescription}\n";
                        break;
                    case 1:
                    case 0: //(Rational ����) RP -3,-2,-1 : ��ȭ,���� ����   2: �̼� �������� �г�Ƽ  3: 2+�̼� ���� ���� �г�Ƽ
                        break;
                    case -1:
                    case -2:
                    case -3:
                        _tendencydescription = $"{GameManager.Instance.GetTextData("conversation").Name}, {GameManager.Instance.GetTextData("intelligence")} " +
                        $"{GameManager.Instance.MyGameData.GetThemeLevelByTendency(ThemeType.Conversation)} {GameManager.Instance.GetTextData("increase")}";
                        break;
                }
                _tendencylevel = GameManager.Instance.MyGameData.Tendency_RP.Level * -1;
                break;
            case TendencyType.Physical:
                _tendencyicon = GameManager.Instance.ImageHolder.GetTendencyIcon(TendencyType.Physical);
                _tendencyname = GameManager.Instance.GetTextData("physical").Name;
                switch (GameManager.Instance.MyGameData.Tendency_RP.Level)
                {
                    case 3:
                    case 2:
                    case 1:
                        _tendencydescription = $"{GameManager.Instance.GetTextData("force").Name}, {GameManager.Instance.GetTextData("nature")} " +
                         $"{GameManager.Instance.MyGameData.GetThemeLevelByTendency(ThemeType.Force)} {GameManager.Instance.GetTextData("increase")}";
                        break;
                    case 0: //Physical ���� RP -3: (-2)+��ü ���� ���� �г�Ƽ  -2: ��ü �������� �г�Ƽ  1,2,3: ����,�ڿ� ����
                    case -1:
                        break;
                    case -2:
                        _tendencydescription = $"{GameManager.Instance.GetTextData("physicalselection")} {GameManager.Instance.GetTextData("sanity").FailDescription}\n";
                        break;
                    case -3:
                        _tendencydescription = $"{GameManager.Instance.GetTextData("physicalselection")} {GameManager.Instance.GetTextData("sanity").FailDescription}\n" +
                       $"{GameManager.Instance.GetTextData("force").Name}, {GameManager.Instance.GetTextData("nature")} " +
                       $"{GameManager.Instance.MyGameData.GetThemeLevelByTendency(ThemeType.Force)} {GameManager.Instance.GetTextData("decrease")}";
                        break;
                }
                _tendencylevel = GameManager.Instance.MyGameData.Tendency_RP.Level * +1;
                break;
            case TendencyType.Mental:
                _tendencyicon = GameManager.Instance.ImageHolder.GetTendencyIcon(TendencyType.Mental);
                _tendencyname = GameManager.Instance.GetTextData("mental").Name;
                switch (GameManager.Instance.MyGameData.Tendency_MM.Level)
                {
                    case -3:
                    case -2:
                    case -1:
                        _tendencydescription = $"{ GameManager.Instance.GetTextData("sanity").FailDescription} { GameManager.Instance.GetTextData("decrease")}";
                        break;
                    case 0: //Mental ���� MM -3,-2,-1: ���ŷ� �Ҹ� ����  2: ���� ������ �г�Ƽ  3:���ŷ� ȸ�� ����
                    case 1:
                        break;
                    case 2:
                        _tendencydescription = $"{GameManager.Instance.GetTextData("mentalselection")} {GameManager.Instance.GetTextData("sanity").FailDescription}";
                        break;
                    case 3:
                        _tendencydescription = $"{GameManager.Instance.GetTextData("mentalselection")} {GameManager.Instance.GetTextData("sanity").FailDescription}\n" +
                            $"{GameManager.Instance.GetTextData("sanity").SuccessDescription} {GameManager.Instance.GetTextData("decrease")}";
                        break;
                }
                _tendencylevel = GameManager.Instance.MyGameData.Tendency_MM.Level * -1;
                break;
            case TendencyType.Material:
                _tendencyicon = GameManager.Instance.ImageHolder.GetTendencyIcon(TendencyType.Material);
                _tendencyname = GameManager.Instance.GetTextData("material").Name;
                switch (GameManager.Instance.MyGameData.Tendency_MM.Level)
                {
                    case -3:
                        _tendencydescription = $"{GameManager.Instance.GetTextData("materialselection")} {GameManager.Instance.GetTextData("sanity").FailDescription}\n" +
                    $"{GameManager.Instance.GetTextData("sanity").SuccessDescription} {GameManager.Instance.GetTextData("decrease")}";
                        break;
                    case -2:
                        _tendencydescription = $"{GameManager.Instance.GetTextData("materialselection")} {GameManager.Instance.GetTextData("sanity").FailDescription}";
                        break;
                    case -1:
                    case 0://Material ���� MM -3: �� ���� ����  -2: ���� ������ �г�Ƽ  1,2,3: �� �Ҹ� ����
                        break;
                    case 1: case 2: case 3:
                        _tendencydescription = $"{GameManager.Instance.GetTextData("gold").FailDescription} {GameManager.Instance.GetTextData("decrease")}";
                        break;
                }
                _tendencylevel = GameManager.Instance.MyGameData.Tendency_MM.Level * +1;
                break;
        }
        TendencyIcon.sprite = _tendencyicon;
        TendencyName.text = _tendencyname;
        TendencyLevel.text = _tendencylevel.ToString();
        TendencyDescription.text = _tendencyname;

        CurrentPreview = TendencyPreview.GetComponent<RectTransform>();
        TendencyPreview.SetActive(true);
    }
    public void OpenSelectionNonePreview(EventDataDefulat _event)
    {
        SelectionNoneText.text = GameManager.Instance.GetTextData(_event.ID).SelectionSubDescription;

        CurrentPreview = SelectionNonePanel.GetComponent<RectTransform>();
        SelectionNonePanel.SetActive(true);
    }
    public void Update()
    {
        if (CurrentPreview==null) return;
        CurrentPreview.anchoredPosition = Camera.main.ScreenToViewportPoint(Input.mousePosition);
    }
    public void ClosePreview() { CurrentPreview.gameObject.SetActive(false); CurrentPreview = null; }
}
