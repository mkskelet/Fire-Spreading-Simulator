using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class handles action of extinguishing grass that is on fire.
/// </summary>
public class ExtinguishAction : BaseAction {
    public ExtinguishAction() {
        circleCursorEnabled = true;
    }

    /// <summary>
    /// Method calls Extinguish() method of all the grass under mouse cursor(selection circle).
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
                        hit.transform.GetComponent<Grass>().Extinguish();
                }
                break;
            case CursorMode.circle:
                Ray cRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] grass = Physics.SphereCastAll(cRay, selectionSize / 2);

                foreach (RaycastHit r in grass) {
                    if (r.transform.tag == "Grass")
                        r.transform.GetComponent<Grass>().Extinguish();
                }
                break;
            default:
                break;
        }
    }
}
