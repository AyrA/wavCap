﻿//uncomment this if you want to keep order of operands.
//basically adds a shitload of parentheses if commented out
//#define OOO

using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace wavCap
{
    /// <summary>
    /// provides a crash proof stack language
    /// </summary>
    public class StackLang
    {
        /// <summary>
        /// Stack of integers for regular operation
        /// </summary>
        private Stack<int> S;
        /// <summary>
        /// Stack of string for string building the formula
        /// </summary>
        private Stack<string> SS;

        /// <summary>
        /// Initializes the StackLanguage class. (Does nothing)
        /// </summary>
        public StackLang()
        {
        }

        /// <summary>
        /// Evaluates code to a formula
        /// </summary>
        /// <param name="Code">code</param>
        /// <returns>maths formula</returns>
        public string eval(string Code)
        {
            SS = new Stack<string>();
            Hashtable HT = new Hashtable();
            foreach (char c in Code)
            {
                switch (c)
                {
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        //push number onto stack
                        SS.Push(c.ToString());
                        break;
                    case '+':
                        //add
                        SS.Push(string.Format("({0}+{1})", getS(), getS()));
                        break;
                    case '-':
                        //subtract
                        SS.Push(string.Format("({0}-{1})", getS(), getS()));
                        break;
                    case '*':
                        //multi
                        SS.Push(string.Format("({0}*{1})", getS(), getS()));
                        break;
                    case '/':
                        //div
                        SS.Push(string.Format("({0}/{1})", getS(), getS()));
                        break;
                    case '%':
                        //modulo
                        SS.Push(string.Format("({0}%{1})", getS(), getS()));
                        break;
                    case '>':
                        //shift right
                        SS.Push(string.Format("({0}>>{1})", getS(), getS()));
                        break;
                    case '<':
                        //shift left
                        SS.Push(string.Format("({0}<<{1})", getS(), getS()));
                        break;
                    case '&':
                        //and
                        SS.Push(string.Format("({0}&{1})", getS(), getS()));
                        break;
                    case '|':
                        //or
                        SS.Push(string.Format("({0}|{1})", getS(), getS()));
                        break;
                    case '^':
                        //xor
                        SS.Push(string.Format("({0}^{1})", getS(), getS()));
                        break;
                    case '~':
                        //complement
                        SS.Push(string.Format("(~{0})", getS()));
                        break;
                    case '[':
                        //increment
                        SS.Push(string.Format("({0}+1)", getS()));
                        break;
                    case ']':
                        //decrement
                        SS.Push(string.Format("({0}-1)", getS()));
                        break;
                    case '!':
                        //power
                        SS.Push(string.Format("Pow({0},{1})", getS(), getS()));
                        break;
                    case ' ':
                        //NOOP
                        break;
                    default:
                        //get predefined value
                        if (char.IsLetter(c))
                        {
                            SS.Push(c.ToString().ToUpper());
                        }
                        else
                        {
                            //invalid char
                            SS.Push("#");
                        }
                        break;
                }
            }
            string s = getS();
            while (s.StartsWith("(") && s.EndsWith(")"))
            {
                s = s.Substring(1, s.Length - 2);
            }
            return SS.Count==0?s:s + " STACK NOT EMPTY!";
        }

        /// <summary>
        /// processes code
        /// </summary>
        /// <param name="Code">code to process</param>
        /// <param name="Vars">variables to pass</param>
        /// <returns>result</returns>
        public int Process(string Code, string[] Vars)
        {
            S = new Stack<int>();
            Hashtable HT = new Hashtable();
            foreach (string Var in Vars)
            {
                HT.Add(Var[0], int.Parse(Var.Substring(1)));
            }
            foreach (char c in Code)
            {
                switch (c)
                {
                    case '0':
                    case '1':
                    case '2':
                    case '3':
                    case '4':
                    case '5':
                    case '6':
                    case '7':
                    case '8':
                    case '9':
                        //push number onto stack
                        S.Push(c - 48);
                        break;
                    case '+':
                        //add
                        S.Push(get() + get());
                        break;
                    case '-':
                        //subtract
                        S.Push(get() - get());
                        break;
                    case '*':
                        //multi
                        S.Push(get() * get());
                        break;
                    case '/':
                        //div
                        S.Push(get() / get(1));
                        break;
                    case '%':
                        //modulo
                        S.Push(get() % get(1));
                        break;
                    case '>':
                        //shift right
                        S.Push(get() >> get());
                        break;
                    case '<':
                        //shift left
                        S.Push(get() << get());
                        break;
                    case '&':
                        //and
                        S.Push(get() & get());
                        break;
                    case '|':
                        //or
                        S.Push(get() | get());
                        break;
                    case '^':
                        //xor
                        S.Push(get() ^ get());
                        break;
                    case '~':
                        //complement
                        S.Push(~get());
                        break;
                    case '[':
                        //increment
                        S.Push(get() + 1);
                        break;
                    case ']':
                        //decrement
                        S.Push(get() - 1);
                        break;
                    case '!':
                        //power
                        S.Push((int)Math.Pow(get(), get()));
                        break;
                    case ' ':
                        //NOOP
                        break;
                    default:
                        //get predefined value
                        S.Push(HT[c] != null ? (int)HT[c] : 0);
                        break;
                }
            }
            return get();
        }

        /// <summary>
        /// returns a number from the stack or 0 if empty
        /// </summary>
        /// <returns>number</returns>
        private int get()
        {
            return get(0);
        }

        /// <summary>
        /// returns a number from the stack
        /// </summary>
        /// <param name="onZeroOrEmpty">Number to return if stack is 0 or empty</param>
        /// <returns>number</returns>
        private int get(int onZeroOrEmpty)
        {
            if (S.Count == 0)
            {
                return onZeroOrEmpty;
            }
            if (S.Peek() == 0)
            {
                S.Pop();
                return onZeroOrEmpty;
            }
            return S.Pop();
        }

        /// <summary>
        /// returns top string element from stack, or _ if empty
        /// </summary>
        /// <returns>string</returns>
        private string getS()
        {
            if (SS.Count == 0)
            {
                return "_";
            }
            string s = SS.Pop();
            //clean value
#if !OOO
		    while (s.StartsWith("((") && s.EndsWith("))"))
            {
                s = s.Substring(1, s.Length - 2);
            }
#endif
            return s;
        }
    }
}
