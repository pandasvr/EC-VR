using Photon.Pun;

namespace Networking {
    using UnityEngine;
    using VRTK;

    public class AvatarHMDSyncAction : MonoBehaviourPunCallbacks {
        [Tooltip("The avatar's head to sync with the HMD. If empty, a child named 'Head' will be used.")]
        public GameObject avatarHead;
        [Tooltip("The avatar's left hand to sync with the left controller. If empty, a child named 'Left Hand' will be used.")]
        public GameObject leftHand;
        [Tooltip("The avatar's right hand to sync with the right controller. If empty, a child named 'Right Hand' will be used.")]
        public GameObject rightHand;
        [Tooltip("The avatar's top point to sync with the HMD. If empty, a child named 'Top' will be used.")]
        public GameObject avatarTop;

        private Transform headsetTransform;
        private Transform leftHandTransform;
        private Transform rightHandTransform;
        private Transform playAreaTransform;

        void Awake() 
        {
            VRTK_SDKManager.instance.AddBehaviourToToggleOnLoadedSetupChange(this);
        }

        void OnDestroy() 
        {
            VRTK_SDKManager.instance.RemoveBehaviourToToggleOnLoadedSetupChange(this);
        }

        protected virtual void OnEnable() 
        {
            headsetTransform = VRTK_DeviceFinder.DeviceTransform(VRTK_DeviceFinder.Devices.Headset).gameObject.transform;
            leftHandTransform = VRTK_DeviceFinder.DeviceTransform(VRTK_DeviceFinder.Devices.LeftController).gameObject.transform;
            rightHandTransform = VRTK_DeviceFinder.DeviceTransform(VRTK_DeviceFinder.Devices.RightController).gameObject.transform;
            playAreaTransform = VRTK_DeviceFinder.PlayAreaTransform();
            Camera.onPreRender += OnCamPreRender;
        }

        protected virtual void OnDisable() 
        {
            Camera.onPreRender -= OnCamPreRender;
        }

        protected virtual void OnCamPreRender(Camera cam) 
        {
            if (cam.gameObject.transform == VRTK_SDK_Bridge.GetHeadsetCamera()) 
            {
                Action();
            }
        }

        protected virtual void Action() 
        {
            if (photonView.IsMine) 
            {
                // The avatar follows the position of the HMD projected down to the play area floor
                FollowTransform(gameObject, headsetTransform, playAreaTransform, playAreaTransform);
                // The avatar's head exactly follows the position and rotation of the HMD
                FollowTransform(avatarHead, headsetTransform, headsetTransform, headsetTransform);
                // The avatar's left hand exactly follows the position and rotation of the left VR controller
                FollowTransform(leftHand, leftHandTransform, leftHandTransform, leftHandTransform);
                // The avatar's right hand exactly follows the position and rotation of the right VR controller
                FollowTransform(rightHand, rightHandTransform, rightHandTransform, rightHandTransform);
                // The avatar's "top" follows the position of the HMD but not its rotation
                // This is useful for putting labels/icons that need to be floating above an avatar's head
                FollowTransform(avatarTop, headsetTransform, headsetTransform, null);
            } 
            else 
            {
                // The avatar's "top" follows the position of the Head but not its rotation
                // This is useful for putting labels/icons that need to be floating above an avatar's head
                FollowTransform(avatarTop, avatarHead.transform, avatarHead.transform, null);
            }
        }

        private static void FollowTransform(GameObject avatarComponent, Transform followXZ, Transform followY, Transform followRotation) 
        {
            if (avatarComponent != null) 
            {
                Vector3 pos = new Vector3(followXZ.position.x, followY.position.y, followXZ.position.z);
                avatarComponent.transform.position = pos;
                if (followRotation != null) 
                {
                    avatarComponent.transform.rotation = followRotation.rotation;
                }
            }
        }
    }
}
