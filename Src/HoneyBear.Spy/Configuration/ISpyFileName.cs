namespace HoneyBear.Spy.Configuration
{
    public interface ISpyFileName
    {
        ISpyBuilder Value(string name);
        ISpyBuilder OverriddenFromAppSetting();
    }
}