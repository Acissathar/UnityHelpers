using UnityEngine;

namespace UnityHelpers.Runtime.Transform
{
    public class SmoothRotationFollow : MonoBehaviour
    {
        #region Public Methods

        /// <summary>
        ///     Reset the stored position and move this game object directly to the target's position so no interpolation should
        ///     take place (i.e. when teleporting)
        /// </summary>
        public void ResetCurrentRotation()
        {
            myRotation = followTarget.rotation;
        }

        #endregion

        #region Editor - Settings

        [SerializeField] private UnityEngine.Transform followTarget;
        private UnityEngine.Transform myTransform;
        [SerializeField] private float smoothSpeed = 20f;
        [SerializeField] private bool extrapolateRotation;
        [SerializeField] private UpdateType updateType;

        #endregion

        #region Private Fields

        private Quaternion myRotation;

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
        private void Awake()
        {
            myTransform = transform;
            //If no target has been selected, choose this transform's parent as target
            if (followTarget == null)
            {
                followTarget = myTransform.parent;
            }

            myRotation = transform.rotation;
        }

        /// <summary>
        ///     This function is called when the object becomes enabled and active.
        /// </summary>
        private void OnEnable()
        {
            ResetCurrentRotation();
        }

        /// <summary>
        ///     Update is called every frame, if the MonoBehaviour is enabled.
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
        ///     Calculates and then handles Smoothing, called in either Update or LateUpdate, but never both.
        /// </summary>
        private void SmoothUpdate()
        {
            myRotation = Smooth(myRotation, followTarget.rotation);

            myTransform.rotation = myRotation;
        }

        /// <summary>
        ///     Calculate the rotation smoothing based on input parameters.
        /// </summary>
        /// <param name="start">Starting position.</param>
        /// <param name="target">Desired ending position.</param>
        /// <param name="smoothTime">Smoothing multiplier to be applied between the two positions.</param>
        /// <returns></returns>
        private Quaternion Smooth(Quaternion currentRotation, Quaternion targetRotation)
        {
            //If 'extrapolateRotation' is set to 'true', calculate a new target rotation;
            if (extrapolateRotation && Quaternion.Angle(currentRotation, targetRotation) < 90f)
            {
                var difference = targetRotation * Quaternion.Inverse(currentRotation);
                targetRotation *= difference;
            }

            return Quaternion.Slerp(currentRotation, targetRotation, Time.deltaTime * smoothSpeed);
        }

        #endregion
    }
}