using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using Senparc.Weixin.MP.Agent;
using Senparc.Weixin.Context;
using Senparc.Weixin.MP.Entities;
using Senparc.Weixin.MP.Helpers;
using Senparc.Weixin.MP.MessageHandlers;
using WitBird.XiaoChangHe.Areas.WeChat.Utilities;
using WitBird.XiaoChangHe.Models;
using WitBird.XiaoChangeHe.Core;

namespace WitBird.XiaoChangHe.Areas.WeChat.MessageHandlers.CustomMessageHandler
{
    /// <summary>
    /// 自定义MessageHandler
    /// </summary>
    public partial class CustomMessageHandler
    {
        protected const string ImgUrl = "http://115.29.38.163/images/";
        protected const string CompanyId = "CB824E58-E2CA-4C95-827A-CA62D528C6A7";

        private Article GetWelcomeInfo()
        {
            return new Article()
            {
                Title = "成都映象小场合以经营地道川菜、干锅、小吃为主要特色。",
                Description = "小场合以经营地道川菜、干锅、小吃为主要特色，在这里您可以找到儿时的回忆，在这里您可以找到妈妈饭菜的味道！",
                PicUrl = "http://115.29.38.163/Images/xchlogo.jpg",
                Url = "http://mp.weixin.qq.com/s?__biz=MjM5MjIzMDIzMQ==&mid=200696040&idx=1&sn=13ba2109e4bf2ff3ee019ee086c3ce47#rd"
            };
        }

        public override IResponseMessageBase OnEvent_ClickRequest(RequestMessageEvent_Click requestMessage)
        {
            IResponseMessageBase reponseMessage = null;
            switch (requestMessage.EventKey)
            {
                case "BeginOrderClick":
                    {
                        var strongResponseMessage = CreateResponseMessage<ResponseMessageNews>();
                        strongResponseMessage.Articles.Add(new Article
                        {
                            Title = "开始预定",
                            Description = "通过微信预订点餐",
                            PicUrl = ImgUrl + "wxbeginorder.jpg",
                            Url = string.Format(Constants.HostDomain + "/order/begin/CB824E58-E2CA-4C95-827A-CA62D528C6A7/{0}?type=Quick", requestMessage.FromUserName)
                        });
                        reponseMessage = strongResponseMessage;
                    }
                    break;
                case "ActivityClick":
                    {
                        var strongResponseMessage = CreateResponseMessage<ResponseMessageNews>();

                        Article article = new Article();

                        article.Title = "小场合活动·微观时代";
                        article.Description = "小场合活动·微观时代";
                        article.PicUrl = "http://test.xgdg.cn/NewContent/images/chu.png";
                        article.Url = string.Format(Constants.HostDomain + "/Activity/Index/CB824E58-E2CA-4C95-827A-CA62D528C6A7/{0}?activityState=1", requestMessage.FromUserName);

                        strongResponseMessage.Articles.Add(article);

                        reponseMessage = strongResponseMessage;
                    }
                    break;
                case "AccountInfoClick":
                    {
                        var strongResponseMessage = CreateResponseMessage<ResponseMessageNews>();
                        string uid = string.Empty;
                        string picUrl = Constants.HostDomain + "/NewContent/images/w.png"; ;
                        try
                        {
                            CrmMemberModel model = new CrmMemberModel();
                            uid = model.getCrmMemberListInfoData(requestMessage.FromUserName).FirstOrDefault().Uid;
                            var prepayAccount = model.GetPrepayAccount(uid);
                            if (prepayAccount != null && prepayAccount.AccountMoney > 0)
                            {
                                picUrl = Constants.HostDomain + "/NewContent/images/chu.png";
                            }
                        }
                        catch (Exception ex)
                        {
                            LogException(ex);
                            uid = ex.StackTrace;
                        }
                        strongResponseMessage.Articles.Add(new Article
                        {
                            Title = "会员信息",
                            Description = "您的会员号：" + uid,
                            PicUrl = picUrl,
                            Url = string.Format(Constants.HostDomain + "/Member/Info/CB824E58-E2CA-4C95-827A-CA62D528C6A7/{0}", requestMessage.FromUserName)
                        });
                        reponseMessage = strongResponseMessage;
                    }
                    break;
                case "VipPay":
                    {
                        var strongResponseMessage = CreateResponseMessage<ResponseMessageNews>();
                        strongResponseMessage.Articles.Add(new Article
                        {
                            Title = "会员充值",
                            Description = "充值成为储值会员",
                            PicUrl = ImgUrl + "wxbeginorder.jpg",
                            Url = string.Format(Constants.HostDomain + "/member/PreRecharge/CB824E58-E2CA-4C95-827A-CA62D528C6A7/{0}?type=Quick", requestMessage.FromUserName)
                        });
                        reponseMessage = strongResponseMessage;
                    }
                    break;
                case "MyOrder"://我的订单
                    {
                        //NewOrder/My/CB824E58-E2CA-4C95-827A-CA62D528C6A7/ovYq8vgkV-hK11i_ftjaoRBM_IMM
                        var strongResponseMessage = CreateResponseMessage<ResponseMessageNews>();
                        strongResponseMessage.Articles.Add(new Article
                        {
                            Title = "我的订单",
                            Description = "我的订单：",
                            PicUrl = ImgUrl + "wxmembercard_h.png",
                            Url = string.Format(Constants.HostDomain + "/NewOrder/My/CB824E58-E2CA-4C95-827A-CA62D528C6A7/{0}", requestMessage.FromUserName)
                        });
                        reponseMessage = strongResponseMessage;
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
                cdb.SaveMember(requestMessage.FromUserName, CompanyId);
            }
            catch (Exception ex)
            {
                LogException(ex);
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
            try
            {
                CrmMemberModel cdb = new CrmMemberModel();
                cdb.SaveMember(requestMessage.FromUserName, CompanyId);
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
            return strongResponseMessage;
        }

        public override IResponseMessageBase OnEvent_UnsubscribeRequest(RequestMessageEvent_Unsubscribe requestMessage)
        {
            var responseMessage = base.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "有空再来";
            try
            {
                CrmMemberModel cdb = new CrmMemberModel();
                cdb.DiscardMember(requestMessage.FromUserName, CompanyId);
            }
            catch (Exception ex)
            {
                LogException(ex);
            }
            return responseMessage;
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