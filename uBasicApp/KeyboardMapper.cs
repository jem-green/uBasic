using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace uBasicApp
{

    /// <summary>
    /// Mapping the PreviewKey key events to the ASCII characters
    /// </summary>
    public class KeyboardMapper
    {
        #region Fields

        Dictionary<int, Key> _keys;

        public struct Key
        {
            int _keyValue;
            byte _normal;
            byte _shift;

            public Key(int keyValue, byte normal ,byte shift)
            {
                _keyValue = keyValue;
                _normal = normal;
                _shift = shift;
            }

            public int KeyValue
            {
                get
                {
                    return (_keyValue);
                }
            }

            public byte Normal
            {
                get
                {
                    return (_normal);
                }
            }

            public byte Shift
            {
                get
                {
                    return (_shift);
                }
            }
        }

        #endregion
        #region Constructors
        public KeyboardMapper()
        {
            _keys = new Dictionary<int, Key>();

            // General

            _keys.Add(13, new Key(13, 13, 0));    // CR
            _keys.Add(27, new Key(27, 27, 0));    // Esc
            _keys.Add(10, new Key(10, 10, 0));    // LF
            _keys.Add(9, new Key(9, 9, 0));       // Tab
            _keys.Add(8, new Key(8, 8, 0));       // Backspace

            // Top Row

            _keys.Add(223, new Key(96, 96, 0));     // ` and ¬
            _keys.Add(49, new Key(49, 49, 33));     // 1 and !
            _keys.Add(50, new Key(50, 50, 34));     // 2 and "
            _keys.Add(51, new Key(51, 51, 35));     // 3 and £ - not ASCII
            _keys.Add(52, new Key(52, 52, 36));     // 4 and $
            _keys.Add(53, new Key(53, 53, 37));     // 5 and $
            _keys.Add(54, new Key(54, 54, 94));     // 6 and ^
            _keys.Add(55, new Key(55, 55, 38));     // 7 and &
            _keys.Add(56, new Key(56, 56, 43));     // 8 and *
            _keys.Add(57, new Key(57, 57, 40));     // 9 and (
            _keys.Add(48, new Key(48, 48, 41));     // 0 and )
            _keys.Add(189, new Key(45, 45, 95));    // - and _
            _keys.Add(187, new Key(61, 61, 43));    // = and +

            // Second Row

            _keys.Add(81, new Key(81, 113, 81));    // Q and q
            _keys.Add(87, new Key(87, 119, 87));    // W and w
            _keys.Add(69, new Key(69, 101, 69));    // E and e
            _keys.Add(82, new Key(82, 114, 82));    // R and r
            _keys.Add(84, new Key(84, 116, 84));    // T and t
            _keys.Add(89, new Key(89, 121, 89));    // Y and y
            _keys.Add(85, new Key(85, 117, 85));    // U and u
            _keys.Add(73, new Key(73, 105, 73));    // I and i
            _keys.Add(79, new Key(79, 111, 79));    // O and o
            _keys.Add(80, new Key(80, 112, 80));    // P and p
            _keys.Add(219, new Key(91, 91, 123));   // [ and {
            _keys.Add(221, new Key(93, 93, 125));   // ] and }

            // Third Row

            _keys.Add(65, new Key(65, 97, 65));     // A and a
            _keys.Add(83, new Key(83, 115, 83));    // S and s
            _keys.Add(68, new Key(68, 100, 68));    // D and d
            _keys.Add(70, new Key(70, 102, 70));    // F and f
            _keys.Add(71, new Key(71, 103, 71));    // G and g
            _keys.Add(72, new Key(72, 104, 72));    // H and h
            _keys.Add(74, new Key(74, 106, 74));    // J and j
            _keys.Add(75, new Key(75, 107, 75));    // K and k
            _keys.Add(76, new Key(76, 108, 76));    // L and l
            _keys.Add(186, new Key(59, 59, 58));    // ; and :
            _keys.Add(192, new Key(39, 39, 64));    // ' and @
            _keys.Add(222, new Key(35, 35, 126));   // # and ~

            // Fourth Row

            _keys.Add(220, new Key(92, 92, 124));   // \ and |
            _keys.Add(90, new Key(90, 122, 90));    // Z and z
            _keys.Add(88, new Key(88, 120, 88));    // X and x
            _keys.Add(67, new Key(67, 99, 67));     // C and c
            _keys.Add(86, new Key(86, 118, 86));    // V and v
            _keys.Add(66, new Key(66, 98, 66));     // B and b
            _keys.Add(78, new Key(78, 110, 78));    // N and n
            _keys.Add(77, new Key(77, 109, 77));    // M and m
            _keys.Add(188, new Key(44, 44, 60));    // , and <
            _keys.Add(190, new Key(46, 46, 62));    // . and >
            _keys.Add(191, new Key(47, 47, 63));    // / and ?

            // Last row

            _keys.Add(32, new Key(32, 32, 32)); // space

        }
        #endregion
        #region Methods
        public byte ToASCII(int key, bool shift, bool ctrl, bool alt)
        {
            byte ascii = 0;
            try
            {
                if (shift == false)
                {
                    ascii = _keys[key].Normal;
                }
                else
                {
                    ascii = _keys[key].Shift;
                }
            }
            catch
            {
                ascii = 0;
            }
            return (ascii);
        }
        #endregion
    }
}
