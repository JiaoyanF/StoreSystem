namespace Tar
{
    /// <summary>
    /// 数据基类
    /// </summary>
    public abstract class BaseData
    {
        public int Tag = int.MinValue;// 表类型
        public int Id = int.MinValue;// id
        public string Name = string.Empty;// 名称
        public string Time = string.Empty;// 时间戳
        protected virtual void Delete() { }
    }
}
