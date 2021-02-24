using UnityEngine;
using System.Collections;

namespace MagicalFX
{
	public class FX_Rotation : MonoBehaviour
	{

		public Vector3 Speed = Vector3.up;
        public float scale;

		void FixedUpdate ()
		{
            if (transform.localScale.x <= 1.5)
            {
                transform.localScale += new Vector3(scale, scale, scale);
            }
			this.transform.Rotate (Speed);
		}
	}
}