using Title.PlayerData;
using UnityEngine;

namespace Title.Common
{
    /// <summary>
    /// タイトル専用プレゼンター
    /// </summary>
    public class TitlePresenter : MonoBehaviour
    {
        [SerializeField] private PlayerInfoView _playerInfoView;

        private void Start()
        {
            _playerInfoView.UpdateView();
        }
    }
}