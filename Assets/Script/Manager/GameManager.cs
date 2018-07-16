using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class GameManager : PersistentSingleton<GameManager> {

    public Player m_Player { get; set; }
    public int GameFrameRate = 300; /*设计游戏帧数，优化游戏*/
    public float TimeScale { get; private set; }
    private bool _paused;
    public bool Paused { get { return _paused; } set { _paused = value; } }
    private bool _gameWin;
    public bool GameWin { get { return _gameWin; } set { _gameWin = value; } }
    private bool _gameOver;
    public bool GameOver { get { return _gameOver; } set { _gameOver = value; } }
    public bool EndTimer { get; set; }

    private float savedTimeScale = 1f; /*保存时间标度*/

    #region Event

    void Start () {
        Application.targetFrameRate = GameFrameRate; /*赋游戏帧数*/
        GameManagerInit();
	}

    public void GameStartTriggerEvent()
    {
        EventManager.TriggerEvent(EventList.GameStartEvent);
    }
    public void GameOverTriggerEvent()
    {
        EventManager.TriggerEvent(EventList.GameOverEvent);
    }
    public void PlayerSpawnTriggerEvent()
    {
        EventManager.TriggerEvent(EventList.PlayerSpawnEvent);
    }
    public void PlayerDeadTriggerEvent()
    {
        EventManager.TriggerEvent(EventList.PlayerDeadEvent);
    }
    #endregion

    public void GameManagerInit()
    {
        TimeScale = 1f;
        Paused = false;
        GameWin = false;
        GameOver = false;
        EndTimer = false;
    }

    public virtual void Pause() /*暂停方法*/
    {
        if (Time.timeScale > 0.0f) /*如果时间大于 0*/
        {
            Instance.SetTimeScale(0.0f); /*暂停方法成功启用时存储时间方法，并将场景运行时间改为 0 ，实现暂停功能*/
            instance.Paused = true; /*暂停 = true*/
        }
        else
            UnPause();
    }

    public virtual void UnPause() /*取消暂停*/
    {
        instance.ResetTimeScale(); /*取消暂停时，调用恢复时间方法*/
        instance.Paused = false;
    }

    public void SetTimeScale(float newTimeScale) /*存储时间方法*/
    {
        savedTimeScale = Time.timeScale; /*将现在的场景时间存储在 SavedTimeScale 的变量中*/
        Time.timeScale = newTimeScale; /*赋予场景以新时间，以上面调用方法为例 “0.0” 即将时间类似于关闭的意思*/
        TimeScale = newTimeScale;
    }

    public void ResetTimeScale() /*恢复时间方法*/
    {
        Time.timeScale = savedTimeScale; /*将存储在 SavedTimeScale 的时间，重新赋还给场景时间*/
        TimeScale = savedTimeScale;
    }
}
