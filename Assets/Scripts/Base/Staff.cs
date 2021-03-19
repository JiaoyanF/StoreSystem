using Tar;
using Def;

/// <summary>
/// 使用者
/// </summary>
public class Staff : BaseData
{
    public PowerType Power;// 权限
    public PostType Post;// 岗位
    public Gender Gene;// 性别
    public Staff(int id)
    {
        this.id = id;
    }
}