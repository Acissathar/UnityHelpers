using UnityEngine;

namespace UnityHelpers.Runtime.Transform
{
	public class SmoothPositionFollow : MonoBehaviour
	{
		#region Editor - Settings
		
		[SerializeField] private UnityEngine.Transform followTarget;
		private UnityEngine.Transform myTransform;
		[SerializeField] private float lerpSpeed = 20.0f;
		[SerializeField] private float smoothDampTime = 0.02f;
		[SerializeField] private bool extrapolatePosition;
		[SerializeField] private SmoothType smoothType;
		[SerializeField] private UpdateType updateType;
		
		#endregion
		
		#region Private Fields

		private Vector3 _currentPosition;
		private Vector3 _localPositionOffset;
		private Vector3 _refVelocity;

		private enum SmoothType
		{
			Lerp,
			SmoothDamp, 
		}
		
		private enum UpdateType
		{
			Update,
			LateUpdate
		}
		
		#endregion
		
		#region Unity Methods

		/// <summary>
		///     Unity calls Awake when an enabled script instance is being loaded.
		/// </summary>
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
		
		/// <summary>
		///     This function is called when the object becomes enabled and active.
		/// </summary>
		private void OnEnable()
		{
			ResetCurrentPosition();
		}

		/// <summary>
		/// Update is called every frame, if the MonoBehaviour is enabled.
		/// </summary>
		private void Update() 
		{
			if (updateType == UpdateType.LateUpdate)
			{
				return;
			}

			SmoothUpdate();
		}

		/// <summary>
		///     LateUpdate is called every frame, if the Behaviour is enabled after all other Update functions.
		/// </summary>
		private void LateUpdate() 
		{
			if (updateType == UpdateType.Update)
			{
				return;
			}

			SmoothUpdate();
		}

		#endregion
		
		#region Smoothing
		
		/// <summary>
		/// Calculates and then handles Smoothing, called in either Update or LateUpdate, but never both.
		/// </summary>
		private void SmoothUpdate()
		{
			_currentPosition = Smooth (_currentPosition, followTarget.position, lerpSpeed);
			myTransform.position = _currentPosition;
		}

		/// <summary>
		/// Calculate the position smoothing, either by Lerp or SmoothDamp.
		/// </summary>
		/// <param name="start">Starting position.</param>
		/// <param name="target">Desired ending position.</param>
		/// <param name="smoothTime">Smoothing multiplier to be applied between the two positions.</param>
		/// <returns></returns>
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
		
		/// <summary>
		/// Reset the stored position and move this game object directly to the target's position so no interpolation should take place (i.e. when teleporting)
		/// </summary>
		public void ResetCurrentPosition()
		{
			Vector3 offset = myTransform.localToWorldMatrix * _localPositionOffset;
			_currentPosition = followTarget.position + offset;
		}
		
		#endregion
	}
}