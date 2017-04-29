# LanguageRecognition
Parser for grammars(LALR, SLR)

Now it's support LR(1) grammar. 

TODO:

1)Add LALR table. 

2)Add SLR table.

3)Implement NFA, convert it do DFA and minimize it for lexer rules.

4)Replace a regex by a DFA from previous step (Not it's works O(N^2) for keywords, but it should be O(N)).

5)Remove reflection from LRParser.cs, generate a methods for each grammar rule, which will use suitable constructor (Now, the suitable constructor is chosen by refliction, it's slow).

6)Add tests.

7)Make a grammar build tool.

8)Generate grammar file parser by my parser.


How it works:

You should create a grammar file and project. 

You should add a reference to LanguageRecognition in your project.

You should run a LanguageRecognition with parameters which include a project directory and grammar file name.
