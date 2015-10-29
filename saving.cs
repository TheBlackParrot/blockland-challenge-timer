function GameConnection::saveProgress(%this) {
	%file = new FileObject();

	%file.openForWrite($Pref::ChallengeTimer::SaveDir @ "/levels/" @ %this.bl_id);

	for(%i=0;%i<=$Pref::ChallengeTimer::Levels;%i++) {
		%file.writeLine(%i TAB %this.levelUnlocked[%i]);
	}

	%file.close();
	%file.delete();
}

function GameConnection::loadProgress(%this) {
	%file = new FileObject();
	%filename = $Pref::ChallengeTimer::SaveDir @ "/levels/" @ %this.bl_id;
	if(!isFile(%filename)) {
		return;
	}

	%file.openForRead(%filename);

	while(!%file.isEOF()) {
		%line = %file.readLine();
		%level = getField(%line, 0);
		%unl = getField(%line, 1);

		%this.levelUnlocked[%level] = %unl;
	}

	messageClient(%this, '', "\c2You may return to any of the levels you have unlocked using /goto");

	%file.close();
	%file.delete();
}