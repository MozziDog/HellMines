using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Slider widthSlider;
    [SerializeField] TMP_Text widthText;
    [Space(10)]
    [SerializeField] Slider heightSlider;
    [SerializeField] TMP_Text heightText;
    [Space(10)]
    [SerializeField] Slider minesSlider;
    [SerializeField] TMP_Text minesText;
    [Space(20)]
    [SerializeField] Toggle levelToggle_easy;
    [SerializeField] Toggle levelToggle_normal;
    [SerializeField] Toggle levelToggle_hard;

    // Start is called before the first frame update
    void Start()
    {
        OnSliderValueChanged_width();
        OnSliderValueChanged_height();
        OnSliderValueChanged_mines();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSliderValueChanged_width()
    {
        Grid._gridSize_x = (int)widthSlider.value;
        widthText.text = Grid._gridSize_x.ToString();
    }

    public void OnSliderValueChanged_height()
    {
        Grid._gridSize_y = (int)heightSlider.value;
        heightText.text = Grid._gridSize_y.ToString();
    }

    public void OnSliderValueChanged_mines()
    {
        Grid._numOfMines = (int)minesSlider.value;
        minesText.text = Grid._numOfMines.ToString();
    }

    public void OnClickStartButton()
    {
        SceneManager.LoadScene("Game");
    }

    public void OnClickLevelEasy()
    {
        Grid._gridSize_x = 9;
        Grid._gridSize_y = 9;
        Grid._numOfMines = 10;
        //levelToggle_easy.isOn = true;
        //levelToggle_normal.isOn = false;
        //levelToggle_hard.isOn = false;
    }
    public void OnClickLevelNormal()
    {
        Grid._gridSize_x = 16;
        Grid._gridSize_y = 16;
        Grid._numOfMines = 40;
        //levelToggle_easy.isOn = false;
        //levelToggle_normal.isOn = true;
        //levelToggle_hard.isOn = false;
    }
    public void OnClickLevelHard()
    {
        Grid._gridSize_x = 30;
        Grid._gridSize_y = 16;
        Grid._numOfMines = 99;
        //levelToggle_easy.isOn = false;
        //levelToggle_normal.isOn = false;
        //levelToggle_hard.isOn = true;
    }
}
