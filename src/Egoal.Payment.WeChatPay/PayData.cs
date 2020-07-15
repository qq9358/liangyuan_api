using Egoal.Extensions;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Egoal.Payment.WeChatPay
{
    /// <summary>
    /// 微信支付协议接口数据类，所有的API接口通信都依赖这个数据结构，
    /// 在调用接口之前先填充各个字段的值，然后进行接口通信，
    /// 这样设计的好处是可扩展性强，用户可随意对协议进行更改而不用重新设计数据结构，
    /// 还可以随意组合出不同的协议数据包，不用为每个协议设计一个数据包结构
    /// </summary>
    public class PayData
    {
        //采用排序的Dictionary的好处是方便对数据包进行签名，不用再签名之前再做一次排序
        private SortedDictionary<string, string> values;

        public PayData()
        {
            values = new SortedDictionary<string, string>();
        }

        /// <summary>
        /// 将Dictionary转成xml
        /// </summary>
        /// <returns>经转换得到的xml串</returns>
        public string ToXml()
        {
            if (values.Count == 0)
            {
                throw new ApiException("WxPayData数据为空");
            }

            StringBuilder xml = new StringBuilder("<xml>");
            foreach (var pair in values)
            {
                if (pair.Value == null)
                {
                    throw new ApiException($"WxPayData内部{pair.Key}值为null");
                }

                xml.Append("<").Append(pair.Key).Append(">").Append("<![CDATA[").Append(pair.Value).Append("]]></").Append(pair.Key).Append(">");
            }
            xml.Append("</xml>");

            return xml.ToString();
        }

        /// <summary>
        /// 将xml转为WxPayData对象并返回对象内部的数据
        /// </summary>
        /// <param name="xml">待转换的xml串</param>
        /// <param name="key"></param>
        /// <returns>经转换得到的Dictionary</returns>
        public SortedDictionary<string, string> FromXml(string xml, string key)
        {
            if (string.IsNullOrEmpty(xml))
            {
                throw new ApiException("将空的xml串转换为WxPayData不合法!");
            }

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.XmlResolver = null;
            xmlDoc.LoadXml(xml);
            XmlNode xmlNode = xmlDoc.FirstChild;
            XmlNodeList nodes = xmlNode.ChildNodes;
            foreach (XmlNode xn in nodes)
            {
                XmlElement xe = xn as XmlElement;
                values[xe.Name] = xe.InnerText;
            }

            try
            {
                if (values["return_code"]?.ToUpper() != "SUCCESS")
                {
                    return values;
                }

                CheckSign(key);
            }
            catch (ApiException ex)
            {
                throw new ApiException(ex.Message, xml);
            }

            return values;
        }

        /// <summary>
        /// 检查签名
        /// </summary>
        /// <param name="key"></param>
        /// <returns>签名正确返回true，签名错误抛出异常</returns>
        public bool CheckSign(string key)
        {
            if (!IsSet("sign"))
            {
                throw new ApiException("WxPayData签名不存在");
            }
            else if (GetValue("sign").IsNullOrEmpty())
            {
                throw new ApiException("WxPayData签名存在但不合法");
            }

            string return_sign = GetValue("sign");

            string cal_sign = MakeSign(key);

            if (cal_sign == return_sign)
            {
                return true;
            }

            throw new ApiException("WxPayData签名验证错误");
        }

        /// <summary>
        /// 生成签名，详见签名生成算法
        /// sign字段不参加签名
        /// </summary>
        /// <param name="key"></param>
        /// <returns>签名</returns>
        public string MakeSign(string key)
        {
            StringBuilder str = new StringBuilder(ToUrl());
            str.Append("&key=").Append(key);

            var md5 = MD5.Create();
            var bs = md5.ComputeHash(Encoding.UTF8.GetBytes(str.ToString()));
            var sb = new StringBuilder();
            foreach (byte b in bs)
            {
                sb.Append(b.ToString("x2"));
            }

            return sb.ToString().ToUpper();
        }

        /// <summary>
        /// Dictionary格式转化成url参数格式
        /// </summary>
        /// <returns>url格式串, 该串不包含sign字段值</returns>
        public string ToUrl()
        {
            StringBuilder buff = new StringBuilder();
            foreach (var pair in values)
            {
                if (pair.Value == null)
                {
                    throw new ApiException($"WxPayData内部{pair.Key}值为null");
                }

                if (pair.Key.ToLower() != "sign" && !pair.Value.IsNullOrEmpty())
                {
                    buff.Append(pair.Key).Append("=").Append(pair.Value).Append("&");
                }
            }

            return buff.ToString().Trim('&');
        }

        /// <summary>
        /// 判断某个字段是否已设置
        /// </summary>
        /// <param name="key">字段名</param>
        /// <returns>若字段key已被设置，则返回true，否则返回false</returns>
        public bool IsSet(string key)
        {
            return values.TryGetValue(key, out string v);
        }

        /// <summary>
        /// 设置某个字段的值
        /// </summary>
        /// <param name="key">字段名</param>
        /// <param name="value">字段值</param>
        public void SetValue(string key, string value)
        {
            values[key] = value;
        }

        /// <summary>
        /// 根据字段名获取某个字段的值
        /// </summary>
        /// <param name="key">字段名</param>
        /// <returns>key对应的字段值</returns>
        public string GetValue(string key)
        {
            values.TryGetValue(key, out string value);
            return value;
        }

        public string ToJson()
        {
            return values.ToJson();
        }
    }
}
