using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class defines behaviour of grass objects and grass in general.
/// Class accesses Simulation.cs, ObjectPool.cs
/// </summary>
public class Grass : MonoBehaviour {
    const float MIN_FIRE_SPREAD_TIME = .3f;
    const float MAX_FIRE_SPREAD_TIME = 7.5f;
    const float MIN_RESISTANCE_TIME = 5;
    const float MAX_RESISTANCE_TIME = 10;
    const float MAX_LOCAL_SPREAD_RADIUS = 3;
    const float MIN_LOCAL_SPREAD_RADIUS = 2;
    const float DISTANT_SPREAD_RADIUS = 1.5f;
    const float LOCAL_SPREAD_CUTOFF_WIND_SPEED = 30;
    const float SPREAD_CHANCE = 20;

    GrassState grassState = GrassState.normal;  
    public Material[] materials;                // should contain 3 materials (normal, burning, burnt)
    public Renderer model;                      // grass model

    // grass parameters
    float resistance;                           // time until plant burns
    float nextSpread;                           // time until next spread

    /// <summary>
    /// Reset grass state, material and resistance.
    /// </summary>
    public void Start() {
        grassState = GrassState.normal;
        UpdateGrassMaterial();
        resistance = Random.Range(MIN_RESISTANCE_TIME, MAX_RESISTANCE_TIME);
    }

    void Update () {
		if(grassState == GrassState.burning && Simulation.PlayState == PlaybackState.play) {
            resistance -= Time.deltaTime * Simulation.PlaybackSpeed;
            nextSpread -= Time.deltaTime * Simulation.PlaybackSpeed;

            if(nextSpread <= 0) {
                SpreadFire();
                nextSpread = Random.Range(MIN_FIRE_SPREAD_TIME, MAX_FIRE_SPREAD_TIME);
            }

            if(resistance <= 0) {
                grassState = GrassState.burnt;
                UpdateGrassMaterial();
            }
        }
	}

    void UpdateGrassMaterial() {
        model.sharedMaterial = materials[(int)grassState];
    }

    void SpreadFire() {
        // spread fire in local area if wind isn't very strong
        if (Wind.WindSpeed < LOCAL_SPREAD_CUTOFF_WIND_SPEED) {
            float windSpeedRatio = Wind.WindSpeed / LOCAL_SPREAD_CUTOFF_WIND_SPEED;
            LocalSpread(Wind.Instance.GetWindDirectionVector() * windSpeedRatio * MAX_LOCAL_SPREAD_RADIUS/2, Mathf.Lerp(MAX_LOCAL_SPREAD_RADIUS, MIN_LOCAL_SPREAD_RADIUS, windSpeedRatio));
        }
        // spread fire further in direction of the wind
        DistantSpread();    
    }

    /// <summary>
    /// Spreads fire in close area around the grass.
    /// </summary>
    /// <param name="center"></param>
    /// <param name="radius"></param>
    void LocalSpread(Vector3 center, float radius) {
        Collider[] colliders = Physics.OverlapSphere(transform.position + center, radius);

        foreach(Collider c in colliders) {
            if(c.CompareTag("Grass")) {
                if (Random.Range(0, 100) < SPREAD_CHANCE) {
                    c.GetComponent<Grass>().SetOnFire();
                }
            }
        }
    }

    /// <summary>
    /// Spreads fire further in line determined by fire direction.
    /// </summary>
    void DistantSpread() {
        Ray ray = new Ray(transform.position, Wind.Instance.GetWindDirectionVector());
        RaycastHit[] hit = Physics.SphereCastAll(ray, DISTANT_SPREAD_RADIUS, Wind.WindSpeed/10);

        foreach (RaycastHit h in hit) {
            if (h.transform.CompareTag("Grass")) {
                if (Random.Range(0, 100) < SPREAD_CHANCE) {
                    h.transform.GetComponent<Grass>().SetOnFire();
                }
            }
        }
    }

    #region Instance methods
    /// <summary>
    /// Sets this instance of grass on fire.
    /// </summary>
    public void SetOnFire() {
        if (grassState != GrassState.burnt) {
            nextSpread = Random.Range(MIN_FIRE_SPREAD_TIME, MAX_FIRE_SPREAD_TIME);
            grassState = GrassState.burning;
            UpdateGrassMaterial();
        }
    }

    /// <summary>
    /// Extinguishes fire on this instance of grass.
    /// </summary>
    public void Extinguish() {
        if(grassState == GrassState.burning) {
            grassState = GrassState.normal;
            UpdateGrassMaterial();
        }
    }
    #endregion

    #region Static grass methods
    /// <summary>
    /// Method generates grass in the vision of main camera.
    /// </summary>
    public static void GenerateGrass() {
        int count = Random.Range(1000, 2500);
        for(int i = 0; i < count; ++i) {
            float screenX = Random.Range(0, Screen.width);
            float screenY = Random.Range(0, Screen.height);

            Ray ray = Camera.main.ScreenPointToRay(new Vector3(screenX, screenY, 0));
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit)) {
                ObjectPool.Instantiate(hit.point);
            }
        }
    }

    /// <summary>
    /// Method returns all rented grass objects to ObjectPool.
    /// </summary>
    public static void ClearAllGrass() {
        while(ObjectPool.RentedObjects.Count > 0)
            ObjectPool.Return(ObjectPool.RentedObjects[ObjectPool.RentedObjects.Count-1]);
    }

    /// <summary>
    /// Method resets all rented grass objects in the scene.
    /// </summary>
    public static void ResetAllGrass() {
        Grass[] grass = FindObjectsOfType<Grass>();

        for (int i = 0; i < ObjectPool.RentedObjects.Count; ++i) {
            ObjectPool.RentedObjects[i].GetComponent<Grass>().Start();
        }
    }
    #endregion
}

enum GrassState {
    normal,
    burning,
    burnt
}
