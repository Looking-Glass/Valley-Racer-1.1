﻿using System;
using UnityEngine;

public class EventManager : ScriptableObject
{
    public static Action playerDeath;
    public static Action playerPressedStart;
    public static Action gameStart;
}