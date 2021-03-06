using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ETD.PlayerControl
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] Transform myCamera = null;
        [SerializeField] float zoomOutCameraHeight = 35f;
        [SerializeField] float zoomOutCameraAngle = 65f;
        [SerializeField] float zoomInCameraHeight = 2.5f;
        [SerializeField] float zoomInCameraAngle = 10f;
        [SerializeField] float dragMovementMultiplyer = 20f;
        [SerializeField] float followMovementMultiplyer = .5f;

        float onePercentofZoomHightDifference;
        float onePercentofZoomAngleDifference;
        float zoomHeightSpeedFactor = 5f;
        float zoomAngleSpeedFactor = 20f;
        //keeps angle changes between 0-25% zoomOutCameraHeight, see exhibit A and B.
        float zoomHeightAngleDifferenceFacter;
        Vector3 mousePosOnClick;
        Vector3 cameraPosOnClick;
        //makes sure camera moves the same speed on x and y axis.
        Vector2 aspectRatioMultiplyer = new Vector2(16, 9);

        private void Start()
        {
            myCamera.position = new Vector3(myCamera.position.x, zoomOutCameraHeight, myCamera.position.z);
            myCamera.rotation = Quaternion.Euler(zoomOutCameraAngle, 0, 0);
            onePercentofZoomHightDifference = (zoomOutCameraHeight - zoomInCameraHeight) / 100;
            onePercentofZoomAngleDifference = (zoomOutCameraAngle - zoomInCameraAngle) / 100;
            zoomHeightAngleDifferenceFacter = zoomHeightSpeedFactor / zoomAngleSpeedFactor;
        }

        private void Update()
        {
            MovementControl();
            ZoomControl();
        }

        private void MovementControl()
        {
            KeyControl();
            //FollowCursor();
            DragControl();
        }

        private void KeyControl()
        {
            if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) 
            { myCamera.position += Vector3.left * followMovementMultiplyer; }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            { myCamera.position += Vector3.right * followMovementMultiplyer; }
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            { myCamera.position += Vector3.forward * followMovementMultiplyer; }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            { myCamera.position += Vector3.back * followMovementMultiplyer; }
        }

        private void FollowCursor()
        {
            Vector3 rawMousePos = myCamera.GetComponent<Camera>().ScreenToViewportPoint(Input.mousePosition);
            Vector3 MousePos = new Vector3(rawMousePos.x * 2 - 1, rawMousePos.y * 2 - 1, rawMousePos.z);
            if (MousePos.x >= 1) { myCamera.position += Vector3.right * followMovementMultiplyer; }
            if (MousePos.x <= -1) { myCamera.position += Vector3.left * followMovementMultiplyer; }
            if (MousePos.y >= 1) { myCamera.position += Vector3.forward * followMovementMultiplyer; }
            if (MousePos.y <= -1) { myCamera.position += Vector3.back * followMovementMultiplyer; }
        }

        private void DragControl()
        {
            if (Input.GetMouseButtonDown(1))
            {
                StoreInitialClickInformation();
            }
            if (Input.GetMouseButton(1))
            {
                DragToMoveCamera();
            }
        }

        private void StoreInitialClickInformation()
        {
            Vector3 rawMousePos = myCamera.GetComponent<Camera>().ScreenToViewportPoint(Input.mousePosition);
            mousePosOnClick = new Vector3
                (rawMousePos.x * aspectRatioMultiplyer.x, rawMousePos.y * aspectRatioMultiplyer.y, rawMousePos.z);
            cameraPosOnClick = myCamera.position;
        }

        private void DragToMoveCamera()
        {
            Vector3 rawMousePos = myCamera.GetComponent<Camera>().ScreenToViewportPoint(Input.mousePosition);
            Vector3 newMousePos = new Vector3
                (rawMousePos.x * aspectRatioMultiplyer.x, rawMousePos.y * aspectRatioMultiplyer.y, rawMousePos.z);
            Vector3 directionMoved = newMousePos - mousePosOnClick;
            myCamera.position = new Vector3
                (cameraPosOnClick.x + (-directionMoved.x * dragMovementMultiplyer),
                cameraPosOnClick.y,
                cameraPosOnClick.z + (-directionMoved.y * dragMovementMultiplyer));
        }

        private void ZoomControl()
        {
            if(Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                ZoomIn();
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                ZoomOut();
            }
        }

        private void ZoomIn()
        {
            ZoomInCameraHeightControl();            
            ZoomInCameraAngleControl();
        }

        private void ZoomOut()
        {
            ZoomOutCameraAngleControl();
            ZoomOutCameraHeightControl();
        }        
        
        private void ZoomInCameraAngleControl()
        {
            if (myCamera.rotation.eulerAngles.x < zoomInCameraAngle) { return; }
            if (myCamera.position.y > //exhibit A
                zoomOutCameraHeight * zoomHeightAngleDifferenceFacter + onePercentofZoomHightDifference) { return; }
            var rotationVector = myCamera.rotation.eulerAngles;
            rotationVector.x -= onePercentofZoomAngleDifference * zoomAngleSpeedFactor;
            rotationVector.x = Mathf.Max(rotationVector.x, zoomInCameraAngle);
            myCamera.rotation = Quaternion.Euler(rotationVector);
        }

        private void ZoomInCameraHeightControl()
        {
            if (myCamera.position.y > zoomInCameraHeight)
            {
                myCamera.position -= new Vector3(0, onePercentofZoomHightDifference * zoomHeightSpeedFactor, 0);
                if (myCamera.position.y < zoomInCameraHeight) { myCamera.position = new Vector3(0, zoomInCameraHeight, 0); }
                dragMovementMultiplyer -= 0.2f; //decreases camera movement speed when zoomed in
            }
        }

        private void ZoomOutCameraHeightControl()
        {
            if (myCamera.position.y < zoomOutCameraHeight)
            {
                myCamera.position += new Vector3(0, onePercentofZoomHightDifference * zoomHeightSpeedFactor, 0);
                if (myCamera.position.y > zoomOutCameraHeight) { myCamera.position = new Vector3(0, zoomOutCameraHeight, 0); }
                dragMovementMultiplyer += 0.2f; //increases camera movement speed when zoomed out
            }
        }

        private void ZoomOutCameraAngleControl()
        {
            if (myCamera.rotation.eulerAngles.x > zoomOutCameraAngle) { return; }
            if (myCamera.position.y > //exhibit B
                (zoomOutCameraHeight * zoomHeightAngleDifferenceFacter) + onePercentofZoomHightDifference) { return; }
            {
                var rotationVector = myCamera.rotation.eulerAngles;
                rotationVector.x += onePercentofZoomAngleDifference * zoomAngleSpeedFactor;
                rotationVector.x = Mathf.Min(rotationVector.x, zoomOutCameraAngle);
                myCamera.rotation = Quaternion.Euler(rotationVector);
            }
        }


    }
}
