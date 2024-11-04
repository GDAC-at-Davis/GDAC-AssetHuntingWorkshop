using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField]
    private float _duration;

    private float _timer;

    private void Awake()
    {
        _timer = _duration;
    }

    private void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            Destroy(gameObject);
        }
    }
}