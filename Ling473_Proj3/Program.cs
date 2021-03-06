﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

// to compile on patas/dryas, type
//
// $ gmcs project3.cs
//
// to run on patas/dryas, then type
//
// $ mono project3.exe
//
// view output on: http://uakari.ling.washington.edu/473/example.html

namespace Ling473_Proj3
{
    class project3
    {
        static void Main (string[] args)
        {
            FSM fsm = new FSM();
            string line = "";
            StreamWriter sw = new StreamWriter("lopez380.html");
            sw.WriteLine("<html><meta http-equiv='Content-Type' content='text/html; charset=UTF-8' /><body>");
            string inputFile = (args.Length == 0) ? @"/opt/dropbox/15-16/473/project3/fsm-input.utf8.txt" : @args[0];
            sw.Write(fsm.Process(line));

            using (StreamReader sr = new StreamReader(inputFile))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    sw.Write(fsm.Process(line));
                    sw.WriteLine("<br/>");
                }
            }   
            sw.WriteLine("</body></html>");
            sw.Close();
            Console.WriteLine("Finished");
            Console.ReadLine();
        }
    }


    class FSM
    {
        //Hashmaps to check
        HashSet<Char> V1 = new HashSet<Char>("เแโใไ"); 
        HashSet<Char> C1 = new HashSet<Char>("กขฃคฅฆงจฉชซฌญฎฏฐฑฒณดตถทธนบปผฝพฟภมยรฤลฦวศษสหฬอฮ");
        HashSet<Char> C2 = new HashSet<Char>("รลวนม");
        HashSet<Char> V2 = new HashSet<Char>("ิีึืุูั็");
        HashSet<Char> T = new HashSet<Char> { '\u0E48', '\u0E49', '\u0E4A', '\u0E4B' };
        HashSet<Char> V3 = new HashSet<Char>("าอยว");
        HashSet<Char> C3 = new HashSet<Char>("งนมดบกยว");

        enum State : int
        {
          zero,one,two,three,four,five,six,seven,eight,nine
  
        };
        //this is to give an idea as to where the spaces have to be inserted
        enum space : int
        {
            none, before, now
        };
        public String Process (String s_in)
        {
            // Initialize State to zero when we start
            State st = State.zero;
            StringBuilder sb = new StringBuilder();
            for (int i =0 ; i< s_in.Length;i++)
            {
                var c = s_in[i];
                space sp = space.none;
                switch (st)
                {
                    case State.zero:
                        if (V1.Contains(c))
                            st = State.one;
                        else if (C1.Contains(c))
                            st = State.two;
                        else
                            throw new Exception("Invalid Input");
                        break;
                    case State.one:
                        if (C1.Contains(c))
                            st = State.two;
                        else
                            throw new Exception("Invalid Input");
                        break;
                    case State.two:
                        if (C2.Contains(c))
                            st = State.three;
                        else if (V2.Contains(c))
                            st = State.four;
                        else if (T.Contains(c))
                            st = State.five;
                        else if (V3.Contains(c))
                            st = State.six;
                        else if (C3.Contains(c))
                            {
                                sp = space.now;
                                st = State.zero;
                            }
                        else if (V1.Contains(c))
                            {
                                sp = space.before;
                                st = State.one;
                            } 
                        else if (C1.Contains(c))
                            {
                                sp = space.before;
                                st = State.two;
                            }
                        else
                            throw new Exception("Invalid Input");
                        break;
                    case State.three:
                        if (V2.Contains(c))
                            st = State.four;
                        else if (T.Contains(c))
                            st = State.five;
                        else if (V3.Contains(c))
                            st = State.six;
                        else if (C3.Contains(c))
                        {
                            sp = space.now;
                            st = State.zero;
                        }
                        else
                            throw new Exception("Invalid Input");
                        break;
                    case State.four:
                        if (T.Contains(c))
                            st = State.five;
                        else if (V3.Contains(c))
                            st = State.six;
                        else if (C3.Contains(c))
                        {
                            sp = space.now;
                            st = State.zero;
                        }
                        else if (V1.Contains(c))
                        {
                            sp = space.before;
                            st = State.one;
                        } 
                        else if (C1.Contains(c))
                        {
                            sp = space.before;
                            st = State.two;
                        }
                        else
                            throw new Exception("Invalid Input");
                        break;
                    case State.five:
                        if (V3.Contains(c))
                            st = State.six;
                        else if (C3.Contains(c))
                        {
                            sp = space.now;
                            st = State.zero;
                        }
                        else if (V1.Contains(c))
                        {
                            sp = space.before;
                            st = State.one;
                        } 
                        else if (C1.Contains(c))
                        {
                            sp = space.before;
                            st = State.two;
                        }
                        else
                            throw new Exception("Invalid Input");
                        break;
                    case State.six:
                        if (C3.Contains(c))
                        {
                            // this will eliminate the need for a special state 7
                            sp = space.now;
                            st = State.zero;
                        }
                        else if (V1.Contains(c))
                        {
                            sp = space.before;
                            st = State.one;
                        } 
                        else if (C1.Contains(c))
                        {
                            sp = space.before;
                            st = State.two;
                        }
                            
                        else
                            throw new Exception("Invalid Input");
                        break;
                    default:
                        throw new Exception("Invalid Input");
                }
                switch (sp)
                {
                    case space.none:
                        sb.Append(c);
                        break;
                    case space.before:
                        //When there is a character after the current character it will go to states 7 and 8 to trigger adding space
                        if (i < s_in.Length-1)
                            sb.Append(" ");
                        sb.Append(c);
                        break;
                    case space.now:
                        sb.Append(c);
                        if (i < s_in.Length - 1)
                            sb.Append(" ");
                        break;
                    default:
                        break;
                }
           }
            return sb.ToString();
        }
    }
}
