10 X1 = 1
20 Y1 = 2
30 X2 = 0
40 Y2 = 0
50 X3 = 0
60 Y3 = 0
70 U = 0
80 IF U=0 THEN X2=X1-1: X3=X1+1: Y2=Y1: Y3=Y1: GOTO 100
90 GOTO 110
100 PRINT "X1=";X2;"Y1=";Y1;"X2=";X2;"Y2=";Y2;"X3=";X3;"Y3=";Y3
110 END