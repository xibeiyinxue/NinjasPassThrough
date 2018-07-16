using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUI : MonoBehaviour {
    [SerializeField]
    private CanvasGroup m_MainMenuGroup;
    [SerializeField]
    private CanvasGroup m_LevelGroup;
    [SerializeField]
    private CanvasGroup m_AudioSettingGroup;
    [SerializeField]
    private CanvasGroup m_InputSettingGroup;
    [SerializeField]
    private CanvasGroup m_LeaderboardGroup;
    [SerializeField]
    private CanvasGroup m_CreateGroup;

    private Stack<CanvasGroup> canvasGroupStack = new Stack<CanvasGroup>();
    private List<CanvasGroup> canvasGroupList = new List<CanvasGroup>();

	void Start () {
        UIManager.Instance.FaderOn(false, 1f);

        //添加 CanvasGroup 进列表
        canvasGroupList.Add(m_MainMenuGroup);
        canvasGroupList.Add(m_LevelGroup);
        canvasGroupList.Add(m_AudioSettingGroup);
        canvasGroupList.Add(m_InputSettingGroup);
        canvasGroupList.Add(m_LeaderboardGroup);
        canvasGroupList.Add(m_CreateGroup);
        //canvasGroupList.Add();

        //将 mainMenuGroup 的 CanvasGroup 推至最前
        canvasGroupStack.Push(m_MainMenuGroup);
        DisPlayMenu();
    }

    void Update () {
		
	}

    public void StartGameButton(string _loadSceneName)
    {
        StartCoroutine(StartLevel(_loadSceneName));
    }
    public void LevelGroupButton()
    {
        canvasGroupStack.Push(m_LevelGroup);
        DisPlayMenu();
    }
    public void AudioGroupButton() /*音乐设置按钮*/
    {
        canvasGroupStack.Push(m_AudioSettingGroup);
        DisPlayMenu();
    }
    public void InputGroupButton() /*按键设置按钮*/
    {
        canvasGroupStack.Push(m_InputSettingGroup);
        DisPlayMenu();
    }
    public void EscButton()
    {
        canvasGroupStack.Push(m_MainMenuGroup);
        DisPlayMenu();
    }
    public void LeaderboardGroupButton()
    {
        canvasGroupStack.Push(m_LeaderboardGroup);
        DisPlayMenu();
    }
    public void CreateGroupButton()
    {
        canvasGroupStack.Push(m_CreateGroup);
        DisPlayMenu();
    }

    private IEnumerator StartLevel(string _sceneName)
    {
        UIManager.Instance.FaderOn(true, 1f);
        yield return new WaitForSeconds(1f);
        LoadSceneManager.LoadScene(_sceneName);
    }

    //该方法将 CanvasGroup 组件的 alpha 、interactable 、blocksRaycasts 进行修改
    private void DisPlayMenu()
    {
        foreach (var item in canvasGroupList)
        {
            item.alpha = 0;
            item.interactable = false;
            item.blocksRaycasts = false;
        }

        if (canvasGroupStack.Count > 0)
        {
            CanvasGroup cg = canvasGroupStack.Peek();
            cg.alpha = 1;
            cg.interactable = true;
            cg.blocksRaycasts = true;
        }
    }
}
