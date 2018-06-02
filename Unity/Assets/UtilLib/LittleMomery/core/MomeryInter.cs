using IMR;

namespace LittleMomery
{
    public class MomeryInter : Interaction<MomeryDataModel, MomeryDataRender>
    {
        public void Insert<T>(string key, T v) where T : class
        {
            sendCmdWithParamters(MomeryDataModel.CMD_INSERT, key, typeof(T), v);
        }

        public void Delete<T>(string key) where T : class
        {
            sendCmdWithParamters(MomeryDataModel.CMD_DELETE, key, typeof(T));
        }

        public T Get<T>(string key) where T : class
        {
            if (!IsExist<T>(key))
                return null;

            return (T)model.cache[typeof(T)][key].val;
        }

        /// <summary>
        /// 注册map
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        public void RegisterMap<T, U>() where T : class where U : MomeryItem
        {
            sendCmdWithParamters(MomeryDataModel.CMD_REGISTER, typeof(T), typeof(U));
        }

        public void ClearAll()
        {
            sendCmd(MomeryDataModel.CMD_CLEARALL);
        }

        public bool IsExist<T>(string key) where T : class
        {
            if (!model.cache.ContainsKey(typeof(T)))
            {
                return false;
            }

            if (!model.cache[typeof(T)].ContainsKey(key))
            {
                return false;
            }
            return true;
        }
    }
}
