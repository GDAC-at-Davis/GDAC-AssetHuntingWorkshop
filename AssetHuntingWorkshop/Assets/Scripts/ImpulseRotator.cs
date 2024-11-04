using Unity.Cinemachine;
using UnityEngine;

public class ImpulseRotator : MonoBehaviour
{
    [SerializeField]
    private CinemachineImpulseSource _source;

    private Vector3 _impulseDir;

    private void Awake()
    {
        _impulseDir = _source.DefaultVelocity;
    }

    private void Update()
    {
        _source.DefaultVelocity = transform.rotation * _impulseDir;
    }
}