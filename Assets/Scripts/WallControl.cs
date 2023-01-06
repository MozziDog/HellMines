using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallControl : MonoBehaviour
{
    [SerializeField] Transform wall_up;
    [SerializeField] Transform wall_down;
    [SerializeField] Transform wall_left;
    [SerializeField] Transform wall_right;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetWallPosition()
    {
        wall_up.position = new Vector3(wall_up.position.x, wall_up.position.y, Grid._gridSize_y + 0f);
        wall_right.position = new Vector3(Grid._gridSize_x+0f, wall_right.position.y, wall_right.position.z);
    }
}
