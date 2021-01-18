using UnityEngine;
using System.Collections;

public class CinemachineBrainHook : MonoBehaviour
{
    public Cinemachine.CinemachineBrain brain;
    public static CinemachineBrainHook singleton;

    // Use this for initialization
    void Awake()
    {
        singleton = this;
        brain = GetComponent<Cinemachine.CinemachineBrain>();
    }
}
