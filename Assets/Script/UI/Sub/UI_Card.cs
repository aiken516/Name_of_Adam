using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class UI_Card : UI_Base, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private GameObject _highlight;
    [SerializeField] private GameObject _selectHighlight;
    [SerializeField] private GameObject _disable;
    [SerializeField] private Image _unitImage;
    [SerializeField] private Image _frameImage;
    [SerializeField] private TextMeshProUGUI _name;

    [SerializeField] private List<GameObject> _stigmataFrames;

    [SerializeField] private Sprite _normalFrame;
    [SerializeField] private Sprite _eliteFrame;

    [SerializeField] private List<Image> _stigmataImages;

    private UI_MyDeck _myDeck;
    private DeckUnit _cardUnit = null;

    private void Start()
    {
        _highlight.SetActive(false);
        _selectHighlight.SetActive(false);
    }

    public void SetCardInfo(UI_MyDeck myDeck, DeckUnit unit, CUR_EVENT eventNum)
    {
        _myDeck = myDeck;
        _cardUnit = unit;
        _eventNum = eventNum;

        _unitImage.sprite = unit.Data.CorruptImage;
        _name.text = unit.Data.Name;

        if(unit.Data.Rarity == Rarity.Normal)
        {
            _frameImage.sprite = _normalFrame;
        }
        else
        {
            _frameImage.sprite = _eliteFrame;
        }

        if (SceneManager.GetActiveScene().name == "DifficultySelectScene")
        {
            SetDisable(unit.IsMainDeck);
        }

        foreach (var frame in _stigmataFrames)
            frame.SetActive(true);

        List<Stigma> unitStigmaList = unit.GetStigma();
        for (int i = 0; i < _stigmataImages.Count; i++)
        {
            if (i < unitStigmaList.Count)
            {
                _stigmataFrames[i].GetComponent<UI_StigmaHover>().SetStigma(unitStigmaList[i]);
                _stigmataImages[i].sprite = unitStigmaList[i].Sprite_28;
                _stigmataImages[i].color = Color.white;
            }
            else
            {
                _stigmataFrames[i].GetComponent<UI_StigmaHover>().SetEnable(false);
                _stigmataImages[i].color = new Color(1f, 1f, 1f, 0f);
            }
        }
    }

    public void SelectCard()
    {
        _selectHighlight.SetActive(!_selectHighlight.activeInHierarchy);
    }

    public void SetDisable(bool disable)
    {
        _disable.SetActive(disable);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_cardUnit.IsMainDeck && SceneManager.GetActiveScene().name == "DifficultySelectScene")
        {
            return;
        }
        else
        {
            _highlight.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_cardUnit.IsMainDeck && SceneManager.GetActiveScene().name == "DifficultySelectScene")
        {
            return;
        }
        else
        {
            _highlight.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        if (_disable.activeSelf)
        {
            return;
        }

        if (_eventNum == CUR_EVENT.GIVE_STIGMA)
        {
            GameManager.UI.ShowPopup<UI_SystemSelect>().Init("CorfirmGiveStigmata", () =>
            {
                _myDeck.OnClickCard(_cardUnit);
                GameManager.Data.RemoveDeckUnit(_cardUnit);
            });
        }
        else
        {
            _myDeck.OnClickCard(_cardUnit);
        }
    }
}
