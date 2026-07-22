using InGame;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Editor
{
    /// <summary>
    /// エディターの入力処理
    /// </summary>
    public class EditorInput : MonoBehaviour
    {
        [SerializeField] private float _keyMoveAmount;

        private void Update()
        {
            PlayPause();
            MoveTimeLine();
        }

        private void MoveTimeLine()
        {
            int moveDirection = 0;

            Keyboard current = Keyboard.current;
            if (current == null) return;

            if (current.rightArrowKey.wasPressedThisFrame)
            {
                moveDirection = 1;
            }
            else if (current.leftArrowKey.wasPressedThisFrame)
            {
                moveDirection = -1;
            }

            if (moveDirection == 0) return;

            float currentTime = StageTimeController.StageTime;
            float songLength = StageTimeController.I.SongClip.length;

            float targetTime = Mathf.Clamp(currentTime + _keyMoveAmount * moveDirection, 0, songLength);

            float moveAmount = targetTime - currentTime;

            StageTimeController.I.MoveStageTime(moveAmount);
        }

        private void PlayPause()
        {
            if (!Keyboard.current.spaceKey.wasPressedThisFrame)
                return;
            
            if (StageTimeController.I.IsPlaying.CurrentValue)
            {
                StageTimeController.I.Pause();
            }
            else
            {
                StageTimeController.I.ReStart();
            }
        }
    }
}