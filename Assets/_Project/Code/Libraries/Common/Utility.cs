using System;

using UnityEngine;

namespace Libraries
{
	public static class Utility
	{
		static Utility()
		{
			UnityEngine.Random.InitState(DateTime.Now.Millisecond);
		}

		public static Vector3 getRangedVector3(float x_min, float x_max, float y_min, float y_max, float z_min, float z_max)
		{
			return new Vector3(UnityEngine.Random.Range(x_min, x_max), UnityEngine.Random.Range(y_min, y_max), UnityEngine.Random.Range(z_min, z_max));
		}
		public static Vector3 getRandomPointInCollider(BoxCollider collider)
		{
			Vector3 extents = collider.size / 2f;
			return collider.transform.TransformPoint(getRangedVector3(-extents.x, extents.x, -extents.y, extents.y, -extents.z, extents.z) + collider.center);
		}
	}

	public static class Extensions
	{
		public static Vector3 NextVector3(this System.Random random, int x, int y, int z)
		{
			return new Vector3(random.Next(x), random.Next(y), random.Next(z));
		}
		public static T NextEnum<T>(this System.Random random)
		{
			Array enum_values = Enum.GetValues(typeof(T));
			return (T)enum_values.GetValue(random.Next(enum_values.Length));
		}
		public static float getKineticEnergy(this Rigidbody rigidbody)
		{
			return 0.5f * rigidbody.mass * Mathf.Pow(rigidbody.velocity.magnitude, 2);
		}
	}
}