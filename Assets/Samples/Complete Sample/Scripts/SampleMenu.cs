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
				sample.target.HideSample();
				sample.target.gameObject.SetActive(false);

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
				if(sample.target == newSample)
				{
					sample.target.gameObject.SetActive(true);
					sample.target.ShowSample();
				}
				else
				{
					sample.target.HideSample();
					sample.target.gameObject.SetActive(false);
				}
			}
		}

		public void GoBack()
		{
			SetSample(null);
		}

		[Serializable]
		public class Sample
		{
			public string name;
			public BaseSample target;
		}
	}
}