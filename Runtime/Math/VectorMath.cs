using UnityEngine;

namespace UnityHelpers.Runtime.Math
{
	/// <summary>
	/// This is a static helper class that offers various methods for calculating and modifying vectors or vector related information.
	/// </summary>
	public static class VectorMath 
	{
		/// <summary>
		/// Calculate a signed angle between 2 vectors using a specified plane as the normal.
		/// </summary>
		/// <param name="vector1">First vector to calculate with.</param>
		/// <param name="vector2">Second vector to calculate with.</param>
		/// <param name="planeNormal">Normal of the plane to base the angle between the two vectors on.</param>
		/// <returns>Signed angle ranging from -180 to 180.</returns>
		public static float GetAngle(Vector3 vector1, Vector3 vector2, Vector3 planeNormal)
		{
			// Calculate angle and sign
			var angle = Vector3.Angle(vector1,vector2);
			var sign = Mathf.Sign(Vector3.Dot(planeNormal,Vector3.Cross(vector1,vector2)));
			
			return angle * sign;
		}
		
		/// <summary>
		/// Calculate the length of the part of a vector that points in the same direction as '_direction' (i.e., the dot product).
		/// </summary>
		/// <param name="vector">The left operand of the dot product.</param>
		/// <param name="direction">The right operand of the dot product. Will be normalized if magnitude above 1.</param>
		/// <returns></returns>
		public static float GetDotProduct(Vector3 vector, Vector3 direction)
		{
			// Normalize vector if necessary
			if (!Mathf.Approximately(direction.sqrMagnitude, 1))
			{
				direction.Normalize();
			}

			return Vector3.Dot(vector, direction);
		}
		
		/// <summary>
		/// Remove all parts from a vector that are pointing in the same direction as 'direction'.
		/// </summary>
		/// <param name="vector">The left operand of the dot product.</param>
		/// <param name="direction">The right operand of the dot product. Will be normalized if magnitude above 1.</param>
		/// <returns>Direction minus the length calculated from the dot product with Vector parameter.</returns>
		public static Vector3 RemoveDotVector(Vector3 vector, Vector3 direction)
		{
			// Normalize vector if necessary
			if (!Mathf.Approximately(direction.sqrMagnitude, 1))
			{
				direction.Normalize();
			}

			var amount = Vector3.Dot(vector, direction);
			
			vector -= direction * amount;
			
			return vector;
		}
		
		/// <summary>
		/// Extract and return parts from a vector that are pointing in the same direction as 'direction'.
		/// </summary>
		/// <param name="vector">The left operand of the dot product.</param>
		/// <param name="direction">The right operand of the dot product. Will be normalized if magnitude above 1.</param>
		/// <returns>Vector containing the direction multiplied by the DotProduct of the parameters.</returns>
		public static Vector3 ExtractDotVector(Vector3 vector, Vector3 direction)
		{
			// Normalize vector if necessary
			if (!Mathf.Approximately(direction.sqrMagnitude, 1))
			{
				direction.Normalize();
			}

			var amount = Vector3.Dot(vector, direction);
			
			return direction * amount;
		}
		
		/// <summary>
		/// Rotate a vector onto a defined plane.
		/// </summary>
		/// <param name="vector">Vector to be rotated.</param>
		/// <param name="planeNormal">Plane normal to rotate vector on.</param>
		/// <param name="upDirection">Up direction of the plane normal.</param>
		/// <returns>Rotated vector.</returns>
		public static Vector3 RotateVectorOntoPlane(Vector3 vector, Vector3 planeNormal, Vector3 upDirection)
		{
			//Calculate rotation;
			var rotation = Quaternion.FromToRotation(upDirection, planeNormal);

			//Apply rotation to vector;
			vector = rotation * vector;
			
			return vector;
		}

		/// <summary>
		/// Project a point onto a line defined by lineStartPosition and lineDirection.
		/// </summary>
		/// <param name="lineStartPosition">Start position of the line.</param>
		/// <param name="lineDirection">Direction of the line.</param>
		/// <param name="point">Point to be projected on the line.</param>
		/// <returns>Projected Vector.</returns>
		public static Vector3 ProjectPointOntoLine(Vector3 lineStartPosition, Vector3 lineDirection, Vector3 point)
		{		
			// Calculate vector pointing from '_lineStartPosition' to '_point';
			var projectLine = point - lineStartPosition;
	
			var dotProduct = Vector3.Dot(projectLine, lineDirection);
	
			return lineStartPosition + lineDirection * dotProduct;
		}
		
		/// <summary>
		/// Increments a vector towards a target vector, using 'speed' and 'deltaTime'.
		/// </summary>
		/// <param name="currentVector">Vector to be incremented.</param>
		/// <param name="speed">How fast to move the current vector towards the target vector.</param>
		/// <param name="deltaTime">Current frame time.</param>
		/// <param name="targetVector">Target vector the current vector is being incremented towards.</param>
		/// <returns>Incremented Vector.</returns>
		public static Vector3 IncrementVectorTowardTargetVector(Vector3 currentVector, float speed, float deltaTime, Vector3 targetVector)
		{
			return Vector3.MoveTowards(currentVector, targetVector, speed * deltaTime);
		}
	}
}
