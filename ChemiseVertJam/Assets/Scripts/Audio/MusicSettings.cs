using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MusicSettings", menuName = "Audio/MusicSettings", order = 1)]
public class MusicSettings : ScriptableObject
{
    public AudioClip[] ambiance1;
    public AudioClip[] ambiance2;
}
