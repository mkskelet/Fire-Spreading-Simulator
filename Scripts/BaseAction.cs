using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for all action classes, implements IAction interface.
/// Class contains abstract OnMouseClick(CursorMode cursorMode, float selectionSize) method for handling mouse click.
/// </summary>
public abstract class BaseAction : IAction {
    protected bool circleCursorEnabled = false;

    public bool CircleCursorEnabled() {
        return circleCursorEnabled;
    }

    public abstract void OnMouseClick(CursorMode cursorMode, float selectionSize);
}
