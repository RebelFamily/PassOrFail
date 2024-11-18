namespace Zain_Meta.Meta_Scripts.MetaRelated
{
    public interface IEnabler
    {
        public void Enable();
        public bool IsEnabled { get; set; }
    }
}