using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutCounterVisual : MonoBehaviour
{
    [SerializeField]
    private CuttingCounter cuttingCounter;

    private const string CUT = "Cut";

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Start()
    {
        cuttingCounter.onCut += CuttingCounter_onCut;
    }

    private void CuttingCounter_onCut(object sender, System.EventArgs e)
    {
        animator.SetTrigger(CUT);
    }

}
