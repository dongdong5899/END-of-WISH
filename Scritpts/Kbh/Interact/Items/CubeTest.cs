using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeTest : MonoBehaviour, IItem
{
   [SerializeField] private ItemInfo itemInfo;
   public ItemInfo ItemInfo => itemInfo;

   [SerializeField] ClickGetter2D umm;

   private void Update()
   {
      if (Input.GetKeyDown(KeyCode.Z))
      {
         umm.HoldInteract();
      }
      if (Input.GetKeyDown(KeyCode.X))
      {
         umm.ReleaseInteract();
      }
   }

   public void DuringHold()
   { 

   }
   public void OnHold()
   {
   }

   public void Release()
   {

   }

   public void OnMouse()
   {

   }

   public void OnMouseIn()
   {

   }

   public void OnMouseOut()
   {

   }

}
