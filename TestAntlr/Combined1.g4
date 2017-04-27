grammar Combined1;
 
expression:
	'(' expression ')'                #Para             |
    expression ('+' | '-') expression #BinaryExpression |
	INT                               #IntExpression
;

INT : '0'..'9'+ ;
 
WS : [ \t\r\n]+ -> skip ;