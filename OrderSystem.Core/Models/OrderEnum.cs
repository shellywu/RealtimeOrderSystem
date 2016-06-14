using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OrderSystem.Core.Models
{
    public enum OrderStatus:int
    {
        Create = 1,
        Processing = 2,
        Done = 3,
        Change = 4,
        Partion=5,
        DoneWithErr=6,
        Cancel=7,
        Canceling=8,
        Canceled = 9,
        CancelFailed = 10,
        Err=888
    }

    public enum MemberLevel
    {
        LowLevel=1,
        MidLevel=2,
        HighLevel=3
    }

    public enum CredentialType:int
    {
        IdCard=1,
        Passport=2,
        other=3
    }
    public enum ProductType:int
    {
        Combo=1,
        Titcket=2,
        Hotel=3,
        Tax=4,
        Travel=5
    }

    public enum TaskState
    {
        Create=0,
        Take=1,
        Finish=2,
        Change=3,
        OrderErr=4,
        CreateCancel=7,
        /// <summary>
        /// 待取消订单
        /// </summary>
        TakeCanceling=8,
        /// <summary>
        /// 已取消
        /// </summary>
        Canceled=9,
        CancelFaild=10
    }
}