// LICENSE:
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
// AUTHORS:
//
//  Moritz Eberl <moritz@semiodesk.com>
//  Sebastian Faubel <sebastian@semiodesk.com>
//
// Copyright (c) Semiodesk GmbH 2015

using IniParser.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Semiodesk.VirtuosoInstrumentation.Configuration
{
    public class IniSectionWrapper
    {
        #region Members
        SectionData _sectionData;
        public bool Locked { get; internal set; }
        public SectionData SectionData { get { return _sectionData; } }
        #endregion

        #region Constructor
        public IniSectionWrapper(SectionData sectionData)
        {
            _sectionData = sectionData;
        }
        #endregion

        #region Methods

        protected void HandleLockedState(string entry)
        {
            throw new Exception(string.Format("Tried to manipulate entry {0} in virtuoso configuration but it is locked.", entry));
        }

        protected string GetStringData(string key)
        {
            KeyData d = _sectionData.Keys.Where(x => x.KeyName == key).FirstOrDefault();
            if (d != null)
                return d.Value;
            return null;
        }

        protected List<string> GetStringListData(string key)
        {
            KeyData d = _sectionData.Keys.Where(x => x.KeyName == key).FirstOrDefault();
            if (d != null)
            {
                return d.Value.Split(',').ToList();
            }
            return null;
        }

        protected bool? GetBoolData(string key)
        {
            KeyData d = _sectionData.Keys.Where(x => x.KeyName == key).FirstOrDefault();

            if (d != null)
            {
                int val;
                if (int.TryParse(d.Value, out val))
                {
                    if (val == 1)
                        return true;
                    if (val == 0)
                        return false;
                }
            }
            return null;
        }

        protected int? GetIntData(string key)
        {
            KeyData d = _sectionData.Keys.Where(x => x.KeyName == key).FirstOrDefault();

            if (d != null)
            {
                int val;
                if (int.TryParse(d.Value, out val))
                    return val;
            }
            return null;
        }

        protected float? GetFloatData(string key)
        {
            KeyData d = _sectionData.Keys.Where(x => x.KeyName == key).FirstOrDefault();

            if (d != null)
            {
                float val;
                if (float.TryParse(d.Value, out val))
                    return val;
            }
            return null;
        }

        protected TimeSpan? GetTimespanData(string key)
        { 
            KeyData d = _sectionData.Keys.Where(x => x.KeyName == key).FirstOrDefault();

            if (d != null)
            {
                int val;
                if (int.TryParse(d.Value, out val))
                {
                    return new TimeSpan(0, val, 0);
                }
            }
            return null;
        }

        protected bool ContainsKey(string key)
        {
            return _sectionData.Keys.Where(x => x.KeyName == key).Any();
        }

        protected void SetStringData(string key, string value)
        {
            if (Locked)
            {
                HandleLockedState(key);
            }

            if (ContainsKey(key))
            {
                KeyData d = new KeyData(key);
                d.Value = value;
                _sectionData.Keys.SetKeyData(d);
            }
            else
            {
                _sectionData.Keys.AddKey(key, value);
            }
        }

        protected void SetStringListData(string key, IEnumerable<string> value)
        {
            if (Locked)
            {
                HandleLockedState(key);
            }

            if (value != null)
            {
                SetStringData(key, string.Join(",", value.ToArray()));
            }
            else
            {
                _sectionData.Keys.RemoveKey(key);
            }
        }

        protected void SetBoolData(string key, bool? value)
        {
            if (Locked)
            {
                HandleLockedState(key);
            }

            if (value.HasValue)
            {
                if (value.Value)
                    SetStringData(key, "1");
                else
                    SetStringData(key, "0");
            }
            else
            {
                _sectionData.Keys.RemoveKey(key);
               
            }
        }

        protected void SetIntData(string key, int? value)
        {
            if (Locked)
            {
                HandleLockedState(key);
            }
             
            if (value.HasValue)
            {
                SetStringData(key, value.ToString());
            }
            else
            {
                _sectionData.Keys.RemoveKey(key);
            }
        }

        protected void SetFloatData(string key, float? value)
        {
            if (Locked)
            {
                HandleLockedState(key);
            }

            if (value.HasValue)
            {
                SetStringData(key, value.ToString());
            }
            else
            {
                _sectionData.Keys.RemoveKey(key);
            }
        }

        protected void SetTimespanData(string key, TimeSpan? value)
        {
            if (Locked)
            {
                HandleLockedState(key);
            }

            if (value.HasValue)
            {
                SetStringData(key, value.Value.TotalMinutes.ToString());
            }
            else
            {
                _sectionData.Keys.RemoveKey(key);
            }
        }
        #endregion

    }

}
