using UnityEngine;

/// <summary>
/// Generalized input, handles multiple buttons and axis + keyboard
/// </summary>
public class ValleyInput : ScriptableObject
{
    public static Vector2 GetAxis()
    {
        var x = 0f;
        var y = 0f;

        //Left
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            x -= 1f;
        //Right
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            x += 1f;
        //Up
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            y += 1f;
        //Down
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            y -= 1f;

        //Use joystick input if no keys used
        if (Mathf.Approximately(x, 0f))
            x = Input.GetAxis("Horizontal");
        if (Mathf.Approximately(y, 0f))
            y = Input.GetAxis("Vertical");

        var axis = new Vector2(x, y);
        return axis;
    }
    
    public static bool GetEnterButtonDown()
    {
        var enter = Input.GetKeyDown(KeyCode.Return);
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        if (!enter)
            enter = Input.GetKeyDown("joystick button 0");
#elif UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
        if (!enter)
            enter = Input.GetKeyDown("joystick button 16");
#endif
        return enter;
    }

    public static bool GetCancelButtonDown()
    {
        var cancel = Input.GetKeyDown(KeyCode.Backspace);
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        if (!cancel)
            cancel = Input.GetKeyDown("joystick button 1");
#elif UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
        if (!cancel)
            cancel = Input.GetKeyDown("joystick button 17");
#endif
        return cancel;
    }

    public static bool GetMuteButtonDown()
    {
        var mute = Input.GetKeyDown(KeyCode.M);
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        if (!mute)
            mute = Input.GetKeyDown("joystick button 3");
#elif UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
        if (!mute)
            mute = Input.GetKeyDown("joystick button 19");
#endif
        return mute;
    }
}
