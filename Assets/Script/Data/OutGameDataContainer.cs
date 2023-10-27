﻿using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 진척도 & 전당 유닛 등의 인자를 저장 & 불러오는 기능

[Serializable]
public class OutGameData
{
    public int ProgressCoin;                 // 진척도 코인
    public List<ProgressItem> ProgressItems; // 진척도 상점의 상품들
    public List<HallUnit> HallUnit;          // 전당 유닛
    public bool TutorialClear = false;
}

[Serializable]
public class ProgressItem
{
    public int ID;             // 진척도 ID
    public string Name;        // 이름
    public int Cost;           // 가격
    public List<int> Prequest; // 선행 조건 ID
    public bool isLock;        // 잠겨있는지 여부
                               // 조건, 요구사항 등에 따라 설명, 이미지 등의 인자가 들어갈 수 있음
}

[Serializable]
public class HallUnit
{
    public int ID;                // 전당 내에서 식별을 위한 ID
    public string UnitName;       // 지금은 유닛 이름으로 받고있지만 ID로 받는 기능이 추가되면 변경해야함
    public DeckUnit deckUnit;
    public List<Stigma> Stigmata; // 유닛이 가지고 있는 낙인
}


public class OutGameDataContainer : MonoBehaviour
{
    // 현재 진행중인 게임에서 관리하는 아웃게임데이터
    OutGameData data;
    string path;

    public void Init()
    {
        // 사용자\AppData\localLow에 있는 SaveData.json의 경로
        path = Path.Combine(Application.persistentDataPath, "OutGameSaveData.json");

        LoadData();
    }

    public void SaveData()
    {
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(path, json);
    }

    public void LoadData()
    {
        try
        {
            // AppData에 파일이 있으면 OutGameData파일이 있으면 불러오기
            string json = File.ReadAllText(path);
            data = JsonUtility.FromJson<OutGameData>(json);
        }
        catch(FileNotFoundException e)
        {
            // 파일이 없을 경우(처음 플레이 시 or 데이터 삭제 시) 새로운 데이터 생성
            Debug.Log(e);
            CreateData();
        }
    }

    public List<DeckUnit> SetHallDeck()
    {
        List<DeckUnit> HallList = new();
        int LastAddedUnitIndex = data.HallUnit.Count;

        foreach (HallUnit unit in data.HallUnit)
        {
            HallList.Add(unit.deckUnit);
            foreach (Stigma stigma in unit.Stigmata)
            {
                unit.deckUnit.AddStigma(stigma);
            }
        }

        HallList[LastAddedUnitIndex - 1].IsMainDeck = false;

        return HallList;
    }

    public void CreateData()
    {
        // Resources폴더 안에 있는 데이터를 복사하여 저장
        TextAsset text = GameManager.Resource.Load<TextAsset>("Data/OutGameData");
        data = JsonUtility.FromJson<OutGameData>(text.text);

        SaveData();
    }

    public void SetProgressCoin(int num)
    {
        data.ProgressCoin += num;
        SaveData();
    }

    public ProgressItem GetProgressItem(int ID)
    {
        return data.ProgressItems.Find(x => x.ID == ID);
    }

    // 현재 확인하는 아이템이 구매 가능한 아이템인지 확인
    public bool GetBuyable(ProgressItem item)
    {
        foreach(int id in item.Prequest)
        {
            if (GetProgressItem(id).isLock)
                return false;
        }

        return true;
    }

    public void BuyProgressItem(int ID)
    {
        ProgressItem item = GetProgressItem(ID);
        item.isLock = false;
        SetProgressCoin(item.Cost);
    }

    public HallUnit FindHallUnitID(int ID)
    {
        return data.HallUnit.Find(x => x.ID == ID);
    }

    public void AddHallUnit(DeckUnit unit)
    {
        HallUnit newUnit = new();

        for (int i = 0; i < Mathf.Infinity; i++)
        {
            if (data.HallUnit.Find(x => x.ID == i) == null)
            {
                newUnit.ID = i;
                break;
            }
        }

        newUnit.deckUnit = unit;
        newUnit.UnitName = unit.Data.Name;
        newUnit.Stigmata = unit.GetChangedStigma();

        Debug.Log(newUnit.UnitName);
        data.HallUnit.Add(newUnit);
        SaveData();

        SceneChanger.SceneChange("MainScene");
    }

    public void DoneTutorial(bool isclear)
    {
        data.TutorialClear = isclear;
    }

    public bool isTutorialClear()
    {
        return data.TutorialClear;
    }

    public void RemoveHallUnit(int ID)
    {
        data.HallUnit.Remove(FindHallUnitID(ID));
    }

    public void DeleteAllData() => File.Delete(path);
}