using System.Diagnostics.Tracing;
using TMPro;
using UnityEngine;

public class BossManagerScript : MonoBehaviour
{
    [SerializeField] bool EndlessMode;

    [SerializeField] int BossHP;

    [SerializeField] GameObject BossPrefab;

    [SerializeField] BossEffectsScript BossEffectScript;

    [SerializeField] PlatformSpawner CanonPlatformSpawnScript;

    [SerializeField] TextMeshProUGUI BossText;
    [SerializeField] GameObject Hearth;
    [SerializeField] GameObject HighScore;

    [SerializeField] GameObject Island;

    private void Start()
    {

        if (EndlessMode)
        {
            Island.SetActive(true);
        }

        SpawnPlatforms();
        UpdateBossText();
    }

    public void SpawnPlatforms()
    {
        CanonPlatformSpawnScript.SpawnPlatfrom();
    }

    void UpdateBossText()
    {
        if (EndlessMode)
        {


            BossText.text = "Score: " + BossHP;
            Hearth.SetActive(false);
        }
        else
        {
            BossText.text = "<3 : " + BossHP;
        }
    }

    public void DamageBoss()
    {
        if (EndlessMode)
        {
            //Score++
            BossHP++;

            BossEffectScript.BossDamageffect();
            SpawnPlatforms();
            int OldScore = PlayerPrefs.GetInt("EndlessScore");

            UpdateBossText();


            if (OldScore <= BossHP)
            {
                Debug.Log("New High Score");
                HighScore.SetActive(true);
                PlayerPrefs.SetInt("EndlessScore", BossHP);
            }
            else
            {
                Debug.Log("Boss took damage but not high score");
            }
        }
        else
        {
            BossHP--;
            Debug.Log("Boss took damage");
            SpawnPlatforms();
            UpdateBossText();
            BossEffectScript.BossDamageffect();
            if (BossHP <= 0 )
            {
                Debug.Log("Boss Dies");
                BossPrefab.SetActive(false);
                Island.SetActive(true);
            }
        }
    }
}
