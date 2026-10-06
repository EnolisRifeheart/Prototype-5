using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerGridController))]
public class Player : MonoBehaviour
{
    PlayerGridController controller;

    void Start()
    {
        controller = GetComponent<PlayerGridController>();

        // Find the Battle Manager after Player loads into the scene.
        BattleManager battleManager = FindFirstObjectByType<BattleManager>();

        if (battleManager != null)
        {
            battleManager.FindPlayer(this);
        }
        else
        {
            Debug.LogError("Player could not find Battle Manager.");
        }
    }

    void Update()
    {
        if (Keyboard.current.wKey.wasPressedThisFrame)
        {
            controller.MoveForward();
        }

        if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            controller.MoveBackward();
        }

        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            controller.MoveLeft();
        }

        if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            controller.MoveRight();
        }

        // Turn 90 degrees.
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            controller.RotateLeft();
        }

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            controller.RotateRight();
        }
    }
}