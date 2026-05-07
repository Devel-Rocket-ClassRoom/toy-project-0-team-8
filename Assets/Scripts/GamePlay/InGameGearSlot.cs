using UnityEngine;
using UnityEngine.UI;

public class InGameGearSlot : MonoBehaviour {
	[SerializeField] private Image _progressBar;
	
	private GearBase _gear;
	
	public void Init(GearBase gear) {
		_gear = gear;
	}

	private void Update() {
		if (_gear == null) { _progressBar.fillAmount = 1f;}
		else {
			_progressBar.fillAmount = 1f - _gear.GetProgressBarAmount();
		}
	}
}