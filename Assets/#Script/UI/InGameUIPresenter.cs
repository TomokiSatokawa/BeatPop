using System;
using InGame.Node;
using R3;
using UnityEngine;

public class InGameUIPresenter : MonoBehaviour
{
    [SerializeField] private JudgementView _judgementView;
    [SerializeField] private NodeController _nodeController;

    public void Start()
    {
        _nodeController.ShowJudge.Subscribe(data => _judgementView.ViewPrefab(data.Item1, data.Item2));
    }
}
        