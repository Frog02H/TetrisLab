using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TeterisLab;

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

        if(this.blocks == null)
        {
            this.blocks = new Vector3Int[tetrominoData.blocks.Length];
            // this.cells = Data.Cells[current_fallingPiece.tetrominoType];
        }

        for(int i = 0; i < this.blocks.Length; i++)
        {
            this.blocks[i] = (Vector3Int)tetrominoData.blocks[i];
        }
    }


}
