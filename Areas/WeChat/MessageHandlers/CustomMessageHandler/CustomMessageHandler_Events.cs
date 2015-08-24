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
using WitBird.XiaoChangHe.Models.Info;

namespace WitBird.XiaoChangHe.Areas.WeChat.MessageHandlers.CustomMessageHandler
{
    /// <summary>
    /// 自定义MessageHandler
    /// </summary>
    public partial class CustomMessageHandler
    {
        protected const string ImgUrl = "http://115.29.38.163/images/";

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
                            Url = string.Format(Constants.HostDomain + "/order/begin/" + Constants.CompanyId + "/{0}?type=Quick", requestMessage.FromUserName)
                        });
                        reponseMessage = strongResponseMessage;
                    }
                    break;
                case "ActivityClick":
                    {
                        var strongResponseMessage = CreateResponseMessage<ResponseMessageNews>();
                        string picUrl = Constants.HostDomain + "/NewContent/images/w.png";
                        string uid = "";

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
                        Article article = new Article();

                        article.Title = "小场合活动·微观时代";
                        article.Description = "小场合活动·微观时代";
                        article.PicUrl = picUrl;
                        article.Url = string.Format(Constants.HostDomain + "/Jump/To?fromUserName={0}&url=/Activity/Index?activityState=1", requestMessage.FromUserName);

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
                            Url = string.Format(Constants.HostDomain + "/Member/Info/" + Constants.CompanyId + "/{0}", requestMessage.FromUserName)
                        });
                        reponseMessage = strongResponseMessage;
                    }
                    break;
                case "VipPay":
                    {
                        string picUrl = Constants.HostDomain + "/NewContent/images/w.png";
                        string uid = "";

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
                        }
                        var strongResponseMessage = CreateResponseMessage<ResponseMessageNews>();

                        DateTime dt = DateTime.Now.AddMinutes(-15);
                        PrepayRecord rec = new PrepayRecordModel().GetUserLastUnPaidComsumingPrepayRecordWithin5Minutes(requestMessage.FromUserName);
                        string description = string.Empty;
                        string title = string.Empty;
                        string url = "";

                        if (rec != null)
                        {
                            title = "支付确认";
                            description = "您本次的消费:现金￥" + (0 - rec.PrepayMoney.Value).ToString() +
                                 "/赠送￥" + (0 - rec.PresentMoney.Value).ToString() +
                                 (rec.PayByScore > 0 ? "/积分" + rec.PayByScore.ToString() : "") +
                                 (rec.DiscountlMoeny > 0 ? ",本次优惠：" + rec.DiscountlMoeny.ToString() : "") +
                                  ",账单时间：" + rec.PrepayDate.Value.ToString("yyyy-MM-dd HH:mm:ss") + "，点击本条消息，完成支付！";
                            url = string.Format(Constants.HostDomain + "/pay/payview/?id={0}&value={1}", requestMessage.FromUserName, rec.RecordId + "|" + DateTime.Now.Ticks);

                            strongResponseMessage.Articles.Add(new Article
                            {
                                Title = title,
                                Description = description,
                                PicUrl = picUrl,
                                Url = url
                            });
                            reponseMessage = strongResponseMessage;
                        }
                        else
                        {
                            description = "未查询到未结账单，可能原因为：\n1.您没有未结账单。\n2.您的账单还未生成。\n3.为保护您账号安全，您的生成的账单已经超期！";
                            
                            var result = CreateResponseMessage<ResponseMessageText>();
                            result.Content = description;

                            reponseMessage = result;
                        }
                    }
                    break;
                case "MyOrder"://我的订单
                    {
                        var strongResponseMessage = CreateResponseMessage<ResponseMessageNews>();
                        string picUrl = Constants.HostDomain + "/NewContent/images/w.png";
                        string uid = "";

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
                            Title = "我的订单",
                            Description = "我的订单：",
                            PicUrl = picUrl,
                            Url = string.Format(Constants.HostDomain + "/NewOrder/My/" + Constants.CompanyId + "/{0}", requestMessage.FromUserName)
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
                cdb.SaveMember(requestMessage.FromUserName, Constants.CompanyId.ToString());
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
                cdb.SaveMember(requestMessage.FromUserName, Constants.CompanyId.ToString());
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
                cdb.DiscardMember(requestMessage.FromUserName, Constants.CompanyId.ToString());
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

        private string getSingMessage(string from, string to, string value, string desciption, string url, string imgUrl)
        {
            string stt = "<xml>" +
                "<ToUserName><![CDATA[{0}]]></ToUserName>" +
                "<FromUserName><![CDATA[{1}]]></FromUserName>" +
                "<CreateTime>12345678</CreateTime>" +
                "<MsgType><![CDATA[news]]></MsgType>" +
                "<ArticleCount>1</ArticleCount>" +
                "<Articles>" +
                "<item>" +
                "<Title><![CDATA[{2}]]></Title> " +
                "<Description><![CDATA[{3}]]></Description>" +
                "<PicUrl><![CDATA[{5}]]></PicUrl>" +
                "<Url><![CDATA[{4}]]></Url>" +
                "</item>" +
                "</Articles>" +
                "</xml> ";
            string a = string.Format(stt, from, to, value, desciption, url, imgUrl);
            //this.WriteTextLog("retimg", a, DateTime.Now);
            return a;
        }
    }
}