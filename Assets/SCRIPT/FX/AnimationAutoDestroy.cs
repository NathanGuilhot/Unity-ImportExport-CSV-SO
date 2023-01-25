using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//https://answers.unity.com/questions/670860/delete-object-after-animation-2d.html
[RequireComponent(typeof(Animator))]
public class AnimationAutoDestroy : MonoBehaviour
{
    public float delay = 0f;

    void Start()
    {
        Destroy(gameObject, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + delay);
    }
}
