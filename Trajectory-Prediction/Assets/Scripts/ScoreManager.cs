using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    #region Singleton
    public static ScoreManager Instance { private set; get; }
    #endregion

    // PUBLIC_FIELDS //
    public TMP_Text scoreText;

    // PRIVATE_FIELDS
    int scoreCount = 0;

    void Awake()
    {
        Instance = this;
    }

    public void Score()
    {
        scoreCount++;
        scoreText.text = scoreCount.ToString();
    }
}
