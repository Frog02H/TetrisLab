using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TeterisLab;
using System;
using Unity.VisualScripting;
using System.Threading;
using UnityEditor.Rendering;

public class FallingPiece : MonoBehaviour
{
    public Board main_board { get; private set; }
    public TetrominoData current_Data { get; private set; }
    public Vector3Int current_Pos { get; private set; }

    public Vector3Int[] blocks { get; private set; }

    public int rotationIndex { get; private set; }

    public float stepDelay = 1f;
    public float moveDelay = 0.5f;
    public float lockDelay = 0.5f;

    private float stepTime;
    private float moveTime;
    private float lockTime;

    public int moveDirection = -1;
    public int rotateDirection = 0;
    public bool isStep = true;
    public bool isHardDrop = false;

    public void Initialize(Board board, Vector3Int pos, TetrominoData tetrominoData)
    {
        this.main_board = board;
        this.current_Pos = pos;
        this.current_Data = tetrominoData;
        this.rotationIndex = 0;

        TimeUpdate_Step();
        TimeUpdate_Move();
        TimeUpdate_Lock(false);

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

    public void FallingPieceUpdate()
    {
        this.TimeUpdate_Lock(true);

        if(rotateDirection != 0)
        {
            Rotate(rotateDirection);
        }

        if(isHardDrop)
        {
            HardDrop();
        }

        if(moveDirection != -1)
        {
            if(Time.time > moveTime)
            {
                TryToMoving(moveDirection);
            }
        }

        if (isStep)
        {
            if (Time.time > stepTime)
            {
                this.Step();
            }
        }
    }

    private void Lock()
    {
        main_board.Set(this);
        main_board.ClearLines();
        main_board.SpawnPiece();
    }

    private bool Move(Vector3Int toward)
    {
        // this.main_board.Clear(this);

        bool valid = this.main_board.isValidPosition(this, toward);

        Debug.Log($"valid:{valid}");

        // 避免 玩家移动操作 和 自动下移 冲突
        if (valid)
        {
            this.current_Pos += toward;
            this.TimeUpdate_Move();
            this.TimeUpdate_Lock(false);
        }

        return valid;
    }

    private void Step()
    {
        TimeUpdate_Step();

        Debug.Log("/////////////// Step ///////////////");
        Move(Vector3Int.down);
        Debug.Log("/////////////// Step_End /////////////");

        if (lockTime >= lockDelay)
        {
            Lock();
        }
    }

    public void HandleMoveInput(int toward)
    {
        moveDirection = toward;
    }

    public void HandleHardDropInput()
    {
        isHardDrop = true;
    }

    public void HandleRotateInput(int toward)
    {
        if(toward > 0)
        {
            toward = 1;
        }
        else if(toward < 0)
        {
            toward = -1;
        }

        rotateDirection = toward;
    }

    private bool TryToMoving(int toward)
    {   
        bool isMove = false;

        // isStep = false;

        // Vector3Int moveToward = Vector3Int.zero;

        if (Time.time > moveTime)
        {

        switch(toward)
        {
            case -1:
                // moveToward = Vector3Int.zero;
                isMove = false;
            break;
            case 0:
                // moveToward = Vector3Int.down;
                isMove = this.Move(Vector3Int.down);
                TimeUpdate_Step();
            break;
            case 1:
                // moveToward = Vector3Int.left;
                isMove = this.Move(Vector3Int.left);
            break;
            case 2:
                // moveToward= Vector3Int.right;
                isMove = this.Move(Vector3Int.right);
            break;
            default:
                isMove = false;
            break;
        }

        /* 
        if (Time.time > moveTime)
        {
            isMove = this.Move(moveToward);

            // if (isMove && moveToward == Vector3Int.down)
            if (isMove && toward == 0)
            {
                TimeUpdate_Step();
            }
        } 
        */
        }

        // isStep = true;

        this.moveDirection = -1;

        return isMove;
    }

    public void HardDrop()
    {
        while (this.Move(Vector3Int.down))
        {
            continue;
        }
        
        isHardDrop = false;

        this.Lock();
    }

    public void Rotate(int direction)
    {
        // this.main_board.Clear(this);

        int originalRotationIndex = this.rotationIndex;

        this.rotationIndex = this.Wrap(this.rotationIndex + direction, 0, 4);

        ApplyRotationMatrix(direction);

        if (!TestWallKicks(this.rotationIndex, direction))
        {
            this.rotationIndex = originalRotationIndex;
            ApplyRotationMatrix(-direction);
        }

        this.rotateDirection = 0;
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

    public void TimeUpdate_Step()
    {
        stepTime = Time.time + stepDelay;
    }

    public void TimeUpdate_Move()
    {
        moveTime = Time.time + stepDelay;
    }

    public void TimeUpdate_Lock(bool isReset)
    {
        if (isReset)
        {
            lockTime += Time.deltaTime;
        }
        else
        {
            lockTime = 0f;
        }
    }
}
