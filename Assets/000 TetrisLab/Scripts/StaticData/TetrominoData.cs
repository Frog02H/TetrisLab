using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TeterisLab
{
    public enum TetrominoType
    {
        I,
        O,
        L,
        J,
        T,
        S,
        Z,
    }

    [System.Serializable]
    public class TetrominoData
    {
        public TetrominoType tetrominoType;
        public Tile tile;
        public Vector2Int[] blocks { get; private set; }

        public void Initialize()
        {
            this.blocks = Data.Cells[this.tetrominoType];

        }
    }
}
