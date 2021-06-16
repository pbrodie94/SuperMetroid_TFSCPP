using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStepHandler : MonoBehaviour
{
    private SamusControl samus;

    private void Start()
    {
        samus = GetComponentInParent<SamusControl>();
    }

    public void Footstep()
    {
        samus.Footstep();
    }
}
