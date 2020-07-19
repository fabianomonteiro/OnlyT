namespace OnlyT.ViewModel
{
	using System.Windows;
	using System.Windows.Input;
	using GalaSoft.MvvmLight;
	using GalaSoft.MvvmLight.Messaging;
	using Messages;
	using OnlyT.AnalogueClock;
	using OnlyT.CountdownTimer;
	using OnlyT.Services.Options;

	// ReSharper disable once ClassNeverInstantiated.Global
	public class CountdownTimerViewModel : ViewModelBase
	{
		private readonly IOptionsService _optionsService;
		private bool _windowedOperation;

		public CountdownTimerViewModel(IOptionsService optionsService)
		{
			_optionsService = optionsService;

			// subscriptions...
			Messenger.Default.Register<CountdownFrameChangedMessage>(this, OnFrameChanged);
			Messenger.Default.Register<CountdownZoomOrPositionChangedMessage>(this, OnZoomOrPositionChanged);
			Messenger.Default.Register<CountdownElementsChangedMessage>(this, OnElementsChanged);
			Messenger.Default.Register<CountdownWindowTransparencyChangedMessage>(this, OnWindowTransparencyChanged);
			Messenger.Default.Register<MousePointerInTimerDisplayChangedMessage>(this, OnMousePointerChanged);
			Messenger.Default.Register<ClockHourFormatChangedMessage>(this, OnDigitalClockFormatChanged);
			Messenger.Default.Register<ClockIsFlatChangedMessage>(this, OnClockIsFlatChanged);
		}

		private string _congregation;
		public string Congregation
		{
			get => _congregation;
			set
			{
				_congregation = value;
				RaisePropertyChanged();
			}
		}

		private string _meetingDescription;
		public string MeetingDescription
		{
			get => _meetingDescription;
			set
			{
				_meetingDescription = value;
				RaisePropertyChanged();
			}
		}

		public int BorderThickness => !WindowedOperation && _optionsService.Options.CountdownFrame ? 3 : 0;

		public int BackgroundOpacity => !WindowedOperation && _optionsService.Options.CountdownFrame ? 100 : 0;

		public double CountdownScale => WindowedOperation ? 1 : _optionsService.Options.CountdownZoomPercent / 100.0;

		public ElementsToShow ElementsToShow => _optionsService.Options.CountdownElementsToShow;

		public bool IsWindowTransparent => !WindowedOperation && _optionsService.Options.IsCountdownWindowTransparent;

		public int CountdownDurationMins => _optionsService.Options.CountdownDurationMins;

		public bool ClockIsFlat => WindowedOperation || _optionsService.Options.ClockIsFlat;

		public bool DigitalTimeFormatShowLeadingZero =>
			_optionsService.Options.ClockHourFormat == ClockHourFormat.Format12LeadingZero ||
			_optionsService.Options.ClockHourFormat == ClockHourFormat.Format12LeadingZeroAMPM ||
			_optionsService.Options.ClockHourFormat == ClockHourFormat.Format24LeadingZero;

		public bool DigitalTimeFormat24Hours =>
			_optionsService.Options.ClockHourFormat == ClockHourFormat.Format24 ||
			_optionsService.Options.ClockHourFormat == ClockHourFormat.Format24LeadingZero;

		public bool DigitalTimeFormatAMPM =>
			_optionsService.Options.ClockHourFormat == ClockHourFormat.Format12AMPM ||
			_optionsService.Options.ClockHourFormat == ClockHourFormat.Format12LeadingZeroAMPM;

		public bool DigitalTimeShowSeconds => _optionsService.Options.ShowDigitalSeconds;

		public Cursor MousePointer =>
			_optionsService.Options.ShowMousePointerInTimerDisplay
				? Cursors.Arrow
				: Cursors.None;

		public bool WindowedOperation
		{
			get => _windowedOperation;
			set
			{
				if (_windowedOperation != value)
				{
					_windowedOperation = value;

					RaisePropertyChanged();
					RaisePropertyChanged(nameof(BorderThickness));
					RaisePropertyChanged(nameof(BackgroundOpacity));
					RaisePropertyChanged(nameof(CountdownScale));
					RaisePropertyChanged(nameof(IsWindowTransparent));
				}
			}
		}

		public HorizontalAlignment HorizontalAlignment
		{
			get
			{
				switch (_optionsService.Options.CountdownScreenLocation)
				{
					case ScreenLocation.Left:
					case ScreenLocation.BottomLeft:
					case ScreenLocation.TopLeft:
						return HorizontalAlignment.Left;

					case ScreenLocation.Right:
					case ScreenLocation.BottomRight:
					case ScreenLocation.TopRight:
						return HorizontalAlignment.Right;

					default:
						return HorizontalAlignment.Center;
				}
			}
		}

		public VerticalAlignment VerticalAlignment
		{
			get
			{
				switch (_optionsService.Options.CountdownScreenLocation)
				{
					case ScreenLocation.Top:
					case ScreenLocation.TopLeft:
					case ScreenLocation.TopRight:
						return VerticalAlignment.Top;

					case ScreenLocation.Bottom:
					case ScreenLocation.BottomLeft:
					case ScreenLocation.BottomRight:
						return VerticalAlignment.Bottom;

					default:
						return VerticalAlignment.Center;
				}
			}
		}

		private void OnFrameChanged(CountdownFrameChangedMessage msg)
		{
			RaisePropertyChanged(nameof(BorderThickness));
			RaisePropertyChanged(nameof(BackgroundOpacity));
		}

		private void OnWindowTransparencyChanged(CountdownWindowTransparencyChangedMessage msg)
		{
			RaisePropertyChanged(nameof(IsWindowTransparent));
		}

		private void OnMousePointerChanged(MousePointerInTimerDisplayChangedMessage message)
		{
			RaisePropertyChanged(nameof(MousePointer));
		}

		private void OnZoomOrPositionChanged(CountdownZoomOrPositionChangedMessage obj)
		{
			RaisePropertyChanged(nameof(CountdownScale));
			RaisePropertyChanged(nameof(HorizontalAlignment));
			RaisePropertyChanged(nameof(VerticalAlignment));
		}

		private void OnElementsChanged(CountdownElementsChangedMessage obj)
		{
			RaisePropertyChanged(nameof(ElementsToShow));
		}

		private void OnDigitalClockFormatChanged(ClockHourFormatChangedMessage obj)
		{
			RaisePropertyChanged(nameof(DigitalTimeFormat24Hours));
			RaisePropertyChanged(nameof(DigitalTimeFormatShowLeadingZero));
			RaisePropertyChanged(nameof(DigitalTimeFormatAMPM));
			RaisePropertyChanged(nameof(DigitalTimeShowSeconds));
		}

		private void OnClockIsFlatChanged(ClockIsFlatChangedMessage msg)
		{
			RaisePropertyChanged(nameof(ClockIsFlat));
		}
	}
}
