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
Each command is represented by exactly one symbol.

Green line
----------
This automatically generated line shows, what is being calculated for ech byte in the buffer.
The formula does not follows the order of operands ans is to be readed strictly from left to the right.
Parentheses are used instead of the order of operands. This makes the formula shorter.
The line warns you of invalid chars (substituted with #), stack underrun (substituted with _) and a not empty stack after executing ("STACK NOT EMPTY!" warning)

Crash proof language
--------------------

The language is crash proof.
It operates on a stack. The stack is emptied for each byte in the buffer.
If bytes are left over they are lost and not carried on. If too few bytes are available, they are substituted with zeros, except for divisions (as it would not be valid), where a 1 is supplied.

Once all commands have been parsed, the top value from the stack is returned