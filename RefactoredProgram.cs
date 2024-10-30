namespace Cleanup
{
    internal class Program
    {
        private const double TargetChangeTime = 1;

        private double _previousTargetSetTime;
        private bool _isTargetSet;
        private dynamic _lockedCandidateTarget;
        private dynamic _lockedTarget;
        private dynamic _target;
        private dynamic _previousTarget;
        private dynamic _activeTarget;
        private dynamic _targetInRangeContainer;

        public void CleanupTest(Frame frame)
        {
            /* Если там дальше по коду в "MORE CLASS CODE" _lockedCandidateTarget и _lockedTarget не используются,
            то вообще удалить метод CheckLockedTargets() */
            CheckLockedTargets();

            _previousTarget = _target;
            _isTargetSet = TrySetTarget(frame);

            SetPreviousTargetTime();
            TargetableEntity.Selected = _target;
        }

        private void SetPreviousTargetTime()
        {
            if (_isTargetSet && _previousTarget != _target)
            {
                _previousTargetSetTime = Time.time;
            }
        }

        private void CheckLockedTargets()
        {
            if (_lockedCandidateTarget && !_lockedCandidateTarget.CanBeTarget)
            {
                _lockedCandidateTarget = null;
            }

            if (_lockedTarget && !_lockedTarget.CanBeTarget)
            {
                _lockedTarget = null;
            }
        }

        private bool TrySetTarget(Frame frame)
        {
            // If target exists and can be targeted, it should stay within Target Change Time since last target change
            if (_target && _target.CanBeTarget && Time.time - _previousTargetSetTime < TargetChangeTime)
            {
                return true;
            }


            if (_lockedTarget && _lockedTarget.CanBeTarget)
            {
                _target = _lockedTarget;
                return true;
            }

            // Sets _activeTarget field
            TrySetActiveTargetFromQuantum(frame);
            if (_activeTarget && _activeTarget.CanBeTarget)
            {
                _target = _activeTarget;
                return true;
            }

            _target = _targetInRangeContainer.GetTarget();
            if (_target)
            {
                return true;
            }

            _target = null;
            return false;
        }

        // MORE CLASS CODE
    }
}