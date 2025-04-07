using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPartialCompoent
{
   MonoBehaviour Owner { get; set; }
   void SetOwner(MonoBehaviour owner);
}
