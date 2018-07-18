using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class calls OnPlaybackStateChanged methods, holds information about playback state and speed.
/// </summary>
public class Simulation : MonoBehaviour {
    static PlaybackState playbackState = PlaybackState.stop;
    public static PlaybackState PlayState {
        get { return playbackState; }
        set {
            playbackState = value;
            onPlaybackStateChanged(playbackState, playbackSpeed);
        }
    }

    static float playbackSpeed = 1;
    public static float PlaybackSpeed {
        get { return playbackSpeed; }
        set {
            playbackSpeed = value;
            onPlaybackStateChanged(playbackState, playbackSpeed);
        }
    }

    public const float MIN_PLAYBACK_SPEED = .1f;
    public const float MAX_PLAYBACK_SPEED = 5;

    public delegate void OnPlaybackStateChanged(PlaybackState playbackState, float playbackSpeed);
    public static OnPlaybackStateChanged onPlaybackStateChanged;

    void Update() {
        if(Input.GetKeyDown(KeyCode.Space)) {
            if (playbackState != PlaybackState.play)
                PlayState = PlaybackState.play;
            else
                PlayState = PlaybackState.pause;
        }
    }
}

public enum PlaybackState {
    stop,
    play,
    pause
}