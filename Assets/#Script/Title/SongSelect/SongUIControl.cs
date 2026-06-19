using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SongUIControl : MonoBehaviour
{
    [SerializeField] private Button _selectButton;
    [SerializeField] private TextMeshProUGUI _nameText;

    private SongSelectData _songData;
    private void Start()
    {
        _selectButton.onClick.AddListener(OnSelect);
    }
    public void SetData(SongSelectData data)
    {
        _songData = data;
        _nameText.text = data.SongData.SongName;
    }
    public void OnSelect()
    {
        SongInfoControl.I.ShowInfo(_songData);
    }
}
