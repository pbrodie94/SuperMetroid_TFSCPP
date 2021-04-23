using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private Text energyText;
    [SerializeField] private Text missileText;

    private void Start()
    {
        if (!energyText)
            energyText = transform.Find("HealthText").GetComponent<Text>();
        if (!missileText)
            missileText = transform.Find("missileText").GetComponent<Text>();
    }

    public void UpdateEnergy(int energy)
    {
        energyText.text = "ENERGY: " + energy;
    }

    public void UpdateMissiles(int missiles)
    {
        missileText.text = missiles.ToString(); ;
    }
}
