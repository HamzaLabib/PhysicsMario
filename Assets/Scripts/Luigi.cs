using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Luigi : Player
{
    protected override bool FacingRight { get; set; } = false;
    protected override void ProccessInput()
    {
        if (isAlive)
        {
            if (Input.GetKey(KeyCode.LeftArrow))
                MoveLeft();
            if (Input.GetKey(KeyCode.RightArrow))
                MoveRight();
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                BurstLeft();
            if (Input.GetKeyDown(KeyCode.RightArrow))
                BurstRight();
            if (Input.GetKeyUp(KeyCode.LeftArrow))
                DampenXLeft();
            if (Input.GetKeyUp(KeyCode.RightArrow))
                DampenXRight();
            if (Input.GetKey(KeyCode.DownArrow))
                MoveDown();
            if (Input.GetKeyDown(KeyCode.UpArrow))
                Jump();
            if (Input.GetKeyDown(KeyCode.RightControl))
                Roll(false);
            if (Input.GetKeyDown(KeyCode.RightShift))
                Roll(true);
            if (Input.GetKeyDown(KeyCode.Return))
                StartChargingBomb();
            if (Input.GetKeyUp(KeyCode.Return))
                ThrowBomb();
        }
    }
}
