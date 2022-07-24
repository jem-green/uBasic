namespace Compukit_UK101_UWP
{
	public partial class BasicProg 
	{
		public string[] BestFitPolynomial16 = new string[] {
			"",
			"",
			" 10 FORI=1TO16:PRINT:NEXT:PRINT\"BEST FIT POLYNOMIAL\"",
			" 20 PRINT\"-------------------\":PRINT:GOTO9000",
			" 30 DIM X(50),Y(50),B(19),A(10,11),Z(19)",
			" 40 INPUT \"NO. OF POINTS\";N:FOR I=1 TO N",
			" 50 PRINT\"X(\"I\")=\";:INPUT X(I)",
			" 60 PRINT,\"Y(\"I\")=\";:INPUT Y(I):NEXT I",
			" 65 INPUT\"DEGREE OF POLYNOMIAL\";N1",
			" 70 N2=N1+1:N3=2*N1+1:N4=N2+1",
			" 80 FOR I=1 TO N3:Z(I)=0:IF I<=N2 THEN A(I,N4)=0",
			" 90 NEXT I",
			" 100 Z(1)=N:B(1)=1:FOR I=1 TO N",
			" 110 A(1,N4)=A(1,N4)+Y(I):FOR J=2 TO N3",
			" 120 B(J)=X(I)*B(J-1):Z(J)=Z(J)+B(J)",
			" 130 IF J<=N2 THEN A(J,N4)=A(J,N4)+B(J)*Y(I)",
			" 140 NEXT J:NEXT I",
			" 150 FOR I=1 TO N2:FOR J=1 TO N2",
			" 160 A(I,J)=Z(I+J-1):NEXT J:NEXT I",
			" 5100 FOR K=1 TO N2:P=A(K,K):A(K,K)=1",
			" 5110 IF P=0 THENPRINT\"ZERO DIAG ELEMENT\":END",
			" 5120 FOR J=K+1TON4:A(K,J)=A(K,J)/P:NEXT J:I=1",
			" 5130 IF I=K THEN 5200",
			" 5140 R=A(I,K):FOR J=1 TO N4",
			" 5150 A(I,J)=A(I,J)-R*A(K,J):NEXT J",
			" 5160 A(I,K)=0",
			" 5200 I=I+1:IF I=N4 THEN 5250",
			" 5210 GOTO 5130",
			" 5250 NEXT K:PRINT\"COEFFS ARE:-\"",
			" 5260 FOR I=1 TO N2:PRINT\"X(\"I\")=\"A(I,N4)",
			" 5270 NEXT:C2=0:INPUT\"TABLE NEEDED\";Q$",
			" 5280 IFLEFT$(Q$,1)=\"Y\"THENPRINT\"X-DATA\",\"Y-DATA\",\"Y CALCULATED\"",
			" 5282 FORI=1TON:Y2=A(1,N4):FORJ=2TON2",
			" 5290 IF X(I)<>0 THEN Y2=Y2+A(J,N4)*X(I)^(J-1)",
			" 5300 NEXT J:Y1=Y(I)-Y2:C2=C2+Y1*Y1",
			" 5305 IFLEFT$(Q$,1)=\"Y\"THENPRINTX(I),Y(I),Y2:NEXT",
			" 5310 IF N=N2 THEN 5330",
			" 5320 C2=C2/(N-N2)",
			" 5330 PRINT\"CHI=\";SQR(ABS(C2)):END",
			" 9000 PRINT\"THIS PROGRAM FITS A POLYNOMIAL OF ANY\"",
			" 9010 PRINT\"DEGREE TO A SET OF DATA OF Y AND X\"",
			" 9020 PRINT\"THE POLYNOMIAL IS:\"",
			" 9030 PRINT\"Y=X(1)+X(2)*X+X(3)*X^2+...+X(N+1)*X^N\"",
			" 9040 PRINT\"THE PROGRAM WILL ALSO PRINT A TABLE OF\"",
			" 9050 PRINT\"THE DATA COMPARED TO CALCULATED RESULTS\"",
			" 9060 PRINT\"AND THE VALUE OF 'CHI': A MEASURE OF 'FITNESS'\"",
			" 9070 PRINT\"(CHI IS SMALLER FOR A BETTER FIT)\"",
			" 9080 GOTO30",
			"OK",
		};
	}
}
