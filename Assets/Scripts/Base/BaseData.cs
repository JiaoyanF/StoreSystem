namespace Tar
{
    /// <summary>
    /// 数据基类
    /// </summary>
    public abstract class BaseData
    {
        public int Tag = int.MinValue;
        public int Id = int.MinValue;
        public string Name = string.Empty;
        protected virtual void Delete() { }
    }
}
