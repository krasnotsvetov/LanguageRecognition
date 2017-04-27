grammar GrammarFileGrammar;

/*
 * Parser Rules
 */

file : line+ EOF;
line : grammarRule ';'| lexerRule ';';
grammarRule : grammarName ':' productions;
productions : production | production '|' productions;
production  : grammarName | lexerToken | grammarName production| lexerToken production |;
grammarName : GrammarName;
lexerToken   : lexerRuleName | STRING;
lexerRuleName : LexerName;
lexerRule : LexerName ':' STRING | LexerName ':' STRING '->' GrammarName;

/*
 * Lexer Rules
 */

WhiteSpace  : (' '|'\t')+ -> skip;
NewLine     : ('\r'?		'\n' | '\r')+ -> skip;
GrammarName : [a-z][a-zA-Z0-9]*;
LexerName	: [A-Z][a-zA-Z0-9]*;
STRING		: ["]~('\r' | '\n' | '"')*["];