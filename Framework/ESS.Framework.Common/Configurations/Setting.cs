namespace ESS.Framework.Common.Configurations
{
    public class Setting
    {
        public bool SkipEventDeserializeError { get; set; }

        public Setting()
        {
            SkipEventDeserializeError = true;
        }
    }
}
