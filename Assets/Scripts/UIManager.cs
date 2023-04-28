using DG.Tweening;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    #region Fields
    public static UIManager Instance;
    public GameData gameData;

    [Header("UI Panels")]
    public GameObject startLoadingPanel;
    public GameObject loadingPanel;
    public GameObject mainMenuPanel;
    public GameObject levelSelectionPanel;
    public GameObject settingsPanel;
    public GameObject gamePlaySettingsPanel;
    public GameObject storePanel;
    public GameObject removeADsSubscriptionPanel;
    public GameObject buyAllThemesSubscriptionPanel;
    public GameObject buyAllCarsSubscriptionPanel;
    public GameObject buyAllLevelSubscriptionPanel;
    public GameObject billingTermsPanel;
    public GameObject exitGamePanel;
    public GameObject rateUsPanel;
    public GameObject skipLevelPanel;
    public GameObject buyMovePanel;
    public GameObject gamePlayPanel;
    public GameObject levelPausedPanel;
    public GameObject levelCompletedPanel;
    public GameObject levelFailedPanel;
    public GameObject[] panelsArray;

    [Header("Start Loading and Loading")]
    public Image startLoadingInner;
    public Image loadingInner;

    [Header("LevelSelection")]
    public GameObject[] levelLocks;

    [Header("Store")]
    public GameObject purchasePromParent;
    public GameObject successfullPurchase;
    public GameObject unsuccessfullPurchase;
    public GameObject gemsStatsGO;
    public GameObject gemsStatsForMoveBuyAndSkipLevel;
    public GameObject IAPStore;
    public GameObject CarsStore;
    public GameObject ThemesStore;
    public TextMeshProUGUI gemsText;
    public TextMeshProUGUI gemsStatsForMoveBuyAndSkipLevelText;
    //IAP
    public Button UnlocAllLevelButton;
    public Button RemoveAdsButton;
    public Button BuyEvetythingButton;
    //Car
    public GameObject[] carBuy;
    public GameObject[] carSelect;
    public TextMeshProUGUI[] carSelectText;
    //Themes
    public GameObject[] themeBuy;
    public GameObject[] themeSelect;
    public TextMeshProUGUI[] themeSelectText;

    [Header("Settings ")]
    public AudioMixer audioMixer;
    public GameObject MMSonudOn;
    public GameObject MMSonudOff;
    public GameObject MMMusicOn;
    public GameObject MMMusicOff;
    public GameObject GPSoundOn;
    public GameObject GPSoundOff;
    public GameObject GPMusicOn;
    public GameObject GPMusicOff;

    [Header("Game Play")]
    public int levelSelected;
    public TextMeshProUGUI levelMoves;
    public TextMeshProUGUI levelText;
    public GameObject wrongMoveTextPrompt;
    public GameObject wrongMoveTextPromptParent;
    public GameController gameController;
    public GameObject skipADsButton;
    public GameObject skipADsButton1;
    public GameObject NextLevelButton;

    #endregion


    #region Unity Methods
    private void Awake() { Instance = this; }

    private void Start()
    {
        _ = StartCoroutine(StartLoading());
        UpdateGemsAmountText();
        if (gameData.isSoundOn) { SoundOn(); } else { SoundOff(); }
        if (gameData.isMusicOn) { MusicOn(); } else { MusicOff(); }
        int i = 0;
        foreach (bool car in gameData.carBuy)
        {
            if (car)
            {
                carBuy[i].SetActive(false);
                carSelect[i].SetActive(true);
            }
            i++;
        }
        int j = 0;
        foreach (bool theme in gameData.themeBuy)
        {
            if (theme)
            {
                themeBuy[j].SetActive(false);
                themeSelect[j].SetActive(true);
            }
            j++;
        }
    }
    #endregion

    #region Start Loading
    private IEnumerator StartLoading()
    {
        PanelActivate(startLoadingPanel.name);
        startLoadingInner.DOFillAmount(1, 9);
        yield return new WaitForSecondsRealtime(9f);
        GoHome();
    }
    #endregion

    #region Loading
    private IEnumerator Loading(GameObject _goto)
    {
        ThingsToEnable_or_Disables(_goto);
        PanelActivate(loadingPanel.name);
        loadingInner.DOFillAmount(1, 3);
        yield return new WaitForSecondsRealtime(3f);
        PanelActivate(_goto.name);
        loadingInner.fillAmount = 0;
    }

    private void ThingsToEnable_or_Disables(GameObject Panel)
    {
        if (Panel == levelSelectionPanel)
        {
            for (int i = 0; i <= gameData.unlockedLevel; i++)
            {
                levelLocks[i].SetActive(false);
            }
        }
        else if (Panel == gamePlayPanel)
        {
            GameController.Instance.StartGame();
        }
    }
    #endregion

    #region Main Menu
    public void OnGameExitButtonClicked()
    {
        PanelActivate(exitGamePanel.name);
    }

    public void OnStartButtonClicked()
    {
        _ = StartCoroutine(Loading(levelSelectionPanel));
    }

    public void OnRateUsButtonClicked()
    {
        PanelActivate(rateUsPanel.name);
    }

    public void OnMoreGamesButtonClicked()
    {

    }

    public void OnRemoveAdsButtonClicked()
    {
        removeADsSubscriptionPanel.SetActive(true);
    }
    #endregion

    #region Level Selection
    public void OnLevelButtonClicked(int level)
    {
        if (level <= gameData.unlockedLevel)
        {
            levelSelected = level;
            _ = StartCoroutine(Loading(gamePlayPanel));
        }
    }
    #endregion

    #region Store
    public void OnIAPButtonClicked()
    {
        IAPStore.SetActive(true);
        CarsStore.SetActive(false);
        ThemesStore.SetActive(false);
    }

    public void OnCarsButtonClicked()
    {
        CarsStore.SetActive(true);
        IAPStore.SetActive(false);
        ThemesStore.SetActive(false);
    }

    public void OnThemesButtonClicked()
    {
        ThemesStore.SetActive(true);
        IAPStore.SetActive(false);
        CarsStore.SetActive(false);
    }

    // IAP Store
    public void OnWatchVideoToGetGemsButtonCliked()
    {
        gameData.gems += 10;
        GameDataManager.Instance.Save();
        UpdateGemsAmountText();
    }

    public void OnBuyGemsButtonClicked() // Not Buy Gems but unlock all themes
    {
        buyAllThemesSubscriptionPanel.SetActive(true);
    }

    public void OnUnlockAllLevelButtonCliked()
    {
        buyAllLevelSubscriptionPanel.SetActive(true);
    }

    public void OnRemoveAdsBurronCliked()
    {
        removeADsSubscriptionPanel.SetActive(true);
    }

    public void OnUnlockEverythingButtonCliked() // Not unlock everything but unlock all cars
    {
        buyAllCarsSubscriptionPanel.SetActive(true);
    }

    // Car Store
    public void CarBuy(int car)
    {
        if (car == 1)
        {
            if (gameData.gems >= 50)
            {
                carBuy[1].SetActive(false);
                carSelect[1].SetActive(true);
                gameData.carBuy[1] = true;
                gameData.gems -= 50;
                GameDataManager.Instance.Save();
            }
            else
            {
                Debug.Log("Gems are't Enough!");
            }
        }
        else if (car == 2)
        {
            if (gameData.gems >= 100)
            {
                carBuy[2].SetActive(false);
                carSelect[2].SetActive(true);
                gameData.carBuy[2] = true;
                gameData.gems -= 150;
                GameDataManager.Instance.Save();
            }
            else
            {
                Debug.Log("Gems are't Enough!");
            }
        }
        else if (car == 3)
        {
            if (gameData.gems >= 200)
            {
                carBuy[3].SetActive(false);
                carSelect[3].SetActive(true);
                gameData.carBuy[3] = true;
                gameData.gems -= 200;
                GameDataManager.Instance.Save();
            }
            else
            {
                Debug.Log("Gems are't Enough!");
            }
        }
    }

    public void CarSelect(int car)
    {
        gameData.carPrefab = car;
        GameDataManager.Instance.Save();
        foreach (TextMeshProUGUI item in carSelectText)
        {
            item.text = "Select";
        }
        carSelectText[car].text = "Selected";
    }

    // Themes Store
    public void ThemeBuy(int theme)
    {
        if (gameData.gems >= 100)
        {
            themeBuy[theme].SetActive(false);
            themeSelect[theme].SetActive(true);
            gameData.themeBuy[theme] = true;
            gameData.gems -= 100;
            GameDataManager.Instance.Save();
        }
        else
        {
            Debug.Log("Gems are't Enough!");
        }
    }

    public void ThemeSelect(int theme)
    {
        gameData.gameplaySceneBG = theme;
        GameDataManager.Instance.Save();
        foreach (TextMeshProUGUI item in themeSelectText)
        {
            item.text = "Select";
        }
        themeSelectText[theme].text = "Selected";
    }
    #endregion

    #region Settings
    public void SoundOn()
    {
        audioMixer.SetFloat("SFX", -3);
        audioMixer.SetFloat("UI", -3);
        MMSonudOn.SetActive(true);
        MMSonudOff.SetActive(false);
        GPSoundOn.SetActive(true);
        GPSoundOff.SetActive(false);
        gameData.isSoundOn = true;
    }

    public void SoundOff()
    {
        audioMixer.SetFloat("SFX", -80);
        audioMixer.SetFloat("UI", -80);
        MMSonudOn.SetActive(false);
        MMSonudOff.SetActive(true);
        GPSoundOn.SetActive(false);
        GPSoundOff.SetActive(true);
        gameData.isSoundOn = false;
    }

    public void MusicOn()
    {
        audioMixer.SetFloat("BGM", -3);
        MMMusicOn.SetActive(true);
        MMMusicOff.SetActive(false);
        GPMusicOn.SetActive(true);
        GPMusicOff.SetActive(false);
        gameData.isMusicOn = true;
    }

    public void MusicOff()
    {
        audioMixer.SetFloat("BGM", -80);
        MMMusicOn.SetActive(false);
        MMMusicOff.SetActive(true);
        GPMusicOn.SetActive(false);
        GPMusicOff.SetActive(true);
        gameData.isMusicOn = false;
    }

    public void PrivacyButton()
    {

    }
    #endregion

    #region Game Play
    public void SetLevelText()
    {
        levelText.text = "LEVEL  " + (1 + levelSelected).ToString();
    }

    public void WrongMoveTextPrompter()
    {
        GameObject wrongMoveText = Instantiate(wrongMoveTextPrompt, wrongMoveTextPromptParent.transform);

        if (wrongMoveTextPromptParent.transform.childCount > 3)
        {
            Destroy(wrongMoveTextPromptParent.transform.GetChild(3).gameObject);
        }
        if (wrongMoveText != null) { Destroy(wrongMoveText, 5f); }
    }

    public void OnGamePlaySettingButtonClicked()
    {
        PanelActivate(gamePlaySettingsPanel.name);
    }

    public void OnRetryButtonCliked()
    {
        GameController.Instance.ResetCarPosition();
    }

    public void OnPauseButtonClicked()
    {
        PanelActivate(levelPausedPanel.name);
    }

    public void OnBuyMoveButtonClicked()
    {
        buyMovePanel.SetActive(true);
        gemsStatsForMoveBuyAndSkipLevelText.text = gameData.gems.ToString();
        gemsStatsForMoveBuyAndSkipLevel.SetActive(true);
    }
    #endregion

    #region Level Pause
    public void OnResumeButtonClicked()
    {
        TouchManager.Instance.gameOver = false;
        PanelActivate(gamePlayPanel.name);
    }

    public void OnHomeButtonClickedFromLevelPause()
    {
        GameController.Instance.ClearGame();
        _ = StartCoroutine(Loading(mainMenuPanel));
    }

    public void OnSkipLevelButtonClicked()
    {
        skipLevelPanel.SetActive(true);
        gemsStatsForMoveBuyAndSkipLevelText.text = gameData.gems.ToString();
        gemsStatsForMoveBuyAndSkipLevel.SetActive(true);
    }
    #endregion

    #region Level Complete
    public void OnReplayButtonClicked()
    {
        TouchManager.Instance.gameOver = false;
        PanelActivate(gamePlayPanel.name);
        GameController.Instance.StartGame();
    }

    public void OnNextLevelButtonClicked()
    {
        TouchManager.Instance.gameOver = false;
        if (levelSelected < 29) { levelSelected++; }
        GameController.Instance.StartGame();
        PanelActivate(gamePlayPanel.name);
    }

    public void OnHomeButtonClickedFromLevelCompleteOrFailed()
    {
        _ = StartCoroutine(Loading(mainMenuPanel));
    }
    #endregion

    #region Skip Level
    public void OnWatchVideoOfSkipLevelButtonClicked()
    {
        // Run This Function if the player watched the ad video
        ADsVideoOfSkipLevelWatchedCompletley();
    }

    private void ADsVideoOfSkipLevelWatchedCompletley()
    {
        if (levelSelected < 9 && gameData.unlockedLevel < 9)
        {
            GameController.Instance.ClearGame();
            TouchManager.Instance.gameOver = false;
            levelSelected++;
            gameData.unlockedLevel++;
            GameDataManager.Instance.Save();
            GameController.Instance.StartGame();
            PanelActivate(gamePlayPanel.name);
            gemsStatsForMoveBuyAndSkipLevel.SetActive(false);
        }
        else
        {
            Debug.Log("All Level Played");
        }
    }

    public void OnPay50GemsToSkipLevelButtonClicked()
    {
        if (gameData.gems >= 50 && levelSelected < 9 && gameData.unlockedLevel < 9)
        {
            GameController.Instance.ClearGame();
            TouchManager.Instance.gameOver = false;
            levelSelected++;
            gameData.gems -= 50;
            gameData.unlockedLevel++;
            GameDataManager.Instance.Save();
            UpdateGemsAmountText();
            GameController.Instance.StartGame();
            PanelActivate(gamePlayPanel.name);
            gemsStatsForMoveBuyAndSkipLevel.SetActive(false);
        }
        else if (gameData.gems < 50 && levelSelected < 9 && gameData.unlockedLevel < 9)
        {
            Debug.Log("Less Gems");
        }
        else
        {
            Debug.Log("All Level Played");
        }
    }

    public void OnNoThanksOfLevelSkipButtonClicked()
    {
        skipLevelPanel.SetActive(false);
        gemsStatsForMoveBuyAndSkipLevel.SetActive(false);
    }
    #endregion

    #region Buy Move
    public void OnWatchVideoOfBuyMoveButtonClicked()
    {
        // Run This Function if the player watched the ad video
        ADsVideoOfBuyMoveWatchedCompletley();
    }

    private void ADsVideoOfBuyMoveWatchedCompletley()
    {
        TouchManager.Instance.gameOver = false;
        buyMovePanel.SetActive(false);
        gemsStatsForMoveBuyAndSkipLevel.SetActive(false);
        TouchManager.Instance.moveCount += 3;
        UpdateMoveText();
    }

    public void OnPay30GemsToBuy3Moves()
    {
        if (gameData.gems >= 30)
        {
            TouchManager.Instance.gameOver = false;
            buyMovePanel.SetActive(false);
            gemsStatsForMoveBuyAndSkipLevel.SetActive(false);
            TouchManager.Instance.moveCount += 3;
            UpdateMoveText();
            gameData.gems -= 30;
            GameDataManager.Instance.Save();
            UpdateGemsAmountText();
        }
    }

    public void OnNoThanksOfBuyMoveButtonClicked()
    {
        if (TouchManager.Instance.moveCount > 0)
        {
            buyMovePanel.SetActive(false);
            gemsStatsForMoveBuyAndSkipLevel.SetActive(false);
        }
        else
        {
            LevelFailed();
        }
    }
    #endregion

    #region Public Methods
    public void PanelActivate(string panelName)
    {
        foreach (GameObject panel in panelsArray)
        {
            panel.SetActive(panelName.Equals(panel.name));
        }
        gemsStatsGO.SetActive(panelName.Equals(mainMenuPanel.name) || panelName.Equals(levelSelectionPanel.name) ||
            panelName.Equals(storePanel.name) || panelName.Equals(settingsPanel.name));
        TouchManager.Instance.gameOver = !panelName.Equals(gamePlayPanel.name);
        gemsStatsForMoveBuyAndSkipLevel.SetActive(false);
    }

    public void GoHome()
    {
        PanelActivate(mainMenuPanel.name);
    }

    public void GoToStore()
    {
        PanelActivate(storePanel.name);
    }

    public void GoToSettings()
    {
        PanelActivate(settingsPanel.name);
    }

    public void LevelFailed()
    {
        AudioManager.Instance.Play("Lose");
        PanelActivate(levelFailedPanel.name);
        GameController.Instance.ClearGame();
    }

    public void GameExitYes()
    {
        Application.Quit();
    }

    public void UpdateMoveText()
    {
        levelMoves.text = TouchManager.Instance.moveCount.ToString();
    }

    public void UpdateGemsAmountText()
    {
        gemsText.text = gameData.gems.ToString();
    }
    #endregion

    public void AllCarPurchase()
    {
        gameData.carBuy[0] = true;
        gameData.carBuy[1] = true;
        gameData.carBuy[2] = true;
        gameData.carBuy[3] = true;
        GameDataManager.Instance.Save();
        carBuy[0].SetActive(false);
        carSelect[0].SetActive(true);
        carBuy[1].SetActive(false);
        carSelect[1].SetActive(true);
        carBuy[2].SetActive(false);
        carSelect[2].SetActive(true);
        carBuy[3].SetActive(false);
        carSelect[3].SetActive(true);
        Invoke(nameof(AllCarPuchaseDone), 0.5f);
    }
    private void AllCarPuchaseDone()
    {
        buyAllCarsSubscriptionPanel.SetActive(false);
        GameObject infoText = Instantiate(successfullPurchase, purchasePromParent.transform);
        Destroy(infoText, 5f);
    }
    public void BackFromAllCarPurchase()
    {
        buyAllCarsSubscriptionPanel.SetActive(false);
    }

    public void AllThemesPurchase()
    {
        for (int i = 0; i < 9; i++)
        {
            gameData.themeBuy[i] = true;
            themeBuy[i].SetActive(false);
            themeSelect[i].SetActive(true);
        }
        GameDataManager.Instance.Save();
        Invoke(nameof(AllThemesPuchaseDone), 0.5f);
    }
    private void AllThemesPuchaseDone()
    {
        buyAllThemesSubscriptionPanel.SetActive(false);
        GameObject infoText = Instantiate(successfullPurchase, purchasePromParent.transform);
        Destroy(infoText, 5f);
    }

    public void BackFromAllThemesPurchase()
    {
        buyAllThemesSubscriptionPanel.SetActive(false);
    }

    public void AllLevelPurchase()
    {
        if (gameData.unlockedLevel < 9)
        {
            gameData.unlockedLevel = 9;
            gameData.unlockedAllLevel = true;
            GameDataManager.Instance.Save();
        }
        buyAllLevelSubscriptionPanel.SetActive(false);
        GameObject infoText = Instantiate(successfullPurchase, purchasePromParent.transform);
        Destroy(infoText, 5f);
    }
    public void BackFromAllLevelPurchase()
    {
        buyAllLevelSubscriptionPanel.SetActive(false);
    }

    public void RemoveAdsPurchase()
    {
        gameData.removeADs = true;
        GameDataManager.Instance.Save();
        removeADsSubscriptionPanel.SetActive(false);
        GameObject infoText = Instantiate(successfullPurchase, purchasePromParent.transform);
        Destroy(infoText, 5f);
    }
    public void BackFromRemoveAdsPurchase()
    {
        removeADsSubscriptionPanel.SetActive(false);
    }


}
