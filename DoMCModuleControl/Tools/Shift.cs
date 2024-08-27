#pragma warning disable IDE0090
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoMCModuleControl.Tools
{
    public class Shift
    {
        public DateTime ShiftDate { get; protected set; }
        public bool IsNight { get; protected set; }

        public readonly static TimeSpan _8 = new TimeSpan(8, 0, 0);
        public readonly static TimeSpan _20 = new TimeSpan(20, 0, 0);
        public readonly static TimeSpan _32 = new TimeSpan(32, 0, 0);
        public Shift(DateTime dt)
        {
            ShiftDate = GetShiftDate(dt);
            IsNight = GetIsNightShift(dt);
        }
        public Shift()
        {
            var dt = DateTime.Now;
            ShiftDate = GetShiftDate(dt);
            IsNight = GetIsNightShift(dt);
        }
        public Shift(DateTime shiftDate, bool isNightShift)
        {
            ShiftDate = shiftDate.Date;
            IsNight = isNightShift;
        }

        public DateTime GetShiftStartDateTime()
        {
            return ShiftDate.Add(IsNight ? _20 : _8);
        }

        protected static DateTime GetShiftDate(DateTime dt)
        {
            if (GetIsNightShift(dt) && dt.TimeOfDay < _8)
            {
                return dt.Date.AddDays(-1);
            }
            else
            {
                return dt.Date;

            }

        }
        protected static bool GetIsNightShift(DateTime dt)
        {
            if (dt.TimeOfDay >= _8 && dt.TimeOfDay < _20) return false;
            return true;
        }
        public static DateTime CurrentShiftDate
        {
            get
            {
                var now = DateTime.Now;
                if (CurrentIsNightShift && now.TimeOfDay < _8)
                {
                    return now.Date.AddDays(-1);
                }
                else
                {
                    return now.Date;

                }
            }
        }
        public static bool CurrentIsNightShift
        {
            get
            {
                var now = DateTime.Now;
                if (now.TimeOfDay >= _8 && now.TimeOfDay < _20) return false;
                return true;
            }
        }
                
        public static int GetShiftNumber(DateTime StartShift, bool IsNightShift)
        {
            int ShiftNumber;
            var days = (StartShift - DateTime.MinValue).TotalDays;
            if (IsNightShift)
            {
                ShiftNumber = System.Convert.ToInt32(Math.Round(((days + 1) % 4 + 1)));
            }
            else
            {
                ShiftNumber = System.Convert.ToInt32(Math.Round(((days + 2) % 4 + 1)));
            }
            return ShiftNumber;
        }
        public Shift PreviousShift()
        {
            if (IsNight)
            {
                return new Shift(ShiftDate, false);
            }
            else
            {
                return new Shift(ShiftDate.AddDays(-1), true);
            }
        }
        public Shift NextShift()
        {
            if (IsNight)
            {
                return new Shift(ShiftDate.AddDays(1), false);
            }
            else
            {
                return new Shift(ShiftDate, true);
            }
        }
        public DateTime ShiftStartsAt()
        {
            if (!IsNight) return ShiftDate.Date.Add(_8); else return ShiftDate.Date.Add(_20);
        }
        public DateTime ShiftEndsAt()
        {
            if (IsNight) return ShiftDate.Date.Add(_32); else return ShiftDate.Date.Add(_20);
        }

        public override bool Equals(object? o)
        {
            if (o is null) return false;
            if (o is not Shift s) return false;
            return s.ShiftDate == ShiftDate && s.IsNight == IsNight;
        }
        public static bool operator ==(Shift s1, Shift s2)
        {
            return s1.ShiftDate == s2.ShiftDate && s1.IsNight == s2.IsNight;
        }
        public static bool operator !=(Shift s1, Shift s2)
        {
            return s1.ShiftDate != s2.ShiftDate || s1.IsNight != s2.IsNight;
        }
        public static bool operator >(Shift s1, Shift s2)
        {
            return s1.ShiftStartsAt() > s2.ShiftStartsAt();
        }
        public static bool operator <(Shift s1, Shift s2)
        {
            return s1.ShiftStartsAt() < s2.ShiftStartsAt();
        }
        public static bool operator >=(Shift s1, Shift s2)
        {
            return s1.ShiftStartsAt() > s2.ShiftStartsAt() || s1.ShiftStartsAt() == s2.ShiftStartsAt();
        }
        public static bool operator <=(Shift s1, Shift s2)
        {
            return s1.ShiftStartsAt() < s2.ShiftStartsAt() || s1.ShiftStartsAt() == s2.ShiftStartsAt();
        }



        public override int GetHashCode()
        {
            return HashCode.Combine(ShiftDate, IsNight);
        }
    }

}
