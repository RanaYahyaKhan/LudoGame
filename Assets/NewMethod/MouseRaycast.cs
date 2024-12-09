using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRaycast : MonoBehaviour
{
    void Update()
    {
        //if (Input.GetMouseButtonDown(0)) // Left mouse button
        //{
        //    Debug.Log("mouse move");
        //    // Convert mouse position to world space
        //    Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //    // Check if the mouse clicked on this sprite's collider
        //    RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
        //    Debug.Log("mouse move" + hit.transform.position);

        //    if (hit.collider != null) // Check if the ray hit a collider
        //    {
        //        Debug.Log($"Hit object: {hit.collider.gameObject.name}, Position: {hit.transform.position}");

        //        if (hit.collider.gameObject == gameObject) // Check if it's this GameObject
        //        {
        //            OnSpriteClicked();
        //        }
        //    }
        //    else
        //    {
        //        Debug.Log("No object hit at mouse position.");
        //    }
        //}
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null)
            {
                Debug.Log("CLICKED " + hit.collider.name);
            }
        }
    }

    void OnSpriteClicked()
    {
        Debug.Log($"Sprite {gameObject.name} was clicked!");
        // Add your custom logic here
    }
}
