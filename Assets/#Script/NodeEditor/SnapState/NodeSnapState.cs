namespace Editor
{
    [System.Serializable]
    public class NodeSnapState : EditorSnapStateBase
    {
        private PoolPrefabType _prefabType;
        public override void OnCreate(int laneIndex, double noteTime)
        {
            EditorNodeData.I.AddNode(_prefabType, noteTime, laneIndex);
        }

        public override void OnDelete(int laneIndex, double noteTime)
        {
            EditorNodeData.I.DeleteNode(noteTime, laneIndex);
        }

        public void SetPrefabType(PoolPrefabType type)
        {
            _prefabType = type;
        }

        public override void OnEnter() { }

        public override void OnExit() { }
    }
}