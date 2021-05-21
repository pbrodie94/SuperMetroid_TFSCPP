using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    private ScreenFade fadeType = ScreenFade.Hud;
    [SerializeField] private Image screenFade;
    [SerializeField] private Slider energySider;
    [SerializeField] private Text energyText;
    [SerializeField] private Text missileText;
    [SerializeField] private Text livesText;

    [SerializeField] private Image[] energyTankSlots;

    private Color filledColour = Color.white;
    private Color emptyColour = Color.grey;
    private Color fadeColour;

    private float fadeRate = 4;
    private bool fade = false;
    private float timeFaded = 0;

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Text[] pauseText;
    private int pauseSelection = 0;
    private bool paused = false;

    private CanvasGroup cg;

    private void Start()
    {
        if (!energyText)
            energyText = transform.Find("HealthText").GetComponent<Text>();
        if (!missileText)
            missileText = transform.Find("missileText").GetComponent<Text>();

        cg = GetComponent<CanvasGroup>();

        fadeColour = screenFade.color;

        emptyColour.a = 150;

        energySider.maxValue = 99;
    }

    private void Update()
    {

        if (Time.time < timeFaded + fadeRate)
        {
            float alpha = 0;
            float a = 0;

            if (fade)
            {
                if (fadeType == ScreenFade.Hud)
                {
                    a = cg.alpha;
                    alpha = 0;
                } else if (fadeType == ScreenFade.Screen)
                {
                    a = screenFade.color.a;
                    alpha = 255;
                }
            }
            else
            {
                if (fadeType == ScreenFade.Hud)
                {
                    a = cg.alpha;
                    alpha = 1;
                }
                else if (fadeType == ScreenFade.Screen)
                {
                    a = screenFade.color.a;
                    alpha = 0;
                }
            }

            a = Mathf.Lerp(a, alpha, fadeRate * Time.deltaTime);

            if (fadeType == ScreenFade.Hud)
            {
                cg.alpha = a;

            } else if (fadeType == ScreenFade.Screen)
            {
                fadeColour.a = a;
                screenFade.color = fadeColour;
            }
        }

        if (paused)
        {

        }

        if (Input.GetButtonDown(InputManager.pause))
        {
            if (paused)
            {
                //Unpause
                pauseMenu.SetActive(false);
                pauseSelection = 0;
                paused = false;
            } else
            {
                //Pause
                pauseMenu.SetActive(true);
                paused = true;
            }
        }
    }

    void PauseMenuSelection(int selection)
    {
        switch (selection)
        {
            case 0:
                //Resume

                pauseMenu.SetActive(false);
                pauseSelection = 0;
                paused = false;

                break;

            case 1:
                //Options

                break;

            case 2:
                //Quit to main menu



                break;

            case 3:
                //Quit to desktop

                Application.Quit();

                break;
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
        fadeType = ScreenFade.Hud;
        this.fade = fade;
        timeFaded = Time.time;
    }

    public void FadeScreen(bool fade)
    {
        fadeType = ScreenFade.Screen;
        this.fade = fade;
        timeFaded = Time.time;
    }

    private enum ScreenFade
    {
        Hud,
        Screen
    }
}


