using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace wavCap
{
    public class StackLang
    {
        private Stack<int> S;
        private Stack<string> SS;

        public StackLang()
        {
        }

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
                        SS.Push(string.Format("~{0}", getS()));
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
                        SS.Push(c.ToString().ToUpper());
                        break;
                }
            }
            string s = getS();
            if (s.Length > 2)
            {
                return SS.Count == 0 ? s.Substring(1, s.Length - 2) : s.Substring(1, s.Length - 2) + " STACK NOT EMPTY!";
            }
            return SS.Count==0?s:s + " STACK NOT EMPTY!";
        }

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

        private int get()
        {
            return get(0);
        }

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

        private string getS()
        {
            if (SS.Count == 0)
            {
                return "_";
            }
            return SS.Pop();
        }
    }
}
