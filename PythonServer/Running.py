# -*- coding: utf-8 -*-
"""
运行
"""
import Event


# class Running:
#     def Verify(self, args):
#         print("登录参数：{0}".format(args))


# 验证登录
def Verify(target, args):
    # print("登录参数：{0}".format(args))
    if args is None:
        return
    cursor = Event.FireEvent("FindData", "Staff", 'id', args[0])
    result = {'result': False}
    if len(cursor) == 0:
        result['reason'] = "user is null"
    elif cursor[0]['password'] == args[1]:
        result['result'] = True
    else:
        result['reason'] = "password error"
    Event.FireEvent("SendMessage", "login:response", result, tar=target)
