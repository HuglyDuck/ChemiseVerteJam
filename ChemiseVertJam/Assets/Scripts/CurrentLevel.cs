using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private int currentLevel;

    void Start()
    {
        currentLevel = 1;
        AudioMusicManager.Instance.PlayMusicForLevel(currentLevel);
    }

    public void ChangeLevel(int newLevel)
    {
        currentLevel = newLevel;
        AudioMusicManager.Instance.PlayMusicForLevel(currentLevel);
    }
}
