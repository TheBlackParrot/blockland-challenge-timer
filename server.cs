// Blockland should be able to handle prefs on its own, check config/{server,client}/prefs.cs
if($Pref::ChallengeTimer::Levels $= "") {
	$Pref::ChallengeTimer::Levels = 7;
}
if($Pref::ChallengeTimer::SaveDir $= "") {
	$Pref::ChallengeTimer::SaveDir = "config/server/challengeBoards";
}
if($Pref::ChallengeTimer::Playertype $= "") {
	$Pref::ChallengeTimer::Playertype = "PlayerNoJet";
}

exec("./support.cs");
exec("./playertype.cs"); // Frozen playertype, feel free to redistribute on its own
exec("./timer.cs");
exec("./unlock.cs");
exec("./board.cs");
exec("./system.cs");
exec("./events.cs");
exec("./commands.cs");
exec("./saving.cs");

$Pref::ChallengeTimer::Version = "1";

function createChallengeBoards() {
	// ChallengeBoard0 is to be used for the challenge overall
	for(%i=0;%i<=$Pref::ChallengeTimer::Levels;%i++) {
		%obj = "ChallengeBoard" @ %i;
		if(!isObject(%obj)) {
			new GuiTextListCtrl(%obj);
		}

		loadLeaderboard(%i);
	}
}
createChallengeBoards();