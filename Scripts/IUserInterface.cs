using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base interface of UI.
/// Contains methods which can be used to register as callbacks when variables that should be displayed on UI are changed.
/// </summary>
public abstract class IUserInterface : MonoBehaviour {
    protected abstract void OnPlaybackStateChanged(PlaybackState playbackState, float playbackSpeed);   // Simulation.cs
    protected abstract void OnWindChanged(float windSpeed, float windDirection);                        // Wind.cs
    protected abstract void OnCursorModeChanged(CursorMode cursorMode, float selectionSize);            // Cursor.cs
    protected abstract void OnActionModeChanged(ActionMode actionMode);                                 // ActionManager.cs
}
