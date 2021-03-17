# -*- coding: utf-8 -*-
"""
连接
"""
import json
import socket
import threading
import sys
import Def
import DBControl
import Event


# 连接通讯
def ConnectSocket():
    try:
        s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        # 防止socket server重启后端口被占用（socket.error: [Errno 98] Address already in use）
        s.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
        # s.bind(('10.0.8.3', 522))
        s.bind(('127.0.0.1', 522))
        s.listen(5)
    except socket.error as msg:
        print(msg)
        sys.exit(1)

    Init()

    while 1:
        conn, addr = s.accept()
        t = threading.Thread(target=ConnectionThread, args=(conn, addr))
        t.start()


# 预处理
def Init():
    print('等待连接ing...')
    DBControl.ConnectDB()
    Event.InitEvent()


# 连接线程
def ConnectionThread(conn, addr):
    tar = str(addr[0]) + ":" + str(addr[1])
    Def.Threads[tar] = conn
    print('*新的连接：{0}'.format(addr))
    SendMessage(tar, '成功连接服务器')
    while 1:
        data = conn.recv(1024)
        # time.sleep(1)
        if data.decode('utf-8') == 'exit' or not data:
            print('*{0}请求断开连接'.format(tar))
            break
        ReceiveMessages(tar, data.decode('utf-8'))
    CloseNet(conn)


# 发送消息
def SendMessage(target, args):
    if isinstance(args, str):
        s = args
    else:
        s = args[0] + '#'
        for var in args[1:]:
            if isinstance(var, dict) or isinstance(var, list):
                var = json.dumps(var)
            s += var
    print('发送：{0}'.format(s))
    Def.Threads[target].send(s.encode('utf-8'))


# 接收消息
def ReceiveMessages(target, mes):
    index = mes.find('#')
    if index == -1:
        print('接收：{0}'.format(mes))
        return
    tag = mes[:index]
    args = mes[index + 1:].split(',')
    if tag in Def.NetTag is None:
        print('错误：网络协议【{0}】不存在'.format(tag))
        return
    print('接收：{0}(tag={1},args={2})'.format(mes, tag, args))
    Event.FireEvent(tag, args[0], args[1], tar=target)
    # if tag == 'login:request':
    #     DBControl.AddData('Goods', 100004, '牛奶', 500, 1)


# 断开连接
def CloseNet(conn):
    conn.close()


if __name__ == '__main__':
    ConnectSocket()
