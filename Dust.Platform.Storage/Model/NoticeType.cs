using System.ComponentModel.DataAnnotations;

namespace Dust.Platform.Storage.Model
{
    public enum NoticeType : byte
    {
        [Display(Name = "扬尘报警")]
        DustAlarm = 0x00
    }
}
