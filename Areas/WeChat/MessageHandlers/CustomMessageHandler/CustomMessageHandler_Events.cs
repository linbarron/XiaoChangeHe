using System;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Senparc.Weixin.MP.Agent;
using Senparc.Weixin.Context;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Helpers;
using Senparc.Weixin.MP.MessageHandlers;
using WitBird.XiaoChangHe.Areas.WeChat.Utilities;
using WitBird.XiaoChangHe.Models;

namespace WitBird.XiaoChangHe.Areas.WeChat.MessageHandlers.CustomMessageHandler
{
    /// <summary>
    /// 自定义MessageHandler
    /// </summary>
    public partial class CustomMessageHandler
    {
        private Article GetWelcomeInfo()
        {
            return new Article()
            {
                Title = "成都映象小场合以经营地道川菜、干锅、小吃为主要特色。",
                Description = "小场合以经营地道川菜、干锅、小吃为主要特色，在这里您可以找到儿时的回忆，在这里您可以找到妈妈饭菜的味道！",
                PicUrl = "http://115.29.38.163/Images/xchlogo.jpg",
                Url ="http://mp.weixin.qq.com/s?__biz=MjM5MjIzMDIzMQ==&mid=200696040&idx=1&sn=13ba2109e4bf2ff3ee019ee086c3ce47#rd"
            };
        }

        public override IResponseMessageBase OnEvent_ClickRequest(RequestMessageEvent_Click requestMessage)
        {
            IResponseMessageBase reponseMessage = null;
            switch (requestMessage.EventKey)
            {
                case "BeginOrderClick":
                    {
                        var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();
                        reponseMessage = strongResponseMessage;
                        strongResponseMessage.Content = "开始点菜";
                    }
                    break;
                case "ActivityClick":
                    {
                        var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();
                        reponseMessage = strongResponseMessage;
                        strongResponseMessage.Content = "小场合活动";
                    }
                    break;
                case "AccountInfoClick":
                    {
                        var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();
                        reponseMessage = strongResponseMessage;
                        strongResponseMessage.Content = "会员信息";
                    }
                    break;
                case "VipPay":
                    {
                        var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();
                        reponseMessage = strongResponseMessage;
                        strongResponseMessage.Content = "会员支付";
                    }
                    break;
                case "MyOrder"://我的订单
                    {
                        //TODO
                    }
                    break;
                case "CompanyInfo"://成都印象餐饮有限公司
                    {
                        //TODO
                    }
                    break;
                case "Xiaochanghe"://成都印象小场合
                    {
                        //TODO
                    }
                    break;
                case "Culture"://小场合文化
                    {
                        //TODO
                    }
                    break;
                default:
                    {
                        var strongResponseMessage = CreateResponseMessage<ResponseMessageText>();
                        strongResponseMessage.Content = "您点击了按钮，EventKey：" + requestMessage.EventKey;
                        reponseMessage = strongResponseMessage;
                    }
                    break;
            }

            return reponseMessage;
        }

        /// <summary>
        /// 微信自动发送过来的位置信息.
        /// </summary>
        public override IResponseMessageBase OnEvent_LocationRequest(RequestMessageEvent_Location requestMessage)
        {
            var responseMessage = CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = string.Format("{0}:{1}", requestMessage.Latitude, requestMessage.Longitude);
            return responseMessage;
        }

        /// <summary>
        /// 通过扫描关注
        /// </summary>
        public override IResponseMessageBase OnEvent_ScanRequest(RequestMessageEvent_Scan requestMessage)
        {
            var strongResponseMessage = CreateResponseMessage<ResponseMessageNews>();
            strongResponseMessage.Articles.Add(GetWelcomeInfo());
            try
            {
                CrmMemberModel cdb = new CrmMemberModel();
                int i = cdb.Save();
            }
            catch (Exception)
            {
                //TODO
            }
            return strongResponseMessage;
        }

        /// <summary>
        /// 订阅（关注）事件
        /// </summary>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_SubscribeRequest(RequestMessageEvent_Subscribe requestMessage)
        {
            var strongResponseMessage = CreateResponseMessage<ResponseMessageNews>();
            strongResponseMessage.Articles.Add(GetWelcomeInfo());
            return strongResponseMessage;
        }

        /// <summary>
        /// 事件之弹出地理位置选择器（location_select）
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IResponseMessageBase OnEvent_LocationSelectRequest(RequestMessageEvent_Location_Select requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "事件之弹出地理位置选择器";
            return responseMessage;
        }
    }
}