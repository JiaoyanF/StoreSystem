# -*- coding: utf-8 -*-
"""
数据库处理
"""
import pymysql

pymysql.install_as_MySQLdb()

global db


def InitDB():
    global db
    # noinspection PyBroadException
    try:
        db.ping()
        return db.cursor()
    except Exception as e:
        return ConnectDB().cursor()


# 关闭数据库
def CloseDB():
    global db
    try:
        db.ping()
        db.cursor().close()
        db.close()  # 关闭数据库
    except Exception as e:
        print(e)


def CommitDB():
    global db
    try:
        db.commit()
    except Exception as e:
        print(e)


# 发生错误回滚
def RollbackDB():
    global db
    try:
        db.rollback()
    except Exception as e:
        print(e)


# 连接数据库
def ConnectDB():
    global db
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
    return db


# 执行sql语句
def Execute(target, source):
    cur = InitDB()
    cur.execute("select {0} form {1}".format(target, source))
    CommitDB()


# 添加数据
def AddData(args):
    # print("添加数据:".format(args))
    cur = FindData(args[0])
    fields = cur.description
    # print(fields)
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
        cur.execute(sql_str)
        CommitDB()
        return True, None
    except Exception as e:
        print(e)
        RollbackDB()
        return False, str(e)


# 修改数据
def UpData(args):
    cursor = InitDB()
    # print("修改数据:{0}".format(args))
    try:
        num = cursor.execute("update %s set %s = '%s' where id = '%s'" % (args[0], args[1], args[2], args[3]))
        print("修改后受影响的行数为：", num)
        CommitDB()
        return True, None
    except Exception as e:
        print(e)
        RollbackDB()
        return False, str(e)


# 删除数据
def DeleteData(args):
    cursor = InitDB()
    # print("删除数据:{0}".format(args))
    try:
        num = cursor.execute("delete from %s where %s = '%s'" % (args[0], args[1], args[2]))
        CommitDB()
        return num > 0, None
    except Exception as e:
        print(e)
        RollbackDB()
        return False, str(e)


# 查找数据
def FindData(args):
    # print("查询参数：{0}".format(args))
    cursor = InitDB()
    if type(args) is str:
        cursor.execute("select * from %s" % args)
    else:
        cursor.execute("select * from %s where %s = '%s'" % (args[0], args[1], args[2]))
    return cursor

# cursor.executemany(sql, list1, list2)   一次操作多条数据
# row1 = cursor.fetchone()# 返回游标当前一条记录，游标后移到下一条
# row2 = cursor.fetchall()# 返回全部记录(元组）
# row3 = cursor.fetchmany(10)# 返回指定条记录
