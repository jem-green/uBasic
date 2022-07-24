namespace Compukit_UK101_UWP
{
	public partial class BasicProg 
	{
		public string[] Gunfight = new string[] {
			"",
			"",
			" 10 FORI=1TO16:PRINT:NEXT:PRINT\"GUNFIGHT!!!\":PRINT\"-----------\":PRINT",
			" 20 INPUT\"INSTRUCTIONS\";Q$:IFLEFT$(Q$,1)=\"N\"THEN100",
			" 30 PRINT:PRINT:PRINT\"THIS IS A WILD WEST GUNFIGHT BETWEEN\"",
			" 40 PRINT\"TWO PLAYORS.\":PRINT\"LEFT PLAYOR:-\"",
			" 50 PRINT\"   MOVE UP BY PRESSING '1'\"",
			" 60 PRINT\"   MOVE DOWN BY PRESSING LEFT SHIFT\"",
			" 70 PRINT\"   FIRE BY PRESSING 'Z'\"",
			" 80 PRINT\"RIGHT PLAYER:-\"",
			" 90 PRINT\"   MOVE UP BY PRESSING RUBOUT\"",
			" 91 PRINT\"   MOVE DOWN BY PRESSING RIGHT SHIFT\"",
			" 92 PRINT\"   FIRE BY PRESSING '/'\"",
			" 93 PRINT\"BODY HIT SCORES 1 POINT\"",
			" 94 PRINT\"HEAD HIT SCORES 2 POINTS\"",
			" 95 PRINT:PRINT\"PRESS SPACE BAR TO START\";",
			" 96 POKE 530,1:POKE 57088,253:WAIT 57088,255,255",
			" 100 SC=53196:FORI=1TO16:PRINT:NEXT",
			" 105 POKE(54221),32:POKE(530),1",
			" 110 DATA 161,32,32,32,32,32",
			" 120 DATA 161,32,32,32,32,161",
			" 130 DATA 177,178,32,32,176,175",
			" 140 DATA 32,177,178,176,175,32",
			" 150 DATA 32,32,161,161,32,32,32,32,161,161,32,32",
			" 160 DATA 32,32,161,161,32,32",
			" 170 KEY=57088:LH=0:RH=0",
			" 180 LS=0:RS=0:T=0:TS=0",
			" 200 FORI=5TO11:FORJ=21TO26",
			" 210 READ P:POKE(SC+I*64+J),P:NEXTJ,I",
			" 220 PL=SC+648:PR=SC+680",
			" 300 PM=PL:M=-1:GOSUB1000",
			" 310 PM=PR:M=1:GOSUB1000",
			" 500 POKE(KEY),254:K=PEEK(KEY)AND4",
			" 505 M=-1:IFPL>54220 THEN 520",
			" 510 IFK=0THENPL=PL+64:PM=PL:GOSUB1000",
			" 520 POKE(KEY),127:K=PEEK(KEY)AND128",
			" 525 IF PL<53452 THEN 540",
			" 530 IFK=0THENPL=PL-64:PM=PL:GOSUB1000",
			" 540 POKE(KEY),254:K=PEEK(KEY)AND2",
			" 545 M=1:IF PR>54220 THEN 560",
			" 550 IFK=0THENPR=PR+64:PM=PR:GOSUB1000",
			" 560 POKE(KEY),191:K=PEEK(KEY)AND4",
			" 565 IF PR<53452 THEN 580",
			" 570 IFK=0THENPR=PR-64:PM=PR:GOSUB1000",
			" 580 M=-1",
			" 600 POKE(KEY),253:K=PEEK(KEY)AND32",
			" 610 IFK=0ANDRH>10THENRH=0:LH=5:PM=PL:GOSUB2000",
			" 620 M=1",
			" 630 POKE(KEY),253:K=PEEK(KEY)AND8",
			" 640 IFK=0ANDLH>10THENLH=0:RH=5:PM=PR:GOSUB2000",
			" 650 LH=LH+1:RH=RH+1:T=T+1",
			" 660 IF T<500 THEN 500",
			" 670 FORI=1TO16:PRINT:NEXT:PRINT\"END OF GAME\":PRINT:PRINT",
			" 680 PRINT\"LEFT PLAYER SCORE:-\";RS",
			" 690 PRINT\"RIGHT PLAYER SCORE:-\";LS:PRINT:PRINT",
			" 700 INPUT\"ANOTHER GAME\";Q$:IFLEFT$(Q$,1)=\"N\"THENEND",
			" 710 RESTORE:GOTO 100",
			" 1000 REM PUT MAN M AT POSN PM",
			" 1010 POKE(PM-193),32:POKE(PM-129),226",
			" 1015 POKE(PM-128),32:POKE(PM-130),32",
			" 1017 POKE(PM-129-M*2),32",
			" 1020 POKE(PM-65),161:POKE(PM-64),197",
			" 1030 POKE(PM-66),195:POKE(PM-1),32",
			" 1040 POKE(PM),190:POKE(PM-2),189",
			" 1050 POKE(PM+64),32:POKE(PM+62),32",
			" 1060 POKE(PM-65-M*2),133:POKE(PM-129-M*2),32",
			" 1070 POKE(PM-1-M*2),32:RETURN",
			" 2000 REM GUNFIRE OF MAN M AT PM",
			" 2005 D=0",
			" 2010 PB=PM-65-M*3:POKE(PB),234.5+M/2",
			" 2015 FORI=1TO100:NEXT",
			" 2020 POKE(PB),45:PB=PB-M:H=PEEK(PB)",
			" 2025 HH=H",
			" 2030 IFH=226ORH=133ORH=190ORH=189THEN3000",
			" 2040 IFH<>32 THEN 2500",
			" 2050 POKE(PB+M),32:D=D+1:IF D<50 THEN 2020",
			" 2060 RETURN",
			" 2500 FORI=0TO100:POKE(PB),I:NEXT",
			" 2510 POKE(PB),32:POKE(PB+M),32:RETURN",
			" 3000 REM MAN HIT",
			" 3010 POKE(PB+M),32",
			" 3020 IF M=-1 THEN PM=PR",
			" 3030 IF M=1 THEN PM=PL",
			" 3040 FORI=-2TO0:FORJ=-3TO1",
			" 3050 POKE(PM+I*64+J),32:NEXT J,I",
			" 3060 POKE(PM),166+M:POKE(PM-M),128",
			" 3065 POKE(PM-M*2),128:POKE(PM-M*3),161",
			" 3070 POKE(PM-M*3-64),226:FORI=1TO1000:NEXT",
			" 3080 POKE(PM-M*3-64),32:POKE(PM-M*3),154:POKE(PM-M*4),154",
			" 3090 POKE(PM-M*5),226:FORI=1TO2000:NEXT",
			" 3100 FORI=0 TO M*5 STEP M:POKE(PM-I),32:NEXT",
			" 3102 IFM=-1THEN RS=RS+1-(H=226)",
			" 3104 IFM=1 THEN LS=LS+1-(H=226)",
			" 3110 M=-M:GOSUB 1000",
			" 3120 RETURN",
			"OK",
		};
	}
}
