using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardMenu : MonoBehaviour {
    [SerializeField]
    private GameObject content;

    private LevelDirector director;
    private PlayerData data;
    private List<Text> timerTexts = new List<Text>();
    private List<Text> nameTexts = new List<Text>();

    private void Awake()
    {
        data = Resources.Load<PlayerData>("PlayerData");
        foreach (Transform item in content.transform)
        {
            nameTexts.Add(item.GetComponent<Text>());
            timerTexts.Add(item.GetChild(0).GetComponent<Text>());
        }
    }

    void Start () {
        data.LeaderboardDatas.Sort();
        for (int i = 0;(i < timerTexts.Count) && (i < data.LeaderboardDatas.Count); i++)
        {
            timerTexts[i].text = data.LeaderboardDatas[i].endtimer;
            nameTexts[i].text = data.LeaderboardDatas[i].name;
        }
	}
}
