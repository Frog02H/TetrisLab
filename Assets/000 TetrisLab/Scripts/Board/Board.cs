using UnityEngine;
using UnityEngine.Tilemaps;
using TeterisLab;
using System;

public class Board : MonoBehaviour
{

    // Tetromino 参数
    public TetrominoData[] tetrominos;
    public FallingPiece fallingPiece;
    public Vector3Int spawnPos;

    // Tilemap 参数
    public Tilemap tileMap { get; private set; }
    public Vector2Int boardSize = new Vector2Int(10, 20);
    public RectInt Bounds
    {
        get
        {
            Vector2Int position = new Vector2Int(-this.boardSize.x / 2, -this.boardSize.y / 2);
            return new RectInt(position, this.boardSize);
        }
    }

    // Action And Event (事件 相关参数)
    // public ActionGroup actionGroup;

    private void Awake()
    {
        this.tileMap = GetComponentInChildren<Tilemap>();
        this.fallingPiece = GetComponentInChildren<FallingPiece>();

        for (int i = 0; i < this.tetrominos.Length; i++)
        {
            this.tetrominos[i].Initialize();
        }
    }

    private void Start()
    {
        SpawnPiece();
    }

    private void Update()
    {
        this.Clear(this.fallingPiece);
        
        this.Set(this.fallingPiece);
    } 

    public void SpawnPiece()
    {
        int pieceIndex = UnityEngine.Random.Range(0, this.tetrominos.Length);

        TetrominoData tetromino = this.tetrominos[pieceIndex];

        this.fallingPiece.Initialize(this, spawnPos, tetromino);
        
        this.Set(fallingPiece);
    }

    public void Set(FallingPiece Piece)
    {
        // Clear(Piece);

        for (int i = 0; i < Piece.blocks.Length; i++)
        {
            Vector3Int tilePos = Piece.blocks[i] + Piece.current_Pos;

            this.tileMap.SetTile(tilePos, Piece.current_Data.tile);
        }
    }

    public void Clear(FallingPiece Piece)
    {
        for (int i = 0; i < Piece.blocks.Length; i++)
        {
            Vector3Int tilePos = Piece.blocks[i] + Piece.current_Pos;

            this.tileMap.SetTile(tilePos, null);
        }
    }

    public bool isValidPosition(FallingPiece piece, Vector3Int toward)
    {
        RectInt bounds = this.Bounds;

        for (int i = 0; i < piece.blocks.Length; i++)
        {
            Vector3Int nextTilePos = piece.current_Pos + piece.blocks[i] + toward;

            if (!bounds.Contains((Vector2Int)nextTilePos))
            {
                Debug.Log($"nextTilePos:{nextTilePos}");
                Debug.Log("Bounds Fail!");
                return false;
            }

            if (this.tileMap.HasTile(nextTilePos))
            {
                Debug.Log($"currentTilePos:{piece.current_Pos + piece.blocks[i]}");
                Debug.Log($"nextTilePos:{nextTilePos}");
                Debug.Log("Tiles Fail!");
                return false;
            }
        }

        return true;
    }

    internal void ClearLines()
    {
        RectInt bounds = Bounds;
        int row = bounds.yMin;

        // 遍历当前范围中所有Tile,跳过未满行,直到遇到一个满行,才执行该行的清除.
        while(row < bounds.yMax)
        {
            if(IsLineFull(row))
            {
                LineClear(row);
            }
            else
            {
                row++;
            }
        }
    }

    private bool IsLineFull(int row)
    {
        RectInt bounds = Bounds;

        for(int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);

            if(!tileMap.HasTile(position))
            {
                return false;
            }
        }

        return true;
    }

    private void LineClear(int row)
    {
        RectInt bounds = Bounds;

        for(int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int position = new Vector3Int(col, row, 0);
            tileMap.SetTile(position, null);
        }
    }

}