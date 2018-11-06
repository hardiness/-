using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using 自定义编辑器;
using System.Text.RegularExpressions;
namespace Authority
{
    [TypeConverter(typeof(TConverter))]
    [Serializable()]
    public abstract class Authority
    {


        public enum User
        {
            操作员=0,工程师,管理员
        }

        private string[] DefaultPassword = {"20180101", "20180601", "20180808" };

        private string[] _Passwords ={"20180101","20180601","20180808"};


        

        [Browsable(false)]
        public string[] Passwords
        {
            get { return _Passwords; }
            set {_Passwords=value; }
        }


        public string ReText { get; set; } = @"[\w]+";
        public bool LoadDefaultPassword(User Usr)
        {
            try
            {
                Passwords[(int)Usr] = DefaultPassword[(int)Usr];
                return true;
            }
            catch (Exception )
            {

                return false;
            }
           
          
        }

        /// <summary>
        /// 添加项目
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        abstract public bool Add(string Usr ,string Menu);
        /// <summary>
        /// 移除项目
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        abstract public bool Remove(string Usr, string Menu);
        /// <summary>
        /// 执行，即对每个项目进行权限设定
        /// </summary>
        /// <returns></returns>
        abstract public bool Performance(string Usr, string Menu);

        /// <summary>
        /// 验证账户密码
        /// </summary>
        /// <param name="user"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        abstract public bool VerifyUser(User Usr ,string Psd);

        /// <summary>
        /// 修改账户密码
        /// </summary>
        /// <param name="Usr"></param>
        /// <param name="Psd"></param>
        /// <returns></returns>
        abstract public bool ModifyPassword(User Usr, string Psd);

        abstract public string VerifyPassword(string Pw1, string Pw2);

        public Dictionary<string, System.Collections.ObjectModel.Collection<string>> Menus { get; set; }
     

        public Authority()
        {
            Menus = new Dictionary<string, System.Collections.ObjectModel.Collection<string>>();
            foreach (var item in Enum.GetNames(typeof(User)))
            {
                Menus.Add(item.ToString(), new System.Collections.ObjectModel.Collection<string>());
            }
           
        }


    }
    [TypeConverter(typeof(TConverter))]
    [Serializable()]
    public class NormalAuthority : Authority
    {
        public override bool Add(string Usr, string Menu)
        {
            if (Menus[Usr].Contains(Menu))
            {
                return false;
            }

            Menus[Usr].Add(Menu);
            return true;
        }

        public override bool ModifyPassword(User Usr, string Psd)
        {
            Passwords[(int)Usr] = Psd;
            return true;
        }

        public override bool Performance(string Usr, string Menu)
        {
            if (Menus[Usr].Contains(Menu))
            {
                return true;
            }
            return false;
        }

        public override bool Remove(string Usr, string Menu)
        {
            if (!Menus[Usr].Contains(Menu))
            {
                return false;
            }

            Menus[Usr].Remove(Menu);
            return true;
        }

        public override string VerifyPassword(string Psw1, string Psw2)
        {
            //throw new NotImplementedException();
              Regex re = new Regex(ReText);
            MatchCollection matches1 = re.Matches(Psw1);
            MatchCollection matches2 = re.Matches(Psw2);

            if (matches1.Count==0 || matches2.Count==0)
            {
                return "";
            }

            if (matches1[0].Value != matches2[0].Value)
            {
                return "";

            }

            return matches1[0].Value;


            //if (matches.Count == 0)
            //{
            //    return "";
            //}

            //return matches[0].ToString();

        }

        public override bool VerifyUser(User Usr, string Psd)
        {
            if (Passwords[(int)Usr]==Psd)
            {
                return true;
            }
            return false;
        }
    }
}
