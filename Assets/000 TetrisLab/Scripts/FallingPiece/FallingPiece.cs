using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TeterisLab;
using System;
using Unity.VisualScripting;

public class FallingPiece : MonoBehaviour
{
    public Board main_board { get; private set; }
    public TetrominoData current_Data { get; private set; }
    public Vector3Int current_Pos { get; private set; }

    public Vector3Int[] blocks { get; private set; }

    public int rotationIndex { get; private set; }

    public bool isReset = false;

    public void Initialize(Board board, Vector3Int pos, TetrominoData tetrominoData)
    {
        this.main_board = board;
        this.current_Pos = pos;
        this.current_Data = tetrominoData;
        this.rotationIndex = 0;

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

        isReset = valid;

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

    public void Rotate(int direction)
    {
        this.main_board.Clear(this);

        int originalRotationIndex = this.rotationIndex;

        this.rotationIndex = this.Wrap(this.rotationIndex + direction, 0, 4);

        ApplyRotationMatrix(direction);

        if (!TestWallKicks(this.rotationIndex, direction))
        {
            this.rotationIndex = originalRotationIndex;
            ApplyRotationMatrix(-direction);
        }
    }

    private void ApplyRotationMatrix(int direction)
    {
        float[] matrix = Data.RotationMatrix;

        for (int i = 0; i < this.blocks.Length; i++)
        {
            Vector3 block = this.blocks[i];

            int x, y;

            switch (this.current_Data.tetrominoType)
            {
                case TetrominoType.I:
                case TetrominoType.O:
                    block.x -= 0.5f;
                    block.y -= 0.5f;
                    x = Mathf.CeilToInt((block.x * matrix[0] * direction) + (block.y * matrix[1] * direction));
                    y = Mathf.CeilToInt((block.x * matrix[2] * direction) + (block.y * matrix[3] * direction));
                    break;

                default:
                    x = Mathf.RoundToInt((block.x * matrix[0] * direction) + (block.y * matrix[1] * direction));
                    y = Mathf.RoundToInt((block.x * matrix[2] * direction) + (block.y * matrix[3] * direction));
                    break;
            }

            this.blocks[i] = new Vector3Int(x, y, 0);
        }
    }

    private bool TestWallKicks(int rotationIndex, int rotationDirection)
    {
        int wallKickIndex = GetWallKickIndex(rotationIndex, rotationDirection);

        for (int i = 0; i < this.current_Data.wallKicks.GetLength(1); i++)
        {
            Vector2Int translation = this.current_Data.wallKicks[wallKickIndex, i];

            if (Move((Vector3Int)translation))
            {
                return true;
            }
        }

        return false;
    }

    private int GetWallKickIndex(int rotationIndex, int rotationDirection)
    {
        int wallKickIndex = rotationIndex * 2;

        if (rotationDirection < 0)
        {
            wallKickIndex--;
        }

        return Wrap(wallKickIndex, 0, this.current_Data.wallKicks.GetLength(0));
    }

    private int Wrap(int input, int min, int max)
    {
        if (input < min)
        {
            return max - (min - input) % (max - min);
        }
        else
        {
            return min + (input - min) % (max - min);
        }
    }
}
