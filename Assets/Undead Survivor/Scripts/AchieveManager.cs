using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchieveManager : MonoBehaviour
{
    public GameObject[] lockCharacters;
    public GameObject[] unlockCharacters;
    public GameObject uiNotice;

    enum Achieve { UnlockPotato, UnlockOrange }
    Achieve[] achieves;
    WaitForSecondsRealtime wait;

    void Awake()
    {
        achieves = (Achieve[])Enum.GetValues(typeof(Achieve));
        wait = new WaitForSecondsRealtime(5);
        if (!PlayerPrefs.HasKey("MyData"))
        {
            Init();
        }
    }

    void Init()
    {
        PlayerPrefs.SetInt("MyData", 1);
        foreach (Achieve achieve in achieves)
        {
            PlayerPrefs.SetInt(achieve.ToString(), 0);
        }
    }
    
    void Start()
    {
        UnlockCharacter();
    }

    void UnlockCharacter()
    {
        for (int i = 0; i < lockCharacters.Length; ++i)
        {
            string achiveName = achieves[i].ToString();
            bool isUnlock = PlayerPrefs.GetInt(achiveName) == 1;
            lockCharacters[i].SetActive(!isUnlock);
            unlockCharacters[i].SetActive(isUnlock);
        }
    }

    void LateUpdate()
    {
        foreach (Achieve achieve in achieves)
        {
            CheckAchieve(achieve);
        }
    }

    void CheckAchieve(Achieve achieve)
    {
        bool isAchieve = false;
        switch(achieve)
        {
        case Achieve.UnlockPotato:
            isAchieve = GameManager.instance.kill >= 10;
            break;
        case Achieve.UnlockOrange:
            isAchieve = GameManager.instance.gameTime == GameManager.instance.maxGameTime;
            break;
        }

        if (isAchieve && PlayerPrefs.GetInt(achieve.ToString()) == 0)
        {
            PlayerPrefs.SetInt(achieve.ToString(), 1);

            for (int i = 0; i < uiNotice.transform.childCount; ++i)
            {
                bool isActive = i == (int)achieve;
                uiNotice.transform.GetChild(i).gameObject.SetActive(isActive);
            }
            StartCoroutine(NoticeRoutine());
        }
    }

    IEnumerator NoticeRoutine()
    {
        uiNotice.SetActive(true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);

        yield return wait;
        uiNotice.SetActive(false);
    }
}
