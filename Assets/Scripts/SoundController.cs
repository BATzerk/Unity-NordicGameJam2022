using System;
using System.Collections;
using System.Collections.Generic;
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
    
    public SoundController Instrance { get; private set; }

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
        Instrance = this;
    }

    void Update()
    {
        timeToNextRandomSound -= Time.deltaTime;
        if (timeToNextRandomSound < 0)
        {
            var clipIndex = Random.Range(0, randomSounds.Length);
            var clip = randomSounds[clipIndex];
            PlayAt(clip, RandomSourcePosition());
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

    void PlayAtSource(AudioClip clip)
    {
        PlayAt(clip, listener.position);
    }
    
    void PlayAt(AudioClip clip, Vector3 at)
    {
        var source = sources[sourceIndex];
        source.Stop();
        source.transform.position = at;
        source.clip = clip;
        source.Play();
        SetTimeToNextSound(clip);
        IncrementSource();
    }
}