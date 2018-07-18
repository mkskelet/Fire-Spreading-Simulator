using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class handles all the calls from UI and updates it based on callbacks defined in IUserInterface.
/// </summary>
public class SidePanel : IUserInterface {
    Animator animator;

    // PLAYBACK
    public GameObject[] playGraphics;
    public GameObject[] pauseGraphics;
    public GameObject[] stopGraphics;
    public Text[] playbackSpeedText;
    public Slider playbackSpeedSlider;

    // WIND
    public RectTransform[] windDirectionIndicators;
    public Text[] windSpeedText;
    public Slider windDirectionSlider;
    public Slider windSpeedSlider;

    // MODE
    public Slider selectionSizeSlider;
    public Text[] selectionSizeText;
    public Image[] modeButton;
    public Image[] modeIndicator;
    public Image selectionCircle;

    void Start () {
        animator = GetComponent<Animator>();

        // register callbacks
        Simulation.onPlaybackStateChanged += OnPlaybackStateChanged;
        Wind.onWindChanged += OnWindChanged;
        Cursor.onCursorModeChanged += OnCursorModeChanged;
        ActionManager.onActionModeChanged += OnActionModeChanged;

        // update UI
        OnPlaybackStateChanged(Simulation.PlayState, Simulation.PlaybackSpeed);
        OnWindChanged(Wind.WindSpeed, Wind.WindDirection);
        OnCursorModeChanged(Cursor.CursorMode, Cursor.SelectionSize);
        OnActionModeChanged(ActionManager.ActionMode);
    }

    #region events
    protected override void OnPlaybackStateChanged(PlaybackState playbackState, float playbackSpeed) {
        SetPlaybackState(playbackState);
        SetPlaybackSpeed(playbackSpeed);
        playbackSpeedSlider.value = (playbackSpeed - Simulation.MIN_PLAYBACK_SPEED) / (Simulation.MAX_PLAYBACK_SPEED - Simulation.MIN_PLAYBACK_SPEED);
    }

    protected override void OnWindChanged(float windSpeed, float windDirection) {
        SetWindDirection(windDirection);
        SetWindSpeed(windSpeed);
        windDirectionSlider.value = windDirection / 360;
        windSpeedSlider.value = windSpeed / Wind.MAX_WIND_SPEED;
    }

    protected override void OnCursorModeChanged(CursorMode cursorMode, float selectionSize) {
        SetSelectionSize(selectionSize);
        SetCursorMode(cursorMode);
        selectionSizeSlider.value = (selectionSize - Cursor.MIN_SELECTION_SIZE) / (Cursor.MAX_SELECTION_SIZE - Cursor.MIN_SELECTION_SIZE);
    }

    protected override void OnActionModeChanged(ActionMode actionMode) {
        SetActionMode(actionMode);
    }
    #endregion

    #region Methods accessible from UI
    public void Close () {
        animator.SetBool("open", false);
    }

    public void Open() {
        animator.SetBool("open", true);
    }

    public void Play() {
        Simulation.PlayState = PlaybackState.play;
    }

    public void Pause() {
        Simulation.PlayState = PlaybackState.pause;
    }

    public void Stop() {
        Simulation.PlayState = PlaybackState.stop;
        Grass.ResetAllGrass();
    }

    public void UpdateWindDirection() {
        Wind.WindDirection = windDirectionSlider.value * 360;
    }

    public void UpdateWindSpeed() {
        Wind.WindSpeed = windSpeedSlider.value * Wind.MAX_WIND_SPEED;
    }

    public void RandomWind() {
        Wind.Instance.SetRandomWind();
    }

    public void UpdatePlaybackSpeed() {
        Simulation.PlaybackSpeed = Simulation.MIN_PLAYBACK_SPEED + (playbackSpeedSlider.value * (Simulation.MAX_PLAYBACK_SPEED - Simulation.MIN_PLAYBACK_SPEED));
    }

    public void UpdateSelectionSize() {
        Cursor.SelectionSize = Cursor.MIN_SELECTION_SIZE + (selectionSizeSlider.value * (Cursor.MAX_SELECTION_SIZE - Cursor.MIN_SELECTION_SIZE));
        Cursor.CursorMode = CursorMode.circle;
    }

    public void UpdateCursorMode() {
        if (Cursor.CursorMode == CursorMode.point) Cursor.CursorMode = CursorMode.circle;
        else Cursor.CursorMode = CursorMode.point;
    }

    public void UpdateActionMode(int selectedMode) {
        ActionManager.ActionMode = (ActionMode)selectedMode;
    }

    public void QuitApplication() {
        Application.Quit();
    }

    public void GenerateGrass() {
        Grass.GenerateGrass();
    }

    public void ClearGrass() {
        Grass.ClearAllGrass();
    }

    public void SetOnFire() {
        float grassPercentageToLight = Random.Range(5, 15);
        
        for (int i = 0; i < ObjectPool.RentedObjects.Count; ++i) {
            if (Random.Range(0, 100) < grassPercentageToLight)
                ObjectPool.RentedObjects[i].GetComponent<Grass>().SetOnFire();
        }
    }
    #endregion

    #region Methods to change UI elements
    void SetActionMode(ActionMode actionMode) {
        foreach (Image i in modeButton) {
            i.enabled = false;
        }
        foreach (Image i in modeIndicator) {
            i.enabled = false;
        }

        modeButton[(int)actionMode].enabled = true;
        modeIndicator[(int)actionMode].enabled = true;
    }

    void SetPlaybackState(PlaybackState ps) {
        foreach (GameObject g in playGraphics) {
            g.SetActive(false);
        }
        foreach (GameObject g in pauseGraphics) {
            g.SetActive(false);
        }
        foreach (GameObject g in stopGraphics) {
            g.SetActive(false);
        }

        switch(ps) {
            case PlaybackState.play:
                foreach (GameObject g in playGraphics) {
                    g.SetActive(true);
                }
                break;
            case PlaybackState.pause:
                foreach (GameObject g in pauseGraphics) {
                    g.SetActive(true);
                }
                break;
            case PlaybackState.stop:
                foreach (GameObject g in stopGraphics) {
                    g.SetActive(true);
                }
                break;
            default:
                break;
        }
    }

    void SetWindDirection(float windDirection) {
        foreach(RectTransform rt in windDirectionIndicators) {
            rt.rotation = Quaternion.Euler(0, 0, windDirection);
        }
    }

    void SetWindSpeed(float windSpeed) {
        foreach(Text t in windSpeedText) {
            int ws = (int)(windSpeed * 10);
            t.text = (float)ws/10 + " m/s";
        }
    }

    void SetPlaybackSpeed(float playbackSpeed) {
        foreach (Text t in playbackSpeedText) {
            int ps = (int)(playbackSpeed * 10);
            t.text = (float)ps/10 + "x";
        }
    }

    void SetSelectionSize(float selectionSize) {
        foreach (Text t in selectionSizeText) {
            int ss = (int)(selectionSize * 10);
            t.text = (float)ss / 10 + " m";
        }
    }

    void SetCursorMode(CursorMode cursorMode) {
        if (Cursor.CursorMode == CursorMode.point) {
            foreach (Text t in selectionSizeText)
                t.text = "POINT";
        }
        else {
            SetSelectionSize(Cursor.SelectionSize);
        }
    }
    #endregion
}
