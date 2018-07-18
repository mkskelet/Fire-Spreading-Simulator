using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Action for placing grass.
/// Uses ObjectPool.cs
/// </summary>
public class PlaceGrassAction : BaseAction {
    public PlaceGrassAction() {
        circleCursorEnabled = true;
    }

    /// <summary>
    /// Places grass at cursor position.
    /// </summary>
    /// <param name="cursorMode"></param>
    /// <param name="selectionSize"></param>
    public override void OnMouseClick(CursorMode cursorMode, float selectionSize) {
        RaycastHit hit;
        
        switch (cursorMode) {
            case CursorMode.point:
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit)) {
                    if (hit.transform.CompareTag("Terrain"))
                        ObjectPool.Instantiate(hit.point);
                }
                break;
            case CursorMode.circle:
                int density = (int)selectionSize;

                for (int i = 0; i < density; ++i) {
                    Vector2 position = Random.insideUnitCircle * selectionSize/2;
                    Ray cRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                    cRay.origin = new Vector3(cRay.origin.x + position.x, cRay.origin.y, cRay.origin.z + position.y);

                    if (Physics.Raycast(cRay, out hit)) {
                        if(hit.transform.CompareTag("Terrain"))
                            ObjectPool.Instantiate(hit.point);
                    }
                }
                break;
            default:
                break;
        }
    }
}
