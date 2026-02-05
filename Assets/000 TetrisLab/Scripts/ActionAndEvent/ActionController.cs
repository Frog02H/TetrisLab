using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using TeterisLab;
using System;

public class ActionController : MonoBehaviour
{
    public static ActionController instance { get; private set; }

    private InputControls inputControls;
    private TetrisContext tetrisContext;

    public TetrisContext context_Controll
    {
        get
        {
            return tetrisContext;
        }

        set
        {
            this.tetrisContext = value;
        }
    }

    private ActionController() { }

    private void Awake()
    {
        if(instance == null || instance == this)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void OnEnable()
    {
        tetrisContext = new TetrisContext();
        inputControls = new InputControls();
        inputControls.Player_KeyBoard.Enable();
        inputControls.Player_KeyBoard.MoveToLeft.started += OnMoveToLeft;
        inputControls.Player_KeyBoard.MoveToRight.started += OnMoveToRight;
        inputControls.Player_KeyBoard.MoveToDown.started += OnMoveToDown;
        inputControls.Player_KeyBoard.HardDrop.started += OnHardDrop;
    }

    private void OnDisable()
    {
        inputControls.Player_KeyBoard.Disable();
        inputControls.Player_KeyBoard.MoveToLeft.started -= OnMoveToLeft;
        inputControls.Player_KeyBoard.MoveToRight.started -= OnMoveToRight;
        inputControls.Player_KeyBoard.MoveToDown.started -= OnMoveToDown;
        inputControls.Player_KeyBoard.HardDrop.started -= OnHardDrop;
    }

    private void OnMoveToLeft(InputAction.CallbackContext context)
    {
        Debug.Log("LEFT!");
        bool isMove = tetrisContext.fallingPiece.Move(Vector3Int.left);
        Debug.Log($"isMove:{isMove}");
    }

    private void OnMoveToRight(InputAction.CallbackContext context)
    {
        Debug.Log("RIGHT!");
        bool isMove = tetrisContext.fallingPiece.Move(Vector3Int.right);
        Debug.Log($"isMove:{isMove}");
    }

    private void OnMoveToDown(InputAction.CallbackContext context)
    {
        Debug.Log("DOWN!");
        bool isMove = tetrisContext.fallingPiece.Move(Vector3Int.down);
        Debug.Log($"isMove:{isMove}");
    }

    private void OnHardDrop(InputAction.CallbackContext context)
    {
        Debug.Log("HardDrop!");
        bool isMove = true;
        while(isMove)
        {
            isMove = tetrisContext.fallingPiece.Move(Vector3Int.down);
        }
        Debug.Log($"isMove:{isMove}");
    }

    private void OnRotateToLeft(InputAction.CallbackContext context)
    {
        Debug.Log("Rotate_To_Left!");
        
        switch(tetrisContext.fallingPiece.current_Data.tetrominoType)
        {
            case TetrominoType.I :
            case TetrominoType.O :
                float x = tetrisContext.fallingPiece.current_Pos.x - 0.5f;
                float y = tetrisContext.fallingPiece.current_Pos.y - 0.5f;
                
            break;
            
            default:

            break;
        }
        Debug.Log(":{Temp}");
    }
}
