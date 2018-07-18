using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Action used to set grass on fire.
/// Uses Grass.cs
/// </summary>
public class SetOnFireAction : BaseAction {
    public SetOnFireAction() {
        circleCursorEnabled = true;
    }

    /// <summary>
    /// Sets grass under mouse cursor on fire.
    /// </summary>
    /// <param name="cursorMode"></param>
    /// <param name="selectionSize"></param>
    public override void OnMouseClick(CursorMode cursorMode, float selectionSize) {
        RaycastHit hit;

        switch (cursorMode) {
            case CursorMode.point:
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit)) {
                    Debug.Log(hit.transform.tag);
                    if (hit.transform.CompareTag("Grass"))
                        hit.transform.GetComponent<Grass>().SetOnFire();
                }
                break;
            case CursorMode.circle:
                Ray cRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] grass = Physics.SphereCastAll(cRay, selectionSize / 2);

                foreach (RaycastHit r in grass) {
                    if (r.transform.tag == "Grass")
                        r.transform.GetComponent<Grass>().SetOnFire();
                }
                break;
            default:
                break;
        }
    }
}
