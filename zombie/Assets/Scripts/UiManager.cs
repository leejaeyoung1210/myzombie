using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UiManager : MonoBehaviour
{
    public Text ammoText;
    public Text scoreText;
    public Text waveText;

    public GameObject gameOverUi;

    

    public void OnEnable()
    {
        SetAmmpText(0, 0);
        SetUpdateScore(0);
        SetWaverInfo(0,0);
        SetActiveGameOverUi(false);        
    }

    public void SetAmmpText(int magAmmo, int remainAmmo)
    {
        ammoText.text = $"{magAmmo}/{remainAmmo}";
    }

    public void SetUpdateScore(int score)
    {
        scoreText.text = $"Score:{score}";
    }

    public void SetWaverInfo(int wave, int count)
    {
        waveText.text = $"Wave: {wave}\nEnemy Left: {count}";
    }

    public void SetActiveGameOverUi(bool active)
    {
        gameOverUi.SetActive(active);
    }

    public void OnClickRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}
