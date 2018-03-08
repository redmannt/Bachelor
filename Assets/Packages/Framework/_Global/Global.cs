﻿/// <summary>
/// Global access
/// </summary>
public static class Global { }

/// <summary>
/// The state the game is in
/// </summary>
public enum GAME_STATE
{
    DEFAULT = -1
}

/// <summary>
/// The type of log written to the console/file
/// </summary>
public enum LOG_TYPE
{
    LOG,
    WARNING,
    ERROR,
    ASSERT
}