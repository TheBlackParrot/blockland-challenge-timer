function GameConnection::unlockLevel(%this, %level) {
	if(%this.levelUnlocked[%level]) {
		return;
	}

	if(%level > $Pref::ChallengeTimer::Levels) {
		%this.errorMessage("$Pref::ChallengeTimer::Levels is lower than the level trying to be unlocked. (level" SPC %level @ ")");
		return;
	}
	if(%level < 0) {
		%this.errorMessage("Level trying to be unlocked must be at least 0 (0 = entire challenge). (" @ %level @ ")");
		return;
	}

	%this.levelUnlocked[%level] = true;
	messageClient(%this, '', "\c6You have unlocked \c3level" SPC %level SPC "\c6to go for the best time in!");

	%this.saveProgress();
}

function GameConnection::lockLevel(%this, %level) {
	if(!%this.levelUnlocked[%level]) {
		return;
	}

	if(%level > $Pref::ChallengeTimer::Levels) {
		%this.errorMessage("$Pref::ChallengeTimer::Levels is lower than the level trying to be locked. (level" SPC %level @ ")");
		return;
	}
	if(%level < 0) {
		%this.errorMessage("Level trying to be locked must be at least 0 (0 = entire challenge). (" @ %level @ ")");
		return;
	}

	%this.levelUnlocked[%level] = false;

	%this.saveProgress();
}

function GameConnection::hasUnlocked(%this, %level) {
	return %this.levelUnlocked[%level] ? %level : -1;
}