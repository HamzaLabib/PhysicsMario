using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mario : Player
{
    protected override bool FacingRight { get; set; } = true;
    protected override void ProccessInput()
    {
        if (isAlive)
        {
            if (Input.GetKey(KeyCode.A))
                MoveLeft();
            if (Input.GetKey(KeyCode.D))
                MoveRight();
            if (Input.GetKeyDown(KeyCode.A))
                BurstLeft();
            if (Input.GetKeyDown(KeyCode.D))
                BurstRight();
            if (Input.GetKeyUp(KeyCode.A))
                DampenXLeft();
            if (Input.GetKeyUp(KeyCode.D))
                DampenXRight();
            if (Input.GetKey(KeyCode.S))
                MoveDown();
            if (Input.GetKeyDown(KeyCode.W))
                Jump();
            if (Input.GetKeyDown(KeyCode.Q))
                Roll(false);
            if (Input.GetKeyDown(KeyCode.E))
                Roll(true);
            if (Input.GetKeyDown(KeyCode.X))
                StartChargingBomb();
            if (Input.GetKeyUp(KeyCode.X))
                ThrowBomb();
        }
    }
}
