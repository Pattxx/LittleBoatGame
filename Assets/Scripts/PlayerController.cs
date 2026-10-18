using System;
using Unity.VisualScripting;
using UnityEngine;
//Translates player input into ship and sail controls
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
      
      float deltaTime = Time.deltaTime;
      //Pooling 1D axis values from input composite bindings (-1 to 1)
      float steer = _controls.ShipControl.Steer.ReadValue<float>();
      float changeSailAngle = _controls.ShipControl.ChangeSailAngle.ReadValue<float>();
      float openSail = _controls.ShipControl.OpenCloseSail.ReadValue<float>();
      
      //Feeding the input to sail and hull motion solvers
      _sailController.AdjustSailOpen(openSail, deltaTime);
      _sailController.AdjustSailTrim(changeSailAngle, deltaTime);
      _shipMovement.Steer(steer, deltaTime);
      _shipMovement.SimulateShipMovement(deltaTime);
   }
}
