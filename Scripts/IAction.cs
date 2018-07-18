using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base interface of action classes.
/// </summary>
public interface IAction {
    bool CircleCursorEnabled();
    void OnMouseClick(CursorMode cursorMode, float selectionSize);
}
