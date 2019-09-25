using System.Collections;
using System.Collections.Generic;
using Asteroids;
using UnityEngine;

public class TinyTank : GameUnit
{
    public delegate void TinyTankDeathDelagate();

    public static event TinyTankDeathDelagate OnTankDeath;

    public static TinyTank Create(GameObject prefab)
    {
        GameObject clone = Instantiate(prefab);
        var existingShip = clone.GetComponent<TinyTank>();
        return existingShip ? existingShip : clone.AddComponent<TinyTank>();
    }  
    public virtual bool IsAlive => gameObject.activeSelf;

    public override void Spawn()
    {
        if (!IsAlive)
        {
            _health.Revive();
            ResetPosition();
            gameObject.SetActive(true);
            ResetRigidbody();
        }
    }

    public virtual void Recover()
    {

    }
    public void ResetPosition()
    {
        //transform.position = GameField.GetRandomWorldPositionXY(transform);
        transform.rotation = Quaternion.identity;
    }

    public void ResetRigidbody()
    {
        var rb = GetComponent<Rigidbody2D>();
        rb.rotation = 0;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = 0;
    }
        
    public void EnableControls()
    {
//        GetComponent<ShipMovement>().enabled = true;
//        GetComponent<ShipShooter>().enabled = true;
    }

    public void DisableControls()
    {
//        GetComponent<ShipMovement>().enabled = false;
//        GetComponent<ShipShooter>().enabled = false;
    }

    protected override void RequestDestruction()
    {
        DisableControls();
        OnOnShipDeath();
        gameObject.SetActive(false);
    }


    protected virtual void OnOnShipDeath()
    {
        OnTankDeath?.Invoke();
    }
}
