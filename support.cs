function GameConnection::errorMessage(%this, %msg) {
	messageClient(%this, 'errorSound', "\c0ERROR:\c3" SPC %msg);
}

function GameConnection::displayCountdown(%this, %time) {
	%frmt = "<font:Impact:36>\c2";
	%sound = "Synth_04_Sound";
	if(%time $= "GO!!") {
		%frmt = "<font:Impact:48>\c4";
		%sound = "Synth_11_Sound";
	}

	%this.play2D(%sound);
	%this.centerPrint(%frmt @ %time, 1);
}

package ChallengeTimerPackage {
	function GameConnection::spawnPlayer(%this) {
		parent::spawnPlayer(%this);

		if(%this.isBeingTimed) {
			%spawn = "_ChallengeSpawn" @ %this.timingFor;
			if(%this.timingFor != 0) {
				%this.player.setTransform(%spawn.getTransform());
				%this.player.setShapeNameColor("0 1 0");
			} else {
				%this.player.setShapeNameColor("0 1 1");
			}
		}
	}

	function serverCmdSuicide(%client) {
		if(%client.isBeingTimed) {
			%client.play2D(errorSound);
			return;
		}

		return parent::serverCmdSuicide(%client);
	}

	function GameConnection::autoAdminCheck(%this) {
		%this.loadProgress();
		return parent::autoAdminCheck(%this);
	}
};
activatePackage(ChallengeTimerPackage);