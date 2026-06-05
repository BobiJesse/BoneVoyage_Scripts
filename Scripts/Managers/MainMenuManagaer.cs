using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManagaer : MonoBehaviour
{
    [Header("SectionButtons")]

    [SerializeField] GameObject MainTitleButtons;
    [SerializeField] GameObject GameModeButtons;
    [SerializeField] GameObject CreditsButtons;
    [SerializeField] GameObject SettingsButtons;

    // Button activations ----- - - - - - - - - -- - - 

    public void DisableButtons()
    {
        MainTitleButtons.SetActive(false);
        GameModeButtons.SetActive(false);
        CreditsButtons.SetActive(false);
        SettingsButtons.SetActive(false);
    }

    public void ActivateMainTitle()
    {
        DisableButtons();
        MainTitleButtons.SetActive(true);
    }

    public void ActivateGameModes()
    {
        DisableButtons();
        GameModeButtons.SetActive(true);
    }

    public void ActivateSettings()
    {
        DisableButtons();
        SettingsButtons.SetActive(true);
    }

    public void ActivateCredits()
    {
        DisableButtons();
        CreditsButtons.SetActive(true);
    }

    // Logics - - -- -- -  -  - - - - -   -- - - - 

    [SerializeField] GameObject EndlessHider;
    [SerializeField] GameObject EndlessPlayButton;

    [SerializeField] TextMeshProUGUI ScoreText;

    private void Awake()
    {

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        ActivateMainTitle();

        // When campaing is completed
        if (PlayerPrefs.HasKey("GameComplete"))
        {
            EndlessHider.SetActive(false);
            EndlessPlayButton.SetActive(true);
        }

        // Endless score
        ScoreText.text = " High Score: " + PlayerPrefs.GetInt("EndlessScore");
    }

    public void PlayCampaing()
    {
        Debug.Log("Play the story mode");
        SceneManager.LoadScene("StartingScene");
    }

    public void PlayEndless()
    {
        Debug.Log("Play endless mode");
        SceneManager.LoadScene("EndlessBoss");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}