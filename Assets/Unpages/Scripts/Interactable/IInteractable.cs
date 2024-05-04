using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public void Interact(InteractorData interactorData);
    public void UnInteract();


}
