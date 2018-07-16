using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerData", menuName = "CreateScriptable/PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    public string playerName; /*玩家姓名*/
    public string endTimer;
    public int minTimer; /*最短时间*/

    public List<LeaderboardData> LeaderboardDatas = new List<LeaderboardData>();
}

[Serializable]
public struct LeaderboardData : IComparable<LeaderboardData>
{
    public int timer;
    public string name;
    public string endtimer;

    public int CompareTo(LeaderboardData x)
    {
        return timer.CompareTo(x.timer);
    }
}