using Input;
using R3;
using Sound;
using UnityEngine;

namespace InGame.Node
{
    public class LaneClick : MonoBehaviour
    {
        [SerializeField] private GameObject[] _laneHighlight; 
        private void Start()
        {
            InputManager.LeftLane.Subscribe(v => Click(v, 0)).AddTo(this);
            InputManager.RightLane.Subscribe(v => Click(v, 1)).AddTo(this);
        }
        public void Click(bool value, int lane)
        {
            if (value)
            {
                //SoundManager.I.PlaySESound(SESoundType.NormalTap);
            }

            _laneHighlight[lane].SetActive(value);
        }
    }

}