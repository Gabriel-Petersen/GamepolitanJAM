using System.Collections.Generic;
using UnityEngine;

public class SongsHolder : MonoBehaviour
{
    [SerializeField] private List<Song> _songs = new();
    public List<Song> Songs { get { return _songs; } }

    private void Start()
    {
        foreach (var component in GetComponents<Song>())
        {
            _songs.Add(component);
        }
    }

    public bool IsAnySongSinging()
    {
        foreach (var song in _songs)
        {
            if (song.IsSinging())
                return true;
        }
        return false;
    }
}
