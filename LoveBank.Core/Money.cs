using System;
using System.Globalization;

namespace LoveBank.Core {
    public class Money {
        private readonly decimal _money;

        public Money(decimal money) {
            _money = money;
        }
        public static implicit operator decimal (Money x) {
            return x._money;
        }
        public static implicit operator Money(decimal x) {
            return new Money(x);
        }
        public static Money operator *(Money x,Money y) {
            return x._money*y._money;
        }
        public static Money operator / (Money x,Money y) {
            return x._money/y._money;
        }

        public static Money  operator +(Money x, Money y) {
            return x._money + y._money;
        }
        public static Money operator  -(Money x,Money y) {
            return x._money-y._money;
        }

        public Money Add(Money x) {
            return this + x;
        }
        /// <summary>
        /// IEEE 规定的舍入标准进行小数位截取
        /// </summary>
        /// <param name="decimals"></param>
        /// <returns></returns>
        public Money Round(int decimals) {
            return Math.Round(_money, decimals);
        }
        /// <summary>
        /// 将Money对象转换成代货币符号并保留两位有效数字的字串
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            return ToString("C2");
        }

        public string ToString(string format) {
            return (Math.Truncate(this * 100) / 100).ToString(format, CultureInfo.CurrentUICulture);
        }
    }

    public static class MoneyExtension {
        public static Money ToMoney(this decimal value) {
            return value;
        }

        public static string ToMoney(this decimal value,string format) {
            return new Money(value).ToString(format);
        }
    }
}