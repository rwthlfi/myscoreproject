using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WallBuilderMenu : MonoBehaviour
{
    [Header("Script Reference")]
    public WallBuilder wallBuilder;
    private string decFormat = "f1";

    [Header("Menu Window")]
    public GameObject mainMenu;
    public GameObject brickMenu;
    public GameObject wallMenu;
    public GameObject foundationMenu;
    public GameObject actingWorkMenu;
    public Button buildButton;

    [Header("Menu UI - Brick")]
    public TextMeshProUGUI brickWidthText;
    public TextMeshProUGUI brickHeightText;
    public TextMeshProUGUI brickMassText;
    public TextMeshProUGUI brickTypeText;

    [Header("Menu UI - Wall")]
    public TextMeshProUGUI wallWidthText;
    public TextMeshProUGUI wallHeightText;
    public TextMeshProUGUI wallTotalBrickText;
    
    [Header("Menu UI - Foundation")]
    public TextMeshProUGUI foundationWidthText;
    public TextMeshProUGUI foundationHeightText;

    [Header("Menu UI - CurrentWork")]
    public Button actingWorkButton;
    public Slider currentWork_DiplacementSlider;
    public TMP_InputField currentWork_DiplacementInputField;
    public TextMeshProUGUI currentWork_DisplacementText;
    public TextMeshProUGUI currentWork_ForceText;

    [Header("Menu UI - Info Message")]
    public GameObject infoObj;
    public Image infoBackground;
    public TextMeshProUGUI info_BrickDoesntHaveSize;
    public TextMeshProUGUI Info_FoundationDoestHaveSize;
    public TextMeshProUGUI Info_BrickIsBiggerComparedToTheWallSize;
    public TextMeshProUGUI Info_foundationSizeIsBiggerComparedToTheWallSize;
    public TextMeshProUGUI Info_BrickDoesntHaveMass;
    public TextMeshProUGUI Info_BuildSuccess;

    private void Start()
    {
        Ui_backToMainMenu();
    }



    //-brick variable
    public void Brick_SetWidth(Slider _slider)
    {
        wallBuilder.brickW = _slider.value; // set the value in the wall builder
        brickWidthText.text = _slider.value.ToString(decFormat); // show in the report
    }
    public void Brick_SetHeight(Slider _slider)
    {
        wallBuilder.brickH = _slider.value;
        brickHeightText.text = _slider.value.ToString(decFormat);
    }

    public void Brick_SetMass(Slider _slider)
    {
        wallBuilder.brickMass = _slider.value;
        brickMassText.text = _slider.value.ToString(decFormat);
    }


    //-Wall
    public void Wall_SetWidth(Slider _slider)
    {
        wallBuilder.wallW= _slider.value;
        wallWidthText.text = _slider.value.ToString(decFormat);
    }
    public void Wall_SetHeight(Slider _slider)
    {
        wallBuilder.wallH = _slider.value;
        wallHeightText.text = _slider.value.ToString(decFormat);
    }

    public void Wall_SetTotalBricks(float _value)
    {
        wallTotalBrickText.text = _value.ToString(decFormat);
    }

    //-Foundation
    public void Foundation_SetWidth(Slider _slider)
    {
        wallBuilder.foundationW = _slider.value;
        foundationWidthText.text = _slider.value.ToString(decFormat);
    }
    public void Foundation_SetHeight(Slider _slider)
    {
        wallBuilder.foundationH = _slider.value;
        foundationHeightText.text = _slider.value.ToString(decFormat);
    }

    //-currentWork
    public void CurrentWork_SetDisplacement(Slider _slider)
    {
        wallBuilder.displacement = _slider.value;
        currentWork_DisplacementText.text = _slider.value.ToString(decFormat);

        //Calculate the force too.
        currentWork_ForceText.text = wallBuilder.ForceCalculation().ToString(decFormat);
        if (wallBuilder.ForceCalculation() > wallBuilder.breakingForce)
        {
            print("Activate");
            //activate the rigibody and break everything
            wallBuilder.EnableBrickPhysics(true);
            wallBuilder.Enable_BrickBreaker(true);
        }

    }
    private void CurrentWork_SetDisplacement(float _value)
    {
        currentWork_DiplacementSlider.value = _value;
        currentWork_DiplacementInputField.text = _value.ToString(decFormat);
        wallBuilder.displacement = _value;
        currentWork_DisplacementText.text = _value.ToString(decFormat);
    }


    //-build wall
    public void Ui_BuildWall()
    {

        if (wallBuilder.CheckTheBuildProperties() == WallBuilder.buildEnum.failed_brickDoesntHaveSize)
        {
            ShowInfo(info_BrickDoesntHaveSize);
            print("brick Doesnt have size");
            return;
        }
        else if(wallBuilder.CheckTheBuildProperties() ==  WallBuilder.buildEnum.failed_foundationDoesntHaveSize)
        {
            ShowInfo(Info_FoundationDoestHaveSize);
            print("foundation doesnt have size");
            return;
        }

        else if (wallBuilder.CheckTheBuildProperties() == WallBuilder.buildEnum.failed_WallToSmall)
        {
            ShowInfo(Info_BrickIsBiggerComparedToTheWallSize);
            print("brick is bigger compare to the Wall size");
            return;
        }
        else if(wallBuilder.CheckTheBuildProperties() == WallBuilder.buildEnum.failed_foundationTooBig)
        {
            ShowInfo(Info_foundationSizeIsBiggerComparedToTheWallSize);
            print("foundation size is bigger compared to the the Wall size");
            return;
        }

        else if (wallBuilder.CheckTheBuildProperties() == WallBuilder.buildEnum.failed_brickDoesntHaveMass)
        {
            ShowInfo(Info_BrickDoesntHaveMass);
            print("Brick doesnt have mass");
            return;
        }

        else
        {
            print("readyToBuilt");
            //reset the displacement
            CurrentWork_SetDisplacement(0);

            //disable the build button
            buildButton.interactable = false;
            actingWorkButton.interactable = false;

            //destroy the wall
            wallBuilder.DestroyTheWall();
            //built the wall
            StartCoroutine(wallBuilder.BuiltTheWall(returnValue =>
            {
                print(returnValue);
                ShowSuccess(Info_BuildSuccess);
                buildButton.interactable = true;
                actingWorkButton.interactable = true;
                //show total bricks used
                wallTotalBrickText.text = wallBuilder.totalBricks.ToString();
            }));
        }
        
    }



    //going back to the main menu
    public void Ui_backToMainMenu()
    {
        brickMenu.SetActive(false);
        wallMenu.SetActive(false);
        foundationMenu.SetActive(false);
        actingWorkMenu.SetActive(false);

        mainMenu.SetActive(true);
    }


    //for navigating the Menus
    public void Ui_MenuNavigation(GameObject _menu)
    {
        mainMenu.SetActive(false);
        brickMenu.SetActive(false);
        wallMenu.SetActive(false);
        foundationMenu.SetActive(false);
        actingWorkMenu.SetActive(false);

        _menu.SetActive(true);
    }

    private void ShowInfo(TextMeshProUGUI _textInfo)
    {
        infoBackground.color = new Color32(249, 165, 27, 255);
        StartCoroutine(CoroutineExtensions.HideAfterSeconds(infoObj, 3f));
        StartCoroutine(CoroutineExtensions.HideAfterSeconds(_textInfo.gameObject, 3f));
    }

    private void ShowSuccess(TextMeshProUGUI _textInfo)
    {
        infoBackground.color = new Color32(76, 184, 78, 255);
        //currently building
        StartCoroutine(CoroutineExtensions.HideAfterSeconds(infoObj, 3f));
        StartCoroutine(CoroutineExtensions.HideAfterSeconds(_textInfo.gameObject, 3f));
    }
}
