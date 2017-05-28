# LanguageRecognition
Parser for grammars(LALR, SLR)

# How to use

  ```
  LanguageRecognition <namespace_name>, <grammar_name>, <grammar_file_path>, <csproj path>
  ```
  Where :
  
  <b>namespace_name</b> - default namespace for generated classes
  
  <b>grammar_name</b>   - name for grammar
  
  <b>namespace_name</b> - path to grammar file
  
  <b>namespace_name</b> - path to .csproj file to insert generated classes
  
  
# Grammar example
  Grammar example can be found in sample.
  
  ```
  program : expressionStatement;
  expressionStatement : expression Semicolon expressionStatement | expression Semicolon;
  expression  : mutable EqualOp expression| sumExpression;
  sumExpression : sumExpression sum term | term;
  sum : SubOp | SumOp;
  mutable : variable;
  term  : term MulOp powExpression | powExpression;
  powExpression : unaryExpression PowOp powExpression | unaryExpression;
  unaryExpression : SubOp unaryExpression | variable | constant | OpenBracket sumExpression CloseBracket;
  variable  : Id;
  constant  : Number;


  Id  : "[_a-zA-Z][_a-zA-Z0-9]*";
  Number  : "[0-9]+";
  WhiteSpace  : "\s" -> skip;
  NewLine : "\n+" -> skip;
  EqualOp : "=";
  SumOp : "\+";
  SubOp : "-";
  MulOp : "\*";
  PowOp : "\^";
  OpenBracket : "\(";
  CloseBracket  : "\)";
  Semicolon : ";";
  ```

# TODO list   
1. Add SLR table.

2. Implement NFA, convert it do DFA and minimize it for lexer rules.

3. Replace a regex by a DFA from previous step (Not it's works O(N^2) for keywords, but it should be O(N)).

4. Remove reflection from LRParser.cs, generate a methods for each grammar rule, which will use suitable constructor (Now, the suitable constructor is chosen by refliction, it's slow).

5. Add tests.

6. Integrate to msbuild process

7. Generate grammar file parser by my generator.

8. Add more effective solution for creating LALR table (dragonbook 4.7.5)

