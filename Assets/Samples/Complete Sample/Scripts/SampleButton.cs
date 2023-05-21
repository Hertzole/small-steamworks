using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hertzole.SmallSteamworks.CompleteSample
{
	public class SampleButton : MonoBehaviour
	{
		[SerializeField]
		private TMP_Text label = default;

		private BaseSample targetSample;

		public event Action<BaseSample> OnClick;

		private void Awake()
		{
			GetComponent<Button>().onClick.AddListener(() => OnClick?.Invoke(targetSample));
		}

		public void SetSample(SampleMenu.Sample sample)
		{
			targetSample = sample.target;
			label.text = sample.name;
		}
	}
}