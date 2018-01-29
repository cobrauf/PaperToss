//-----------------------------------------------------------------------
// <copyright file="HelloARController.cs" company="Google">
//
// Copyright 2017 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------

namespace GoogleARCore.HelloAR
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Rendering;
    using GoogleARCore;
    using UnityEngine.UI;


    /// <summary>
    /// Controlls the HelloAR example.
    /// </summary>
    public class HelloARController : MonoBehaviour
    {
        /// <summary>
        /// The first-person camera being used to render the passthrough camera.
        /// </summary>
        public Camera m_firstPersonCamera;

        /// <summary>
        /// A prefab for tracking and visualizing detected planes.
        /// </summary>
        public GameObject m_trackedPlanePrefab;

        /// <summary>
        /// A model to place when a raycast from a user touch hits a plane.
        /// </summary>
        //public GameObject m_andyAndroidPrefab;

        //-------------------------------------------DY Variables
        public GameObject sceneParent;
        public Text lookAroundText;//tell players to pan the camera
        public Text placeSceneText;//tell players to place the game scene
        public Text suggestResetSceneText;//suggest reset scene if does not detect scene present
        public Text timeoutText;//tells user taking too long to scan

        public Canvas inGameCanvas; //will turn this off if suggestResetSceneText activates
        public GameObject tooClose2Text3D;//will not place scene if too close text active
        public GameObject scanLinesGO;//vertical and horizontal lines for scanning effect

        private int sceneUpdateCount, sceneDetectionCount, timeoutCount;//used to trigger these events at intervals
		private bool shouldShowSceneParent;
		private bool sceneParentActive;
		private bool showSearchBar;
        private bool sceneDetectionActive;

        /// <summary>
        /// A gameobject parenting UI for displaying the "searching for planes" snackbar.
        /// </summary>
        public GameObject m_searchingForPlaneUI;

        private List<TrackedPlane> m_newPlanes = new List<TrackedPlane>();

        private List<TrackedPlane> m_allPlanes = new List<TrackedPlane>();

        private Color[] m_planeColors = new Color[] {
            new Color(1.0f, 1.0f, 1.0f),
            new Color(0.956f, 0.262f, 0.211f),
            new Color(0.913f, 0.117f, 0.388f),
            new Color(0.611f, 0.152f, 0.654f),
            new Color(0.403f, 0.227f, 0.717f),
            new Color(0.247f, 0.317f, 0.709f),
            new Color(0.129f, 0.588f, 0.952f),
            new Color(0.011f, 0.662f, 0.956f),
            new Color(0f, 0.737f, 0.831f),
            new Color(0f, 0.588f, 0.533f),
            new Color(0.298f, 0.686f, 0.313f),
            new Color(0.545f, 0.764f, 0.290f),
            new Color(0.803f, 0.862f, 0.223f),
            new Color(1.0f, 0.921f, 0.231f),
            new Color(1.0f, 0.756f, 0.027f)
        };

        /// <summary>
        /// The Unity Update() method.
        /// </summary>
        
		void Start () {
            lookAroundText.enabled = false;
            placeSceneText.enabled = false;
            timeoutText.enabled = false;
            suggestResetSceneText.enabled = false;
            scanLinesGO.SetActive(false);

#if UNITY_EDITOR
            
#else
            //turns scene parent off if not in Unity editor
            sceneParent.SetActive (false);
#endif
        }

        #region Events
        void OnEnable()
        {            
            EventsManager.OnSceneResetEvent += SceneResetActions;
            EventsManager.OnScenePlacedEvent += ScenePlacedActions;
        }

        void OnDisable()
        {            
            EventsManager.OnSceneResetEvent -= SceneResetActions;
            EventsManager.OnScenePlacedEvent -= ScenePlacedActions;
        }

        void ScenePlacedActions()
        {           
            timeoutText.enabled = false;
        }

        void SceneResetActions ()
        {
            sceneParent.SetActive(false);
            suggestResetSceneText.enabled = false;
            timeoutText.enabled = false;
        }        
        #endregion

        public void Update ()
        {
			//-------------------------------------------DY Mod Start
			//these if statements determine whether we should show the scene parent or not, also suggest reset scene
			if (!GameManager.instance.inPlayMode)
			{
                shouldShowSceneParent = false;
                sceneDetectionActive = false;
            }
            else if (GameManager.instance.inPlayMode && !GameManager.instance.scenePlaced)
			{
                shouldShowSceneParent = true;
                timeoutCount++;//keep track of timeout timer
                sceneDetectionActive = false;
            }
            else if (GameManager.instance.inPlayMode && GameManager.instance.scenePlaced)
			{
                shouldShowSceneParent = false;
                sceneDetectionActive = true;
			}

			#region Keyboard testing
			if (Input.GetKeyDown(KeyCode.F))
			{
				if (shouldShowSceneParent)
				{
					sceneParent.transform.position = new Vector3(0f, -1f, 3.5f);
					sceneParent.transform.LookAt(m_firstPersonCamera.transform);
					sceneParent.transform.rotation = Quaternion.Euler(0.0f,
						sceneParent.transform.rotation.eulerAngles.y, sceneParent.transform.rotation.z);
					sceneParent.SetActive(true);
					sceneParentActive = true;
					EventsManager.ScenePlaced();					
				}
			}          
            #endregion
            //-------------------------------------------DY Mod End


            #region More Original Code

            _QuitOnConnectionErrors();

            // The tracking state must be FrameTrackingState.Tracking in order to access the Frame.
            if (Frame.TrackingState != FrameTrackingState.Tracking)
            {
                const int LOST_TRACKING_SLEEP_TIMEOUT = 15;
                Screen.sleepTimeout = LOST_TRACKING_SLEEP_TIMEOUT;
                return;
            }

            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Frame.GetNewPlanes(ref m_newPlanes);

            // Iterate over planes found in this frame and instantiate corresponding GameObjects to visualize them.
            for (int i = 0; i < m_newPlanes.Count; i++)
            {
                // Instantiate a plane visualization prefab and set it to track the new plane. The transform is set to
                // the origin with an identity rotation since the mesh for our prefab is updated in Unity World
                // coordinates.
                GameObject planeObject = Instantiate(m_trackedPlanePrefab, Vector3.zero, Quaternion.identity,
                    transform);
                planeObject.GetComponent<TrackedPlaneVisualizer>().SetTrackedPlane(m_newPlanes[i]);

                // Apply a random color and grid rotation.
                planeObject.GetComponent<Renderer>().material.SetColor("_GridColor", Color.white);
                //planeObject.GetComponent<Renderer>().material.SetColor("_GridColor", m_planeColors[Random.Range(0,
                //    m_planeColors.Length - 1)]);
                planeObject.GetComponent<Renderer>().material.SetFloat("_UvRotation", Random.Range(0.0f, 360.0f));
            }
			#endregion

			// Disable the snackbar UI when not in play mode, or in play mode and no planes are valid.
			bool showSearchingUI = false;
			Frame.GetAllPlanes(ref m_allPlanes);
			if (GameManager.instance.inPlayMode)
			{
				for (int i = 0; i < m_allPlanes.Count; i++)
				{
					if (m_allPlanes[i].IsValid)
					{
						showSearchingUI = false;
						break;
					}else
                    {
                        showSearchingUI = true;
                    }
				}
			}
			else
			{
				showSearchingUI = false;
			}
			m_searchingForPlaneUI.SetActive(showSearchingUI);

			//-------------------------------------------DY Mod Start	

			sceneUpdateCount++;
            sceneDetectionCount++;
            
			Ray myRay = new Ray (m_firstPersonCamera.transform.position, m_firstPersonCamera.transform.forward);
			TrackableHit myHit;
			TrackableHitFlag myRaycastFilter = TrackableHitFlag.PlaneWithinBounds | TrackableHitFlag.PlaneWithinPolygon;

            //if raycast hits plane, and we should show scene
            if (Session.Raycast (myRay, myRaycastFilter, out myHit) && sceneUpdateCount > 10 && shouldShowSceneParent)
            {
                //then pin the scene to the plane and set scene active
				sceneUpdateCount = 0;
                timeoutCount = 0;
                timeoutText.enabled = false;

                var anchor = Session.CreateAnchor (myHit.Point, Quaternion.identity);
				sceneParent.transform.position = myHit.Point;
				sceneParent.transform.SetParent (anchor.transform);
				sceneParent.transform.LookAt (m_firstPersonCamera.transform);
				sceneParent.transform.rotation = Quaternion.Euler (0.0f,
					sceneParent.transform.rotation.eulerAngles.y, sceneParent.transform.rotation.z);
                if (!sceneParent.activeInHierarchy)
                {
                    sceneParent.SetActive(true);
                }
                sceneParentActive = true;

                //turn on/off certain texts
                lookAroundText.enabled = false;
                placeSceneText.enabled = true;
                suggestResetSceneText.enabled = false;

                //disable the scan lines
                if (scanLinesGO.activeInHierarchy)
                {
                    scanLinesGO.SetActive(false);
                }
                sceneParent.GetComponent<PlaneAttachment>().Attach(myHit.Plane);
            }
            //but if raycast didn't hit
            else if (sceneUpdateCount > 10 && shouldShowSceneParent)
            {
                //set scene inactive
                sceneUpdateCount = 0;
                if (sceneParent.activeInHierarchy)
                {
                    sceneParent.SetActive(false);
                }
                sceneParentActive = false;

                lookAroundText.enabled = true;
                placeSceneText.enabled = false;
                suggestResetSceneText.enabled = false;

                if (!scanLinesGO.activeInHierarchy)
                {
                    scanLinesGO.SetActive(true);
                }

                if (timeoutCount > 300)
                {
                    timeoutText.enabled = true;
                    lookAroundText.enabled = false;
                }

            }//if scene placed but couldn't detect scene
            else if (sceneDetectionCount > 120 && sceneDetectionActive && !SceneDetection.isDetectingScene)
            {
                //suggest reset scene
                sceneDetectionCount = 0;
                suggestResetSceneText.enabled = true;
                inGameCanvas.enabled = false;
            }//if scene placed and detecting scene
            else if (sceneDetectionActive && SceneDetection.isDetectingScene)
            {
                sceneDetectionCount = 0;
                suggestResetSceneText.enabled = false;
                inGameCanvas.enabled = true;
            }

            Touch touch;
            if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
            {
                return;
            }

			//place scene if touch and active
			if (sceneParentActive && shouldShowSceneParent && !tooClose2Text3D.activeInHierarchy) {				
				EventsManager.ScenePlaced ();
			}//play error effect if too close
            if (tooClose2Text3D.activeInHierarchy)
            {
                HitPanel.instance.MissFlash();
                SoundManager.instance.PlayMissSFX();
            }
			//--------------------------------------DY Mod End



			#region Original Code
			//            TrackableHit hit;
			//            TrackableHitFlag raycastFilter = TrackableHitFlag.PlaneWithinBounds | TrackableHitFlag.PlaneWithinPolygon;

			//            if (Session.Raycast(m_firstPersonCamera.ScreenPointToRay(touch.position), raycastFilter, out hit))
			//            {
			//                // Create an anchor to allow ARCore to track the hitpoint as understanding of the physical
			//                // world evolves.
			//                var anchor = Session.CreateAnchor(hit.Point, Quaternion.identity);
			//
			//				#region Original code
			//				//                // Intanstiate an Andy Android object as a child of the anchor; it's transform will now benefit
			//				//                // from the anchor's tracking.
			//				//                var andyObject = Instantiate(m_andyAndroidPrefab, hit.Point, Quaternion.identity,
			//				//                    anchor.transform);
			//				//
			//				//                // Andy should look at the camera but still be flush with the plane.
			//				//                andyObject.transform.LookAt(m_firstPersonCamera.transform);
			//				//                andyObject.transform.rotation = Quaternion.Euler(0.0f,
			//				//                    andyObject.transform.rotation.eulerAngles.y, andyObject.transform.rotation.z);
			//				//
			//				//                // Use a plane attachment component to maintain Andy's y-offset from the plane
			//				//                // (occurs after anchor updates).
			//				//                andyObject.GetComponent<PlaneAttachment>().Attach(hit.Plane);
			//				#endregion
			//
			//				//-------------------------------------------DY Mod Start
			//				if (!GameManager.instance.scenePlaced) {
			//					sceneParent.transform.position = hit.Point;
			//					sceneParent.transform.SetParent (anchor.transform);
			//					sceneParent.transform.LookAt(m_firstPersonCamera.transform);
			//					sceneParent.transform.rotation = Quaternion.Euler(0.0f,
			//						sceneParent.transform.rotation.eulerAngles.y, sceneParent.transform.rotation.z);
			//					sceneParent.SetActive (true);
			//					GameManager.instance.scenePlaced = true;
			//					GameManager.instance.sceneParent = sceneParent;
			//					GameManager.instance.initialScale = sceneParent.transform.localScale.x;
			//					sceneParent.GetComponent<PlaneAttachment>().Attach(hit.Plane);
			//					EventsManager.ScenePlaced ();
			//				}	
			//				//-------------------------------------------DY Mod End
			//            }
			#endregion

		}//end Update

		/// <summary>
		/// Quit the application if there was a connection error for the ARCore session.
		/// </summary>
		private void _QuitOnConnectionErrors()
        {
            // Do not update if ARCore is not tracking.
            if (Session.ConnectionState == SessionConnectionState.DeviceNotSupported)
            {
                _ShowAndroidToastMessage("This device does not support ARCore.");
                Application.Quit();
            }
            else if (Session.ConnectionState == SessionConnectionState.UserRejectedNeededPermission)
            {
                _ShowAndroidToastMessage("Camera permission is needed to run this application.");
                Application.Quit();
            }
            else if (Session.ConnectionState == SessionConnectionState.ConnectToServiceFailed)
            {
                _ShowAndroidToastMessage("ARCore encountered a problem connecting.  Please start the app again.");
                Application.Quit();
            }
        }

        /// <summary>
        /// Show an Android toast message.
        /// </summary>
        /// <param name="message">Message string to show in the toast.</param>
        /// <param name="length">Toast message time length.</param>
        private static void _ShowAndroidToastMessage(string message)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            if (unityActivity != null)
            {
                AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
                unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity,
                        message, 0);
                    toastObject.Call("show");
                }));
            }
        }
    }
}
