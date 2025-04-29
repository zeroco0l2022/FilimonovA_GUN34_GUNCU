using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum NeighbourType
{
    None = 0,
    Left = 1 << 0,
    Right = 1 << 1,
    Top = 1 << 2,
    Bottom = 1 << 3,
    LeftTop = 1 << 4,
    LeftBottom = 1 << 5,
    RightTop = 1 << 6,
    RightBottom = 1 << 7,
    All = Left | Right | Top | Bottom | LeftTop | LeftBottom | RightTop | RightBottom
}

public enum Team
{
    Player1,
    Player2
}
