# -*- coding: utf-8 -*-
"""
运行
"""
import Event
import Def


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
        result['user'] = cursor[0]
    else:
        result['reason'] = "password error"
    Event.FireEvent("SendMessage", "login:verify", result, tar=target)


# 获取商品数据
def GetGoodsData(target, args):
    if len(args) == 2:
        cursor = Event.FireEvent("FindData", "Goods", args)
    else:
        cursor = Event.FireEvent("FindData", "Goods")
    result = {'data': cursor}
    Event.FireEvent("SendMessage", "goods:data", result, tar=target)
