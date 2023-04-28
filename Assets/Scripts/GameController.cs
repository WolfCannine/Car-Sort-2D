using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    [HideInInspector]
    public bool isGameOver;
    [SerializeField]
    private GameData gameData;
    [SerializeField]
    private Camera m_Camera;
    [SerializeField]
    private Image m_ThemesPanel;
    [SerializeField]
    private UIManager uiManager;
    [SerializeField]
    private LevelManager[] level;
    private GameObject carPrefab;
    [HideInInspector]
    public List<GameObject> spawnedCarPrefabs = new();
    private int currentLevel;



    private void Awake()
    {
        Instance = this;
    }

    //private void Start() { StartGame(); }

    public void StartGame()
    {
        UIManager.Instance.SetLevelText();
        carPrefab = GameManager.Instance.carPrefabs[gameData.carPrefab];
        currentLevel = UIManager.Instance.levelSelected;
        UIManager.Instance.skipADsButton.SetActive(gameData.unlockedLevel < 9);
        UIManager.Instance.skipADsButton1.SetActive(gameData.unlockedLevel < 9);
        UIManager.Instance.NextLevelButton.SetActive(currentLevel < 9);
        m_Camera.orthographicSize = currentLevel is < 5 ? 6.5f : currentLevel is 5 or 6 ? 7f : currentLevel is 7 or 8 ? 8f : 9f;
        TouchManager.Instance.moveCount = level[currentLevel].levelMoves;
        uiManager.levelMoves.text = level[currentLevel].levelMoves.ToString();
        CarInstantiate();
    }

    public void CarInstantiate()
    {
        level[currentLevel].levelEnvironment.SetActive(true);
        m_ThemesPanel.sprite = GameManager.Instance.gameplaySceneBG[gameData.gameplaySceneBG];
        TouchManager.Instance.SetParkingCell();
        TouchManager.Instance.SetRowList();
        for (int i = 0; i < level[currentLevel].carAmount; i++)
        {
            GameObject spawnedPrefab = Instantiate(carPrefab, level[currentLevel].carPosition[i].position, Quaternion.identity);
            spawnedPrefab.transform.SetParent(level[currentLevel].carPosition[i].parent);
            spawnedPrefab.GetComponent<Car>().CarColor = level[currentLevel].carColor[i];
            spawnedPrefab.GetComponent<Car>().SetParkingCell();
            spawnedCarPrefabs.Add(spawnedPrefab);
        }
        AudioManager.Instance.Play("GameStart");
    }

    public void EndGame()
    {
        if (currentLevel == gameData.unlockedLevel && currentLevel < 9)
        {
            gameData.unlockedLevel++;
            gameData.gems += (currentLevel + 1) * 10;
            GameDataManager.Instance.Save();
            UIManager.Instance.UpdateGemsAmountText();
        }
        else
        {
            gameData.gems += 2;
            GameDataManager.Instance.Save();
            UIManager.Instance.UpdateGemsAmountText();
        }

        // TODO: Show the victory screen and display the final score and time
        foreach (Cell parkingCell in TouchManager.Instance.parkingCells)
        {
            parkingCell.IsOccupide = false;
        }
        TouchManager.Instance.parkingCells.Clear();
        TouchManager.Instance.rowsList.Clear();
        foreach (GameObject go in spawnedCarPrefabs)
        {
            go.GetComponent<Car>().parkingCell = null;
            Destroy(go);
        }
        spawnedCarPrefabs.Clear();
        level[currentLevel].levelEnvironment.SetActive(false);
        uiManager.PanelActivate(uiManager.levelCompletedPanel.name);
    }

    public void ClearGame()
    {
        foreach (Cell parkingCell in TouchManager.Instance.parkingCells)
        {
            parkingCell.IsOccupide = false;
        }
        TouchManager.Instance.parkingCells.Clear();
        TouchManager.Instance.rowsList.Clear();
        foreach (GameObject go in spawnedCarPrefabs)
        {
            go.GetComponent<Car>().parkingCell = null;
            Destroy(go);
        }
        spawnedCarPrefabs.Clear();
        level[currentLevel].levelEnvironment.SetActive(false);
    }

    public void EndGameDelay()
    {
        TouchManager.Instance.gameOver = true;
        Invoke(nameof(EndGame), 1f);
    }

    public void ResetCarPosition()
    {
        ClearGame();
        StartGame();
    }
}

[Serializable]
public class LevelManager
{
    public string name;

    public int levelMoves;
    public int carAmount;

    public Color[] carColor;

    public Transform[] carPosition;

    public GameObject levelEnvironment;
}
