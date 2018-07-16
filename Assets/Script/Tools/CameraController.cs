using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraController : MonoBehaviour
{
    private Transform target; /*目标*/
    [SerializeField]
    private Vector3 offset; /*移动的位置*/

    //private AudioSource m_Audio;

    private UnityAction onPlayerSight;

    //private void Awake()
    //{
    //    m_Audio = GetComponent<AudioSource>();
    //}

    private void Start()
    {
        //if (m_Audio != null)
        //{
        //    m_Audio.Play();
        //}
    }

    void OnEnable()
    {
        onPlayerSight = OnPlayerSight;
        EventManager.StartListening(EventList.PlayerSpawnEvent, onPlayerSight); /*将跟随玩家移动的方法写入玩家生成的监听*/
    }
    void OnDisable()
    {
        EventManager.StopListening(EventList.PlayerSpawnEvent, onPlayerSight); /*取消对玩家生成的监听*/
    }

    private void LateUpdate()
    {
        if (target == null) return;
        transform.position = target.position - offset; /*自身的相对位置等于目标位置减去移动的位置*/
    }

    private void OnPlayerSight()
    {
        target = GameManager.Instance.m_Player.transform; /*目标为游戏管理者生成的玩家*/
    }
}
