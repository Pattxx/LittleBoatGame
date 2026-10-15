using System;
using Unity.VisualScripting;
using UnityEngine;
[RequireComponent(typeof(ShipMovement))]
[RequireComponent(typeof(SailController))]
public class PlayerController : MonoBehaviour
{
   private ShipMovement _shipMovement;
   private SailController _sailController;
   private ShipContols _controls;


   private void Awake()
   {
      _controls = new ShipContols();
      _shipMovement = GetComponent<ShipMovement>();
      _sailController = GetComponent<SailController>();
   }

   private void OnEnable()
   {
      _controls.ShipControl.Enable();
   }

   private void OnDisable()
   {
      _controls.ShipControl.Disable();
   }

   private void Update()
   {
      //reading inputs
      float deltaTime = Time.deltaTime;
      float steer = _controls.ShipControl.Steer.ReadValue<float>();
      float changeSailAngle = _controls.ShipControl.ChangeSailAngle.ReadValue<float>();
      float openSail = _controls.ShipControl.OpenCloseSail.ReadValue<float>();
      
      // loading input
      _sailController.FixOpen(openSail, deltaTime);
      _sailController.FixTrim(changeSailAngle, deltaTime);
      _shipMovement.Steer(steer, deltaTime);
      _shipMovement.SimulateShipMovement(deltaTime);
   }
}
