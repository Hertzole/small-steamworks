using System;
using UnityEngine;
using UnityEngine.UI;

namespace Hertzole.SmallSteamworks.CompleteSample
{
	public class SampleMenu : MonoBehaviour
	{
		[SerializeField]
		private SampleButton sampleButtonPrefab = default;
		[SerializeField]
		private RectTransform sampleButtonsContainer = default;
		[SerializeField]
		private Sample[] samples = default;

		private Canvas myCanvas;
		private GraphicRaycaster myRaycaster;

		private void Awake()
		{
			myCanvas = GetComponent<Canvas>();
			myRaycaster = GetComponent<GraphicRaycaster>();

			foreach (Sample sample in samples)
			{
				sample.Target.HideSample();
				sample.Target.gameObject.SetActive(false);

				SampleButton newButton = Instantiate(sampleButtonPrefab, sampleButtonsContainer);
				newButton.SetSample(sample);
				newButton.OnClick += SetSample;
			}
		}

		private void SetSample(BaseSample newSample)
		{
			myCanvas.enabled = newSample == null;
			myRaycaster.enabled = newSample == null;

			foreach (Sample sample in samples)
			{
				if (sample.Target == newSample)
				{
					sample.Target.gameObject.SetActive(true);
					sample.Target.ShowSample();
				}
				else
				{
					sample.Target.HideSample();
					sample.Target.gameObject.SetActive(false);
				}
			}
		}

		public void GoBack()
		{
			SetSample(null);
		}

		[Serializable]
		public sealed class Sample
		{
			[SerializeField]
			private string name;
			[SerializeField]
			private BaseSample target;

			public string Name
			{
				get { return name; }
			}
			public BaseSample Target
			{
				get { return target; }
			}
		}
	}
}