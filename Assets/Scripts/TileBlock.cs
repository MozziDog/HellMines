using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBlock : MonoBehaviour
{
    public const int maxHp = 50;

    public int pos_x;
    public int pos_y;

    public bool isMine = false;
    public bool isFlagged = false;
    public bool isRevealed = false;
    public int hp;

    [SerializeField] int nearbyMineCount;
    private MeshRenderer meshRenderer;

    [Space(10)]
    [SerializeField] Texture2D texture_unrevealed;
    [SerializeField] Texture2D texture_bomb;
    [SerializeField] Texture2D texture_flag;
    [SerializeField] Texture2D[] texture_numbers;



    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.mainTexture = texture_unrevealed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTileAsMineTile()
    {
        isMine = true;
        // 테스트용
        // GetComponent<MeshRenderer>().material.SetColor("_BaseColor", new Color(1, 0, 0));
    }

    public void SetNearbyMineCount(int value)
    {
        nearbyMineCount = value;
    }

    public void DoDamage(int damage)
    {
        hp -= damage;
        if(hp <= 0)
        {
            Reveal();
        }
    }

    public void Reveal()
    {
        if (isMine)
        {
            // 게임 오버
            meshRenderer.material.mainTexture = texture_bomb;
            GameManager.instance.GameOver();
            return;
        }

        if (this.isRevealed)
        {
            Debug.LogWarning("Revealing once revealed");
            return;
        }

        isRevealed = true;
        Grid._leftTiles -= 1;
        Grid.instance.CheckGameClear();
        if (this.nearbyMineCount == 0)
        {
            DestroyBlock();
            for (int i = Mathf.Max(0, pos_x - 1); i <= Mathf.Min(pos_x + 1, Grid._gridSize_x - 1); i++)
                for (int j = Mathf.Max(0, pos_y - 1); j <= Mathf.Min(pos_y + 1, Grid._gridSize_y - 1); j++)
                {
                    if (!Grid._tiles[i][j].isMine && !Grid._tiles[i][j].isRevealed)
                        Grid._tiles[i][j].Reveal();
                }
        }
        else
        {
            // TODO: 모양 바꾸기
            meshRenderer.material.mainTexture = texture_numbers[this.nearbyMineCount];
        }

    }

    public void SetFlag()
    {
        isFlagged = !isFlagged;
        // TODO: 모양 바꾸기
        if(isFlagged)
            meshRenderer.material.mainTexture = texture_flag;
        else
        {
            if (isRevealed)
                meshRenderer.material.mainTexture = texture_numbers[this.nearbyMineCount];
            else
                meshRenderer.material.mainTexture = texture_unrevealed;
        }
        Grid.instance.CheckGameClear();
    }

    public void DestroyBlock()
    {
        gameObject.SetActive(false);
    }
}
