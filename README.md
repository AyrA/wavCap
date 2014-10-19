WavCap
======

Wave capture and playback library with example program

In response to a guy claiming Windows not a real OS in [this video](http://youtu.be/vCEUyx-SxPw).

On Windows we expect developers to know how to open an audio device and do not need silly /dev streams.


Console
=======
The console has a live script editor.

Yellow line
-----------
This is the code editor.
The usable symbols are shown below in the console.
Each command is represented by exactly one symbol. Use the arrow keys to navigate around and the other keys to replace chars in the script. Use INS to insert a char and DEL to remove a char. Backspace also works.
Changes are effective immediately.

Green line
----------
This automatically generated line shows, what is being calculated for ech byte in the buffer.
The formula does not follows the order of operands and is to be readed strictly from left to the right.
Parentheses are used instead of the order of operands. This makes the formula shorter.
The line warns you of invalid chars (substituted with #), stack underrun (substituted with _) and a not empty stack after executing ("STACK NOT EMPTY!" warning). All 3 cases are not problematic but may indicate mistakes made during script editing.

Crash proof language
--------------------

The language is crash proof (by applying common sense).
It operates on a stack. The stack is emptied for each byte in the buffer.
If bytes are left over they are lost and not carried on. If too few bytes are available, they are substituted with zeros, except for divisions (as it would not be valid), where a 1 is supplied.

Once all commands have been parsed, the top value from the stack is returned. The stack operates with 32 bit integers. The returned value is then cast to a byte for the buffer. This equals doing a modulo with 256.
