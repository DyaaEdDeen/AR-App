using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

[RequireComponent(typeof(ARRaycastManager), typeof(ARPlaneManager))]
public class AddObject : MonoBehaviour {

    [SerializeField] private GameObject prefab;

    private ARRaycastManager aRRaycastManager;
    private ARPlaneManager aRPlaneManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private List<GameObject> gameObjects = new List<GameObject>();

    private GameObject selectedObject = null;
    private bool canMove = true;
    private bool isMoving;

    private bool canRotate;
    private float rotationSpeed = 100f;
    private float initialRotationAngle;
    private bool isRotating;

    private bool canScale;
    private float initialDistance;
    private Vector3 initialScale;
    private bool isScaling;

    private void Awake() {
        aRRaycastManager = GetComponent<ARRaycastManager>();
        aRPlaneManager = GetComponent<ARPlaneManager>();
    }

    private void OnEnable() {
        EnhancedTouch.TouchSimulation.Enable();
        EnhancedTouch.EnhancedTouchSupport.Enable();
        EnhancedTouch.Touch.onFingerDown += FingerDown;
        EnhancedTouch.Touch.onFingerMove += FingerMove;
        EnhancedTouch.Touch.onFingerUp += FingerUp;
    }

    private void FingerUp(Finger finger) {
        if (selectedObject != null) {
            selectedObject = null;
        }
        if (isRotating) {
            isRotating = false;
        }
        if (isScaling) {
            isScaling = false;
        }
        if (isMoving) {
            isMoving = false;
        }
    }

    private void FingerMove(Finger finger) {
        if (selectedObject != null && !isRotating) {
            if (aRRaycastManager.Raycast(finger.screenPosition, hits, TrackableType.PlaneWithinPolygon)) {
                Pose hitPose = hits[0].pose;
                selectedObject.transform.position = hitPose.position; // Move object
            }
        }
    }

    private void Update() {
        if (canScale) {
            Scale();
        }
        if (canRotate) {
            Rotate();
        }

    }

    private void Rotate() {
        if (Touch.activeTouches.Count == 2 && selectedObject != null) {
            Touch touch1 = Touch.activeTouches[0];
            Touch touch2 = Touch.activeTouches[1];

            // Detect if the two-finger gesture just began
            if (touch1.phase == UnityEngine.InputSystem.TouchPhase.Began || touch2.phase == UnityEngine.InputSystem.TouchPhase.Began) {
                // Calculate the initial angle between the two fingers
                initialRotationAngle = GetAngleBetweenTouches(touch1.screenPosition, touch2.screenPosition);
                isRotating = true;
            } else if (isRotating && (touch1.phase == UnityEngine.InputSystem.TouchPhase.Moved || touch2.phase == UnityEngine.InputSystem.TouchPhase.Moved)) {
                // Calculate the current angle between the two fingers
                float currentRotationAngle = GetAngleBetweenTouches(touch1.screenPosition, touch2.screenPosition);

                // Apply the delta rotation to the object
                float angleDifference = currentRotationAngle - initialRotationAngle;
                selectedObject.transform.Rotate(Vector3.up, angleDifference, Space.World);

                // Update the initial rotation angle for smooth rotation
                initialRotationAngle = currentRotationAngle;
            } else if (touch1.phase == UnityEngine.InputSystem.TouchPhase.Ended || touch2.phase == UnityEngine.InputSystem.TouchPhase.Ended) {
                // Reset the rotating flag when fingers are lifted
                isRotating = false;
            }
        }
    }

    private void Scale() {
        if (Touch.activeTouches.Count == 2 && selectedObject != null) {
            Touch touch1 = Touch.activeTouches[0];
            Touch touch2 = Touch.activeTouches[1];

            // If both touches just began
            if (touch1.phase == UnityEngine.InputSystem.TouchPhase.Began || touch2.phase == UnityEngine.InputSystem.TouchPhase.Began) {
                initialDistance = Vector2.Distance(touch1.screenPosition, touch2.screenPosition);
                initialScale = selectedObject.transform.localScale;
                isScaling = true;
            } else if (touch1.phase == UnityEngine.InputSystem.TouchPhase.Moved || touch2.phase == UnityEngine.InputSystem.TouchPhase.Moved) {
                // Calculate the new scale based on the pinch gesture
                float currentDistance = Vector2.Distance(touch1.screenPosition, touch2.screenPosition);
                if (Mathf.Approximately(initialDistance, 0)) return;

                float scaleFactor = currentDistance / initialDistance;
                selectedObject.transform.localScale = initialScale * scaleFactor;
            }
        }
    }

    private float GetAngleBetweenTouches(Vector2 pos1, Vector2 pos2) {
        Vector2 direction = pos2 - pos1;
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }

    public void EnableMovement() {
        canMove = true;
        canRotate = false;
        canScale = false;
    }

    public void EnableRotation() {
        canMove = false;
        canRotate = true;
        canScale = false;
    }

    public void EnableScale() {
        canMove = false;
        canRotate = false;
        canScale = true;
    }

    public void ClearAllObjects() {
        if (gameObjects.Count == 0) return;
        foreach (GameObject go in gameObjects) {
            Destroy(go);
        }
        gameObjects.Clear();
    }

    private void FingerDown(Finger finger) {
        if (finger.index != 0) return;

        Ray ray = Camera.main.ScreenPointToRay(finger.currentTouch.screenPosition);
        RaycastHit hitObject;

        if (Physics.Raycast(ray, out hitObject)) {
            if (hitObject.transform.tag == "Trackable") {
                selectedObject = hitObject.transform.gameObject;
                return;
            }
        }

        if (aRRaycastManager.Raycast(finger.currentTouch.screenPosition, hits, TrackableType.PlaneWithinPolygon) && canMove) {
            isMoving = true;
            foreach (ARRaycastHit hit in hits) {
                Pose pose = hit.pose;
                Quaternion rotation = pose.rotation;
                rotation.y = Mathf.Deg2Rad * 180;
                GameObject obj = Instantiate(prefab, pose.position, rotation);
                gameObjects.Add(obj);
            }
        }
    }

    private void OnDisable() {
        EnhancedTouch.TouchSimulation.Enable();
        EnhancedTouch.EnhancedTouchSupport.Enable();
        EnhancedTouch.Touch.onFingerDown -= FingerDown;
        EnhancedTouch.Touch.onFingerMove -= FingerMove;
        EnhancedTouch.Touch.onFingerUp -= FingerUp;
    }
}
