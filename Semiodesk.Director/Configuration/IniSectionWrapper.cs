using IniParser.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Semiodesk.Director.Configuration
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
                return;

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
                return;

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
                return;

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
                return;
             
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
                return;

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
                return;

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
