# -*- coding: utf-8 -*-
"""
连接
"""

import socket
import threading
import sys
import Def
import DBControl


def socket_service():
    try:
        s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        # 防止socket server重启后端口被占用（socket.error: [Errno 98] Address already in use）
        s.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
        s.bind(('10.0.8.3', 522))
        # s.bind(('127.0.0.1', 522))
        s.listen(5)
    except socket.error as msg:
        print(msg)
        sys.exit(1)
    DBControl.ConnectDB()
    print('等待连接ing...')

    while 1:
        conn, addr = s.accept()
        t = threading.Thread(target=ConnectionThread, args=(conn, addr))
        t.start()


# 连接线程
def ConnectionThread(conn, addr):
    print('*新的连接：{0}'.format(addr))
    SendMessage(conn, '成功连接服务器')
    while 1:
        data = conn.recv(1024)
        # time.sleep(1)
        if data.decode('utf-8') == 'exit' or not data:
            print('*{0}请求断开连接'.format(addr))
            # SendMessage(conn, 'exit')
            # conn.send(bytes('exit'.encode('utf-8')))
            break
        ReceiveMessages(data.decode('utf-8'))
    CloseNet(conn)


# 发送消息
def SendMessage(conn, tag, *args):
    s = tag + '#'
    for var in args:
        s += var

    conn.send(s.encode('utf-8'))


# 接收消息
def ReceiveMessages(mes):
    index = mes.find('#')
    if index == -1:
        print('接收：{0}'.format(mes))
        return
    tag = mes[:index]
    args = mes[index + 1:]
    s = Def.NetTag.get(tag)
    if s is None:
        print('错误：网络协议【{0}】不存在'.format(tag))
        return
    print('接收：{0}(tag={1},args={2})'.format(mes, tag, args))
    if s == '00000':
        DBControl.AddData('Goods', 100003, '娃哈哈', 50, 1)


# 断开连接
def CloseNet(conn):
    conn.close()


if __name__ == '__main__':
    socket_service()
