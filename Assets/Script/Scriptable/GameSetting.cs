using UnityEngine;

[CreateAssetMenu(fileName = "GameSetting", menuName = "CreateScriptable/GameSetting", order = 2)]
public class GameSetting : ScriptableObject {
    public float musicValume; /*音乐的值*/
    public float effectValume; /*音效的值*/
    public float mainValume;
}
