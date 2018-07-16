using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {
    [SerializeField]
    private Image _bG;
    [SerializeField]
    private AudioMixerSnapshot _paused, _unpaused; /*暂停与非暂停时的音量*/
    [SerializeField]                 
    private CanvasGroup _pauseGroup; 
    [SerializeField]
    private CanvasGroup _audioGroup; 
    [SerializeField]
    private CanvasGroup _inputGroup;
    [SerializeField]
    private CanvasGroup _backMainGroup;
    [SerializeField]
    private CanvasGroup _replayGroup;
    [SerializeField]
    private CanvasGroup _gameOver;
    [SerializeField]
    private CanvasGroup _gameWin;

    private List<CanvasGroup> canvasGroupList = new List<CanvasGroup>();
    private Stack<CanvasGroup> canvasGroupStack = new Stack<CanvasGroup>();
    
    private void Start(){
        canvasGroupList.Clear();
        canvasGroupStack.Clear();
        canvasGroupList.Add(_pauseGroup);
        canvasGroupList.Add(_audioGroup);
        canvasGroupList.Add(_inputGroup);
        canvasGroupList.Add(_backMainGroup);
        canvasGroupList.Add(_replayGroup);
        canvasGroupList.Add(_gameOver);
        canvasGroupList.Add(_gameWin);
        _bG.gameObject.SetActive(false);
        DisPlayMenu();
    }

    private void Update()
    {
        if (GameManager.Instance.GameOver)
        {
            GameOverGroup();
        }
        else if (GameManager.Instance.GameWin)
        {
            GameWinGroup();
        }
    }

    void LowPass () {
        if (GameManager.Instance.TimeScale == 0)
        {
            _paused.TransitionTo(.01f); /*暂停声音过渡*/
        }
        else
        {
            _unpaused.TransitionTo(.01f); /*非暂停声音过渡*/
        }
	}
    public void Back()
    {
        if (canvasGroupStack.Count == 0)
        {
            _bG.gameObject.SetActive(true);
            PauseButton();
        }
        else
        {
            if (canvasGroupStack.Count > 0)
            {
                canvasGroupStack.Pop();
                if (canvasGroupStack.Count == 0)
                {
                    UnPauseButton();
                    _bG.gameObject.SetActive(false);
                }
            }
        }
        DisPlayMenu();
    }
    public void PauseButton()
    {
        GameManager.Instance.Pause();
        LowPass();
        canvasGroupStack.Push(_pauseGroup);
        DisPlayMenu();
    }
    public void UnPauseButton()
    {
        GameManager.Instance.Pause();
        LowPass();
        if (canvasGroupStack.Count > 0)
        {
            canvasGroupStack.Pop();
        }
        DisPlayMenu();
    }
    public void AudioGroupButton()
    {
        if (canvasGroupStack.Count > 1)
        {
            canvasGroupStack.Pop();
        }
        canvasGroupStack.Push(_audioGroup);
        DisPlayMenu();
    }
    public void InputGroupButton()
    {
        if (canvasGroupStack.Count > 1)
        {
            canvasGroupStack.Pop();
        }
        canvasGroupStack.Push(_inputGroup);
        DisPlayMenu();
    }
    public void ReplayGroupButton()
    {
        canvasGroupStack.Push(_replayGroup);
        DisPlayMenu();
    }
    public void BackMainButton()
    {
        canvasGroupStack.Push(_backMainGroup);
        DisPlayMenu();
    }
    public void ReplayGameButton(string loadSceneName)
    {
        GameManager.Instance.UnPause();
        StartCoroutine(StartLevel(loadSceneName));
    }

    private IEnumerator StartLevel(string loadSceneName)
    {
        GameManager.Instance.GameOverTriggerEvent();
        UIManager.Instance.FaderOn(true, 1f);
        yield return new WaitForSeconds(1f);
        LoadSceneManager.LoadScene(loadSceneName);
        GameManager.Instance.GameManagerInit();
        if (loadSceneName != "MainMenuScene")
        {
            GameManager.Instance.GameStartTriggerEvent();
        }
        UIManager.Instance.FaderOn(false, 1f);
    }

    private void GameOverGroup()
    {
        LowPass();
        canvasGroupStack.Push(_gameOver);
        DisPlayMenu();
    }
    private void GameWinGroup()
    {
        LowPass();
        canvasGroupStack.Push(_gameWin);
        DisPlayMenu();
    }

    private void DisPlayMenu() /*将 UI 组别的显示与隐藏方式*/
    {
        foreach (var item in canvasGroupList) /*代表如果被放置到列表内，则将该 UI 组的 CanvasGroupList 设置如以下*/
        {
            item.alpha = 0;
            item.interactable = false;
            item.blocksRaycasts = false;
        }

        if (canvasGroupStack.Count > 0) /*如果栈内的数值大于0，则将当前的 Group 抛出*/
        {
            CanvasGroup cg = canvasGroupStack.Peek();
            cg.alpha = 1;
            cg.interactable = true;
            cg.blocksRaycasts = true;
        }
    }
}
