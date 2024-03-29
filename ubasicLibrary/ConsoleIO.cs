﻿//  Copyright (c) 2017, Jeremy Green All rights reserved.

using System;
using System.Collections.Generic;
using System.Text;

namespace ubasicLibrary
{
    public class ConsoleIO : IConsoleIO
    {
        // Printing constraints

        #region Fields

        int consoleWidth = 75;
        int zoneWidth = 15;
        int compactWidth = 3;
        int hpos = 0;
        int vpos = 0;
        string input = "";
        string output = "";

        #endregion

        #region Properties

        public string Input
        {
            set
            {
                input = value;
            }
        }

        public string Output
        {
            get
            {
                return (output);
            }
        }

        public int Hpos
        {
            get
            {
                return (hpos);
            }
        }

        public int Vpos
        {
            get
            {
                return (vpos);
            }
        }

        public int Console
        {
            get
            {
                return (vpos);
            }
            set
            {
                consoleWidth = value;
            }
        }

        public int Zone
        {
            get
            {
                return (zoneWidth);
            }
            set
            {
                zoneWidth = value;
            }
        }

        public int Compact
        {
            get
            {
                return (compactWidth);
            }
            set
            {
                compactWidth = value;
            }
        }

        #endregion

        #region Methods

        public void Out(string s)
        {
            string check = s.TrimEnd(' ');
            hpos = hpos + s.Length;
            if (hpos > consoleWidth)
            {
                if (check.Length > 0)
                {
                    hpos = s.Length;
                    vpos = vpos + 1;
                    System.Console.Out.Write("\n");
                    System.Console.Out.Write(s);
                }
                else
                {
                    hpos = 0;
                    vpos = vpos + 1;
                    System.Console.Out.Write("\n");
                }
            }
            else
            {
                System.Console.Out.Write(s);
                // fix the carriage return not setting hpos
                if (s.EndsWith("\n"))
                {
                    hpos = 0;
                }
            }
        }

        public string In()
        {
            ConsoleKeyInfo key = new ConsoleKeyInfo();
            string value = "";
            do
            {
                while (System.Console.KeyAvailable == false)
                {
                    System.Threading.Thread.Sleep(250); // Loop until input is entered.
                }
                key = System.Console.ReadKey(false);
                // Issue here with deleting characters should allow for this
                if ( key.Key == ConsoleKey.Backspace)
                {
                    if (value.Length > 0)
                    {
                        value = value.Substring(0, value.Length - 1);
                        System.Console.Write(' ');
                        System.Console.CursorLeft = System.Console.CursorLeft - 1;

                    }
                    else
                    {
                        
                        System.Console.CursorLeft = System.Console.CursorLeft + 1;
                    }
                }
                else
                {
                    value = value + key.KeyChar;
                }
            }
            while (key.Key != ConsoleKey.Enter);
            return (value);
        }

        public void Error(string e)
        {
            System.Console.Error.Write(e);
        }

        #endregion

    }
}
