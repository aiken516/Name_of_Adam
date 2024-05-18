using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UI_HallCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private GameObject _highlight;
    [SerializeField] private List<GameObject> _stigmaFrames;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private GameObject infoButton;
    [SerializeField] private Image _frameImage;

    public Image UnitImage;

    private List<Image> _stigmaImages;
    private List<DeckUnit> _mainDeck;
    private List<HallUnit> _hallUnitList;

    private bool _isEnable;
    public bool _isElite;
    public int HallUnitID; //���� ��Ī��ų ID
    public Sprite NormalImage;
    public Sprite EliteImage;

    public void Init()
    {
        _hallUnitList = GameManager.OutGameData.FindHallUnitList();
        _mainDeck = GameManager.Data.GameDataMain.DeckUnits;

        // _mainDeck을 HallUnitID 순서대로 정렬
        _mainDeck.Sort((x, y) => x.HallUnitID.CompareTo(y.HallUnitID));

        _stigmaImages = new List<Image>();
        foreach (var frame in _stigmaFrames)
            _stigmaImages.Add(frame.GetComponentsInChildren<Image>()[1]);

        if (_mainDeck.Count <= HallUnitID || _mainDeck[HallUnitID].IsMainDeck == false)
        {
            _isEnable = false;
            DisableUI();
            return;
        }

        foreach (var frame in _stigmaFrames)
            frame.SetActive(true);

        _isEnable = true;
        UnitImage.sprite = _mainDeck[HallUnitID].Data.CorruptImage;
        UnitImage.color = Color.white;
        _nameText.SetText(_mainDeck[HallUnitID].Data.Name);
        infoButton.SetActive(true);

        List<Stigma> stigmas = _mainDeck[HallUnitID].GetStigma();
        for (int i = 0; i < _stigmaImages.Count; i++)
        {
            if (i < stigmas.Count)
            {
                _stigmaFrames[i].GetComponent<UI_StigmaHover>().SetStigma(stigmas[i]);
                _stigmaImages[i].sprite = stigmas[i].Sprite_28;
                _stigmaImages[i].color = Color.white;
            }
            else
            {
                _stigmaFrames[i].GetComponent<UI_StigmaHover>().SetEnable(false);
                _stigmaImages[i].color = new Color(1f, 1f, 1f, 0f);            
            }
        }

        if (_mainDeck[HallUnitID].Data.Rarity == Rarity.Normal)
        {
            _frameImage.sprite = NormalImage;
        }
        else
        {
            _frameImage.sprite = EliteImage;
        }

        _canvasGroup.alpha = 1f;
        if (GameManager.OutGameData.IsUnlockedItem(14) == false)
        {
            if (HallUnitID == 1 || HallUnitID == 2)
            {
                _isEnable = false;
                _canvasGroup.alpha = 0.7f;
            }
        }

        _highlight.SetActive(false);
    }

    private void DisableUI()
    {
        UnitImage.color = new Color(1f, 1f, 1f, 0f);
        _nameText.SetText("");
        infoButton.SetActive(false);
        foreach (var frame in _stigmaFrames)
            frame.SetActive(false);
    }

    public void OnClick()
    {
        if (HallUnitID == 1 || HallUnitID == 2)
        {
            if (!GameManager.OutGameData.IsUnlockedItem(14))
            {
                GameManager.Sound.Play("UI/ClickSFX/ClickFailSFX");
                return;
            }
        }

        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");

        if (GameManager.OutGameData.IsUnlockedItem(17))
        {
            GameManager.UI.ShowPopup<UI_MyDeck>("UI_MyDeck").HallEliteDeckInit(_isElite, OnSelect);
        }
        else
        {
            GameManager.UI.ShowPopup<UI_MyDeck>("UI_MyDeck").HallDeckInit(_isElite, OnSelect);
        }
    }

    //������ ���� GameDataMain �� ����ǰ� �ϱ�
    public void OnSelect(DeckUnit unit)
    {
        DeckUnit afterDeckUnit = unit;
        HallUnit afterHallUnit = _hallUnitList.Find(x => x.ID == afterDeckUnit.HallUnitID);

        if (_mainDeck.Count <= HallUnitID)
        {
            // 신규 유닛이면 추가
            GameManager.Data.GameDataMain.DeckUnits.Add(unit);
        }
        else
        {
            // 이전 유닛이 있다면 스왑
            DeckUnit beforeDeckUnit = GameManager.Data.GetDeck().Find(x => x.HallUnitID == HallUnitID);
            HallUnit beforeHallUnit = _hallUnitList.Find(x => x.ID == beforeDeckUnit.HallUnitID);

            beforeDeckUnit.IsMainDeck = false;
            beforeHallUnit.IsMainDeck = false;
            beforeDeckUnit.HallUnitID = afterDeckUnit.HallUnitID;
            beforeHallUnit.ID = afterHallUnit.ID;
        }

        // GameData Deck 수정
        afterDeckUnit.IsMainDeck = true;
        afterHallUnit.IsMainDeck = true;
        afterDeckUnit.HallUnitID = HallUnitID;
        afterHallUnit.ID = HallUnitID;

        // GameDataMain Deck 수정
        _mainDeck[HallUnitID].IsMainDeck = false;
        _mainDeck[HallUnitID].HallUnitID = afterDeckUnit.HallUnitID;
        _mainDeck[HallUnitID] = unit;
        _mainDeck[HallUnitID].IsMainDeck = true;
        _mainDeck[HallUnitID].HallUnitID = HallUnitID;

        if (unit.Data.Rarity == Rarity.Normal)
        {
            _frameImage.sprite = NormalImage;
        }
        else
        {
            _frameImage.sprite = EliteImage;
        }

        UnitImage.sprite = _mainDeck[HallUnitID].Data.Image;
        UnitImage.color = Color.white;

        GameManager.UI.ClosePopup();
        GameManager.UI.ClosePopup();

        Init();
    }

    public void OnInfoButton()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        UI_UnitInfo ui = GameManager.UI.ShowPopup<UI_UnitInfo>("UI_UnitInfo");

        ui.SetUnit(_mainDeck[HallUnitID]);
        ui.Init();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_isEnable)
            _highlight.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_isEnable)
            _highlight.SetActive(false);
    }
}
