using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Common.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundController : MonoBehaviour
{
    // References
    [SerializeField] private AudioClip[] randomSounds;
    [SerializeField] private AudioSource[] sources;
    [SerializeField] private int sourceIndex = 0;
    [SerializeField] private float randomSoundDistance = 4;
    [SerializeField] private float randomSoundDistanceVariance = 1;
    [SerializeField] private float randomSoundMinInterval = 0.5f;
    [SerializeField] private float randomSoundMaxInterval = 5;
    [SerializeField] private float timeToNextRandomSound = 1;

    [SerializeField] private Transform listener;
    
    public static SoundController Instance { get; private set; }

    public Vector3 RandomXZCoordinate()
    {
        var halfVariance = randomSoundDistanceVariance / 2;
        float GetCoord()
        {
            var direction = Random.Range(0, 2) == 0 ? -1 : 1;
            return direction * randomSoundDistance + Random.Range(-halfVariance, halfVariance);
        }

        return new Vector3(GetCoord(), 0, GetCoord());
    }
    
    public Vector3 RandomSourcePosition()
    {
        // var rotation = Quaternion.Euler(0, 90, 0);
        // var distance = Vector3.forward * (randomSoundMaxInterval + Random.Range(0, randomSoundDistanceVariance));
        // return listener.position + rotation * distance;
        return listener.position + RandomXZCoordinate();
    }
    
    void Start()
    {
        Instance = this;
    }

    void Update()
    {
        timeToNextRandomSound -= Time.deltaTime;
        if (timeToNextRandomSound < 0)
        {
            var source = sources[sourceIndex];
            if (source.isPlaying)
            {
                SetTimeToNextSound(source.clip);
                return;
            }
        
            PlayRandomAt(randomSounds, RandomSourcePosition());
        }
    }

    void SetTimeToNextSound(AudioClip clip)
    {
        if (sources.Length == 1)
        {
            timeToNextRandomSound = Random.Range(Mathf.Max(randomSoundMinInterval, clip.length), Mathf.Max(randomSoundMinInterval, clip.length + 0.5f));
        }
        else
        {
            timeToNextRandomSound = Random.Range(randomSoundMinInterval, randomSoundMinInterval);
        }
    }

    void IncrementSource()
    {
        sourceIndex++;
        if (sourceIndex >= sources.Length)
        {
            sourceIndex = 0;
        }
    }

    public void PlayRandom(AudioClip[] clips, ulong delay = 0)
    {
        PlayRandomAt(clips, listener.position, delay);
    }
    
    public void Play(AudioClip clip, ulong delay = 0)
    {
        PlayAt(clip, listener.position, delay);
    }

    public void PlayRandomAt(AudioClip[] clips, Vector3 at, ulong delay = 0)
    {
        PlayAt(clips.RandomItem(), at, delay);
    }

    public void PlayAt(AudioClip clip, Vector3 at, ulong delay = 0)
    {
        if (clip == null)
        {
            Debug.Log("Missing sound.");
            return;
        }
        
        var source = sources[sourceIndex];
        source.Stop();
        source.transform.position = at;
        source.clip = clip;
        source.Play(delay);
        SetTimeToNextSound(clip);
        IncrementSource();
    }
}