using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TeterisLab
{
    public class TetrisContext
    {
        public Board board;
        public FallingPiece fallingPiece;
        
        public void Init(Board newBoard, FallingPiece newFallingPiece)
        {
            this.board = newBoard;
            this.fallingPiece = newFallingPiece;
        }
    }
}
