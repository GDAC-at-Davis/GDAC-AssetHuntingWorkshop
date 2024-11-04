using UnityEngine;

public class PlayPitchShifted : MonoBehaviour
{
    [SerializeField]
    private AudioSource _source;

    [SerializeField]
    private float _range;

    public void Play()
    {
        _source.pitch = Random.Range(1f - _range, 1f + _range);
        _source.Play(0);
    }
}