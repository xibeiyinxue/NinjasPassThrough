using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelDirector : Singleton<LevelDirector> {

    private Player m_Player;
    private PlayerData data;

    private int minTimer;
    private int _currentTimer;
    public int t, m, s;
    private string _endTimer;

    private int _playerLifeCount;

    private Transform last_CheckPoints;
    private Transform current_CheckPoints;

    public Player CurrentPlayer { get; private set; }
    public int CurrentTimer { get { return _currentTimer; }
        set
        {
            _currentTimer = value;
            if (minTimer > _currentTimer)
            {
                data.minTimer = value;
                _currentTimer = value;
            }
        }
    }
    public int MinTimer { get { return minTimer; } }
    public int PlayerLifeCount { get { return _playerLifeCount; } }

    public Transform Current_CheckPoints { get {return current_CheckPoints; } set { current_CheckPoints = value; } }

    private UnityAction onGameStart;
    private UnityAction onPlayerDead;
    private UnityAction onGameOver;

    private void OnEnable()
    {
        onGameStart = Start;
        EventManager.StartListening(EventList.GameStartEvent, onGameStart);
    }
    private void OnDisable()
    {
        EventManager.StopListening(EventList.GameStartEvent, onGameStart);
    }

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        m_Player = Resources.Load<Player>("Prefabs/Player");
        data = Resources.Load<PlayerData>("PlayerData");
        minTimer = data.minTimer;
    }
    
    private void Start()
    {
        _playerLifeCount = 3;
        if (UIManager.Instance != null)
        {
            UIManager.Instance.FaderOn(false, 1f);
        }
        StartCoroutine(Decorate());
        t = m = s = 0;
    }

    void Update () {
        if (GameManager.Instance.TimeScale != 0 && !GameManager.Instance.EndTimer)
        {
            TimerStart();
        }
    }

    private void TimerStart()
    {
        s++;
        if (s == 60)
        {
            m++;
            s = 0;
        }
        if (m== 60)
        {
            t++;
            m = 0;
        }
        _currentTimer = ((t * 60) + m) * 60 + s;
    }

    private IEnumerator Decorate()
    {
        yield return new WaitForSeconds(0f);
        CurrentPlayer = Instantiate(m_Player, m_Player.transform.position, Quaternion.identity);
        GameManager.Instance.m_Player = CurrentPlayer;

        GameManager.Instance.PlayerSpawnTriggerEvent();
        onPlayerDead = OnPlayerDead;
        EventManager.StartListening(EventList.PlayerDeadEvent, onPlayerDead);
    }

    public void OnPlayerDead()
    {
        _playerLifeCount--;
        if (_playerLifeCount > 0)
        {
            StartCoroutine(Decorate());
            EventManager.StopListening(EventList.PlayerDeadEvent, onPlayerDead);
        }
        else
        {
            GameManager.Instance.Pause();
            GameOver();
            EventManager.StopListening(EventList.PlayerDeadEvent, onPlayerDead);
        }
    }

    public void GameOver()
    {
        GameManager.Instance.GameOver = true;
        GameManager.Instance.GameOverTriggerEvent();
    }

    public void GameWin()
    {
        GameManager.Instance.Pause();
        EventManager.StopListening(EventList.PlayerDeadEvent, onPlayerDead);
        GameManager.Instance.GameWin = true;
        GameManager.Instance.EndTimer = true;
        _endTimer = string.Format("{0:D2}:{1:D2}:{2:D2}", t, m, s);
        data.endTimer = _endTimer;
        data.minTimer = _currentTimer;
        AddHistoryTimer();
        GameManager.Instance.GameOverTriggerEvent();
    }

    private void AddHistoryTimer()
    {
        if (CurrentTimer <= 0) return;

        if (data.LeaderboardDatas.Count >= 10)
        {
            for (int i = 0; i < data.LeaderboardDatas.Count; i++)
            {
                if (CurrentTimer > data.LeaderboardDatas[i].timer)
                {
                    LeaderboardData leaderboardData = new LeaderboardData();
                    leaderboardData.endtimer = data.endTimer;
                    leaderboardData.timer = data.minTimer;
                    leaderboardData.name = data.playerName;
                    data.LeaderboardDatas.Add(leaderboardData);
                    break;
                }
            }
            if (data.LeaderboardDatas.Count > 10)
                data.LeaderboardDatas.RemoveAt(data.LeaderboardDatas.Count - 2);
        }
        else
        {
            LeaderboardData leaderboardData = new LeaderboardData();
            leaderboardData.endtimer = data.endTimer;
            leaderboardData.timer = data.minTimer;
            leaderboardData.name = data.playerName;
            data.LeaderboardDatas.Add(leaderboardData);
        }
    }
}
