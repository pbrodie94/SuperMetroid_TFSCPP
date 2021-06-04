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

    [Header("Menu Screens")]
    [SerializeField] private GameObject instructionsPanel;
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private GameObject optionsPanel;


    private void Start()
    {
        menuSelection = 0;

        for (int i = 0; i < menuItems.Length; i++)
        {
            menuItems[i].color = defaultColor;

            menuItems[i].GetComponent<TitleMenuItem>().menuItemIndex = i;
        }

        menuItems[menuSelection].color = selectedColor;
    }

    private void Update()
    {
        if (!instructionsPanel.activeSelf && !creditsPanel.activeSelf && !optionsPanel.activeSelf)
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

        } else if (instructionsPanel.activeSelf)
        {
            if (Input.anyKeyDown)
            {
                instructionsPanel.SetActive(false);
            }
        } else if (creditsPanel.activeSelf)
        {
            if (Input.anyKeyDown)
            {
                creditsPanel.SetActive(false);
            }
        }
    }

    public void SelectItem(int selection)
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

                break;

            case 2:
                //Show credits

                creditsPanel.SetActive(true);

                break;

            case 3:
                //Options

                optionsPanel.SetActive(true);

                break;

            case 4:
                //Quit
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
                break;
        }
    }

    public void UpdateMenuSelection(int selectionIndex)
    {
        menuSelection = selectionIndex;
    }

    public void BackButton()
    {
        //Close options menu

        optionsPanel.SetActive(false);
    }
}
