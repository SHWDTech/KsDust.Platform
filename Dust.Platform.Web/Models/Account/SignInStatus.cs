namespace Dust.Platform.Web.Models.Account
{
    /// <summary>
    /// 登陆状态
    /// </summary>
    public enum SignInStatus : byte
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success = 0x00,

        /// <summary>
        /// 已锁定
        /// </summary>
        LockedOut = 0x01,

        /// <summary>
        /// 需要二次验证
        /// </summary>
        RequiresVerification = 0x02,

        /// <summary>
        /// 失败
        /// </summary>
        Failure = 0xFF
    }
}