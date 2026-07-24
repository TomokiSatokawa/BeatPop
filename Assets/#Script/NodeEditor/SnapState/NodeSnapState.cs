namespace Editor
{
    [System.Serializable]
    public class NodeSnapState : EditorSnapStateBase
    {
        private PoolPrefabType _prefabType;
        private int _convertLevel = 0;
        public override void OnCreate(int laneIndex, double noteTime)
        {
            EditorNodeData.I.AddNode(_prefabType, noteTime, laneIndex, _convertLevel);
        }

        public override void OnDelete(int laneIndex, double noteTime)
        {
            EditorNodeData.I.DeleteNode(noteTime, laneIndex);
        }

        public void SetPrefabType(PoolPrefabType type)
        {
            _prefabType = type;
        }

        public void SetConvertLevel(int level)
        {
            _convertLevel = level;
        }
        public override void OnEnter() { }

        public override void OnExit() { }
    }
}