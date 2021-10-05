﻿//  Copyright (c) 2017, Jeremy Green All rights reserved.

using System;
using System.Collections.Generic;
using System.Text;

namespace uBasicLibrary
{
    public class Parameter
    {
        #region Fields

        object _value = "";
        SourceType _source = SourceType.None;
        public enum SourceType
        {
            None = 0,
            Command = 1,
            Registry = 2,
            App = 3
        }

        #endregion
        #region Constructor
        public Parameter()
        {
        }
        public Parameter(object value)
        {
            this._value = value;
            _source = SourceType.App;
        }
        public Parameter(object value, SourceType source)
        {
            _value = value;
            this._source = source;
        }
        #endregion
        #region Parameters
        public object Value
        {
            set
            {
                this._value = value;
            }
            get
            {
                return (_value);
            }
        }

        public SourceType Source
        {
            set
            {
                _source = value;
            }
            get
            {
                return (_source);
            }
        }
        #endregion
        #region Methods
        public override string ToString()
        {
            return (Convert.ToString(_value));
        }
        #endregion
    }
}
