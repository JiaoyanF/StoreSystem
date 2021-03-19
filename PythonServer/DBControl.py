# -*- coding: utf-8 -*-
"""
数据库处理
"""
import pymysql
pymysql.install_as_MySQLdb()

global db, cursor


# 连接数据库
def ConnectDB():
    global cursor, db
    db = pymysql.connect(
        host='119.29.65.81',
        # host='127.0.0.1',
        port=3306,  # 端口号：默认可不填
        user='root',
        passwd='0522',
        db='StoreSystem',  # 数据库名
        charset='utf8',  # 数据库字符集
        cursorclass=pymysql.cursors.DictCursor,  # 字典显示
    )
    # 获取游标对象
    cursor = db.cursor()


# 执行sql语句
def Execute(target, source):
    CheckDB()
    global cursor, db
    cursor.execute("select {0} form {1}".format(target, source))


# 添加数据
def AddData(args):
    CheckDB()
    global cursor, db
    cursor.execute("select * from %s" % args[0])
    fields = cursor.description
    print(fields)
    head = []
    for field in fields:
        head.append(field[0])
    head_str = ",".join(head)
    print(head_str)
    data = []
    for arg in args[1:]:
        arg = "'" + str(arg) + "'"
        data.append(arg)
    data_str = ",".join(data)
    print(data_str)
    sql_str = "insert into %s(%s) values (%s)" % (args[0], head_str, data_str)
    # noinspection PyBroadException
    try:
        # 插入数据
        cursor.execute(sql_str)
        # 提交到数据库
        db.commit()
    except Exception as e:
        print(e)
        # 发生错误回滚
        db.rollback()


# 修改数据
def UpData(args):
    CheckDB()
    global cursor, db
    print("修改数据:".format(args))
    num = cursor.execute("update %s where %s = %s" % (args[0], args[1], args[2]))
    print("修改后受影响的行数为：", num)


# 删除数据
def DeleteData(args):
    CheckDB()
    global cursor, db
    lis = FindData(args)
    print("删除数据:{0}".format(lis))
    cursor.execute("delete from %s where %s = %s" % (args[0], args[1], args[2]))


# 查找数据
def FindData(args):
    CheckDB()
    print("查询参数：{0}".format(args))
    global cursor, db
    if len(args) > 1:
        cursor.execute("select * from %s where %s = %s" % (args[0], args[1], args[2]))
    else:
        cursor.execute("select * from %s" % args[0])
    return cursor.fetchall()


def CheckDB():
    global cursor, db
    if cursor is None:
        ConnectDB()


# 关闭数据库
def CloseDB():
    global cursor, db
    # 关闭数据库
    db.close()

# cursor.executemany(sql, list1, list2)   一次操作多条数据
# row1 = cursor.fetchone()# 返回游标当前一条记录，游标后移到下一条
# row2 = cursor.fetchall()# 返回全部记录(元组）
# row3 = cursor.fetchmany(10)# 返回指定条记录
