# -*- coding: utf-8 -*-
"""
连接
"""

import socket
import threading
import sys


def socket_service():
    try:
        s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        # 防止socket server重启后端口被占用（socket.error: [Errno 98] Address already in use）
        s.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
        s.bind(('10.0.8.3', 522))
        s.listen(5)
    except socket.error as msg:
        print(msg)
        sys.exit(1)
    print('等待连接ing...')

    while 1:
        conn, addr = s.accept()
        t = threading.Thread(target=deal_data, args=(conn, addr))
        t.start()


def deal_data(conn, addr):
    print('*新的连接：{0}'.format(addr))
    conn.send('成功连接服务器！'.encode('utf-8'))
    while 1:
        data = conn.recv(1024)
        print('接收：{0}'.format(data.decode('utf-8')))
        # time.sleep(1)
        if data.decode('utf-8') == 'exit' or not data:
            print('*{0}请求断开连接'.format(addr))
            conn.send(bytes('exit'.encode('utf-8')))
            break
    conn.close()


if __name__ == '__main__':
    socket_service()

"""

# 连接数据库
db = MySQLdb.connect(
    host='192.168.0.0',
    port='3360',  # 端口号：默认可不填
    user='fky',
    passwd='123456',
    db='my2',  # 数据库名
    charset='utf8'  # 数据库字符集
)

# 获取游标对象
conn = db.cursor()
# 执行sql语句
conn.execute('select * form role')
# 返回游标当前一条记录，游标后移到下一条
row1 = conn.fetchone()
print(row1)
# 返回全部记录(元组）
row2 = conn.fetchall()
# 返回指定条记录
row3 = conn.fetchmany(10)

# 遍历conn里的记录
for i in range(conn.rowcount):
    row = conn.fetchone()
    if row[1] == '哈哈':
        print('检查点-->>找到对象')
        break

sql_str = '''
insert into sole('name','sex','occupation') values  ('椒盐','女','法师')
'''
try:
    # 插入数据
    conn.execute(sql_str)
    # 提交到数据库
    db.commit()
except:
    # 发生错误回滚
    db.rollback()

# 关闭数据库
db.close()

# 关于python对数据库的详细操作：https://www.runoob.com/python/python-mysql.html


for n in range(10):
    print(f'第{n + 1}条数据')

"""
