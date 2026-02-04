using UnityEngine;
using UnityEngine.Tilemaps;
using TeterisLab;

public class Board : MonoBehaviour
{
    public Tilemap tileMap { get; private set; }
    public TetrominoData[] tetrominos;

    public FallingPiece fallingPiece;
    public Vector3Int spawnPos;

    private void Awake()
    {
        this.tileMap = GetComponentInChildren<Tilemap>();
        this.fallingPiece = GetComponentInChildren<FallingPiece>();

        for(int i = 0; i < this.tetrominos.Length; i++)
        {
            this.tetrominos[i].Initialize();
        }
    }

    private void Start()
    {
        SpawnPiece();
    }

    public void SpawnPiece()
    {
        int pieceIndex = Random.Range(0, this.tetrominos.Length);

        TetrominoData tetromino = this.tetrominos[pieceIndex];

        this.fallingPiece.Initialize(this, spawnPos, tetromino);
        this.Set(fallingPiece);
    }

    public void Set(FallingPiece Piece)
    {
        for(int i = 0; i < Piece.blocks.Length; i++)
        {
            Vector3Int tilePos = Piece.blocks[i] + Piece.current_Pos;

            this.tileMap.SetTile(tilePos, Piece.current_Data.tile);
        }
    }
}