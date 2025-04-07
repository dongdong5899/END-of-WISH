using System;
using System.Collections.Generic;
using UnityEngine;

public class InteractGetter<T> : MonoBehaviour
   where T : IInteractable
{
   protected Action OnDuringHold;

   public virtual void Update()
   {
      OnDuringHold?.Invoke();
   }


   public virtual bool HoldInteract()
   {
      T value = Select();

      if (value is not null)
      {
         value.OnHold();
         OnDuringHold += value.DuringHold;

         return true;
      }
      else
      {
         return false;
      }
   } 

   public virtual bool ReleaseInteract()
   {
      T value = UnSelect();

      if (value is not null)
      {
         OnDuringHold -= value.DuringHold;
         value.Release();

         return true;
      }
      else
      {
         return false;
      }
   } 


   protected virtual T Select()
   {
      throw new System.Exception
         ($"[{gameObject}]의 Component 중, InteractGetter를 상속받고, 제대로 구현하지 않은 함수가 있습니다. ");
   }
   protected virtual T UnSelect()
   {
      throw new System.Exception
         ($"[{gameObject}]의 Component 중, InteractGetter를 상속받고, 제대로 구현하지 않은 함수가 있습니다. ");
   }

}
public static class InteractableHelper
{
   public static ItemInfo GetItemInfo(this IInteractable obj)
   {
      IItem result = (obj as IItem);

      if (result is not null) return result.ItemInfo;
      return new ItemInfo();
   }
}


public interface IInteractable
{
   void OnHold();
   void DuringHold();
   void Release();

   void OnMouseIn();
   void OnMouse();
   void OnMouseOut();
}


public interface IItem : IInteractable
{ public ItemInfo ItemInfo { get; } }

public struct ItemInfo { public string name; }