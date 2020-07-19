namespace OnlyT.Services.CountdownTimer
{
    using System;
	using OnlyT.Services.Options.MeetingStartTimes;

	internal class CountdownTriggerPeriod
    {
        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public MeetingStartTime MeetingStartTime { get; set; }
    }
}
