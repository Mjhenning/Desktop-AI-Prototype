using System;
using UnityEngine;
using UnityEngine.InputSystem;

public enum DraggedDirection {
    Up,
    Down,
    Right,
    Left
}

public class SwipeHandler : MonoBehaviour
{
    public static SwipeHandler instance;

    [SerializeField] float swipeResistance = 100;
    Vector2 startTouchPosition;
    Vector2 currentTouchPosition => UI_Manager.instance.input.Player.Swipe.ReadValue<Vector2>();

    void OnEnable()
    {
        UI_Manager.instance.input.Player.Click.performed += OnSwipeStarted;
        UI_Manager.instance.input.Player.Click.canceled += OnSwipePerformed;
    }

    void OnDisable()
    {
        UI_Manager.instance.input.Player.Click.performed -= OnSwipeStarted;
        UI_Manager.instance.input.Player.Click.canceled -= OnSwipePerformed;
    }

    public void OnSwipeStarted(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // Store the start position when the swipe starts
            startTouchPosition = currentTouchPosition;
        }
    }

    public void OnSwipePerformed(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            Vector2 endTouchPosition = currentTouchPosition;

            Vector2 delta = endTouchPosition - startTouchPosition;
            Vector2 direction = Vector2.zero;

            if (Mathf.Abs(delta.x) > swipeResistance)
            {
                direction.x = Mathf.Clamp(delta.x, -1, 1);
            }
            if (Mathf.Abs(delta.y) > swipeResistance)
            {
                direction.y = Mathf.Clamp(delta.y, -1, 1);
            }

            if (direction != Vector2.zero)
            {
                DraggedDirection draggedDirection = GetDragDirection(direction);
                // Trigger the event
                EventsManager.HandleSwipeDirection(draggedDirection);
            }
        }
    }

    DraggedDirection GetDragDirection(Vector2 dragVector)
    {
        float positiveX = Mathf.Abs(dragVector.x);
        float positiveY = Mathf.Abs(dragVector.y);
        DraggedDirection draggedDir;
        if (positiveX > positiveY)
        {
            draggedDir = (dragVector.x > 0) ? DraggedDirection.Right : DraggedDirection.Left;
        }
        else
        {
            draggedDir = (dragVector.y > 0) ? DraggedDirection.Up : DraggedDirection.Down;
        }
        return draggedDir;
    }
}
