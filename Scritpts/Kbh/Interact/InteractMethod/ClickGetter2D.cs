using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ClickType
{
   Mouse,
   Aim
}

public class ClickGetter2D : InteractGetter<IInteractable>
{
   public static ClickGetter2D instance;

   public ClickType clickType = ClickType.Mouse;
   [Space]
   [SerializeField] private LayerMask whatIsItem;

   public IInteractable viewObject;
   public IInteractable selectedObject;

   public bool IsSelected => selectedObject is not null;

   public override void Update()
   {
      base.Update();

      IInteractable mouseOver = GetInteractByMouse();

      if (mouseOver != viewObject)
      {
         viewObject?.OnMouseOut();
         mouseOver?.OnMouseIn();
      }
      else
      {
         viewObject?.OnMouse();
      }

      viewObject = mouseOver;

   }

   protected override IInteractable Select() 
   {
      if (IsSelected) return null;

      selectedObject = viewObject;
      return selectedObject;
   }
   protected override IInteractable UnSelect()
   {
      if (!IsSelected) return null;
      IInteractable previousSelection = selectedObject;

      selectedObject = null;

      return previousSelection;
   }


   public IInteractable GetInteractByMouse()
   {
      IInteractable target = null;

      Ray ray = new Ray();
      if (clickType == ClickType.Mouse)
      {
         ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      }
      else if(clickType == ClickType.Aim)
      {
         ray = new Ray(
            Camera.main.transform.position,
            Camera.main.transform.forward);
      }

      RaycastHit hit;

      bool isHit = Physics.Raycast(ray, out hit, 20, whatIsItem);

      if(isHit)
         hit.collider?.TryGetComponent(out target);

      return target;
   }

}
