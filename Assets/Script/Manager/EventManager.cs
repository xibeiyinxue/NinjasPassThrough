using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;


public class EventList
{
    public const string GameStartEvent = "GameStartEvent";
    public const string GameOverEvent = "GameOverEvent";
    public const string PlayerDeadEvent = "PlayerDeadEvent";
    public const string PlayerSpawnEvent = "PlayerSpawnEvent";
}

public class EventManager : PersistentSingleton<EventManager>
{
    private static Dictionary<string, UnityEvent> eventDictionary = new Dictionary<string, UnityEvent>(); /*定义一个 EventDictionary 字典*/

    void Init()
    {
        if (eventDictionary == null) /*如果字典为空*/
            eventDictionary = new Dictionary<string, UnityEvent>(); /*则新建一个字典*/
    }

    public static void StartListening(string eventName, UnityAction listener) /*表示对一个事件的监听写入*/
    {
        UnityEvent thisEvent = null; /*初始时该事件为空*/
        if (eventDictionary.TryGetValue(eventName, out thisEvent)) /*Key 引用 EventList 的类型，Value 引用需要监听的事件*/
        {
            thisEvent.AddListener(listener); /*将该事件加入到监听列表*/
        }
        else
        {
            thisEvent = new UnityEvent(); /*事件没有命名，则此该事件自己新立名*/
            thisEvent.AddListener(listener); /*将该事件加入到监听列表*/
            eventDictionary.Add(eventName, thisEvent); /*将该监听列表加入到字典中*/
        }
    }

    public static void StopListening(string eventName, UnityAction listener) /*表示对一个事件的监听停止*/
    {
        UnityEvent thisEvent = null;
        if (eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.RemoveListener(listener); /*将该事件初始化*/
        }
    }

    public static void TriggerEvent(string eventName) /*触发该事件， eventName 中，即 Key （钥匙）中储存了哪些事件，将这些事件都触发*/
    {
        UnityEvent thisEvent = null;
        if (eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(); /*调用事件*/
        }
    }

    public static void ClearEvent(string eventName)
    {
        UnityEvent thisEvent = null;
        if (eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            eventDictionary.Remove(eventName);
        }
    }

    public static void ClearAllEvents()
    {
        eventDictionary.Clear();
    }
}