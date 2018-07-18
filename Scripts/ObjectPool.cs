using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class takes care of pooling grass objects.
/// Instantiates poolSize objects at start of the game so that we don't have to instantiate them during simulation.
/// </summary>
public class ObjectPool : MonoBehaviour {
    public static ObjectPool Instance = null;
    static List<GameObject> pooledObjects = new List<GameObject>();
    static List<GameObject> rentedObjects = new List<GameObject>();
    public static List<GameObject> RentedObjects {
        get { return rentedObjects; }
    }

    public static int poolSize = 10000;

    void Start() {
        if (!Instance)
            Instance = this;
        
        pooledObjects.Capacity = poolSize;
        for (int i = 0; i < poolSize; ++i) {
            GameObject g = GameObject.Instantiate(Resources.Load("Models/Grass"), Vector3.up * 1000, Quaternion.identity, transform) as GameObject;

            pooledObjects.Add(g);
            g.SetActive(false);
        }
    }
    
    public static bool IsInPool(GameObject obj) {
        return pooledObjects.Contains(obj);
    }

    /// <summary>
    /// Returns grass object to object pool, removes it from rented objets list and disables it.
    /// </summary>
    /// <param name="obj"></param>
    public static void Return(GameObject obj) {
        if (rentedObjects.Contains(obj)) rentedObjects.Remove(obj);
        obj.transform.position = Vector3.up * 1000;
        pooledObjects.Add(obj);
        obj.SetActive(false);
    }

    /// <summary>
    /// "Instantiates" grass from object pool and changes it's position.
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public static GameObject Instantiate(Vector3 position) {
        GameObject g;

        if (pooledObjects.Count > 0) {
            g = pooledObjects[pooledObjects.Count - 1];
            pooledObjects.RemoveAt(pooledObjects.Count - 1);
            g.transform.position = position;
        }
        else {
            g = GameObject.Instantiate(Resources.Load("Models/Grass"), position, Quaternion.identity) as GameObject;
        }

        rentedObjects.Add(g);
        g.SetActive(true);
        g.GetComponent<Grass>().Start();
        return g;
    }
}
