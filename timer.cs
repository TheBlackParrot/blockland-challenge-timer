function GameConnection::startTimer(%this) {
	cancel(%this.timerSched);

	%this.timerStart = mFloatLength($Sim::Time, 2);
	%this.updateTimer();
}

function GameConnection::updateTimer(%this) {
	cancel(%this.timerSched);
	%this.timerSched = %this.schedule(33, updateTimer);

	%this.showTimer();
}

function GameConnection::showTimer(%this) {
	%this.bottomPrint("<font:Arial Bold:20>\c6" @ getTimeString(mFloatLength($Sim::Time - %this.timerStart, 2)), 5, 1);
}

function GameConnection::stopTimer(%this) {
	cancel(%this.timerSched);
	%this.showTimer();
}