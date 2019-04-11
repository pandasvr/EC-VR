<<<<<<< HEAD
﻿//======= Copyright (c) Valve Corporation, All rights reserved. ===============
//
// Purpose: Basic throwable object
//
//=============================================================================

using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace Valve.VR.InteractionSystem
{
	//-------------------------------------------------------------------------
	[RequireComponent( typeof( Interactable ) )]
	[RequireComponent( typeof( Rigidbody ) )]
    [RequireComponent( typeof(VelocityEstimator))]
	public class Throwable : MonoBehaviour
	{
		[EnumFlags]
		[Tooltip( "The flags used to attach this object to the hand." )]
		public Hand.AttachmentFlags attachmentFlags = Hand.AttachmentFlags.ParentToHand | Hand.AttachmentFlags.DetachFromOtherHand | Hand.AttachmentFlags.TurnOnKinematic;

        [Tooltip("The local point which acts as a positional and rotational offset to use while held")]
        public Transform attachmentOffset;

		[Tooltip( "How fast must this object be moving to attach due to a trigger hold instead of a trigger press? (-1 to disable)" )]
        public float catchingSpeedThreshold = -1;

        public ReleaseStyle releaseVelocityStyle = ReleaseStyle.GetFromHand;

        [Tooltip("The time offset used when releasing the object with the RawFromHand option")]
        public float releaseVelocityTimeOffset = -0.011f;

        public float scaleReleaseVelocity = 1.1f;

		[Tooltip( "When detaching the object, should it return to its original parent?" )]
		public bool restoreOriginalParent = false;

        

		protected VelocityEstimator velocityEstimator;
        protected bool attached = false;
        protected float attachTime;
        protected Vector3 attachPosition;
        protected Quaternion attachRotation;
        protected Transform attachEaseInTransform;

		public UnityEvent onPickUp;
        public UnityEvent onDetachFromHand;
        public UnityEvent<Hand> onHeldUpdate;

        
        protected RigidbodyInterpolation hadInterpolation = RigidbodyInterpolation.None;

        protected new Rigidbody rigidbody;

        [HideInInspector]
        public Interactable interactable;


        //-------------------------------------------------
        protected virtual void Awake()
		{
			velocityEstimator = GetComponent<VelocityEstimator>();
            interactable = GetComponent<Interactable>();



            rigidbody = GetComponent<Rigidbody>();
            rigidbody.maxAngularVelocity = 50.0f;


            if(attachmentOffset != null)
            {
                // remove?
                //interactable.handFollowTransform = attachmentOffset;
            }

		}


        //-------------------------------------------------
        protected virtual void OnHandHoverBegin( Hand hand )
		{
			bool showHint = false;

            // "Catch" the throwable by holding down the interaction button instead of pressing it.
            // Only do this if the throwable is moving faster than the prescribed threshold speed,
            // and if it isn't attached to another hand
            if ( !attached && catchingSpeedThreshold != -1)
            {
                float catchingThreshold = catchingSpeedThreshold * SteamVR_Utils.GetLossyScale(Player.instance.trackingOriginTransform);

                GrabTypes bestGrabType = hand.GetBestGrabbingType();

                if ( bestGrabType != GrabTypes.None )
				{
					if (rigidbody.velocity.magnitude >= catchingThreshold)
					{
						hand.AttachObject( gameObject, bestGrabType, attachmentFlags );
						showHint = false;
					}
				}
			}

			if ( showHint )
			{
                hand.ShowGrabHint();
			}
		}


        //-------------------------------------------------
        protected virtual void OnHandHoverEnd( Hand hand )
		{
            hand.HideGrabHint();
		}


        //-------------------------------------------------
        protected virtual void HandHoverUpdate( Hand hand )
        {
            GrabTypes startingGrabType = hand.GetGrabStarting();
            
            if (startingGrabType != GrabTypes.None)
            {
				hand.AttachObject( gameObject, startingGrabType, attachmentFlags, attachmentOffset );
                hand.HideGrabHint();
            }
		}

        //-------------------------------------------------
        protected virtual void OnAttachedToHand( Hand hand )
		{
            //Debug.Log("<b>[SteamVR Interaction]</b> Pickup: " + hand.GetGrabStarting().ToString());

            hadInterpolation = this.rigidbody.interpolation;

            attached = true;

			onPickUp.Invoke();

			hand.HoverLock( null );
            
            rigidbody.interpolation = RigidbodyInterpolation.None;
            
		    velocityEstimator.BeginEstimatingVelocity();

			attachTime = Time.time;
			attachPosition = transform.position;
			attachRotation = transform.rotation;

		}


        //-------------------------------------------------
        protected virtual void OnDetachedFromHand(Hand hand)
        {
            attached = false;

            onDetachFromHand.Invoke();

            hand.HoverUnlock(null);
            
            rigidbody.interpolation = hadInterpolation;

            Vector3 velocity;
            Vector3 angularVelocity;

            GetReleaseVelocities(hand, out velocity, out angularVelocity);

            rigidbody.velocity = velocity;
            rigidbody.angularVelocity = angularVelocity;
        }


        public virtual void GetReleaseVelocities(Hand hand, out Vector3 velocity, out Vector3 angularVelocity)
        {
            if (hand.noSteamVRFallbackCamera && releaseVelocityStyle != ReleaseStyle.NoChange)
                releaseVelocityStyle = ReleaseStyle.ShortEstimation; // only type that works with fallback hand is short estimation.

            switch (releaseVelocityStyle)
            {
                case ReleaseStyle.ShortEstimation:
                    velocityEstimator.FinishEstimatingVelocity();
                    velocity = velocityEstimator.GetVelocityEstimate();
                    angularVelocity = velocityEstimator.GetAngularVelocityEstimate();
                    break;
                case ReleaseStyle.AdvancedEstimation:
                    hand.GetEstimatedPeakVelocities(out velocity, out angularVelocity);
                    break;
                case ReleaseStyle.GetFromHand:
                    velocity = hand.GetTrackedObjectVelocity(releaseVelocityTimeOffset);
                    angularVelocity = hand.GetTrackedObjectAngularVelocity(releaseVelocityTimeOffset);
                    break;
                default:
                case ReleaseStyle.NoChange:
                    velocity = rigidbody.velocity;
                    angularVelocity = rigidbody.angularVelocity;
                    break;
            }

            if (releaseVelocityStyle != ReleaseStyle.NoChange)
                velocity *= scaleReleaseVelocity;
        }

        //-------------------------------------------------
        protected virtual void HandAttachedUpdate(Hand hand)
        {


            if (hand.IsGrabEnding(this.gameObject))
            {
                hand.DetachObject(gameObject, restoreOriginalParent);

                // Uncomment to detach ourselves late in the frame.
                // This is so that any vehicles the player is attached to
                // have a chance to finish updating themselves.
                // If we detach now, our position could be behind what it
                // will be at the end of the frame, and the object may appear
                // to teleport behind the hand when the player releases it.
                //StartCoroutine( LateDetach( hand ) );
            }

            if (onHeldUpdate != null)
                onHeldUpdate.Invoke(hand);
        }


        //-------------------------------------------------
        protected virtual IEnumerator LateDetach( Hand hand )
		{
			yield return new WaitForEndOfFrame();

			hand.DetachObject( gameObject, restoreOriginalParent );
		}


        //-------------------------------------------------
        protected virtual void OnHandFocusAcquired( Hand hand )
		{
			gameObject.SetActive( true );
			velocityEstimator.BeginEstimatingVelocity();
		}


        //-------------------------------------------------
        protected virtual void OnHandFocusLost( Hand hand )
		{
			gameObject.SetActive( false );
			velocityEstimator.FinishEstimatingVelocity();
		}
	}

    public enum ReleaseStyle
    {
        NoChange,
        GetFromHand,
        ShortEstimation,
        AdvancedEstimation,
    }
}
=======
﻿//======= Copyright (c) Valve Corporation, All rights reserved. ===============
//
// Purpose: Basic throwable object
//
//=============================================================================

using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace Valve.VR.InteractionSystem
{
	//-------------------------------------------------------------------------
	[RequireComponent( typeof( Interactable ) )]
	[RequireComponent( typeof( Rigidbody ) )]
	[RequireComponent( typeof( VelocityEstimator ) )]
	public class Throwable : MonoBehaviour
	{
		[EnumFlags]
		[Tooltip( "The flags used to attach this object to the hand." )]
		public Hand.AttachmentFlags attachmentFlags = Hand.AttachmentFlags.ParentToHand | Hand.AttachmentFlags.DetachFromOtherHand;

		[Tooltip( "Name of the attachment transform under in the hand's hierarchy which the object should should snap to." )]
		public string attachmentPoint;

		[Tooltip( "How fast must this object be moving to attach due to a trigger hold instead of a trigger press?" )]
		public float catchSpeedThreshold = 0.0f;

		[Tooltip( "When detaching the object, should it return to its original parent?" )]
		public bool restoreOriginalParent = false;

		public bool attachEaseIn = false;
		public AnimationCurve snapAttachEaseInCurve = AnimationCurve.EaseInOut( 0.0f, 0.0f, 1.0f, 1.0f );
		public float snapAttachEaseInTime = 0.15f;
		public string[] attachEaseInAttachmentNames;

		private VelocityEstimator velocityEstimator;
		private bool attached = false;
		private float attachTime;
		private Vector3 attachPosition;
		private Quaternion attachRotation;
		private Transform attachEaseInTransform;

		public UnityEvent onPickUp;
		public UnityEvent onDetachFromHand;

		public bool snapAttachEaseInCompleted = false;


		//-------------------------------------------------
		void Awake()
		{
			velocityEstimator = GetComponent<VelocityEstimator>();

			if ( attachEaseIn )
			{
				attachmentFlags &= ~Hand.AttachmentFlags.SnapOnAttach;
			}

			Rigidbody rb = GetComponent<Rigidbody>();
			rb.maxAngularVelocity = 50.0f;
		}


		//-------------------------------------------------
		private void OnHandHoverBegin( Hand hand )
		{
			bool showHint = false;

			// "Catch" the throwable by holding down the interaction button instead of pressing it.
			// Only do this if the throwable is moving faster than the prescribed threshold speed,
			// and if it isn't attached to another hand
			if ( !attached )
			{
				if ( hand.GetStandardInteractionButton() )
				{
					Rigidbody rb = GetComponent<Rigidbody>();
					if ( rb.velocity.magnitude >= catchSpeedThreshold )
					{
						hand.AttachObject( gameObject, attachmentFlags, attachmentPoint );
						showHint = false;
					}
				}
			}

			if ( showHint )
			{
				ControllerButtonHints.ShowButtonHint( hand, Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger );
			}
		}


		//-------------------------------------------------
		private void OnHandHoverEnd( Hand hand )
		{
			ControllerButtonHints.HideButtonHint( hand, Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger );
		}


		//-------------------------------------------------
		private void HandHoverUpdate( Hand hand )
		{
			//Trigger got pressed
			if ( hand.GetStandardInteractionButtonDown() )
			{
				hand.AttachObject( gameObject, attachmentFlags, attachmentPoint );
				ControllerButtonHints.HideButtonHint( hand, Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger );
			}
		}

		//-------------------------------------------------
		private void OnAttachedToHand( Hand hand )
		{
			attached = true;

			onPickUp.Invoke();

			hand.HoverLock( null );

			Rigidbody rb = GetComponent<Rigidbody>();
			rb.isKinematic = true;
			rb.interpolation = RigidbodyInterpolation.None;

			if ( hand.controller == null )
			{
				velocityEstimator.BeginEstimatingVelocity();
			}

			attachTime = Time.time;
			attachPosition = transform.position;
			attachRotation = transform.rotation;

			if ( attachEaseIn )
			{
				attachEaseInTransform = hand.transform;
				if ( !Util.IsNullOrEmpty( attachEaseInAttachmentNames ) )
				{
					float smallestAngle = float.MaxValue;
					for ( int i = 0; i < attachEaseInAttachmentNames.Length; i++ )
					{
						Transform t = hand.GetAttachmentTransform( attachEaseInAttachmentNames[i] );
						float angle = Quaternion.Angle( t.rotation, attachRotation );
						if ( angle < smallestAngle )
						{
							attachEaseInTransform = t;
							smallestAngle = angle;
						}
					}
				}
			}

			snapAttachEaseInCompleted = false;
		}


		//-------------------------------------------------
		private void OnDetachedFromHand( Hand hand )
		{
			attached = false;

			onDetachFromHand.Invoke();

			hand.HoverUnlock( null );

			Rigidbody rb = GetComponent<Rigidbody>();
			rb.isKinematic = false;
			rb.interpolation = RigidbodyInterpolation.Interpolate;

			Vector3 position = Vector3.zero;
			Vector3 velocity = Vector3.zero;
			Vector3 angularVelocity = Vector3.zero;
			if ( hand.controller == null )
			{
				velocityEstimator.FinishEstimatingVelocity();
				velocity = velocityEstimator.GetVelocityEstimate();
				angularVelocity = velocityEstimator.GetAngularVelocityEstimate();
				position = velocityEstimator.transform.position;
			}
			else
			{
				velocity = Player.instance.trackingOriginTransform.TransformVector( hand.controller.velocity );
				angularVelocity = Player.instance.trackingOriginTransform.TransformVector( hand.controller.angularVelocity );
				position = hand.transform.position;
			}

			Vector3 r = transform.TransformPoint( rb.centerOfMass ) - position;
			rb.velocity = velocity + Vector3.Cross( angularVelocity, r );
			rb.angularVelocity = angularVelocity;

			// Make the object travel at the release velocity for the amount
			// of time it will take until the next fixed update, at which
			// point Unity physics will take over
			float timeUntilFixedUpdate = ( Time.fixedDeltaTime + Time.fixedTime ) - Time.time;
			transform.position += timeUntilFixedUpdate * velocity;
			float angle = Mathf.Rad2Deg * angularVelocity.magnitude;
			Vector3 axis = angularVelocity.normalized;
			transform.rotation *= Quaternion.AngleAxis( angle * timeUntilFixedUpdate, axis );
		}


		//-------------------------------------------------
		private void HandAttachedUpdate( Hand hand )
		{
			//Trigger got released
			if ( !hand.GetStandardInteractionButton() )
			{
				// Detach ourselves late in the frame.
				// This is so that any vehicles the player is attached to
				// have a chance to finish updating themselves.
				// If we detach now, our position could be behind what it
				// will be at the end of the frame, and the object may appear
				// to teleport behind the hand when the player releases it.
				StartCoroutine( LateDetach( hand ) );
			}

			if ( attachEaseIn )
			{
				float t = Util.RemapNumberClamped( Time.time, attachTime, attachTime + snapAttachEaseInTime, 0.0f, 1.0f );
				if ( t < 1.0f )
				{
					t = snapAttachEaseInCurve.Evaluate( t );
					transform.position = Vector3.Lerp( attachPosition, attachEaseInTransform.position, t );
					transform.rotation = Quaternion.Lerp( attachRotation, attachEaseInTransform.rotation, t );
				}
				else if ( !snapAttachEaseInCompleted )
				{
					gameObject.SendMessage( "OnThrowableAttachEaseInCompleted", hand, SendMessageOptions.DontRequireReceiver );
					snapAttachEaseInCompleted = true;
				}
			}
		}


		//-------------------------------------------------
		private IEnumerator LateDetach( Hand hand )
		{
			yield return new WaitForEndOfFrame();

			hand.DetachObject( gameObject, restoreOriginalParent );
		}


		//-------------------------------------------------
		private void OnHandFocusAcquired( Hand hand )
		{
			gameObject.SetActive( true );
			velocityEstimator.BeginEstimatingVelocity();
		}


		//-------------------------------------------------
		private void OnHandFocusLost( Hand hand )
		{
			gameObject.SetActive( false );
			velocityEstimator.FinishEstimatingVelocity();
		}
	}
}
>>>>>>> LoginUI
