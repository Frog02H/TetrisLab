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
        if (instance == null || instance == this)
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
        inputControls.Player_KeyBoard.MoveToLeft.performed += OnMoveToLeft;
        inputControls.Player_KeyBoard.MoveToRight.performed += OnMoveToRight;
        inputControls.Player_KeyBoard.MoveToDown.performed += OnMoveToDown;
        inputControls.Player_KeyBoard.HardDrop.performed += OnHardDrop;
        inputControls.Player_KeyBoard.RotateToLeft.performed += OnRotateToLeft;
        inputControls.Player_KeyBoard.RotateToRight.performed += OnRotateToRight;
    }

    private void OnDisable()
    {
        inputControls.Player_KeyBoard.Disable();
        inputControls.Player_KeyBoard.MoveToLeft.performed -= OnMoveToLeft;
        inputControls.Player_KeyBoard.MoveToRight.performed -= OnMoveToRight;
        inputControls.Player_KeyBoard.MoveToDown.performed -= OnMoveToDown;
        inputControls.Player_KeyBoard.HardDrop.performed -= OnHardDrop;
        inputControls.Player_KeyBoard.RotateToLeft.performed -= OnRotateToLeft;
        inputControls.Player_KeyBoard.RotateToRight.performed -= OnRotateToRight;
    }

    private void OnMoveToLeft(InputAction.CallbackContext context)
    {
        Debug.Log("LEFT!");
        // bool isMove = tetrisContext.fallingPiece.HandleMoveInput(Vector3Int.left);
        // tetrisContext.fallingPiece.HandleMoveInput(Vector3Int.left);
        tetrisContext.fallingPiece.HandleMoveInput(1);
        // 如果没有Updates
        // tetrisContext.board.Set(tetrisContext.fallingPiece);
        // Debug.Log($"isMove:{isMove}");
    }

    private void OnMoveToRight(InputAction.CallbackContext context)
    {
        Debug.Log("RIGHT!");
        // bool isMove = tetrisContext.fallingPiece.HandleMoveInput(Vector3Int.right);
        // tetrisContext.fallingPiece.HandleMoveInput(Vector3Int.right);
        tetrisContext.fallingPiece.HandleMoveInput(2);
        // Debug.Log($"isMove:{isMove}");
    }

    private void OnMoveToDown(InputAction.CallbackContext context)
    {
        Debug.Log("DOWN!");
        // bool isMove = tetrisContext.fallingPiece.HandleMoveInput(Vector3Int.down);
        // tetrisContext.fallingPiece.HandleMoveInput(Vector3Int.down);
        tetrisContext.fallingPiece.HandleMoveInput(0);
        // Debug.Log($"isMove:{isMove}");
    }

    private void OnHardDrop(InputAction.CallbackContext context)
    {
        Debug.Log("HardDrop!");
        // tetrisContext.fallingPiece.HardDrop();
        tetrisContext.fallingPiece.HandleHardDropInput();
        // Debug.Log($"isMove:{isMove}");
    }

    private void OnRotateToLeft(InputAction.CallbackContext context)
    {
        Debug.Log("Rotate_To_Left!");
        // tetrisContext.fallingPiece.Rotate(-1);
        tetrisContext.fallingPiece.HandleRotateInput(-1);
        }

    private void OnRotateToRight(InputAction.CallbackContext context)
    {
        Debug.Log("Rotate_To_Right!");
        // tetrisContext.fallingPiece.Rotate(1);
        tetrisContext.fallingPiece.HandleRotateInput(1);
    }

    /*     
    private void OnTimeUpdateStep(InputAction.CallbackContext context)
    {
        Debug.Log("TimeUpdate_Step!");
        tetrisContext.fallingPiece.TimeUpdate_Step();
    }

    private void OnTimeUpdateMove(InputAction.CallbackContext context)
    {
        Debug.Log("TimeUpdate_Move!");
        tetrisContext.fallingPiece.TimeUpdate_Move();
    }

    private void OnTimeUpdateLock(InputAction.CallbackContext context)
    {
        Debug.Log("TimeUpdate_Lock!");
        tetrisContext.fallingPiece.TimeUpdate_Lock(true);
    } 
    */

}
