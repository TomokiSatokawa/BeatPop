namespace Editor
{
    [System.Serializable]
    public class SectionSnapState : EditorSnapStateBase
    {
        public override void OnCreate(int laneIndex, double noteTime)
        {
            EditorNodeData.I.AddSection((float)noteTime);
        }

        public override void OnDelete(int laneIndex, double noteTime)
        {
            EditorNodeData.I.RemoveSection((float)noteTime);
        }

        public override void OnEnter() { }

        public override void OnExit() { }
    }

}