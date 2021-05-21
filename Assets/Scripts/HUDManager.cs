using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [Header("HUD")]
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

    [Header("Pause Menu")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private Text[] pauseMenuItems;
    private int menuSelection = 0;
    private Color defaultColor = Color.white;
    private Color selectedColor = Color.cyan;
    private bool paused = false;

    [Header("Game Over Menu")]
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private Text[] goText;
    private bool gameOver = false;

    private GameManager gm;
    private CanvasGroup cg;

    private void Start()
    {
        if (!energyText)
            energyText = transform.Find("HealthText").GetComponent<Text>();
        if (!missileText)
            missileText = transform.Find("missileText").GetComponent<Text>();

        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

        cg = GetComponentInChildren<CanvasGroup>();

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
                    alpha = 3;
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
            PauseMenu();
        }

        if (gameOver)
        {
            GameOverMenu();
        }

        //Checks for pause button
        if (Input.GetButtonDown(InputManager.pause))
        {
            Pause();
        }
    }

    private void PauseMenu()
    {
        //Gets input for menu selection
        if (Input.GetButtonDown(InputManager.select) && Input.GetAxisRaw(InputManager.select) > 0)
        {
            //Changes the menu selection index
            menuSelection--;

            //Loops the menu if the selection goes out of bounds
            if (menuSelection < 0)
            {
                menuSelection = pauseMenuItems.Length - 1;
            }

        } else if (Input.GetButtonDown(InputManager.select) && Input.GetAxisRaw(InputManager.select) < 0)
        {
            //Changes the menu selection index
            menuSelection++;

            //Loops the menu if the selection goes out of bounds
            if (menuSelection > pauseMenuItems.Length - 1)
            {
                menuSelection = 0;
            }
        }

        //Sets all menu items to unselected colour
        for (int i = 0; i < pauseMenuItems.Length; i++)
        {
            pauseMenuItems[i].color = defaultColor;
        }

        //Sets the selected menu item to the selected colour
        pauseMenuItems[menuSelection].color = selectedColor;

        //Selects the selected menu item on button press
        if (Input.GetButtonDown(InputManager.submit))
        {
            PauseMenuSelection(menuSelection);
        }
    }

    private void Pause()
    {
        //Pauses and unpauses
        if (paused)
        { 
            //Sets pause menu invisible
            pauseMenu.SetActive(false);

            //Turns off the screen fade
            Color c = screenFade.color;
            c.a = 0;
            screenFade.color = c;

            //Resets timescale to normal speed
            Time.timeScale = 1;

            paused = false;
        } else
        {
            //Shows pause menu
            pauseMenu.SetActive(true);

            //Sets the screen tint
            Color c = screenFade.color;
            c.a = 0.5f;
            screenFade.color = c;

            //Ensures the pause menu starts at the top
            menuSelection = 0;

            //Sets all menu items to unselected
            for (int i = 0; i < pauseMenuItems.Length; i++)
            {
                pauseMenuItems[i].color = defaultColor;
            }

            //Sets the selected menu item to selected colour
            pauseMenuItems[menuSelection].color = selectedColor;

            //Stops time
            Time.timeScale = 0;

            paused = true;
        }
    }

    private void PauseMenuSelection(int selection)
    {
        //Directs pause menu selections
        switch (selection)
        {
            case 0:
                //Resume

                Pause();

                break;

            case 1:
                //Restart Level

                Pause();
                gm.RestartLevel();

                break;

            //case 2:
                //Options

                 //break;

            case 2:
                //Quit to Main Menu

                Pause();
                gm.LoadLevel(LevelManager.MainMenu);

                break;

            case 3:
                //Quit to Desktop

                Application.Quit();

                break;
        }
    }

    private void GameOverMenu()
    {
        //Gets input for menu selection
        if (Input.GetButtonDown(InputManager.select) && Input.GetAxisRaw(InputManager.select) > 0)
        {
            //Changes the menu selection index
            menuSelection--;

            //Loops the menu if the selection goes out of bounds
            if (menuSelection < 0)
            {
                menuSelection = goText.Length - 1;
            }

        }
        else if (Input.GetButtonDown(InputManager.select) && Input.GetAxisRaw(InputManager.select) < 0)
        {
            //Changes the menu selection index
            menuSelection++;

            //Loops the menu if the selection goes out of bounds
            if (menuSelection > goText.Length - 1)
            {
                menuSelection = 0;
            }
        }

        //Sets all menu items to unselected colour
        for (int i = 0; i < goText.Length; i++)
        {
            goText[i].color = defaultColor;
        }

        //Sets the selected menu item to the selected colour
        goText[menuSelection].color = selectedColor;

        //Selects the selected menu item on button press
        if (Input.GetButtonDown(InputManager.submit))
        {
            GameOverSelection(menuSelection);
        }
    }

    public void GameOver()
    {
        gameOver = true;
        Time.timeScale = 0;
        gameOverMenu.SetActive(true);
        menuSelection = 0;

        for (int i = 0; i < goText.Length; i++)
        {
            goText[i].color = defaultColor;
        }

        goText[menuSelection].color = selectedColor;
    }

    private void GameOverSelection(int selection)
    {
        //Directs pause menu selections
        switch (selection)
        {
            case 0:
                //Restart Level
                gameOver = false;
                gameOverMenu.SetActive(false);

                Time.timeScale = 1;
                gm.RestartLevel();

                cg.alpha = 1;

                break;

            case 1:
                //Quit to Main Menu
                gameOver = false;
                gameOverMenu.SetActive(false);

                Time.timeScale = 1;
                gm.LoadLevel(LevelManager.MainMenu);

                break;

            case 2:
                //Quit to Desktop

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


