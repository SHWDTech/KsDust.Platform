namespace Dust.Platform.Storage.Model
{
    public enum ApplicationTypes
    {
        JavaScript = 0x00,

        NativeConfidential = 0x01
    };

    public enum RoleStatus
    {
        Enabled = 0x00,

        Disabled = 0x01,

        Suspend = 0x02
    }
}