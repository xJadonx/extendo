using System.Collections.Generic;
using UnityEngine;

namespace Extendo.FocusSystem
{
	public class FocusDirector : MonoBehaviour
	{
		private static readonly List<FocusDirector> focusDirectors = new();
		public                  FocusTarget         defaultFocusTarget;
		public                  FocusTarget         FocusTarget { get; private set; }

		public static bool InFocus(FocusTarget focusTarget)
		{
			for (int i = 0; i < focusDirectors.Count; i++)
				if (focusDirectors[i].FocusTarget == focusTarget)
					return true;

			return false;
		}

		public static FocusDirector GetDirectorFromTarget(FocusTarget focusTarget)
		{
			for (int i = 0; i < focusDirectors.Count; i++)
				if (focusDirectors[i].FocusTarget == focusTarget)
					return focusDirectors[i];

			return null;
		}

		public static bool TryGetDirectorFromTarget(FocusTarget focusTarget, out FocusDirector director)
		{
			director = GetDirectorFromTarget(focusTarget);
			return director;
		}

		protected virtual void Awake()
		{
			focusDirectors.Add(this);
			Switch(defaultFocusTarget);
		}

		protected virtual void OnDestroy()
		{
			focusDirectors.Remove(this);
			RemoveFocus();
		}

		public void Switch(FocusTarget focusTarget)
		{
			if (!focusTarget)
				return;

			// Already in focus
			if (this.FocusTarget == focusTarget)
				return;

			// Detach existing focus
			RemoveFocus();

			// Reset attached director to default
			if (TryGetDirectorFromTarget(focusTarget, out var linkedDirector))
				linkedDirector.ResetToDefault();

			// Set Focus
			this.FocusTarget = focusTarget;
			this.FocusTarget.onFocus.Invoke();
		}

		public void SwitchIfAvailable(FocusTarget focusTarget)
		{
			if (!focusTarget)
				return;

			if (InFocus(focusTarget))
				return;

			Switch(focusTarget);
		}

		private void RemoveFocus()
		{
			if (!FocusTarget)
				return;

			FocusTarget.onLostFocus.Invoke();
			FocusTarget = null;
		}

		public void ResetToDefault()
		{
			Switch(defaultFocusTarget);
		}
	}
}