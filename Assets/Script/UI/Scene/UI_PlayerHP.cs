using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_PlayerHP : MonoBehaviour
{
    [SerializeField]
    private GameObject[] HPJemImages;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    public void Init()
    {
        for (int i = 0; i < HPJemImages.Length; i++)
        {
            if (i < GameManager.Data.GameData.PlayerHP)
                HPJemImages[i].SetActive(true);
            else
                HPJemImages[i].SetActive(false);
        }
    }

    public void DecreaseHP(int hp)
    {
        for (int i = 0; i < hp; i++) 
        {
            GameManager.Data.GameData.PlayerHP -= 1;
            PlayerHPEffect effect = HPJemImages[GameManager.Data.GameData.PlayerHP].GetComponent<PlayerHPEffect>();
            if (effect == null) 
            {
                Debug.LogError("PlayerHPEffect ���� ����");
                return;
            }
            effect.StartDecreaseHPEffect();
        }
    }
}