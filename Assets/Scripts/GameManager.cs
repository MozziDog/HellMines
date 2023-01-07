using Cinemachine;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] GameObject GameoverUI;
    [SerializeField] GameObject GameClearUI;

    [SerializeField] GameObject _grid;
    [SerializeField] GameObject _player;
    [SerializeField] WallControl _wallControl;

    // Start is called before the first frame update
    void Start()
    {
        if (Grid._gridSize_x == 0 && Grid._gridSize_y == 0)
        {
            Debug.LogError("Mine Error, Go back to Main Menu");
            var input = _player.GetComponent<StarterAssetsInputs>();
            input.cursorInputForLook = false;
            input.cursorLocked = false;
            input.SetCursorState(false);
            SceneManager.LoadScene("MainMenu");
        }

        Time.timeScale = 1f;
        instance = this;
        _grid.GetComponent<Grid>().Init();
        _wallControl.SetWallPosition();
        SpawnPlayer();
        // SetCameraToFollowPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnPlayer()
    {
        var grid = _grid.GetComponent<Grid>();
        _player.transform.position = new Vector3(Grid._gridSize_x / 2f, Grid._gridSize_y / 2f, 1f);
    }

    void SetCameraToFollowPlayer()
    {
        var followcam = GameObject.Find("PlayerFollowCamera");
        followcam.GetComponent<CinemachineVirtualCamera>().Follow = _player.transform;
    }

    public void GameOver()
    {
        var input = _player.GetComponent<StarterAssetsInputs>();
        GameoverUI.SetActive(true);
        input.cursorInputForLook = false;
        input.cursorLocked = false;
        input.SetCursorState(false);
        Time.timeScale = 0f;
    }

    public void GameClear()
    {
        var input = _player.GetComponent<StarterAssetsInputs>();
        GameClearUI.SetActive(true);
        input.cursorInputForLook = false;
        input.cursorLocked = false;
        input.SetCursorState(false);
        Time.timeScale = 0f;
    }

    public void Restart()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
