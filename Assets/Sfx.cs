using System.Collections.Generic;
using UnityEngine;

// Looks up a scene AudioSource by GameObject name once and reuses it.
public static class Sfx
{
    static readonly Dictionary<string, AudioSource> cache = new Dictionary<string, AudioSource>();

    static AudioSource Get(string name)
    {
        if (!cache.TryGetValue(name, out AudioSource source) || !source)
        {
            GameObject go = GameObject.Find(name);
            source = go ? go.GetComponent<AudioSource>() : null;
            if (source)
                cache[name] = source;
        }
        return source;
    }

    public static void Play(string name)
    {
        AudioSource source = Get(name);
        if (source)
            source.Play();
    }

    public static void PlayIfIdle(string name)
    {
        AudioSource source = Get(name);
        if (source && !source.isPlaying)
            source.Play();
    }
}
