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
    [SerializeField] AudioClip _buttonSound;
    private AudioSource _audio;

    // Start is called before the first frame update
    void Start()
    {
        _audio = GetComponent<AudioSource>();
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
    }
    public void OnClickLevelNormal()
    {
        Grid._gridSize_x = 16;
        Grid._gridSize_y = 16;
        Grid._numOfMines = 40;
    }
    public void OnClickLevelHard()
    {
        Grid._gridSize_x = 30;
        Grid._gridSize_y = 16;
        Grid._numOfMines = 99;
    }

    public void PlayButtonSound()
    {
        _audio.PlayOneShot(_buttonSound);
    }
}
