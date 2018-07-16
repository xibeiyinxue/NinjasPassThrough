using UnityEngine;

[CreateAssetMenu(fileName = "InputKeyCodeSet", menuName = "CreateScriptable/InputKeyCodeSet", order = 3)]
public class InputKeyCodeSetting : ScriptableObject
{
    public KeyCode _left = KeyCode.LeftArrow; /*Buttons[0]*/
    public KeyCode _right = KeyCode.RightArrow; /*Buttons[1]*/
    public KeyCode _jump = KeyCode.Space; /*Buttons[2]*/
    public KeyCode _attack = KeyCode.Z; /*Buttons[3]*/
}