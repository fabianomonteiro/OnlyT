using OnlyT.Services.Options.MeetingStartTimes;

namespace OnlyT.Services.CountdownTimer
{
    public interface ICountdownTimerTriggerService
    {
        bool IsInCountdownPeriod(out IsInCountdownPeriodResult result);

        void UpdateTriggerPeriods();
    }
}
