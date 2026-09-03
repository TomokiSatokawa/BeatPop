using Common.PlaySystem;
using Title.Custom;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Title
{
    /// <summary>
    /// ƒCƒ“ƒQ[ƒ€‚ÌƒJƒƒ‰İ’è 
    /// </summary>
    public class CameraSettingsController : MonoBehaviour
    {
        [SerializeField] private Camera _camera;

        private void Start()
        {
            if (SongPlayContext.I)
            {
                _camera.TryGetComponent(out UniversalAdditionalCameraData cameraData);
                var flag = OtherCustomFlags.Create(SongPlayContext.I.PatternData.OtherPattern.Flags);
                cameraData.renderPostProcessing = !flag.Has(CustomOtherType.UsePostProcess);
            }
        }
    }
}