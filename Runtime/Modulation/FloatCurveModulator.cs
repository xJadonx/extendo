using UnityEngine;

namespace Extendo.Modulation
{
	[AddComponentMenu("Extendo/Modulation/Float Curve Modulator")]
	public class FloatCurveModulator : CurveModulator<float>
	{
		public FloatCurveModulator()
		{
			scale = 1f;
			curve = DefaultCurve;
		}

		public AnimationCurve curve;

		protected override float GetValue()
		{
			return curve.Evaluate(Mathf.Repeat(timer.TimeValue + offset, 1f)) * scale;
		}
	}
}