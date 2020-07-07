using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public float ejectForce = 3; 
    void Start()
    {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.AddForce(Vector3.up * ejectForce + Random.insideUnitSphere * ejectForce * 0.5f, ForceMode.Impulse);
        rb.AddTorque(Random.insideUnitSphere * 200);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && !other.isTrigger ) // interagiert nicht mit dem richtigen trigger
        {
            LOBEvents.current.ItemPickup(Player.Item.ItemType.Key);
            Destroy(gameObject);
        }
    }
}
