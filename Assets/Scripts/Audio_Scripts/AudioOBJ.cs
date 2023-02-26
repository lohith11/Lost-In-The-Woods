using UnityEngine;

[CreateAssetMenu(fileName = "New Sound", menuName = "Sound", order = 1)]
public class AudioOBJ : ScriptableObject 
{
    public string soundName;
    [Range(1,100)]
    public float range;
    public AudioClip clip;
     
}