namespace FischBot.Services.EightBallService
{
    public interface IEightBallService
    {
        (string phrase, int level) GetEightBallResult();
    }
}