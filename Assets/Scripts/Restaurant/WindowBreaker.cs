using UnityEngine;
using Mirror;

public class WindowBreaker : NetworkBehaviour
{
    public Rigidbody[] _rigidbody;
    private bool windowBrokenFinish;
    public AudioSource audioSource;

    [SyncVar(hook = nameof(WindowBrokenOn))]
    public bool windowBrokenOn = false;
    void WindowBrokenOn(bool _old, bool _new) { }

    private void Start()
    {
        if (windowBrokenOn)
        {
            if (!windowBrokenFinish)
                BreakGlass();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Chair")
        {
            if (!windowBrokenFinish)
            {
                audioSource.Play();
                BreakGlass();
            }
        }
    }

    private void BreakGlass()
    {
        windowBrokenFinish = true;
        windowBrokenOn = true;

        _rigidbody = GetComponentsInChildren<Rigidbody>();
        _rigidbody[0] = _rigidbody[1];

        foreach (Rigidbody rigid in _rigidbody)
        {
            rigid.isKinematic = false;
            rigid.transform.GetChild(0).transform.localPosition = new Vector3(0, 0, 0.08f);
        }
    }
}