using UnityEngine;

namespace UnityHelpers.Runtime.Transform
{
	public class SmoothPositionFollow : MonoBehaviour
	{
		#region Editor - Component References
		
		[SerializeField] private UnityEngine.Transform followTarget;
		private UnityEngine.Transform myTransform;
		
		#endregion
		
		#region Editor - Smooth Settings
		
		[SerializeField, Tooltip("How fast the current position will be smoothed toward the target position when 'Lerp' is selected as smoothType.")] private float lerpSpeed = 20.0f;

		[SerializeField, Tooltip("How fast the current position will be smoothed toward the target position when 'SmoothDamp' is selected as smoothType.")] private float smoothDampTime = 0.02f;
		
		[SerializeField, Tooltip("Should position values be extrapolated to compensate for delay caused by smoothing.")] private bool extrapolatePosition;
		
		private enum SmoothType
		{
			Lerp,
			SmoothDamp, 
		}

		[SerializeField] private SmoothType smoothType;
		
		private enum UpdateType
		{
			Update,
			LateUpdate
		}
		[SerializeField] private UpdateType updateType;
		
		#endregion
		
		#region Private Fields

		private Vector3 _currentPosition;
		private Vector3 _localPositionOffset;
		private Vector3 _refVelocity;

		#endregion
		
		#region Unity Lifecycle Methods

		private void Awake () 
		{
			
			myTransform = transform;
			
			//If no target has been selected, choose this transform's parent as the target
			if (followTarget == null)
			{
				followTarget = myTransform.parent;
			}

			_currentPosition = transform.position;
			_localPositionOffset = myTransform.localPosition;
		}
		
		private void OnEnable()
		{
			ResetCurrentPosition();
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
			_currentPosition = Smooth (_currentPosition, followTarget.position, lerpSpeed);
			myTransform.position = _currentPosition;
		}

		private Vector3 Smooth(Vector3 start, Vector3 target, float smoothTime)
		{
			Vector3 offset = myTransform.localToWorldMatrix * _localPositionOffset;
			
			if (extrapolatePosition) 
			{
				Vector3 difference = target - (start - offset);
				target += difference;
			}
			
			target += offset;
			
			switch (smoothType)
			{
				case SmoothType.Lerp:
				{
					return Vector3.Lerp(start, target, Time.deltaTime * smoothTime);
				}
				case SmoothType.SmoothDamp:
				{
					return Vector3.SmoothDamp(start, target, ref _refVelocity, smoothDampTime);
				}
				default:
				{
					return Vector3.zero;
				}
			}
		}

		#endregion
		
		#region Public Methods
		
		//Reset stored position and move this game object directly to the target's position
		//Call this function if the target has just been moved a larger distance, and no interpolation should take place (teleporting)
		public void ResetCurrentPosition()
		{
			//Convert local position offset to world coordinates;
			Vector3 offset = myTransform.localToWorldMatrix * _localPositionOffset;
			//Add position offset and set current position;
			_currentPosition = followTarget.position + offset;
		}
		
		#endregion
	}
}