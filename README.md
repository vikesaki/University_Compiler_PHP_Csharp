# University_Compiler_PHP_Csharp
translate PHP code to C# code. (kinda)  
consisting of parser, lexer, and some basic semantic analyst.

## Lexer and Parser
works with basic PHP feature such as  

+ variable declaration
+ function declaration
+ conditional case (if)
+ loop case (while, for, foreach, dowhile)


## Semantic analyst
check the parser output and separate variables either to *global*, *local* or *used*.

## Test
some test that *worked* with the code is provided in the *compiler test list.txt*.  
the input file located in *bin/debug/net6.0/input.txt*
