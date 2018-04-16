﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Public enum for referencing individual audio samples - to be spawned wherever
// Generic samples only!
public enum SamplesList { HookBang, ComputerBeep, WaterSplash, BottleSpray, BottleFilling,
    Ladder, BBSliding, Error, Correct, Pop, Clink, WaterRunning, Poof };



public class SoundManager : MonoBehaviour {

    // Prefabs, which will be used for pooling
    // Much better idea than setting their properties in code

    // ---------------------------------------------------------------------

    // Generic prefab for spawning one-time samples in the scene
    public GameObject GenericPrefab;

    // ---------------------------------------------------------------------

    
    // Actual in-scene GameObjects which play the sounds
    // The hidden scripts are used to transparently access the actual sfx interface
    // since otherwise GetComponent<> would be called every time we want to play something.
    // Public but hidden in inspector, since they will be overwritten 

    public GameObject ACSource;
    public GameObject LightSource;

    #region TopDoor

    public GameObject TopDoorSource;

    [HideInInspector]
    public TopDoorScript TopDoor;

    #endregion


    // ---------------------------------------------------------------------
    // Raw AudioClips which are used to play one-shot samples in the scene
    // Only generic (with no static location) samples are listed here
    // Static meaning static 3D location or a script attached to a persistent game object

    public AudioClip SfxHookBang;
    public AudioClip SfxComputerBeep;
    public AudioClip SfxWaterSplash;
    public AudioClip SfxBottleSpray;
    public AudioClip SfxBottleFilling;
    public AudioClip SfxLadder;
    public AudioClip SfxBBSliding;
    public AudioClip SfxError;
    public AudioClip SfxCorrect;
    public AudioClip SfxPop;
    public AudioClip SfxClink;
    public AudioClip SfxWaterRunning;
    public AudioClip SfxPoof;

    // ---------------------------------------------------------------------

    private int _PoolSize = 30000;
    private GameObject[] _SfxParticlesPool;
    private int _FreshPoolInd = 0;

    public AudioClip ClipFromEnum(SamplesList en)
    {
        switch (en)
        {
            case SamplesList.HookBang:
                return SfxHookBang;

            case SamplesList.ComputerBeep:
                return SfxComputerBeep;

            case SamplesList.WaterSplash:
                return SfxWaterSplash;

            case SamplesList.BottleSpray:
                return SfxBottleSpray;

            case SamplesList.BottleFilling:
                return SfxBottleFilling;

            case SamplesList.Ladder:
                return SfxLadder;

            case SamplesList.BBSliding:
                return SfxBBSliding;

            case SamplesList.Error:
                return SfxError;

            case SamplesList.Correct:
                return SfxCorrect;

            case SamplesList.Pop:
                return SfxPop;

            case SamplesList.Clink:
                return SfxClink;

            case SamplesList.WaterRunning:
                return SfxWaterRunning;

            case SamplesList.Poof:
                return SfxPoof;
        }

        return null;
    }

    private GameObject SfxFromPool()
    {
        _FreshPoolInd %= _PoolSize;

        // TODO adaptive pool
        if (_SfxParticlesPool[_FreshPoolInd].activeSelf)
            return null;

        return _SfxParticlesPool[_FreshPoolInd++];
    }

    public GameObject Play3DAt(AudioClip clip, Transform where)
    {
        return Play3DAt(clip, where.position);
    }

    public GameObject Play3DAt(SamplesList en, Transform where)
    {
        return Play3DAt(ClipFromEnum(en), where);
    }

    public GameObject Play3DAt(SamplesList en, Vector3 where)
    {
        return Play3DAt(ClipFromEnum(en), where);
    }

    public GameObject Play3DAt(AudioClip clip, Vector3 v3)
    {
        GameObject tmp = SfxFromPool();
        tmp.GetComponent<AudioSource>().clip = clip;
        tmp.transform.position = v3;
        tmp.GetComponent<AudioSource>().spatialBlend = 1.0f; //full 3D
        tmp.SetActive(true);
        tmp.GetComponent<AudioSource>().Play();
        return tmp;
    }


    public GameObject Play2D(SamplesList en)
    {
        return Play2D(ClipFromEnum(en));
    }

    public GameObject Play2D(AudioClip clip)
    {
        GameObject tmp = SfxFromPool();
        tmp.GetComponent<AudioSource>().clip = clip;
        tmp.GetComponent<AudioSource>().spatialBlend = 0.0f; //full 2D
        tmp.SetActive(true);
        tmp.GetComponent<AudioSource>().Play();
        return tmp;
    }


    public void SetACVolume(float x)
    {
        ACSource.GetComponent<AudioSource>().volume = x;
    }


    // Use this for initialization
    void Start () {

        _SfxParticlesPool = new GameObject[_PoolSize];
        
        for(int i = 0; i < _PoolSize; i++)
        {
            GameObject tmp = (GameObject)Instantiate(GenericPrefab);
            tmp.SetActive(false);
            _SfxParticlesPool[i] = tmp;
        }

        TopDoor = TopDoorSource.GetComponent<TopDoorScript>();
    }
	
	// No need for this
	void Update () {		
	}
}
