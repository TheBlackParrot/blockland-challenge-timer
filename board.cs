// i'd hook into parents here, but they're all named differently
function saveLeaderboard(%which) {
	%board = "ChallengeBoard" @ %which;
	if(!isObject(%board)) {
		warn(%board SPC "does not exist! \c4(saving)");
		return;
	}

	%filename = $Pref::ChallengeTimer::SaveDir @ "/leaderboards/" @ %board;
	%file = new FileObject();
	%file.openForWrite(%filename);

	for(%i=0;%i<%board.rowCount();%i++) {
		%bl_id = %board.getRowID(%i);
		%data = %board.getRowText(%i);
		%file.writeLine(%bl_id TAB %data);
	}

	%file.close();
	%file.delete();
}

function loadLeaderboard(%which) {
	%board = "ChallengeBoard" @ %which;
	if(!isObject(%board)) {
		warn(%board SPC "does not exist! \c4(loading)");
		return;
	}

	if(!isFile($Pref::ChallengeTimer::SaveDir @ "/leaderboards/" @ %board)) {
		return;
	}

	%filename = $Pref::ChallengeTimer::SaveDir @ "/leaderboards/" @ %board;
	%file = new FileObject();
	%file.openForRead(%filename);

	while(!%file.isEOF()) {
		%line = %file.readLine();
		%bl_id = getField(%line, 0);
		%data = getFields(%line, 1);

		if(%board.getRowNumByID(%bl_id) != -1) {
			%board.setRowByID(%bl_id, %data);
		} else {
			%board.addRow(%bl_id, %data);
		}
	}

	%file.close();
	%file.delete();
}

function GameConnection::updateLeaderboard(%this, %which) {
	%board = "ChallengeBoard" @ %which;
	if(!isObject(%board)) {
		warn(%board SPC "does not exist! \c4(updating)");
		return;
	}

	%bl_id = %this.bl_id;
	%result = mFloatLength($Sim::Time - %this.timerStart, 2);
	%data = %this.name TAB %result;

	%previous = getField(%board.getRowText(0), 1);

	if(%board.getRowNumByID(%bl_id) != -1) {
		%previous = getField(%board.getRowTextByID(%bl_id), 1);
		if(%result < %previous) {
			%board.setRowByID(%bl_id, %data);
			messageClient('', "\c4New personal record!");
		}
	} else {
		%board.addRow(%bl_id, %data);
	}

	%board.sortNumerical(1, 1);
	
	if(%board.getRowNumByID(%bl_id) == 0 && %result < %previous) {
		%str = "Level" SPC %which;
		if(%which == 0) {
			%str = "Overall Challenge";
		}
		messageAll('MsgAdminForce', "\c4NEW RECORD FOR\c3" SPC %str @ ":\c6" SPC getTimeString(%result) @ ", set by\c5" SPC %this.name @ "!");
	}

	saveLeaderboard(%which);
}