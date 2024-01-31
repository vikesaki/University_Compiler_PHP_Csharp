# Compiler_PHP_Python
translate PHP code to Python code. (kinda)  
consisting of parser, lexer, and some basic semantic analyst.

## Lexer, Parser, and Translator
works with basic PHP feature such as  

+ variable declaration
+ function declaration
+ conditional case (if, elif, else)
+ loop case (while, for, foreach, dowhile)
+ increment and decrement

known issue

+ cant call function inside conditions
+ parser didn't read conditional and loop inside function

## Semantic analyst
check the parser output and separate variables either to *global*, *local* or *used*.

## Optimization
remove all the *global* that is not *used*.
to run the optimization, the *optimization* value inside *Program.cs* need to be *true*

known issue

+ wont remove the unused local var


## Test
some test that *worked* with the code is provided in the *compiler test list.txt*.  
the input file located in *bin/debug/net6.0/input.txt*
