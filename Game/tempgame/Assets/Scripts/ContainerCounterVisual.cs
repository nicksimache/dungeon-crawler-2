using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerCounterVisual : MonoBehaviour
{
    [SerializeField]
    private ContainerCounter containterCounter;

    private const string OPEN_CLOSE = "OpenClose";

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Start()
    {
        containterCounter.OnPlayerGrabObject += ContainterCounter_OnPlayerGrabObject;
    }

    private void ContainterCounter_OnPlayerGrabObject(object sender, System.EventArgs e)
    {
        animator.SetTrigger(OPEN_CLOSE);
    }
}
