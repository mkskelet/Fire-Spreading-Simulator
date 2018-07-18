using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class holds cursor information, cursor mode and size of selection circle.
/// Other classes can add their methods to call when OnCursorModeChanged is called.
/// </summary>
public class Cursor : MonoBehaviour {
    public const float MIN_SELECTION_SIZE = 1;
    public const float MAX_SELECTION_SIZE = 25;

    static CursorMode cursorMode = CursorMode.point;
    public static CursorMode CursorMode {
        get { return cursorMode; }
        set {
            cursorMode = value;
            onCursorModeChanged(cursorMode, selectionSize);
        }
    }

    static float selectionSize = 1;
    public static float SelectionSize {
        get { return selectionSize; }
        set {
            selectionSize = value;
            onCursorModeChanged(cursorMode, selectionSize);
        }
    }

    public delegate void OnCursorModeChanged(CursorMode cursorMode, float selectionSize);
    public static OnCursorModeChanged onCursorModeChanged;
}

public enum CursorMode {
    point,
    circle
}
