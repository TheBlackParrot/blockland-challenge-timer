function serverCmdStopTimer(%client) {
	if(!%client.isBeingTimed) {
		%client.errorMessage("You are not being timed for a challenge!");
		return;
	}
	%client.stopChallengeTimed(%client.timingFor, 1);
}
function serverCmdTimeLevel(%client, %which) {
	%client.startChallengeTimed(%which);
}

function serverCmdLeaderboard(%client, %which) {
	if(%which $= "") {
		%which = 0;
	}
	if(%which > $Pref::ChallengeTimer::Levels) {
		%which = 0;
	}

	%board = "ChallengeBoard" @ %which;
	if(!isObject(%board)) {
		%client.errorMessage("Specified leaderboard doesn't exist, this shouldn't be happening.");
		return;
	}

	for(%i=0;%i<%board.rowCount();%i++) {
		%col = "\c6";
		%data = %board.getRowText(%i);

		if(%client.name $= getField(%data, 0)) {
			%col = "\c5";
		}
		messageClient(%client, '', "\c2" @ %i+1 @ "." @ %col SPC getField(%data, 0) SPC "\c6--\c3" SPC getTimeString(getField(%data, 1)));
	}
}

function serverCmdGoTo(%client, %which) {
	if(%which > $Pref::ChallengeTimer::Levels) {
		%which = $Pref::ChallengeTimer::Levels;
	}
	if(%which < 1) {
		%which = 1;
	}
	if(%client.isBeingTimed) {
		%client.play2D(errorSound);
		return;
	}

	if(%client.hasUnlocked(%which)) {
		%spawn = "_ChallengeSpawn" @ %which;
		if(!isObject(%spawn)) {
			%client.errorMessage(strReplace(%spawn, "_", "") SPC "is not a named brick. You cannot warp to this level.");
			return;
		}

		%spawn.setPlayerTransform(%client);
	} else {
		%client.errorMessage("You have not unlocked this level yet!");
	}
}

function serverCmdResetTimer(%client) {
	if($Sim::Time - %client.lastReset < 5) {
		%client.play2D(errorSound);
		return;
	}
	%client.lastReset = $Sim::Time;

	%val = %client.timingFor;
	serverCmdStopTimer(%client);

	if(!isObject(%client.player)) {
		%client.spawnPlayer();
	} else {
		%client.instantRespawn();
	}

	schedule(33, 0, serverCmdTimeLevel, %client, %val);
}