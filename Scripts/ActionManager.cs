using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Class holds list of all IAction instances and calls their OnMouseClick methods.
/// </summary>
public class ActionManager : MonoBehaviour {
    static ActionMode actionMode = ActionMode.view;
    public static ActionMode ActionMode {
        get { return actionMode; }
        set {
            actionMode = value;
            onActionModeChanged(actionMode);
        }
    }

    public delegate void OnActionModeChanged(ActionMode actionMode);
    public static OnActionModeChanged onActionModeChanged;

    public List<IAction> action = new List<IAction>();

    void Start() {
        action.Add(new ViewAction());
        action.Add(new PlaceGrassAction());
        action.Add(new EraseAction());
        action.Add(new SetOnFireAction());
        action.Add(new ExtinguishAction());
    }

    void Update() {
        // only continue on mouse click
        if (!Input.GetMouseButtonDown(0))
            return;

        // check if click hit UI
        if (EventSystem.current.currentSelectedGameObject != null || EventSystem.current.IsPointerOverGameObject())
            return;

        action[(int)ActionMode].OnMouseClick(Cursor.CursorMode, Cursor.SelectionSize);
    }
}

public enum ActionMode {
    view,
    placeGrass,
    removeGrass,
    setOnFire,
    extinguish
}
