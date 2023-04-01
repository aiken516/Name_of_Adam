using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager s_instance;
    public static GameManager Instance { get { Init(); return s_instance; } }
    
    [SerializeField] private BattleManager _battle;
    public static BattleManager Battle => Instance._battle;
    
    [SerializeField] private CutSceneManager _cutScene;
    public static CutSceneManager CutScene => Instance._cutScene;

    [SerializeField] private UIManager _ui;
    public static UIManager UI => Instance._ui;

    private ResourceManager _resource = new ResourceManager();
    public static ResourceManager Resource => Instance._resource;

    #region 현재 안 씀
    [SerializeField] private StageManager _stageMNG;
    public static StageManager StageMNG => Instance._stageMNG;

    DataManager _dataMNG = new DataManager();
    public static DataManager DataMNG => Instance._dataMNG;
    #endregion

    void Awake()
    {
        if (s_instance != null)
            Destroy(gameObject); // 이미 GameManager가 있으면 이 오브젝트를 제거
        else
            Init();
    }

    private static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@GameManager");

            if (go == null)
            {
                go = new GameObject("@GameManager");
                go.AddComponent<GameManager>();
            }

            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<GameManager>();
        }
    }

    private void Update()
    {
        // 디버그용 입력기
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //  SceneChanger.SceneChange("StageSelectScene");
        }
    }
}