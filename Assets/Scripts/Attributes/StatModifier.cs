namespace RPG.Attributes
{
    [System.Serializable]
    public class StatModifier
    {
        public FinalStat stat;
        public float value;

        public StatModifier(FinalStat stat, float value)
        {
            this.stat = stat;
            this.value = value;
        }
    }
}
