using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using TracerLibrary;

namespace uBasicLibrary
{
    public class uBasic : IInterpreter
    {

        [DllImport("ubasic.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ubasic_run();

        [DllImport("ubasic.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ubasic_init(byte[] program);

        [DllImport("ubasic.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int ubasic_finished();

        // Check returning data types from methods

        //[DllImport("interface.dll", CallingConvention = CallingConvention.Cdecl)]
        //public static extern int getPc();

        //[DllImport("interface.dll", CallingConvention = CallingConvention.Cdecl)]
        //public static extern IntPtr outChar();

        //// Checking passing data types into methods

        //[DllImport("interface.dll", CallingConvention = CallingConvention.Cdecl)]
        //public static extern void inChar([MarshalAs(UnmanagedType.LPStr)] string msg);

        //// Callback function to receive messages from C

        // Define the delegate type

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void CallbackDelegate(string valueStr);

        // Import the C functions

        [DllImport("ubasic.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void ubasic_callback(IntPtr cb);

        //[DllImport("ubasic.dll", CallingConvention = CallingConvention.Cdecl)]
        //public static extern void trigger_callback(string value);

        // Store the delegate as an instance field to prevent it from being garbage collected
        private CallbackDelegate _callerDelegate;

        IDefaultIO _defaultIO = null;
        bool _ended = false;
        readonly byte[] _program;

        public uBasic(byte[] program, IDefaultIO consoleIO)
        {
            _defaultIO = consoleIO;
            _program = program;

            // Assign the delegate to the instance field
            _callerDelegate = new CallbackDelegate(MyCallback);
            IntPtr ptr = Marshal.GetFunctionPointerForDelegate(_callerDelegate);
            ubasic_callback(ptr);

        }

        public void Init(int position)
        {
            Debug.WriteLine("In Init()");
            ubasic_init(_program);
            Debug.WriteLine("Out Init()");
        }

        public void Run()
        {
            Debug.WriteLine("In Run()");
            ubasic_run();
            Debug.WriteLine("Out Run()");
        }

        public void Stop()
        {
            Debug.WriteLine("In Stop()");
            _ended = true;
            Debug.WriteLine("Out Stop()");
        }

        /// <summary>
        /// Finished()
        /// </summary>
        /// <returns></returns>
        public bool IsFinished()
        {
            Debug.WriteLine("In Finished()");
            bool finished = false;
            if ((ubasic_finished() != 0) || _ended)
            {
                TraceInternal.TraceVerbose("Out Finished() - true");
                finished = true;
            }
            Debug.WriteLine("Out Finished()");
            return (finished);
        }

        /// <summary>
        /// Raise exception
        /// </summary>
        /// <param name="text"></param>
        public void Abort(string text)
        {
            Debug.WriteLine("In Abort()");
            string message = text + " @ "; // + currentLineNumber;
            throw new Exception(message);
        }

        private void MyCallback(string valueStr)
        {
            _defaultIO.Out(valueStr);
        }
    }
}
