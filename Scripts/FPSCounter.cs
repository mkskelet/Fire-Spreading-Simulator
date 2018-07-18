using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class displays current frames per second on given Text object(s).
/// </summary>
public class FPSCounter : MonoBehaviour {
    public Text[] fpsCounter;
    float updateTime = .15f;
    float nextUpdate = 0;

    int frames = 0;
	
	void Update () {
        frames++;
        if (nextUpdate < Time.timeSinceLevelLoad) {
            int fps = (int)(frames / updateTime);

            foreach(Text t in fpsCounter) {
                t.text = fps.ToString() + " FPS";
            }

            frames = 0;
            nextUpdate = Time.timeSinceLevelLoad + updateTime;
        }
	}
}
