namespace Ks.Dust.PeroidReportGenerator
{
    public enum ExecuteResult
    {
        Done = 0x00,

        UnknowArgument = 0x10,

        NoneArgument = 0x20,

        StopWithException = 0x30,

        Failed = 0x40
    }
}
