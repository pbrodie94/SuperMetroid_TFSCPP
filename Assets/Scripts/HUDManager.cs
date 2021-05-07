using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private Slider energySider;
    [SerializeField] private Text energyText;
    [SerializeField] private Text missileText;
    [SerializeField] private Text livesText;

    [SerializeField] private Image[] energyTankSlots;

    private Color filledColour = Color.white;
    private Color emptyColour = Color.grey;

    private float fadeRate = 3;
    private bool fade = false;
    private float timeFaded = 0;

    private CanvasGroup cg;

    private void Start()
    {
        if (!energyText)
            energyText = transform.Find("HealthText").GetComponent<Text>();
        if (!missileText)
            missileText = transform.Find("missileText").GetComponent<Text>();

        cg = GetComponent<CanvasGroup>();

        emptyColour.a = 150;

        energySider.maxValue = 99;
    }

    private void Update()
    {

        if (Time.time < timeFaded + fadeRate)
        {
            float alpha = 0;

            if (fade)
            {
                alpha = 0;
            }
            else
            {
                alpha = 1;
            }

            float a = cg.alpha;
            a = Mathf.Lerp(a, alpha, fadeRate * Time.deltaTime);
            cg.alpha = a;
        }
    }

    public void InitializeEnergy(int energy, int energyTanks, int maxEnergyTanks)
    {

        for (int i = 0; i < energyTankSlots.Length; i++)
        {
            if (i + 1 < maxEnergyTanks)
            {
                energyTankSlots[i].enabled = true;
            }
            else
            {
                energyTankSlots[i].enabled = false;
            }
        }

        SetEnergyTanks(energyTanks, maxEnergyTanks);

        energyText.text = energy.ToString();
        energySider.value = energy;
    }

    public void UpgradeEnergyTanks(int energy, int energyTanks, int maxEnergyTanks)
    {
        for (int i = 0; i < energyTankSlots.Length; i++)
        {
            if (i + 1 < maxEnergyTanks)
            {
                energyTankSlots[i].enabled = true;

            }
            else
            {
                energyTankSlots[i].enabled = false;
            }
        }

        SetEnergyTanks(energyTanks, maxEnergyTanks);

        energyText.text = energy.ToString();
        energySider.value = energy;
    }

    public void UpdateEnergy(int energy, int energyTanks, int maxEnergyTanks)
    {
        SetEnergyTanks(energyTanks, maxEnergyTanks);

        energyText.text = energy.ToString();
        energySider.value = energy;
    }

    void SetEnergyTanks(int energyTanks, int maxEnergyTanks)
    {
        for (int i = 0; i < maxEnergyTanks; i++)
        {
            energyTankSlots[i].enabled = true;

            if (i + 1 <= energyTanks)
            {
                energyTankSlots[i].color = filledColour;
            }
            else
            {
                energyTankSlots[i].color = emptyColour;
            }
        }
    }

    public void UpdateMissiles(int missiles)
    {
        missileText.text = missiles.ToString();
    }

    public void UpdateLives(int lives)
    {
        livesText.text = lives.ToString();
    }

    public void FadeUI(bool fade)
    {
        this.fade = fade;
        timeFaded = Time.time;
    }
}
