using UnityEngine;

//Generalized input, handles multiple buttons and axis + keyboard
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
        
#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        //Use dpad input if no keys or joy used
        if (Mathf.Approximately(x, 0f))
            x = Input.GetAxis("Horizontal (dpad)");
        if (Mathf.Approximately(y, 0f))
            y = Input.GetAxis("Vertical (dpad)");

#elif UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
        //Left
        if (Input.GetKeyDown("joystick button 7"))
            x = 1f;
        //Right
        if (Input.GetKeyDown("joystick button 8"))
            x = 1f;
        //Up
        if (Input.GetKeyDown("joystick button 5"))
            y = 1f;
        //Down
        if (Input.GetKeyDown("joystick button 6"))
            y = 1f;
#endif

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
        if (!enter)
            enter = Input.GetKeyDown("joystick button 17");
#endif
        return cancel;
    }
}
