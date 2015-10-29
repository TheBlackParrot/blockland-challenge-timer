function GameConnection::startChallengeTimed(%this, %which) {
	if(%this.hasUnlocked(%which) == -1) {
		%this.errorMessage("You have not unlocked this level yet!");
		return;
	}

	if(%this.isBeingTimed) {
		return;
	}

	%spawn = "_ChallengeSpawn" @ %which;
	if(%which == 0) {
		// use the checkpoint system in this case
		%spawn = "_ChallengeSpawn1";
		serverCmdClearCheckpoint(%this);
		if(!isObject(%spawn)) {
			%this.errorMessage(strReplace(%spawn, "_", "") SPC "is not a named brick. This level cannot be properly timed.");
			return;
		}
	} else {
		if(!isObject(%spawn)) {
			%this.errorMessage(strReplace(%spawn, "_", "") SPC "is not a named brick. This level cannot be properly timed.");
			return;
		}
	}

	if(!isObject(%this.player)) {
		// would just %this.spawnPlayer(); but i can see that being abused
		%this.errorMessage("You must be alive to be timed!");
		return;
	}

	%this.player.setTransform(%spawn.getTransform());
	%this.player.changeDatablock(PlayerFrozenArmor);
	%this.player.setVelocity("0 0 0");

	%this.isBeingTimed = true;
	%this.timingFor = %which;

	%this.displayCountdown(5);
	%this.schedule(1000, displayCountdown, 4);
	%this.schedule(2000, displayCountdown, 3);
	%this.schedule(3000, displayCountdown, 2);
	%this.schedule(4000, displayCountdown, 1);
	%this.schedule(5000, displayCountdown, "GO!!");

	if(isObject(%this.player)) {
		if(%this.timingFor == 0) {
			%this.player.setShapeNameColor("0 1 1");
		} else {
			%this.player.setShapeNameColor("0 1 0");
		}
		%this.player.schedule(5000, changeDatablock, $Pref::ChallengeTimer::Playertype);
	}
	%this.schedule(5000, startTimer);
}

function GameConnection::stopChallengeTimed(%this, %which, %noComplete) {
	if(!%this.isBeingTimed) {
		return;
	}
	if(%which != %this.timingFor) {
		return;
	}

	%this.stopTimer();
	%this.isBeingTimed = false;

	if(isObject(%this.player)) {
		%this.player.setShapeNameColor("1 0 0");
	}

	if(%noComplete == 1) {
		messageClient(%this, '', "\c0Timer manually stopped.");
		return;
	}

	%this.updateLeaderboard(%this.timingFor);
	messageClient(%this, '', "\c2You earned a time of" SPC getTimeString(mFloatLength($Sim::Time - %this.timerStart, 2)) @ "!");

	%this.timingFor = -1;
}