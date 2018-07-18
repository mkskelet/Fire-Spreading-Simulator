using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class takes care of erasing grass placed on the ground, inherits from BaseAction class.
/// Class uses ObjectPool.cs.
/// </summary>
public class EraseAction : BaseAction {
    public EraseAction() {
        circleCursorEnabled = true;
    }

    /// <summary>
    /// Removes grass at cursor position (or inside selection circle depending on cursor mode) by returning grass object to ObjectPool.
    /// </summary>
    /// <param name="cursorMode"></param>
    /// <param name="selectionSize"></param>
    public override void OnMouseClick(CursorMode cursorMode, float selectionSize) {
        RaycastHit hit;

        switch (cursorMode) {
            case CursorMode.point:
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit)) {
                    if (hit.transform.CompareTag("Grass"))
                        ObjectPool.Return(hit.transform.gameObject);
                }
                break;
            case CursorMode.circle:
                Ray cRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] grass = Physics.SphereCastAll(cRay, selectionSize / 2);

                foreach(RaycastHit r in grass) {
                    if(r.transform.tag == "Grass")
                        ObjectPool.Return(r.transform.gameObject);
                }
                break;
            default:
                break;
        }
    }
}
