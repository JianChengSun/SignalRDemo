using System;
using System.Threading;

namespace ProgressReporting.Services
{
    public class Job
    {
        public event EventHandler<EventArgs> ProgressChanged;
        public event EventHandler<EventArgs> Completed;

        private volatile int _progress;
        private volatile bool _completed;
        private CancellationTokenSource _cancellationTokenSource;

        public Job(string id)
        {
            Id = id;
            _cancellationTokenSource = new CancellationTokenSource();
        }

        public Job(string id, string barid, string proDisplay, string cplDisplay, string startBtn)
        {
            Id = id;
            _cancellationTokenSource = new CancellationTokenSource();
            BarId = barid;
            progressDisplay = proDisplay;
            completeDisplay = cplDisplay;
            startButton = startBtn;
        }

        public string Id { get; private set; }

        public string BarId { get; private set; }
        public string progressDisplay { get; private set; }
        public string completeDisplay { get; private set; }
        public string startButton { get; private set; }

        public int Progress
        {
            get { return _progress; }
        }

        public bool IsComplete
        {
            get { return _completed; }
        }

        public CancellationToken CancellationToken
        {
            get { return _cancellationTokenSource.Token; }
        }

        public void ReportProgress(int progress)
        {
            if (_progress != progress)
            {
                _progress = progress;
                OnProgressChanged();
            }
        }

        public void ReportComplete()
        {
            if (!IsComplete)
            {
                _completed = true;
                OnCompleted();
            }
        }

        protected virtual void OnCompleted()
        {
            var handler = Completed;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        protected virtual void OnProgressChanged()
        {
            var handler = ProgressChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        public void Cancel()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}