using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginPannel : MonoBehaviour {
    [SerializeField]
    private InputField inputField = null;
    private PlayerData playerData;

    [SerializeField]
    private Button startButton = null;
    [SerializeField]
    private Text startButtonText = null;

    private void Awake()
    {
        playerData = Resources.Load<PlayerData>("PlayerData");
        UIManager.Instance.FaderOn(false, 1f);
        if (startButton != null)
        {
            startButton.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        SetPlayerName();
    }

    private void SetPlayerName()
    {
        playerData.playerName = inputField.text;

        if (playerData.playerName != "")
        {
            startButtonText.text = "忍者" + playerData.playerName + "开始行动";
            startButton.gameObject.SetActive(true);
        }
        else
        {
            startButton.gameObject.SetActive(false);
        }
    }

    public void StartGame()
    {
        StartCoroutine(LastScene());
    }

    private IEnumerator LastScene()
    {
        UIManager.Instance.FaderOn(true, 1f);
        yield return new WaitForSeconds(1f);
        LoadSceneManager.LoadScene("MainMenuScene");
    }
}
