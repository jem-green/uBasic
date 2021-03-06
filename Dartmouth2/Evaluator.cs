// Copyright (C) 1988 Jack W. Crenshaw. All rights reserved. 

using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using log4net;
using uBasicLibrary;

namespace Dartmouth2
{
    public class Evaluator
    {
        #region Variables

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //protected System.IO.TextReader In = null;
        //protected System.IO.TextWriter Out = null;
        //protected System.IO.TextWriter Error = null;

        readonly Stack<object> stack;

        private readonly Tokenizer tokenizer;

        const int MAX_VARNUM = 26;
        readonly int[] variables = new int[MAX_VARNUM];
        readonly Hashtable stringVariables;
        readonly Hashtable numericVariables;
        readonly Hashtable numericArrayVariables;
        readonly Hashtable stringArrayVariables;

        // functions

        public struct function_index
        {
            private readonly int programTextPosition;
            private readonly int @params;
            private readonly string[] param;

            public function_index(int pos, int parameters, string[] parameter)
            {
                this.programTextPosition = pos;
                this.@params = parameters;
                this.param = parameter;
            }
            public int ProgramTextPosition { get { return programTextPosition; } }
            public int parameters { get { return @params; } }
            public string[] parameter { get { return param; } }

        }
        const int MAX_FUNCTIONS = 26;
        public function_index[] functions;

        int randomize = 0;

        #endregion
        #region Constructors

        public Evaluator(Tokenizer tokenizer)
        {
            stack = new Stack<object>();
            this.tokenizer = tokenizer;
            stringVariables = new Hashtable();
            numericVariables = new Hashtable();
            numericArrayVariables = new Hashtable();
            stringArrayVariables = new Hashtable();
            functions = new function_index[MAX_FUNCTIONS];
        }

        #endregion Constructors
        #region Properties



        #endregion Properties
        #region Methods

        public void Randomize()
        {
            Debug("Randomize");
            randomize = Environment.TickCount;
            Debug("Randomize");
        }

        /// <summary>
        /// Expression
        /// </summary>
        /// <returns></returns>
        public void Expression()
        {
            Tokenizer.Token op;
            Debug("Expression");

            // check if negative number

            if (tokenizer.GetToken() == Tokenizer.Token.TOKENIZER_MINUS)
            {
                tokenizer.NextToken();
                stack.Push((double)0);
                Term();
                Subtract();
            }
            else
            {
                Term();
            }
            op = tokenizer.GetToken();
            Debug("Expression: token " + Convert.ToString(op));
            while (op == Tokenizer.Token.TOKENIZER_PLUS || op == Tokenizer.Token.TOKENIZER_MINUS || op == Tokenizer.Token.TOKENIZER_AMPERSAND || op == Tokenizer.Token.TOKENIZER_BAR)
            {
                tokenizer.NextToken();
                Term();
                switch (op)
                {
                    case Tokenizer.Token.TOKENIZER_PLUS:
                        {
                            Add();
                            break;
                        }
                    case Tokenizer.Token.TOKENIZER_MINUS:
                        {
                            Subtract();
                            break;
                        }
                    case Tokenizer.Token.TOKENIZER_AMPERSAND:
                        {
                            //t1 = t1 & t2;
                            break;
                        }
                    case Tokenizer.Token.TOKENIZER_BAR:
                        {
                            //t1 = t1 | t2;
                            break;
                        }
                }
                op = tokenizer.GetToken();
            }
            Debug("Expression");
        }

        /// <summary>
        /// Term
        /// </summary>
        /// <returns></returns>
        private void Term()
        {
            Tokenizer.Token op;
            Debug("term");

            Debug("term: token " + tokenizer.GetToken());
            Exponent();
            op = tokenizer.GetToken();
            Debug("term: token " + op);

            while (op == Tokenizer.Token.TOKENIZER_ASTR || op == Tokenizer.Token.TOKENIZER_SLASH || op == Tokenizer.Token.TOKENIZER_MOD)
            {
                tokenizer.NextToken();
                Debug("term: token " + tokenizer.GetToken());
                Exponent();

                switch (op)
                {
                    case Tokenizer.Token.TOKENIZER_ASTR:
                        {
                            Multiply();
                            break;
                        };
                    case Tokenizer.Token.TOKENIZER_SLASH:
                        {
                            Divide();
                            break;
                        }
                }
                op = tokenizer.GetToken();
            }
            Debug("term");
        }

        /// <summary>
        /// Complex
        /// </summary>
        /// <returns></returns>
        private void Exponent()
        {
            Tokenizer.Token op;
            Debug("exponent");

            Debug("exponent: token " + tokenizer.GetToken());
            switch (tokenizer.GetToken())
            {
                case Tokenizer.Token.TOKENIZER_FUNCTION:
                    {
                        break;
                    }

                default:
                    {
                        Factor();
                        break;
                    }
            }

            op = tokenizer.GetToken();
            Debug("exponent: token " + op);
            while (op == Tokenizer.Token.TOKENIZER_EXPONENT)
            {
                tokenizer.NextToken();
                Debug("exponent: token " + tokenizer.GetToken());
                switch (tokenizer.GetToken())
                {
                    case Tokenizer.Token.TOKENIZER_FUNCTION:
                        {
                            break;
                        }
                    default:
                        {
                            Factor();
                            break;
                        }
                }

                switch (op)
                {
                    case Tokenizer.Token.TOKENIZER_EXPONENT:
                        {
                            Power();
                            break;
                        };
                }
                op = tokenizer.GetToken();
            }
            Debug("term");
        }

        /// <summary>
        /// Factor
        /// </summary>
        private void Factor()
        {
            object f;
            function_index function;
            Debug("Factor");

            Debug("Factor: token " + tokenizer.GetToken());
            string varName;
            int num;
            switch (tokenizer.GetToken())
            {
                case Tokenizer.Token.TOKENIZER_FN:
                    {
                        tokenizer.AcceptToken(Tokenizer.Token.TOKENIZER_FN);
                        varName = tokenizer.GetNumericArrayVariable();
                        Debug("factor: function " + varName);
                        tokenizer.AcceptToken(Tokenizer.Token.TOKENIZER_NUMERIC_ARRAY_VARIABLE);
                        num = varName[0] - (int)'a';
                        function = functions[num];

                        // a number of paramerters that could be expressions until the ')'

                        do
                        {
                            if (tokenizer.GetToken() == Tokenizer.Token.TOKENIZER_COMMA)
                            {
                                tokenizer.NextToken();
                            }
                            else if (tokenizer.GetToken() == Tokenizer.Token.TOKENIZER_CR)
                            {
                                // Skip
                            }
                            else
                            {
                                Expression();
                                // this will be left the stack in reverse order
                            }
                        }
                        while (tokenizer.GetToken() != Tokenizer.Token.TOKENIZER_RIGHTPAREN);
                        tokenizer.AcceptToken(Tokenizer.Token.TOKENIZER_RIGHTPAREN);

                        // assign the expressions to the variables in the correct order

                        for (int i = function.parameters - 1; i >= 0; i--)
                        {
                            f = PopDouble();
                            Debug("Factor: function numeric " + Convert.ToString(f));
                            SetNumericVariable(function.parameter[i], (double)f);
                        }

                        // now jump to the function execute and then restore the position and continue 

                        int current_pos = tokenizer.GetPosition();
                        tokenizer.Init(function.ProgramTextPosition);
                        Expression();
                        tokenizer.Init(current_pos);
                        break;
                    }
                case Tokenizer.Token.TOKENIZER_ABS:
                    {
                        tokenizer.AcceptToken(Tokenizer.Token.TOKENIZER_ABS);
                        tokenizer.AcceptToken(Tokenizer.Token.TOKENIZER_LEFTPAREN);
                        Expression();
                        tokenizer.AcceptToken(Tokenizer.Token.TOKENIZER_RIGHTPAREN);
                        Abs();
                        break;
                    }
                case Tokenizer.Token.TOKENIZER_ATN:
                    {
                        tokenizer.AcceptToken(Tokenizer.Token.TOKENIZER_ATN);
                        tokenizer.AcceptToken(Tokenizer.Token.TOKENIZER_LEFTPAREN);
                        Expression();
                        tokenizer.AcceptToken(Tokenizer.Token.TOKENIZER_RIGHTPAREN);
                        Atn();
                        break;
                    }
                case Tokenizer.Token.TOKENIZER_COS:
                    {
                        tokenizer.AcceptToken(Tokenizer.Token.TOKENIZER_COS);
                        tokenizer.AcceptToken(Tokenizer.Token.TOKENIZER_LEFTPAREN);
                        Expression();
                        tokenizer.AcceptToken(Tokenizer.Token.TOKENIZER_RIGHTPAREN);
                        Cos();
                        break;
                    }
                case Tokenizer.Token.TOKENIZER_EXP:
                    {
                        tokenizer.AcceptToken(Tokenizer.Token.TOKENIZER_EXP);
                        tokenizer.AcceptToken(Tokenizer.Token.TOKENIZER_LEFTPAREN);
                        Expression();
                        tokenizer.AcceptToken(Tokenizer.Token.TOKENIZER_RIGHTPAREN);
                        Exp();
                        break;
                    }
                case Tokenizer.Token.TOKENIZER_INT:
                    {
                        tokenizer.AcceptToken(Tokenizer.Token.TOKENIZER_INT);
                        tokenizer.AcceptToken(Tokenizer.Token.TOKENIZER_LEFTPAREN);
                        Expression();
                        tokenizer.AcceptToken(Tokenizer.Token.TOKENIZER_RIGHTPAREN);
                        Int();
                        break;
                    }
                case Tokenizer.Token.TOKENIZER_LOG:
                    {
                        tokenizer.AcceptToken(Tokenizer.Token.TOKENIZER_LOG);
                        tokenizer.AcceptToken(Tokenizer.Token.TOKENIZER_LEFTPAREN);
                        Expression();
                        tokenizer.AcceptToken(Tokenizer.Token.TOKENIZER_RIGHTPAREN);
                        Log();
                        break;
                    }
                case Tokenizer.Token.TOKENIZER_RND:
                    {
                        tokenizer.AcceptToken(Tokenizer.Token.TOKENIZER_RND);
                        tokenizer.AcceptToken(Tokenizer.Token.TOKENIZER_LEFTPAREN);
                        Expression();
                        tokenizer.AcceptToken(Tokenizer.Token.TOKENIZER_RIGHTPAREN);
                        Rnd();
                        break;
                    }
                case Tokenizer.Token.TOKENIZER_SIN:
                    {
                        tokenizer.AcceptToken(Tokenizer.Token.TOKENIZER_SIN);
                        tokenizer.AcceptToken(Tokenizer.Token.TOKENIZER_LEFTPAREN);
                        Expression();
                        tokenizer.AcceptToken(Tokenizer.Token.TOKENIZER_RIGHTPAREN);
                        Sin();
                        break;
                    }
                case Tokenizer.Token.TOKENIZER_SQR:
                    {
                        tokenizer.AcceptToken(Tokenizer.Token.TOKENIZER_SQR);
                        tokenizer.AcceptToken(Tokenizer.Token.TOKENIZER_LEFTPAREN);
                        Expression();
                        tokenizer.AcceptToken(Tokenizer.Token.TOKENIZER_RIGHTPAREN);
                        SquareRoot();
                        break;
                    }
                case Tokenizer.Token.TOKENIZER_TAN:
                    {
                        tokenizer.AcceptToken(Tokenizer.Token.TOKENIZER_TAN);
                        tokenizer.AcceptToken(Tokenizer.Token.TOKENIZER_LEFTPAREN);
                        Expression();
                        tokenizer.AcceptToken(Tokenizer.Token.TOKENIZER_RIGHTPAREN);
                        Tan();
                        break;
                    }
                case Tokenizer.Token.TOKENIZER_NUMBER:
                    {
                        f = tokenizer.GetNumber();
                        Debug("Factor: number " + Convert.ToString(f));
                        tokenizer.AcceptToken(Tokenizer.Token.TOKENIZER_NUMBER);
                        stack.Push(f);
                        break;
                    }
                case Tokenizer.Token.TOKENIZER_INTEGER:
                    {
                        f = (double)tokenizer.GetInteger();
                        Debug("Factor: integer " + Convert.ToString(f));
                        tokenizer.AcceptToken(Tokenizer.Token.TOKENIZER_INTEGER);
                        stack.Push(f);
                        break;
                    }
                case Tokenizer.Token.TOKENIZER_STRING:
                    {
                        f = tokenizer.Getstring();
                        Debug("Factor: string " + Convert.ToString(f));
                        tokenizer.AcceptToken(Tokenizer.Token.TOKENIZER_STRING);
                        stack.Push((string)f);
                        break;
                    }

                case Tokenizer.Token.TOKENIZER_LEFTPAREN:
                    {
                        tokenizer.AcceptToken(Tokenizer.Token.TOKENIZER_LEFTPAREN);
                        Expression();
                        tokenizer.AcceptToken(Tokenizer.Token.TOKENIZER_RIGHTPAREN);
                        break;
                    }
                case Tokenizer.Token.TOKENIZER_STRING_VARIABLE:
                    {
                        f = GetStringVariable(tokenizer.GetStringVariable());
                        Debug("Factor: string " + Convert.ToString(f));
                        tokenizer.AcceptToken(Tokenizer.Token.TOKENIZER_STRING_VARIABLE);
                        stack.Push(f);
                        break;
                    }
                case Tokenizer.Token.TOKENIZER_NUMERIC_VARIABLE:
                    {
                        f = GetNumericVariable(tokenizer.GetNumericVariable());
                        Debug("Factor: numeric " + Convert.ToString(f));
                        tokenizer.AcceptToken(Tokenizer.Token.TOKENIZER_NUMERIC_VARIABLE);
                        stack.Push(f);
                        break;
                    }

                case Tokenizer.Token.TOKENIZER_NUMERIC_ARRAY_VARIABLE:
                    {
                        int numeric;
                        int dimension = 0;
                        int[] dimensions = new int[10];
                        varName = tokenizer.GetNumericArrayVariable();

                        dimensions[0] = 0;
                        tokenizer.AcceptToken(Tokenizer.Token.TOKENIZER_NUMERIC_ARRAY_VARIABLE);
                        do
                        {
                            if (tokenizer.GetToken() == Tokenizer.Token.TOKENIZER_COMMA)
                            {
                                tokenizer.NextToken();
                            }
                            else
                            {
                                Expression();
                                numeric = (int)Math.Truncate(PopDouble());
                                dimension++;
                                dimensions[dimension] = numeric;
                            }
                        }
                        while (tokenizer.GetToken() != Tokenizer.Token.TOKENIZER_RIGHTPAREN);
                        tokenizer.AcceptToken(Tokenizer.Token.TOKENIZER_RIGHTPAREN);

                        f = GetNumericArrayVariable(varName, dimension, dimensions);
                        Debug("Factor: numeric array " + Convert.ToString(f));
                        stack.Push(f);
                        break;

                    }

                case Tokenizer.Token.TOKENIZER_STRING_ARRAY_VARIABLE:
                    {
                        int numeric;
                        int dimension = 0;
                        int[] dimensions = new int[10];
                        varName = tokenizer.GetNumericArrayVariable();

                        dimensions[0] = 0;
                        tokenizer.AcceptToken(Tokenizer.Token.TOKENIZER_STRING_ARRAY_VARIABLE);
                        do
                        {
                            if (tokenizer.GetToken() == Tokenizer.Token.TOKENIZER_COMMA)
                            {
                                tokenizer.NextToken();
                            }
                            else
                            {
                                Expression();
                                numeric = (int)Math.Truncate(PopDouble());
                                dimension++;
                                dimensions[dimension] = numeric;
                            }
                        }
                        while (tokenizer.GetToken() != Tokenizer.Token.TOKENIZER_RIGHTPAREN);
                        tokenizer.AcceptToken(Tokenizer.Token.TOKENIZER_RIGHTPAREN);

                        f = GetStringArrayVariable(varName, dimension, dimensions);
                        Debug("Factor: string array " + Convert.ToString(f));
                        stack.Push(f);
                        break;

                    }

                default:
                    {
                        num = tokenizer.GetIntegerVariable();
                        f = GetIntVariable(num);
                        Debug("Factor: int " + Convert.ToString(f));
                        tokenizer.AcceptToken(Tokenizer.Token.TOKENIZER_INTEGER);
                        stack.Push(f);
                        break;
                    }
            }
            Debug("Factor");
        }

        /// <summary>
        /// Relation
        /// </summary>
        /// <returns></returns>
        public void Relation()
        {
            Tokenizer.Token op;

            Debug("Relation");
            Expression();
            op = tokenizer.GetToken();

            Debug("relation: token " + Convert.ToString(op));
            while (op == Tokenizer.Token.TOKENIZER_LT || op == Tokenizer.Token.TOKENIZER_GT || op == Tokenizer.Token.TOKENIZER_EQ)
            {
                tokenizer.NextToken();

                // Check here if the op is a combined <= or <> or >= in this order

                if ((op == Tokenizer.Token.TOKENIZER_LT) && (tokenizer.GetToken() == Tokenizer.Token.TOKENIZER_EQ))
                {
                    op = Tokenizer.Token.TOKENIZER_LTEQ;
                    tokenizer.NextToken();
                }
                else if ((op == Tokenizer.Token.TOKENIZER_LT) && (tokenizer.GetToken() == Tokenizer.Token.TOKENIZER_GT))
                {
                    op = Tokenizer.Token.TOKENIZER_NOTEQ;
                    tokenizer.NextToken();
                }
                else if ((op == Tokenizer.Token.TOKENIZER_GT) && (tokenizer.GetToken() == Tokenizer.Token.TOKENIZER_EQ))
                {
                    op = Tokenizer.Token.TOKENIZER_GTEQ;
                    tokenizer.NextToken();
                }

                Expression();

                switch (op)
                {
                    case Tokenizer.Token.TOKENIZER_LT:
                        {
                            Less();
                            break;
                        }
                    case Tokenizer.Token.TOKENIZER_GT:
                        {
                            Greater();
                            break;
                        }
                    case Tokenizer.Token.TOKENIZER_EQ:
                        {
                            Equal();
                            break;
                        }
                    case Tokenizer.Token.TOKENIZER_LTEQ:
                        {
                            LessEqual();
                            break;
                        }
                    case Tokenizer.Token.TOKENIZER_NOTEQ:
                        {
                            NotEqual();
                            break;
                        }
                    case Tokenizer.Token.TOKENIZER_GTEQ:
                        {
                            GreaterEqual();
                            break;
                        }
                }
                op = tokenizer.GetToken();
            }
            Debug("Relation");
        }

        #region functions

        //---------------------------------------------------------------}
        // SQRT Top of Stack with Primary

        private void SquareRoot()
        {
            object first;
            double number;

            if (stack.Count > 0)
            {
                first = stack.Pop();
                if (first.GetType() == typeof(string))
                {
                    // only expecting an integer or double
                    Expected("double");
                }
                else
                {
                    if ((double)first >= 0)
                    {
                        number = Math.Sqrt((double)first);
                        Debug("PopSqr: " + number);
                        stack.Push(number);
                    }
                    else
                    {
                        Expected("positive");
                    }
                }
            }
        }

        //---------------------------------------------------------------}
        // ABS Top of Stack with Primary

        private void Abs()
        {
            // This just removes the ecimal part with no rounding acording to the specification

            object first;
            double number;

            if (stack.Count > 0)
            {
                first = stack.Pop();
                if (first.GetType() == typeof(string))
                {
                    // only expecting an integer or double
                    Expected("double");
                }
                else
                {
                    number = Math.Abs((double)first);
                    Debug("Abs: " + number);
                    stack.Push(number);
                }
            }
        }

        //---------------------------------------------------------------}
        // INT Top of Stack with Primary

        private void Int()
        {
            // This just removes the decimal part with no rounding acording to the specification

            object first;
            double number;

            if (stack.Count > 0)
            {
                first = stack.Pop();
                if (first.GetType() == typeof(string))
                {
                    // only expecting an integer or double
                    Expected("double");
                }
                else
                {
                    number = Math.Truncate((double)first);
                    Debug("Int: " + number);
                    stack.Push(number);
                }
            }
        }

        //---------------------------------------------------------------}
        // RND Top of Stack with Primary

        private void Rnd()
        {
            object first;
            double number;

            if (stack.Count > 0)
            {
                first = stack.Pop();
                if (first.GetType() == typeof(string))
                {
                    // only expecting an integer or double
                    Expected("double");
                }
                else
                {
                    // the specification implies that the parameter has no value
                    // and that the same random sequence is always derived when the
                    // program starts
                    //Random r = new Random((int)Math.Truncate((double)first));
                    Random r = new Random(randomize);
                    randomize--;
                    number = r.NextDouble();
                    Debug("Rnd: " + number);
                    stack.Push(number);
                }
            }
        }

        //---------------------------------------------------------------}
        // SIN Top of Stack with Primary

        private void Sin()
        {
            object first;

            if (stack.Count > 0)
            {
                first = stack.Pop();
                if (first.GetType() == typeof(string))
                {
                    // only expecting an integer or double
                    Expected("double");
                }
                else
                {
                    stack.Push(Math.Sin((double)first));
                }
            }
        }

        //---------------------------------------------------------------}
        // COS Top of Stack with Primary

        private void Cos()
        {
            object first;

            if (stack.Count > 0)
            {
                first = stack.Pop();
                if (first.GetType() == typeof(string))
                {
                    // only expecting an integer or double
                    Expected("double");
                }
                else
                {
                    stack.Push(Math.Cos((double)first));
                }
            }

        }

        //---------------------------------------------------------------}
        // TAN Top of Stack with Primary

        private void Tan()
        {
            object first;

            if (stack.Count > 0)
            {
                first = stack.Pop();
                if (first.GetType() == typeof(string))
                {
                    // only expecting an integer or double
                    Expected("double");
                }
                else
                {
                    stack.Push(Math.Tan((double)first));
                }
            }
        }

        //---------------------------------------------------------------}
        // ATN Top of Stack with Primary

        private void Atn()
        {
            object first;

            if (stack.Count > 0)
            {
                first = stack.Pop();
                if (first.GetType() == typeof(string))
                {
                    // only expecting an integer or double
                    Expected("double");
                }
                else
                {
                    stack.Push(Math.Atan((double)first));
                }
            }
        }

        //---------------------------------------------------------------}
        // ATN Top of Stack with Primary

        private void Exp()
        {
            object first;

            if (stack.Count > 0)
            {
                first = stack.Pop();
                if (first.GetType() == typeof(string))
                {
                    // only expecting an integer or double
                    Expected("double");
                }
                else
                {
                    stack.Push(Math.Exp((double)first));
                }
            }
        }

        //---------------------------------------------------------------}
        // ATN Top of Stack with Primary

        private void Log()
        {
            object first;

            if (stack.Count > 0)
            {
                first = stack.Pop();
                if (first.GetType() == typeof(string))
                {
                    // only expecting an integer or double
                    Expected("double");
                }
                else
                {
                    stack.Push(Math.Log((double)first));
                }
            }
        }

        //---------------------------------------------------------------}
        // ASC Top of Stack with Primary

        private void Asc()
        {
            object first;

            if (stack.Count > 0)
            {
                first = stack.Pop();
                if (first.GetType() != typeof(string))
                {
                    // only expecting an integer or double
                    Expected("string");
                }
                else
                {
                    stack.Push((string)first);
                }
            }
        }

        //---------------------------------------------------------------}
        // LEN Top of Stack 

        private void Len()
        {
            object first;

            if (stack.Count > 0)
            {
                first = stack.Pop();
                if (first.GetType() != typeof(string))
                {
                    // only expecting an integer or double
                    Expected("string");
                }
                else
                {
                    stack.Push(first.ToString().Length);
                }
            }
        }

        //---------------------------------------------------------------}
        // LEFT$ Top of Stack
        // 1 - length -> first
        // 0 - string -> second

        private void Left()
        {
            object first;
            object second;
            string value;
            int length;

            if (stack.Count > 1)
            {
                first = stack.Pop();
                if (first.GetType() != typeof(string))
                {
                    if (stack.Count > 0)
                    {
                        second = stack.Pop();
                        if (second.GetType() == typeof(string))
                        {
                            length = (int)Math.Truncate(Convert.ToDouble(first));
                            value = first.ToString();
                            if (length < 1)
                            {
                                stack.Push("");
                            }
                            else if (length >= value.Length)
                            {
                                stack.Push(value);
                            }
                            else
                            {
                                stack.Push(value.Substring(0, length));
                            }
                        }
                        else
                        {
                            Expected("string");
                        }
                    }
                }
                else
                {
                    Expected("double");
                }
            }
        }

        //---------------------------------------------------------------}
        // RIGHT$ Top of Stack
        // 1 - length -> first
        // 0 - string -> second

        private void right()
        {
            object first;
            object second;
            string value;
            int length;

            if (stack.Count > 1)
            {
                first = stack.Pop();
                if (first.GetType() != typeof(string))
                {
                    if (stack.Count > 0)
                    {
                        second = stack.Pop();
                        if (second.GetType() == typeof(string))
                        {
                            length = (int)Math.Truncate(Convert.ToDouble(first));
                            value = first.ToString();
                            if (length < 1)
                            {
                                stack.Push("");
                            }
                            else if (length >= value.Length)
                            {
                                stack.Push(value);
                            }
                            else
                            {
                                stack.Push(value.Substring(value.Length-length, length));
                            }
                        }
                        else
                        {
                            Expected("string");
                        }
                    }
                }
                else
                {
                    Expected("double");
                }
            }
        }

        #endregion functions

        #region Relation        

        //---------------------------------------------------------------}
        // LESS THAN Top of Stack with Primary

        void Less()
        {
            object first;
            object second;
            int compare;
            Debug("Less");

            if (stack.Count > 1)
            {
                first = stack.Pop();
                if (first.GetType() == typeof(string))
                {
                    if (stack.Count > 0)
                    {
                        second = stack.Pop();
                        if (second.GetType() != typeof(string))
                        {
                            // only expecting an integer or double
                            Expected("string");
                        }
                        else
                        {
                            // -ve first < second, 0 first=second, +ve first > second
                            compare = string.Compare(first.ToString(), second.ToString());
                            if (compare > 1)
                            {
                                stack.Push(true);
                            }
                            else
                            {
                                stack.Push(false);
                            }
                        }
                    }
                }
                else
                {
                    if (stack.Count > 0)
                    {
                        second = stack.Pop();
                        if (second.GetType() == typeof(string))
                        {
                            // only expecting an integer or double
                            Expected("double");
                        }
                        else
                        {
                            bool truth = (double)first > (double)second;
                            Debug("Less: " + truth);
                            stack.Push(truth);
                        }
                    }
                }
            }
            Debug("Less");
        }

        //---------------------------------------------------------------}
        // LESS THAN OR EQUAL Top of Stack with Primary

        void LessEqual()
        {
            object first;
            object second;
            int compare;

            if (stack.Count > 1)
            {
                first = stack.Pop();
                if (first.GetType() == typeof(string))
                {
                    if (stack.Count > 0)
                    {
                        second = stack.Pop();
                        if (second.GetType() != typeof(string))
                        {
                            // only expecting an integer or double
                            Expected("string");
                        }
                        else
                        {
                            // -ve first < second, 0 first=second, +ve first > second
                            compare = string.Compare(first.ToString(), second.ToString());
                            if ((compare > 0) || (compare == 0))
                            {
                                stack.Push(true);
                            }
                            else
                            {
                                stack.Push(false);
                            }
                        }
                    }
                }
                else
                {
                    if (stack.Count > 0)
                    {
                        second = stack.Pop();
                        if (second.GetType() == typeof(string))
                        {
                            // only expecting an integer or double
                            Expected("double");
                        }
                        else
                        {
                            stack.Push((double)first >= (double)second);
                        }
                    }
                }
            }
        }

        //---------------------------------------------------------------}
        // GREATER THAN Top of Stack with Primary

        void Greater()
        {
            object first;
            object second;
            int compare;

            if (stack.Count > 1)
            {
                first = stack.Pop();
                if (first.GetType() == typeof(string))
                {
                    if (stack.Count > 0)
                    {
                        second = stack.Pop();
                        if (second.GetType() != typeof(string))
                        {
                            // only expecting an integer or double
                            Expected("string");
                        }
                        else
                        {
                            // -ve first < second, 0 first=second, +ve first > second
                            compare = string.Compare(first.ToString(), second.ToString());
                            if (compare < 0)
                            {
                                stack.Push(true);
                            }
                            else
                            {
                                stack.Push(false);
                            }
                        }
                    }
                }
                else
                {
                    if (stack.Count > 0)
                    {
                        second = stack.Pop();
                        if (second.GetType() == typeof(string))
                        {
                            // only expecting an integer or double
                            Expected("double");
                        }
                        else
                        {
                            stack.Push((double)first < (double)second);
                        }
                    }
                }
            }
        }

        //---------------------------------------------------------------}
        // GREATER THAN OR EQUAL Top of Stack with Primary

        void GreaterEqual()
        {
            object first;
            object second;
            int compare;

            if (stack.Count > 1)
            {
                first = stack.Pop();
                if (first.GetType() == typeof(string))
                {
                    if (stack.Count > 0)
                    {
                        second = stack.Pop();
                        if (second.GetType() != typeof(string))
                        {
                            // only expecting an integer or double
                            Expected("string");
                        }
                        else
                        {
                            // -ve first < second, 0 first=second, +ve first > second
                            compare = string.Compare(first.ToString(), second.ToString());
                            if ((compare < 0) || (compare == 0))
                            {
                                stack.Push(true);
                            }
                            else
                            {
                                stack.Push(false);
                            }
                        }
                    }
                }
                else
                {
                    if (stack.Count > 0)
                    {
                        second = stack.Pop();
                        if (second.GetType() == typeof(string))
                        {
                            // only expecting an integer or double
                            Expected("double");
                        }
                        else
                        {
                            stack.Push((double)first <= (double)second);
                        }
                    }
                }
            }
        }

        //---------------------------------------------------------------}
        // EQUAL Top of Stack with Primary

        void Equal()
        {
            object first;
            object second;
            Debug("Equal");
            if (stack.Count > 1)
            {
                first = stack.Pop();
                if (first.GetType() == typeof(string))
                {
                    if (stack.Count > 0)
                    {
                        second = stack.Pop();
                        if (second.GetType() != typeof(string))
                        {
                            // only expecting an integer or double
                            Expected("string");
                        }
                        else
                        {
                            stack.Push(string.Equals(first.ToString(), second.ToString()));
                        }
                    }
                }
                else
                {
                    if (stack.Count > 0)
                    {
                        second = stack.Pop();
                        if (second.GetType() == typeof(string))
                        {
                            // only expecting an integer or double
                            Expected("double");
                        }
                        else
                        {
                            bool truth = (double)first == (double)second;
                            Debug("Equal:" + truth);
                            stack.Push(truth);
                        }
                    }
                }
            }
        }

        //---------------------------------------------------------------}
        // NOT EQUAL Top of Stack with Primary

        void NotEqual()
        {
            object first;
            object second;
            Debug("NotEqual");

            if (stack.Count > 1)
            {
                first = stack.Pop();
                bool truth;
                if (first.GetType() == typeof(string))
                {
                    if (stack.Count > 0)
                    {
                        second = stack.Pop();
                        if (second.GetType() != typeof(string))
                        {
                            // only expecting an integer or double
                            Expected("string");
                        }
                        else
                        {
                            truth = string.Equals(first.ToString(), second.ToString());
                            Debug("NotEqual: " + truth);
                            stack.Push(truth);
                        }
                    }
                }
                else
                {
                    if (stack.Count > 0)
                    {
                        second = stack.Pop();
                        if (second.GetType() == typeof(string))
                        {
                            // only expecting an integer or double
                            Expected("double");
                        }
                        else
                        {
                            truth = (double)first != (double)second;
                            Debug("NotEqual: " + truth);
                            stack.Push(truth);
                        }
                    }
                }
            }
            Debug("NotEqual");
        }

        #endregion

        #region types

        //---------------------------------------------------------------}
        // BOOLEAN Top of Stack with Primary

        public Boolean PopBoolean()
        {
            object first;
            Boolean truth = false;

            if (stack.Count > 0)
            {
                first = stack.Pop();
                if ((first.GetType() == typeof(string)) || (first.GetType() == typeof(double)) || (first.GetType() == typeof(int)))
                {
                    // only expecting an integer or double
                    Expected("boolean");
                }
                else
                {
                    truth = (Boolean)first;
                }
            }
            Debug("PopBoolean: " + truth);
            return (truth);
        }

        //---------------------------------------------------------------}
        // DOUBLE Top of Stack with Primary

        public Double PopDouble()
        {
            object first;
            Double number = 0;

            if (stack.Count > 0)
            {
                first = stack.Pop();
                if ((first.GetType() == typeof(string)) || (first.GetType() == typeof(Boolean)))
                {
                    // only expecting an integer or double
                    Expected("Double");
                }
                else
                {
                    number = (Double)first;
                }
            }
            Debug("PopDouble: " + number);
            return (number);
        }

        //---------------------------------------------------------------}
        // INTEGER Top of Stack with Primary

        public int PopInteger()
        {
            object first;
            int integer = 0;

            if (stack.Count > 0)
            {
                first = stack.Pop();
                if ((first.GetType() == typeof(String)) || (first.GetType() == typeof(double)) || (first.GetType() == typeof(Boolean)))
                {
                    // only expecting an integer or double
                    Expected("integer");
                }
                else
                {
                    integer = (int)first;
                }
            }
            Debug("PopInteger: " + integer);
            return (integer);
        }

        //---------------------------------------------------------------}
        // STRING Top of Stack with Primary

        public String PopString()
        {
            object first;
            string value = "";

            if (stack.Count > 0)
            {
                first = stack.Pop();
                if ((first.GetType() == typeof(Boolean)) || (first.GetType() == typeof(double)) || (first.GetType() == typeof(int)))
                {
                    // only expecting an integer or double
                    Expected("string");
                }
                else
                {
                    value = (string)first;
                }
            }
            Debug("PopString: " + value);
            return (value);
        }

        #endregion types

        #region operators

        //---------------------------------------------------------------}
        // ADD Top of Stack with Primary

        void Add()
        {
            object first;
            object second;
            double number;

            Debug("Add");

            if (stack.Count > 1)
            {
                first = stack.Pop();
                if (first.GetType() == typeof(string))
                {
                    if (stack.Count > 1)
                    {
                        second = stack.Pop();
                        if (second.GetType() == typeof(string))
                        {
                            stack.Push((string)second + (string)first);
                        }
                        else
                        {
                            // only expecting a string
                            Expected("String");
                        }
                    }
                }
                else
                {
                    if (stack.Count > 0)
                    {
                        second = stack.Pop();
                        if (second.GetType() == typeof(string))
                        {
                            // only expecting an integer or double
                            Expected("double");
                        }
                        else
                        {
                            number = (double)second + (double)first;
                            Debug("PopAdd: " + second + "+" + first + "=" + number);
                            stack.Push(number);
                        }
                    }
                }
            }
            Debug("Add");
        }

        //---------------------------------------------------------------}
        // SUBTRACT Top of Stack with Primary

        void Subtract()
        {
            object first;
            object second;
            double number;

            if (stack.Count > 1)
            {
                first = stack.Pop();
                if (first.GetType() == typeof(string))
                {
                    // only expecting an int or double
                    Expected("double");
                }
                else
                {
                    if (stack.Count > 0)
                    {
                        second = stack.Pop();
                        if (second.GetType() == typeof(string))
                        {
                            // only expecting an int
                            Expected("double");
                        }
                        else
                        {
                            number = (double)second - (double)first;
                            Debug("PopSubtract: " + second + "-" + first + "=" + number);
                            stack.Push(number);
                        }
                    }
                }
            }
        }

        //---------------------------------------------------------------}
        // MULTIPLY Top of Stack with Primary

        void Multiply()
        {
            object first;
            object second;
            double numeric;

            if (stack.Count > 1)
            {
                first = stack.Pop();
                if (first.GetType() == typeof(string))
                {
                    // only expecting an integer or double
                    Expected("double");
                }
                else
                {
                    if (stack.Count > 0)
                    {
                        second = stack.Pop();
                        if (second.GetType() == typeof(string))
                        {
                            // only expecting an integer or double
                            Expected("double");
                        }
                        else
                        {
                            numeric = (double)second * (double)first;
                            Debug("PopMultiply: " + second + "*" + first + "=" + numeric);
                            stack.Push(numeric);
                        }
                    }
                }
            }
        }

        //---------------------------------------------------------------}
        // DIVIDE Top of Stack with Primary

        void Divide()
        {
            object first;
            object second;
            double number;

            if (stack.Count > 1)
            {
                first = stack.Pop();
                if (first.GetType() == typeof(string))
                {
                    // only expecting an integer or double
                    Expected("double");
                }
                else
                {
                    if (stack.Count > 0)
                    {
                        second = stack.Pop();
                        if (second.GetType() == typeof(string))
                        {
                            // only expecting an integer or double
                            Expected("double");
                        }
                        else
                        {
                            number = (double)second / (double)first;
                            Debug("PopDivide: " + second + "/" + first + "=" + number);
                            stack.Push(number);
                        }
                    }
                }
            }
        }

        //---------------------------------------------------------------} 
        // AND Top of Stack with Primary

        void And()
        {
            object first;
            object second;

            if (stack.Count > 1)
            {
                first = stack.Pop();
                if (first.GetType() != typeof(Boolean))
                {
                    // only expecting an integer or double
                    Expected("boolean");
                }
                else
                {
                    if (stack.Count > 0)
                    {
                        second = stack.Pop();
                        if (second.GetType() != typeof(Boolean))
                        {
                            // only expecting an integer or double
                            Expected("boolean");
                        }
                        else
                        {
                            stack.Push((Boolean)first && (Boolean)second);
                        }
                    }
                }
            }
        }

        //---------------------------------------------------------------} 
        // AND Top of Stack with Primary

        void Or()
        {
            object first;
            object second;

            if (stack.Count > 1)
            {
                first = stack.Pop();
                if (first.GetType() != typeof(Boolean))
                {
                    // only expecting an integer or double
                    Expected("boolean");
                }
                else
                {
                    if (stack.Count > 0)
                    {
                        second = stack.Pop();
                        if (second.GetType() != typeof(Boolean))
                        {
                            // only expecting an integer or double
                            Expected("boolean");
                        }
                        else
                        {
                            stack.Push((Boolean)first || (Boolean)second);
                        }
                    }
                }
            }
        }

        //---------------------------------------------------------------}
        // POWER Top of Stack with Primary

        private void Power()
        {
            object first;
            object second;
            double number;

            if (stack.Count > 1)
            {
                first = stack.Pop();
                if (first.GetType() == typeof(string))
                {
                    // only expecting an integer or double
                    Expected("double");
                }
                else
                {
                    if (stack.Count > 0)
                    {
                        second = stack.Pop();
                        if (second.GetType() == typeof(string))
                        {
                            // only expecting an integer or double
                            Expected("double");
                        }
                        else
                        {
                            number = Math.Pow((double)second, (double)first);
                            Debug("PopPower: " + number);
                            stack.Push(number);
                        }
                    }
                }
            }
        }

        #endregion operators

        public int GetIntVariable(int varnum)
        {
            if (varnum >= 0 && varnum <= MAX_VARNUM)
            {
                return variables[varnum];
            }
            else
            {
                return 0;
            }
        }

        public string GetStringVariable(string varName)
        {
            // Not sure what happens if the variable doesnt exit
            // think this should error but wonder what the specification says

            if (stringVariables.ContainsKey(varName))
            {
                Debug("get string variable:" + (string)stringVariables[varName]);
                return ((string)stringVariables[varName]);
            }
            else
            {
                return ("");
            }
        }

        public double GetNumericVariable(string varName)
        {
            if (numericVariables.ContainsKey(varName))
            {
                Debug("get numeric variable:" + (double)numericVariables[varName]);
                return ((double)numericVariables[varName]);
            }
            else
            {
                return (0);
            }
        }

        public double GetNumericArrayVariable(string varName, int positions, int[] position)
        {
            uBasicLibrary.Array data;

            if (numericArrayVariables.ContainsKey(varName))
            {
                data = (uBasicLibrary.Array)numericArrayVariables[varName];
                return ((double)data.Get(position));
            }
            else
            {
                return (0);
            }
        }

        public string GetStringArrayVariable(string varName, int positions, int[] position)
        {
            uBasicLibrary.Array data;

            if (stringArrayVariables.ContainsKey(varName))
            {
                data = (uBasicLibrary.Array)stringArrayVariables[varName];
                return ((string)data.Get(position));
            }
            else
            {
                return ("");
            }
        }

        public void DeclareNumericArrayVariable(string varName, int dimensions, int[] dimension)
        {
            uBasicLibrary.Array data;
            if (numericArrayVariables.ContainsKey(varName))
            {
                Expected("Array already defined " + varName + "(");
            }
            data = new uBasicLibrary.Array(varName, dimensions, dimension,(double)0);
            numericArrayVariables.Add(varName, data);
        }

        public void DeclareStringArrayVariable(string varName, int dimensions, int[] dimension)
        {
            uBasicLibrary.Array data;
            if (stringArrayVariables.ContainsKey(varName))
            {
                Expected("Array already defined " + varName + "(");
            }
            data = new uBasicLibrary.Array(varName, dimensions, dimension, (string)"");
            stringArrayVariables.Add(varName, data);
        }

        public void SetIntVariable(int varnum, int integer)
        {
            if (varnum >= 0 && varnum <= MAX_VARNUM)
            {
                variables[varnum] = integer;
            }
        }

        public void SetStringVariable(string varName, string value)
        {
            if (stringVariables.ContainsKey(varName))
            {
                stringVariables.Remove(varName);
            }
            stringVariables.Add(varName, value);
            Debug("varName=" + varName + " value=" + value);
        }

        public void SetNumericVariable(string varName, double number)
        {
            if (numericVariables.ContainsKey(varName))
            {
                numericVariables.Remove(varName);
            }
            numericVariables.Add(varName, number);
            Debug("varName=" + varName + " number=" + number);
        }

        public void SetNumericArrayVariable(string varName, int positions, int[] position, double number)
        {

            uBasicLibrary.Array data;
            if (!numericArrayVariables.ContainsKey(varName))
            {
                // it apperas that if no DIM then defaults to 10 items
                int[] dimension = new int[10];
                dimension[0] = 1;
                DeclareNumericArrayVariable(varName, positions, dimension);
            }
            data = (uBasicLibrary.Array)numericArrayVariables[varName];
            data.Set(position, number);
        
            Debug("varName=" + varName + " number=" + number);
        }

        public void SetStringArrayVariable(string varName, int positions, int[] position, string value)
        {

            uBasicLibrary.Array data;
            if (!stringArrayVariables.ContainsKey(varName))
            {
                // it apperas that if no DIM then defaults to 10 items
                int[] dimension = new int[10];
                dimension[0] = 1;
                DeclareStringArrayVariable(varName, positions, dimension);
            }
            data = (uBasicLibrary.Array)stringArrayVariables[varName];
            data.Set(position, value);

            Debug("varName=" + varName + " value=" + value);
        }

        #endregion
        #region Private

        //--------------------------------------------------------------
        // Debug

        void Debug(string s)
        {
            if (log.IsDebugEnabled == true) { log.Debug(s); }
        }

        //--------------------------------------------------------------
        // Info

        void Info(string s)
        {
            if (log.IsInfoEnabled == true) { log.Info(s); }
        }


        //--------------------------------------------------------------
        // Report What Was Expected 

        public void Expected(string s)
        {
            throw new System.ArgumentException("Unexpected", s + " expected");
        }

        #endregion
    }
}
