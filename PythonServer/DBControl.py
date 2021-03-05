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
        host='127.0.0.1',
        port=3306,  # 端口号：默认可不填
        user='root',
        passwd='0522',
        db='StoreSystem',  # 数据库名
        charset='utf8'  # 数据库字符集
    )
    # 获取游标对象
    cursor = db.cursor()


# 执行sql语句
def Execute(target, source):
    global cursor, db
    cursor.execute("select {0} form {1}".format(target, source))


# 添加数据
def AddData(list_name: str, *args):
    global cursor, db
    cursor.execute("select * from %s" % list_name)
    fields = cursor.description
    print(fields)
    head = []
    for field in fields:
        head.append(field[0])
    head_str = ",".join(head)
    print(head_str)
    data = []
    for arg in args:
        arg = "'" + str(arg) + "'"
        data.append(arg)
    data_str = ",".join(data)
    print(data_str)
    sql_str = "insert into %s(%s) values (%s)" % (list_name, head_str, data_str)
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
def ModifyData():
    global cursor, db
    print("修改数据")


# 删除数据
def DeleteData():
    global cursor, db
    print("删除数据")


# 查找数据
def FindData(list_name: str, *args):
    global cursor, db
    cursor.execute("select * from %s" % list_name)
    # 遍历cursor里的记录
    for i in range(cursor.rowcount):
        row = cursor.fetchone()
        if row[1] == '哈哈':
            print('检查点-->>找到对象')
            break


# 关闭数据库
def CloseDB():
    global cursor, db
    # 关闭数据库
    db.close()

# # 返回游标当前一条记录，游标后移到下一条
# row1 = cursor.fetchone()
# print(row1)
# # 返回全部记录(元组）
# row2 = cursor.fetchall()
# # 返回指定条记录
# row3 = cursor.fetchmany(10)
