using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TeterisLab;
using System;

public class FallingPiece : MonoBehaviour
{
    public Board main_board { get; private set; }
    public TetrominoData current_Data { get; private set; }
    public Vector3Int current_Pos { get; private set; }

    public Vector3Int[] blocks { get; private set; }

    public void Initialize(Board board, Vector3Int pos, TetrominoData tetrominoData)
    {
        this.main_board = board;
        this.current_Pos = pos;
        this.current_Data = tetrominoData;

        ActionController.instance.context_Controll.Init(board, this);

        if (this.blocks == null)
        {
            this.blocks = new Vector3Int[tetrominoData.blocks.Length];
            // this.cells = Data.Cells[current_fallingPiece.tetrominoType];
        }

        for (int i = 0; i < this.blocks.Length; i++)
        {
            this.blocks[i] = (Vector3Int)tetrominoData.blocks[i];
        }
    }

    public bool Move(Vector3Int toward)
    {
        this.main_board.Clear(this);

        bool valid = this.isValidPosition(this, toward);

        Debug.Log($"valid:{valid}");

        if (valid)
        {
            this.current_Pos += toward;
        }

        return valid;
    }

    private bool isValidPosition(FallingPiece piece, Vector3Int toward)
    {
        for (int i = 0; i < piece.blocks.Length; i++)
        {
            Vector3Int nextTilePos = piece.current_Pos + piece.blocks[i] + toward;

            if (!piece.main_board.Bounds.Contains((Vector2Int)nextTilePos))
            {
                Debug.Log($"nextTilePos:{nextTilePos}");
                Debug.Log("Bounds Fail!");
                return false;
            }

            if (piece.main_board.tileMap.HasTile(nextTilePos))
            {
                Debug.Log($"currentTilePos:{piece.current_Pos + piece.blocks[i]}");
                Debug.Log($"nextTilePos:{nextTilePos}");
                Debug.Log("Tiles Fail!");
                return false;
            }
        }

        return true;
    }
}
