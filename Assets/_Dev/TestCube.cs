using UnityEngine;
using InteractionSystem.Runtime.Core; 

public class TestCube : BaseInteractable
{
    public override void OnInteract()
    {
        Debug.Log("Test Cube Etkileþimi Baþarýlý!");
    }
}