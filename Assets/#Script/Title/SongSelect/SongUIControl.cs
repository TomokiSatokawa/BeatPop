using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SongUIControl : MonoBehaviour
{
    [SerializeField] private Button _selectButton;
    [SerializeField] private TextMeshProUGUI _nameText;

    private IReadOnlySongData _songData;
    private void Start()
    {
        _selectButton.onClick.AddListener(OnSelect);
    }
    public void SetData(IReadOnlySongData data)
    {
        _songData = data;
        _nameText.text = data.SongName;
    }
    public void OnSelect()
    {
        SongInfoControl.I.ShowInfo(_songData);
    }
}
