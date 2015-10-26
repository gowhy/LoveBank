
using System.Collections.Generic;
using LoveBank.Common.Plugins;
using LoveBank.Core.Domain;
using LoveBank.Web.Admin.Models;

namespace LoveBank.Web.Admin.Helper {
    public static class PaymentInfoExtensions {

        //public static PaymentInfoModel ToModel(this Payment payment) {
        //    return new PaymentInfoModel {
        //                                    ClassName = payment.ClassName,
        //                                    Id = payment.ID,
        //                                    IsEffect = payment.IsEffect,
        //                                    Config = string.IsNullOrEmpty(payment.Config.Trim()) ? null : JsonConvert.DeserializeObject<Dictionary<string, PaymentInfo.ConfigInfo>>(payment.Config.Trim()),
        //                                    Description = payment.Description,
        //                                    Name = payment.Name,
        //                                    FeeAmount = payment.FeeAmount,
        //                                    FeeType = payment.FeeType,
        //                                    Logo = payment.Logo,
        //                                    OnlinePay = payment.OnlinePay,
        //                                    Sort = payment.Sort,
        //                                    TotalAmount = payment.TotalAmount,
        //                                    IsInstall = true
        //                                };
        //}

        //public static PaymentInfoModel ToModel(this PaymentInfo info) {
        //    return new PaymentInfoModel {
        //                                    ClassName = info.ClassName,
        //                                    Config = info.Config,
        //                                    IsEffect = false,
        //                                    Name = info.Name,
        //                                    OnlinePay = info.OnlinePaly,
        //                                    IsInstall = false,
        //                                    RegUrl = info.RegUrl
        //                                };
        //}
    }
}