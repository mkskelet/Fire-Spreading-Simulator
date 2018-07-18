using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Class defines behaviour of SelectionCircle UI element.
/// Containts OnCursorModeChanged method for changing cursor mode.
/// Uses Cursor.cs
/// </summary>
public class SelectionCircle : MonoBehaviour {
    bool showCircle = false;

    float pixelsPerUnit = 25;

    RectTransform rectTransform;
    Image image;

    void Start() {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();

        Cursor.onCursorModeChanged += OnCursorModeChanged;

        // calculate pixels per unit
        RaycastHit hit, hit1;
        Ray ray = Camera.main.ScreenPointToRay(Vector3.zero);
        if (Physics.Raycast(ray, out hit)) {

        }
        ray = Camera.main.ScreenPointToRay(Vector3.right);
        if (Physics.Raycast(ray, out hit1)) {

        }
        pixelsPerUnit = 1/(hit1.point.x - hit.point.x);
    }

    /// <summary>
    /// Changes selection circle size and updates cursor mode.
    /// </summary>
    /// <param name="cursorMode"></param>
    /// <param name="selectionSize"></param>
    void OnCursorModeChanged(CursorMode cursorMode, float selectionSize) {
        if (cursorMode == CursorMode.circle)
            showCircle = true;
        else showCircle = false;

        rectTransform.sizeDelta = new Vector2(selectionSize * pixelsPerUnit, selectionSize * pixelsPerUnit);
    }

    void Update () {
        if(showCircle)
            rectTransform.position = Input.mousePosition;       // updates selection circle position

        // shows (hides) cursor and selection circle respectively
        if ((!UnityEngine.Cursor.visible || image.enabled) &&
            (EventSystem.current.currentSelectedGameObject != null || EventSystem.current.IsPointerOverGameObject() || !showCircle)) {
            UnityEngine.Cursor.visible = true;
            image.enabled = false;
        }
        else if((UnityEngine.Cursor.visible || !image.enabled) &&
            (EventSystem.current.currentSelectedGameObject == null && !EventSystem.current.IsPointerOverGameObject()) &&
            showCircle) {
            UnityEngine.Cursor.visible = false;
            image.enabled = true;
        }
    }
}
