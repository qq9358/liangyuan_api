using System;
using System.Collections.Generic;
using System.Text;

namespace Egoal.Settings.Dto
{
    /// <summary>
    /// 用户登录重试次数和锁定时间
    /// </summary>
    public class LoginLockParaOutput
    {
        /// <summary>
        /// 重试次数
        /// </summary>
        public int LockStaffMaxLoginErrorTimes { get; set; }

        /// <summary>
        /// 锁定时间
        /// </summary>
        public int LockStaffMinutes { get; set; }
    }
}
