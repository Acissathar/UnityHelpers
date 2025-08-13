using UnityEngine;

namespace UnityHelpers.Runtime.Transform
{
	public class SmoothRotationFollow : MonoBehaviour 
	{
		#region Editor - Component References
		
		[SerializeField] private UnityEngine.Transform followTarget;
		private UnityEngine.Transform myTransform;
		
		#endregion
		
		#region Editor - Smooth Settings

		[SerializeField]  private float smoothSpeed = 20f;
		
		[SerializeField] private bool extrapolateRotation;
		
		private enum UpdateType
		{
			Update,
			LateUpdate
		}
		[SerializeField] private UpdateType updateType;

		#endregion
		
		#region Private Fields

		private Quaternion myRotation;

		#endregion
		
		#region Unity Lifecycle Methods
		
		private void Awake () 
		{
			myTransform = transform;
			//If no target has been selected, choose this transform's parent as target
			if (followTarget == null)
			{
				followTarget = myTransform.parent;
			}
			
			myRotation = transform.rotation;
		}
		
		private void OnEnable()
		{
			ResetCurrentRotation();
		}

		private void Update () 
		{
			if (updateType == UpdateType.LateUpdate)
			{
				return;
			}

			SmoothUpdate();
		}

		private void LateUpdate () 
		{
			if (updateType == UpdateType.Update)
			{
				return;
			}

			SmoothUpdate();
		}

		#endregion
		
		#region Smoothing
		
		private void SmoothUpdate()
		{
			//Smooth current rotation;
			myRotation = Smooth (myRotation, followTarget.rotation);

			//Set rotation;
			myTransform.rotation = myRotation;
		}

		//Smooth a rotation toward a target rotation based on 'smoothTime';
		private Quaternion Smooth(Quaternion currentRotation, Quaternion targetRotation)
		{
			//If 'extrapolateRotation' is set to 'true', calculate a new target rotation;
			if (extrapolateRotation && Quaternion.Angle(currentRotation, targetRotation) < 90f) 
			{
				var difference = targetRotation * Quaternion.Inverse (currentRotation);
				targetRotation *= difference;
			}
			
			return Quaternion.Slerp (currentRotation, targetRotation, Time.deltaTime * smoothSpeed);
		}

		#endregion
		
		#region Public Methods
		
		//Reset stored rotation and rotate this game object to match the target's rotation
		//Call this function if the target has just been rotated and no interpolation should take place (instant rotation)
		public void ResetCurrentRotation()
		{
			myRotation = followTarget.rotation;
		}
		
		#endregion
	}
}
