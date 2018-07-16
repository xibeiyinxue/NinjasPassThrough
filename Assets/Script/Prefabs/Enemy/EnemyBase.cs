using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class EnemyBase : MonoBehaviour, IHealth {
    #region 怪物基础设置
    protected Transform playerTrans; /*怪物会进行攻击的人物*/
    protected float m_DetectionRange = 25; /*怪物的探测范围*/
    protected float m_AttackRange = 8; /*怪物的攻击范围*/
    protected EnemyAttack m_Bullet; /*怪物的攻击精灵*/
    protected float m_CD = 4f; /*怪物的冷却时间*/
    protected float distance; /*创建一个表示范围的值*/

    protected bool m_StepOn = false; /*踩踏*/
    protected bool m_AttackBool = false;
    protected bool m_Sober = false; /*苏醒*/
    protected bool m_Die = false;

    private Vector3 m_ReTrans;

    private UnityAction onGameStart;
    private UnityAction onPlayerSpaw; /*玩家生成事件*/

    protected int health = 20; /*基础生命值*/
    public int Health { get { return health; } protected set { health = value; } }

    public virtual void Damage(int val, GameObject initiator)
    {
        Health -= val;
    }

    #endregion

    private void OnEnable()
    {
        onPlayerSpaw = OnPlayerSpaw;
        EventManager.StartListening(EventList.PlayerSpawnEvent, onPlayerSpaw); /*开始对玩家生成的事件进行监听*/
        onGameStart = OnGameStart;
        EventManager.StartListening(EventList.GameStartEvent, onGameStart); /*开始对游戏开始的事件进行监听*/
    }
    void OnDisable()
    {
        EventManager.StopListening(EventList.PlayerSpawnEvent, onPlayerSpaw); /*关闭对玩家生成事件的监听*/
        EventManager.StopListening(EventList.GameStartEvent, onGameStart); /*关闭对游戏开始事件的监听*/
    }
    private void OnPlayerSpaw()
    {
        playerTrans = GameManager.Instance.m_Player.transform; /*目标的位置由游戏管理者中的玩家位置决定*/
    }

    private void OnGameStart()
    {
        Init();
    }

    protected void Awake()
    {
        Vector3 vec3 = transform.position;
        m_ReTrans = vec3;
    } 

    protected void Init()
    {
        transform.position = m_ReTrans;
        if (gameObject.activeSelf == false)
        {
            gameObject.SetActive(true);
        }
    }
}
