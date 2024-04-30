using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_ESCOption : UI_Popup
{
    public void QuitButton()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        GameManager.UI.OnOffESCOption();
    }

    public void OptionButton()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");

        var option = GameManager.UI.ShowPopup<UI_Option>();
        option.GetComponent<Canvas>().sortingOrder = UIManager.ESCOrder + 1;
        GameManager.UI.ESCOPopups.Push(option);
    }

    public void GoToMainButton()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");

        //UI_SystemSelect systemSelect = GameManager.UI.ShowPopup<UI_SystemSelect>();
        //systemSelect.GetComponent<Canvas>().sortingOrder = UIManager.ESCOrder + 1;
        //systemSelect.Init("Restart", YesGoToMain, () => GameManager.UI.ESCOPopups.Pop());
        //GameManager.UI.ESCOPopups.Push(systemSelect);

        YesGoToMain();
    }

    public void YesGoToMain()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "BattleScene")
        {
            // 배틀에서 얻거나 사용한 검은 정수 초기화
            int darkEssenseGap = BattleManager.Data.BattlePrevDarkEssence - GameManager.Data.DarkEssense;
            GameManager.Data.DarkEssenseChage(darkEssenseGap);
        }
        else if (sceneName == "EventScene")
        {
            // 유닛 환원을 통해 얻은 검은 정수 초기화
            HarlotSceneController sc = FindObjectOfType<HarlotSceneController>();
            if (sc != null)
            {
                int darkEssenseGap = sc.PreDarkEssence - GameManager.Data.DarkEssense;
                GameManager.Data.DarkEssenseChage(darkEssenseGap);
            }
        }
        else
        {
            if (sceneName != "CutScene") // 처음 컷씬에서 세이브 오류 발생하니 제외
                GameManager.SaveManager.SaveGame();
        }

        Time.timeScale = 1;
        GameManager.VisualEffect.ClearAllEffect();
        GameManager.UI.CloseAllOption();

        SceneChanger.SceneChange("MainScene");
    }

    public void ExitButton()
    {
        GameManager.Sound.Play("UI/ButtonSFX/UIButtonClickSFX");
        Application.Quit();
    }
}
