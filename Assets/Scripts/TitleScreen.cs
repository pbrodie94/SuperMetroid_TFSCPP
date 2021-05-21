using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{
    private int menuSelection = 0;
    [SerializeField] private Text[] menuItems;
    private Color defaultColor = Color.white;
    private Color selectedColor = Color.cyan;

    [SerializeField] private GameObject instructionsPanel;
    private bool instructions = false;

    [SerializeField] private GameObject creditsPanel;
    private bool credits = false;

    //[SerializeField] private GameObject optionsPanel;
    private bool options = false;

    private void Start()
    {

        menuSelection = 0;

        for (int i = 0; i < menuItems.Length; i++)
        {
            menuItems[i].color = defaultColor;
        }

        menuItems[menuSelection].color = selectedColor;
    }

    private void Update()
    {
        if (!instructions && !credits && !options)
        {
            if (Input.GetButtonDown(InputManager.select) && Input.GetAxisRaw(InputManager.select) > 0)
            {
                menuSelection--;

                if (menuSelection < 0)
                {
                    menuSelection = menuItems.Length - 1;
                }
            }
            else if (Input.GetButtonDown(InputManager.select) && Input.GetAxisRaw(InputManager.select) < 0)
            {
                menuSelection++;

                if (menuSelection > menuItems.Length - 1)
                {
                    menuSelection = 0;
                }
            }

            for (int i = 0; i < menuItems.Length; i++)
            {
                menuItems[i].color = defaultColor;
            }

            menuItems[menuSelection].color = selectedColor;

            if (Input.GetButtonDown(InputManager.submit))
            {
                SelectItem(menuSelection);
            }

        } else if (instructions)
        {
            if (Input.anyKeyDown)
            {
                instructionsPanel.SetActive(false);
                instructions = false;
            }
        } else if (credits)
        {
            if (Input.anyKeyDown)
            {
                creditsPanel.SetActive(false);
                credits = false;
            }
        }
    }


    void SelectItem(int selection)
    {
        switch (selection)
        {
            case 0:
                //Load Level
                SceneManager.LoadScene(LevelManager.Level);

                break;

            case 1:
                //Show Instructions Page

                instructionsPanel.SetActive(true);
                instructions = true;

                break;

            case 2:
                //Show credits

                creditsPanel.SetActive(true);
                credits = true;

                break;

            case 3:
                //Options

                break;

            case 4:
                //Quit

                Application.Quit();

                break;
        }
    }
}
