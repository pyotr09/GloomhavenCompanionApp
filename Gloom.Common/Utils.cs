namespace Gloom.Common;

public class Utils
{
    public static int GenerateId()
    {
        var ticks = DateTime.Now.Ticks % 65535;
        ushort ts = Convert.ToUInt16(ticks);
        var randid = new Random().Next(512);

        var result = ts * 512 + randid;
        return result;
    }
}