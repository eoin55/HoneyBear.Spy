namespace HoneyBear.Spy.Configuration
{
    public interface ISpyFileLocation
    {
        ISpyBuilder Value(string path);
        ISpyBuilder OverriddenFromAppSetting();
        ISpyBuilder OverriddenFromEnvironmentVariable(string name);
        ISpyBuilder OverriddenFromEnvironmentVariable();

        ISpyBuilder AppendProductName(string name);
        ISpyBuilder AppendProductNameFromAppSetting();
    }
}