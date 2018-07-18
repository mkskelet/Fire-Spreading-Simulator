using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class holds information about wind and calls OnWindChanged methods.
/// </summary>
public class Wind : MonoBehaviour {
    public static Wind Instance = null;
    public const float MAX_WIND_SPEED = 50;
    
    static float windDirection = 0;
    public static float WindDirection {
        get { return windDirection; }
        set {
            windDirection = value;
            onWindChanged(windSpeed, windDirection);
        }
    }

    static float windSpeed = 0;
    public static float WindSpeed {
        get { return windSpeed; }
        set {
            windSpeed = value;
            onWindChanged(windSpeed, windDirection);
        }
    }

    public delegate void OnWindChanged(float windSpeed, float windDirection);
    public static OnWindChanged onWindChanged;

    void Start () {
        if (!Instance)
            Instance = this;
        else enabled = false;

        onWindChanged += OWC;
	}

    /// <summary>
    /// Changes rotation of this object so we can use transform.right as direction vector.
    /// </summary>
    /// <param name="windSpeed"></param>
    /// <param name="windDirection"></param>
    void OWC(float windSpeed, float windDirection) {
        transform.rotation = Quaternion.Euler(0, -windDirection, 0);
    }

    #region Public methods
    /// <summary>
    /// Returns vector that is defined by this objects rotation.
    /// </summary>
    /// <returns></returns>
    public Vector3 GetWindDirectionVector() {
        return transform.right;
    }
    
    public void SetRandomWind() {
        WindDirection = Random.Range(0f, 360f);
        WindSpeed = Random.Range(0, MAX_WIND_SPEED);
    }
    #endregion
}
