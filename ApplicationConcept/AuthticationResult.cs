namespace ApplicationConcept
{
    public enum AuthticationResult
    {
        InValidRefreshToken = 0x00,

        EmptyRefreshToken = 0x01,

        AuthticationError = 0x02,

        InvalidAccessToken = 0x03,

        RefreshTokenException = 0x04,

        LoginException = 0x05,

        RefreshTokenSuccessed = 0xFE,

        LoginSuccessed = 0xFF
    }
}
